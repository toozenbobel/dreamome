FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["VoiceService/VoiceService.csproj", "VoiceService/"]
RUN dotnet restore "VoiceService/VoiceService.csproj"
COPY . .
WORKDIR "/src/VoiceService"
RUN dotnet build "VoiceService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VoiceService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VoiceService.dll"]
