# Set the base image to the official .NET 6 SDK image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory to /app
WORKDIR /app

# Copy the console app project file to the container and restore dependencies
COPY DotaBot.csproj .
RUN dotnet restore

# Copy the entire console app project to the container
COPY . .

# Build the console app
RUN dotnet publish -c Release -o out

# Create the final runtime image
FROM mcr.microsoft.com/dotnet/runtime:6.0 AS runtime

# Set the working directory to /app and copy the console app from the build stage
WORKDIR /app
COPY --from=build /app/out .

# Start the console app when the container starts
ENTRYPOINT ["dotnet", "DotaBot.dll"]