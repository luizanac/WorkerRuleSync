FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/WorkerRuleSync.ServerB/WorkerRuleSync.ServerB.csproj", "src/WorkerRuleSync.ServerB/"]
RUN dotnet restore "src/WorkerRuleSync.ServerB/WorkerRuleSync.ServerB.csproj"
COPY . .
WORKDIR "/src/src/WorkerRuleSync.ServerB"
RUN dotnet build "WorkerRuleSync.ServerB.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkerRuleSync.ServerB.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkerRuleSync.ServerB.dll"]
