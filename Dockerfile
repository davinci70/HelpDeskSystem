FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY src/HelpDesk.API/HelpDesk.API.csproj src/HelpDesk.API/
COPY src/HelpDesk.Application/HelpDesk.Application.csproj src/HelpDesk.Application/
COPY src/HelpDesk.Domain/HelpDesk.Domain.csproj src/HelpDesk.Domain/
COPY src/HelpDesk.Infrastructure/HelpDesk.Infrastructure.csproj src/HelpDesk.Infrastructure/

RUN dotnet restore src/HelpDesk.API/HelpDesk.API.csproj

COPY src/ ./src/

RUN dotnet build src/HelpDesk.API/HelpDesk.API.csproj --configuration Release --no-restore

RUN dotnet publish src/HelpDesk.API/HelpDesk.API.csproj --configuration Release --no-restore --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "HelpDesk.API.dll"]