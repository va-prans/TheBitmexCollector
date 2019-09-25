using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Bitmex.NET;
using Bitmex.NET.Dtos;
using Bitmex.NET.Models;
using Bitmex.NET.Models.Socket;
using Microsoft.EntityFrameworkCore;

namespace TheBitmexCollector
{
    public class TheCollector
    {
        private IBitmexApiService _bitmexApiService;
        private IBitmexApiSocketService _bitmexApiSocketService;
        private IBitmexAuthorization _bitmexAuthorization;
        private BitmexApiSubscriptionInfo _subLiquidation;
        private BitmexApiSubscriptionInfo _subInstrument;

        private DateTime _lastupdate;
        private static System.Timers.Timer timer;
        private static System.Timers.Timer timer1;

        private bool timerStarted = false;
        private CollectorContext _collectorContext;
        private bool started = false;

        BlockingCollection<LiquidationDto> dataQueue = new BlockingCollection<LiquidationDto>(new ConcurrentQueue<LiquidationDto>());

        public void Start()
        {
            try
            {
                _collectorContext = new CollectorContext();
                _collectorContext.Database.Migrate();
                started = true;
            }
            catch (Exception e)
            {
                started = false;
                Debug.WriteLine("Startup failed, trying again in 5 seconds..");
                Thread.Sleep(5000);
                if (!started)
                {
                    Start();
                }
            }
            CreateServices();
            try
            {
                InitializeSubs();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
               
            }
            StartTimer();
        }

        private void StartTimer()
        {
            if (!timerStarted)
            {
                timer = new System.Timers.Timer();
                timer.Interval = 1000 * 60;

                timer.Elapsed += CheckForNoUpdate;
                timer.AutoReset = true;
                timer.Enabled = true;
                timerStarted = true;

                timer1 = new System.Timers.Timer();
                timer1.Interval = 1000 * 60 * 10;

                timer1.Elapsed += FetchTradeBins;
                timer1.AutoReset = true;
                timer1.Enabled = true;
                Console.WriteLine("Timer started.");
                Debug.WriteLine("Timer started.");

                FetchTradeBins(null, null);
            }          
        }

        private async void FetchTradeBins(Object source, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("Fetching bins.");
            Debug.WriteLine("Fetching bins.");
            using (var context = new CollectorContext())
            {
                
                try
                {
                    var latestBin = context.TradeBins.OrderByDescending(x => x.Timestamp).FirstOrDefault();
                    var @params = new TradeBucketedGETRequestParams
                    {
                        BinSize = "1m",
                        Symbol = "XBTUSD",
                        Count = 200,
                        Reverse = true
                    };

                    // act
                    var result = new List<TradeBucketedDto>();
                    try
                    {
                        result = _bitmexApiService.Execute(BitmexApiUrls.Trade.GetTradeBucketed, @params).Result;
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }

                    foreach (var bin in result)
                    {
                        if (latestBin == null || bin.Timestamp.LocalDateTime > latestBin.Timestamp)
                        {
                            await context.TradeBins.AddAsync(new TradeBin()
                            {
                                Symbol = bin.Symbol,
                                Timestamp = bin.Timestamp.UtcDateTime,
                                Open = bin.Open.Value,
                                High = bin.High.Value,
                                Low = bin.Low.Value,
                                Close = bin.Close.Value,
                                Trades = bin.Trades,
                                Volume = bin.Volume.Value,
                                ForeignNotional = bin.ForeignNotional,
                                HomeNotional = bin.HomeNotional
                            });
                        }
                    }

                    await context.SaveChangesAsync();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    
                }
            }
        }

        private void CheckForNoUpdate(Object source, System.Timers.ElapsedEventArgs e)
        {
            var diffInSecondsWebsocketTemp = (DateTime.Now - _lastupdate).TotalSeconds;
            Console.WriteLine("Time since last update: " + diffInSecondsWebsocketTemp);
            if (diffInSecondsWebsocketTemp > 60)
            {
                Console.WriteLine("Connection invalid, reconnecting.");
                try
                {
                    _bitmexApiSocketService.Unsubscribe(_subLiquidation);
                    _bitmexApiSocketService.Unsubscribe(_subInstrument);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
               
                _lastupdate = DateTime.Now;
                CreateServices();
                InitializeSubs();
            }
            else
            {
                Console.WriteLine("Connection still valid.");
            }

        }
        private async Task HandleLiquidation(LiquidationDto dto)
        {
            try
            {
                using (var context = new CollectorContext())
                {
                    _lastupdate = DateTime.Now;
                    if (dto.Price != null)
                    {
                        if (dto.LeavesQty != null)
                        {
                            if (await _collectorContext.Liquidations.FirstOrDefaultAsync(a => a.LiquidationId == dto.OrderId) == null)
                            {
                                Console.WriteLine(DateTime.Now + ": " + dto.Symbol + " " + (dto.Side == "Sell" ? "long" : "short") +
                                                  " liquidation at " + dto.Price.Value + " with quantity " + dto.LeavesQty.Value);
                                Debug.WriteLine(DateTime.Now + ": " + dto.Symbol + " " + (dto.Side == "Sell" ? "long" : "short") +
                                                  " liquidation at " + dto.Price.Value + " with quantity " + dto.LeavesQty.Value);
                                await context.Liquidations.AddAsync(new Liquidation()
                                {
                                    LiquidationId = dto.OrderId,
                                    Symbol = dto.Symbol,
                                    Timestamp = DateTime.Now.ToUniversalTime(),
                                    Direction = dto.Side == "Sell" ? "Long" : "Short",
                                    Price = dto.Price.Value,
                                    Quantity = dto.LeavesQty.Value
                                });
                                await context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void InitializeSubs()
        {
            _subLiquidation = BitmetSocketSubscriptions.CreateLiquidationSubsription(message =>
            {
                foreach (var dto in message.Data)
                {
                    dataQueue.Add(dto);
                }
            });
            _bitmexApiSocketService.Subscribe(_subLiquidation);

            _subInstrument = BitmetSocketSubscriptions.CreateInstrumentSubsription(message =>
            {
                foreach (var dto in message.Data)
                {
                  _lastupdate = DateTime.Now;
                }
            });
            _bitmexApiSocketService.Subscribe(_subInstrument);
            dataQueue.CompleteAdding();
            dataQueue = new BlockingCollection<LiquidationDto>(new ConcurrentQueue<LiquidationDto>());
            Task.Run(async () =>
            {
                while (!dataQueue.IsCompleted)
                {
                    LiquidationDto liquidationDto = null;
                    try
                    {
                        liquidationDto = dataQueue.Take();
                    }
                    catch (InvalidOperationException) { }

                    if (liquidationDto != null)
                    {
                        await HandleLiquidation(liquidationDto);
                    }
                }
                Console.WriteLine("\r\nNo more items to take.");
            });
        }

        private void CreateServices()
        {
            try
            {
                _bitmexAuthorization = new BitmexAuthorization()
                {
                    BitmexEnvironment = BitmexEnvironment.Prod,
                    Key = "Your_API_Key",
                    Secret = "Your_API_Secret"
                };
                _bitmexApiService = BitmexApiService.CreateDefaultApi(_bitmexAuthorization);
                _bitmexApiSocketService = BitmexApiSocketService.CreateDefaultApi(_bitmexAuthorization);
                _bitmexApiSocketService.Connect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
