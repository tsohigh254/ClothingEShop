# Use the official .NET 8 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj file and restore dependencies
COPY ["ClothingEShop/ClothingEShop.csproj", "ClothingEShop/"]
RUN dotnet restore "ClothingEShop/ClothingEShop.csproj"

# Copy the rest of the source code
COPY ClothingEShop/ ClothingEShop/

# Build the application
WORKDIR /src/ClothingEShop
RUN dotnet build "ClothingEShop.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "ClothingEShop.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET 8 runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install PostgreSQL client tools (optional, for debugging)
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*

# Copy the published app
COPY --from=publish /app/publish .

# Create logs directory
RUN mkdir -p /app/logs

# Expose port 10000 for Render
EXPOSE 10000

# Set environment variable for port
ENV ASPNETCORE_URLS=http://+:10000

# Set the entry point
ENTRYPOINT ["dotnet", "ClothingEShop.dll"]