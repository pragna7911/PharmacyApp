# Use the official .NET 8 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
#EXPOSE 80
EXPOSE 8080
EXPOSE 8081

# Copy the published files from Jenkins pipeline
COPY ./publish /app

# Set the entry point
ENTRYPOINT ["dotnet", "Wellgistics.Pharmacy.api.dll"]
