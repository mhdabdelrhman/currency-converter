FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CurrencyConverter.Presentation/CurrencyConverter.Presentation.csproj", "CurrencyConverter.Presentation/"]
COPY ["src/CurrencyConverter.Application/CurrencyConverter.Application.csproj", "CurrencyConverter.Application/"]
COPY ["src/CurrencyConverter.Infrastructure/CurrencyConverter.Infrastructure.csproj", "CurrencyConverter.Infrastructure/"]
COPY Directory.Packages.props .
COPY Directory.Build.props .
RUN dotnet restore "./CurrencyConverter.Presentation/CurrencyConverter.Presentation.csproj"
COPY src/ .
WORKDIR "/src/CurrencyConverter.Presentation"
RUN dotnet build "./CurrencyConverter.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CurrencyConverter.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CurrencyConverter.Presentation.dll"]