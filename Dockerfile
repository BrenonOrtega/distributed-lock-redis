FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as base
EXPOSE 80
WORKDIR /app

COPY SimpleLock/SimpleLock.csproj SimpleLock
COPY DistributedLock.Commons/DistributedLock.Commons.csproj DistributedLock.Commons
RUN dotnet restore SimpleLock

COPY SimpleLock/ /SimpleLock
COPY DistributedLock.Commons /DistributedLock.Commons
WORKDIR /SimpleLock
RUN dotnet build 

FROM base as publish
RUN dotnet publish -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine as final
WORKDIR /app
COPY --from=publish /publish .
ENTRYPOINT ["dotnet", "SimpleLock.dll", "docker-instance"]