# 🏗 Stage 1: Build the app using .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything in the current folder (project root) -> copy All files and folders inside the SmartCacheManagementSystem into /src
COPY . .

# Publish the project using the .csproj file in this folder
RUN dotnet publish SmartCacheManagementSystem.csproj -c Release -o /app/publish

# 🏃‍♂️ Stage 2: Use ASP.NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output from build stage
# Go into the build stage container’s /app/publish folder, and copy everything into the current directory (.) of the final image, which is /app due to WORKDIR /app.
COPY --from=build /app/publish .

# Run the application
ENTRYPOINT ["dotnet", "SmartCacheManagementSystem.dll"]

