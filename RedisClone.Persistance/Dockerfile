FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RedisClone.Persistance/RedisClone.Persistance.csproj", "RedisClone.Persistance/"]
RUN dotnet restore "RedisClone.Persistance/RedisClone.Persistance.csproj"
COPY . .
WORKDIR "/src/RedisClone.Persistance"
RUN dotnet build "RedisClone.Persistance.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RedisClone.Persistance.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RedisClone.Persistance.dll"]
