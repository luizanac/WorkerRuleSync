FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/WorkerRuleSync.Server/WorkerRuleSync.Server.csproj", "src/WorkerRuleSync.Server/"]
COPY ["src/WorkerRuleSync.Sdk/WorkerRuleSync.Sdk.csproj", "src/WorkerRuleSync.Sdk/"]
RUN dotnet restore "src/WorkerRuleSync.Server/WorkerRuleSync.Server.csproj"
COPY . .
WORKDIR "/src/src/WorkerRuleSync.Server"
RUN dotnet build "WorkerRuleSync.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkerRuleSync.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkerRuleSync.Server.dll"]
