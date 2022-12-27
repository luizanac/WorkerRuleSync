FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/WorkerRuleSync.WorkerA/WorkerRuleSync.WorkerA.csproj", "src/WorkerRuleSync.WorkerA/"]
COPY ["src/WorkerRuleSync.Sdk/WorkerRuleSync.Sdk.csproj", "src/WorkerRuleSync.Sdk/"]
RUN dotnet restore "src/WorkerRuleSync.WorkerA/WorkerRuleSync.WorkerA.csproj"
COPY . .
WORKDIR "/src/src/WorkerRuleSync.WorkerA"
RUN dotnet build "WorkerRuleSync.WorkerA.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkerRuleSync.WorkerA.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkerRuleSync.WorkerA.dll"]
