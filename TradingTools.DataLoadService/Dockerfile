#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TradingTools.DataLoadService/TradingTools.DataLoadService.csproj", "TradingTools.DataLoadService/"]
RUN dotnet restore "TradingTools.DataLoadService/TradingTools.DataLoadService.csproj"
COPY . .
WORKDIR "/src/TradingTools.DataLoadService"
RUN dotnet build "TradingTools.DataLoadService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TradingTools.DataLoadService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TradingTools.DataLoadService.dll"]