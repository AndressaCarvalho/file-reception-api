FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/FileReception.sln .

COPY src/FileReception.Api/FileReception.Api.csproj FileReception.Api/
COPY src/FileReception.Domain/FileReception.Domain.csproj FileReception.Domain/
COPY src/FileReception.Infrastructure/FileReception.Infrastructure.csproj FileReception.Infrastructure/
COPY src/FileReception.Service/FileReception.Service.csproj FileReception.Service/
COPY src/FileReception.Tests/FileReception.Tests.csproj FileReception.Tests/

RUN dotnet restore FileReception.sln

COPY src/ .

WORKDIR /src/FileReception.Api
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8443
ENTRYPOINT ["dotnet", "FileReception.Api.dll"]
