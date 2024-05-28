# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TenkiAme/TenkiAme.csproj", "TenkiAme/"]
RUN dotnet restore "./TenkiAme/TenkiAme.csproj"
COPY . .
WORKDIR "/src/TenkiAme"
RUN dotnet build "./TenkiAme.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish Stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TenkiAme.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Nginx Stage
FROM nginx:latest AS nginx
COPY --from=publish /app/publish /usr/share/nginx/html
EXPOSE 80

# Final Stage
FROM nginx AS final
