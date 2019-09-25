#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as build-image

WORKDIR /home/
 
COPY Bitmex.NET ./Bitmex.NET
 
WORKDIR /home/app
 
COPY . .

RUN dotnet restore


RUN dotnet publish ./TheBitmexCollector/TheBitmexCollector.csproj -o /publish/
 
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

WORKDIR /publish


COPY --from=build-image /publish .
COPY wait-for.sh wait-for.sh
RUN chmod +x wait-for.sh
RUN apt-get -q update && apt-get -qy install netcat
ENTRYPOINT ["dotnet", "TheBitmexCollector.dll"]
