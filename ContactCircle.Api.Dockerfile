FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /workspace

COPY ./ContactCircle.Api ./workspace

RUN dotnet restore ./workspace
RUN dotnet publish ./workspace -c Release -o publish

# Run
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /workspace
COPY --from=build-env /workspace/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "ContactCircle.Api.dll", "--urls", "http://0.0.0.0:8080"]