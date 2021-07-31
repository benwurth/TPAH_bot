FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TPHA_bot/TPHA_bot.fsproj", "TPHA_bot/"]
COPY ["TPHA_bot.Shared/TPHA_bot.Shared.fsproj", "TPHA_bot.Shared/"]
RUN dotnet restore "TPHA_bot/TPHA_bot.fsproj"
COPY . .
WORKDIR "/src/TPHA_bot"
RUN dotnet build "TPHA_bot.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TPHA_bot.fsproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TPHA_bot.dll"]
