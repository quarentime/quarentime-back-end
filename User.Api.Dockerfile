FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /workspace

COPY ./User.Api ./User.Api
COPY ./Quarentime.Common ./Quarentime.Common

RUN dotnet restore ./User.Api
RUn dotnet restore ./Quarentime.Common
RUN dotnet publish ./User.Api -c Release -o publish

# Run
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /workspace
COPY --from=build-env /workspace/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "User.Api.dll", "--urls", "http://0.0.0.0:8080"]