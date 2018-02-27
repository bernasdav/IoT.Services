FROM microsoft/dotnet:2.0-sdk AS build-env
WORKDIR /app/MQTTClient

# copy csproj and restore as distinct layers
COPY /MQTTClient/*.csproj /app/MQTTClient/
COPY /IoT.Services.Contracts/*.csproj /app/IoT.Services.Contracts/
COPY /IoT.Services.EventBus/*.csproj /app/IoT.Services.Contracts/
RUN dotnet restore

# copy everything else and build
COPY /MQTTClient/. /app/MQTTClient/
COPY /IoT.Services.Contracts/. /app/IoT.Services.Contracts/
COPY /IoT.Services.EventBus/. /app/IoT.Services.EventBus/
RUN dotnet publish -c Release -o out

# build runtime image
FROM microsoft/dotnet:2.0-runtime 
WORKDIR /app/MQTTClient
COPY --from=build-env /app/MQTTClient/out ./
ENTRYPOINT ["dotnet", "IoT.Services.MqttServices.dll"]
