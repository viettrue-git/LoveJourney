FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/LoveJourney.Api/LoveJourney.Api.csproj", "src/LoveJourney.Api/"]
COPY ["src/LoveJourney.Application/LoveJourney.Application.csproj", "src/LoveJourney.Application/"]
COPY ["src/LoveJourney.Domain/LoveJourney.Domain.csproj", "src/LoveJourney.Domain/"]
COPY ["src/LoveJourney.Infrastructure/LoveJourney.Infrastructure.csproj", "src/LoveJourney.Infrastructure/"]
RUN dotnet restore "src/LoveJourney.Api/LoveJourney.Api.csproj"
COPY . .
WORKDIR "/src/src/LoveJourney.Api"
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LoveJourney.Api.dll"]
