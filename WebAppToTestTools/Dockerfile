FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["WebAppToTestTools/WebAppToTestTools.csproj", "WebAppToTestTools/"]
RUN dotnet restore "WebAppToTestTools/WebAppToTestTools.csproj"
COPY . .
WORKDIR "/src/WebAppToTestTools"
RUN dotnet build "WebAppToTestTools.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WebAppToTestTools.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebAppToTestTools.dll"]