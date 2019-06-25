#FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build-env
#WORKDIR /app

# Copy csproj and restore as distinct layers
#COPY *.csproj ./

# Copy everything else and build
# COPY . ./
#RUN dotnet restore
#RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
#COPY --from=build-env /app/GBM.Portfolio.API/out .
COPY GBM.Portfolio.API/out .
ENTRYPOINT ["dotnet", "GBM.Portfolio.API.dll"]