FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
COPY . /build
WORKDIR /build
RUN dotnet restore -s https://api.nuget.org/v3/index.json; \
    dotnet build --no-restore -c Release; \    
    dotnet publish ./SwarmFeatures.SchedulerWeb/SwarmFeatures.SchedulerWeb.csproj -c Release -o /scheduler --no-build; \
    dotnet publish ./SwarmFeatures.SwarmAutoProxy/SwarmFeatures.SwarmAutoProxy.csproj -c Release -o /proxy --no-build; \
    dotnet nuget locals http-cache --clear;\
    dotnet nuget locals temp --clear

######## Scheduler
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim as scheduler
COPY --from=build /scheduler /scheduler
WORKDIR /scheduler
EXPOSE 80
ENTRYPOINT ["dotnet", "SwarmFeatures.SchedulerWeb.dll"]

######### proxy
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim as proxy
COPY --from=build /proxy /proxy
WORKDIR /proxy
EXPOSE 80
ENTRYPOINT ["dotnet", "SwarmFeatures.SwarmAutoProxy.dll"]
