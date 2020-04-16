FROM mcr.microsoft.com/dotnet/core/runtime:3.1

COPY Evo2HomeMqttNet/bin/Release/netcoreapp3.1/publish/ app/

ENTRYPOINT ["dotnet", "app/Evo2HomeMqttNet.dll"]