#FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build-env
#WORKDIR /app

# Copy csproj and restore as distinct layers
#COPY *.csproj ./

# Copy everything else and build
# COPY . ./
#RUN dotnet restore
#RUN dotnet publish -c Release -o out

# Build runtime image
#FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
FROM dotnetcorejava
WORKDIR /app
#COPY --from=build-env /app/GBM.Portfolio.API/out .
COPY dynamodb ./dynamodb
COPY GBM.Portfolio.API/out .
COPY start.sh /opt/bin/
#RUN apt-get update
#RUN apt-get --assume-yes install default-jre
#RUN java -Djava.library.path=./dynamodb/DynamoDBLocal_lib -jar ./dynamodb/DynamoDBLocal.jar -sharedDb
#ENTRYPOINT ["dotnet", "GBM.Portfolio.API.dll"]
ENTRYPOINT ["/opt/bin/start.sh"]