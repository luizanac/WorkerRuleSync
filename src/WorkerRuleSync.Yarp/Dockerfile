﻿FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/WorkerRuleSync.Yarp/WorkerRuleSync.Yarp.csproj", "src/WorkerRuleSync.Yarp/"]
RUN dotnet restore "src/WorkerRuleSync.Yarp/WorkerRuleSync.Yarp.csproj"
COPY . .
WORKDIR "/src/src/WorkerRuleSync.Yarp"
RUN dotnet build "WorkerRuleSync.Yarp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkerRuleSync.Yarp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkerRuleSync.Yarp.dll"]
