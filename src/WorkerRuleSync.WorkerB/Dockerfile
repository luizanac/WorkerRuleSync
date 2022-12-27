FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/WorkerRuleSync.WorkerB/WorkerRuleSync.WorkerB.csproj", "src/WorkerRuleSync.WorkerB/"]
COPY ["src/WorkerRuleSync.Sdk/WorkerRuleSync.Sdk.csproj", "src/WorkerRuleSync.Sdk/"]
RUN dotnet restore "src/WorkerRuleSync.WorkerB/WorkerRuleSync.WorkerB.csproj"
COPY . .
WORKDIR "/src/src/WorkerRuleSync.WorkerB"
RUN dotnet build "WorkerRuleSync.WorkerB.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkerRuleSync.WorkerB.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkerRuleSync.WorkerB.dll"]
