# docker build -t nergy101/pcnotes:v1 .
# docker run -d -p 80:5001 -e nergy101/pcnotes:v1


# Use the official .NET Core SDK image as a base
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# Set the working directory to /app
WORKDIR /app

# Copy the .csproj file to the current directory
# Copy the remaining source code to the current directory
COPY . .

# Restore any NuGet packages
RUN dotnet restore

# Publish the application to a directory named "out"
RUN dotnet publish -c Release -o out

# Use the official ASP.NET Core runtime image as a base
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS runtime

# Set the working directory to /app
WORKDIR /app

# Copy the published output from the build image to the current directory
COPY --from=build /app/out .

# Expose port 5001 to the outside world
EXPOSE 5001

# Start the application
ENTRYPOINT ["dotnet", "PCNotes.Server.dll"]