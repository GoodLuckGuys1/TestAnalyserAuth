FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TestAnalyserAuth/TestAnalyserAuth.csproj", "TestAnalyserAuth/"]
RUN dotnet restore "TestAnalyserAuth/TestAnalyserAuth.csproj"
COPY . .
WORKDIR "/src/TestAnalyserAuth"
RUN dotnet build "TestAnalyserAuth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestAnalyserAuth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestAnalyserAuth.dll"]
