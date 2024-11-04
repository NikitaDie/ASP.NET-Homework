FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Homework_CRUD_Scafolding_04/Homework_CRUD_Scafolding_04.csproj", "Homework_CRUD_Scafolding_04/"]
RUN dotnet restore "Homework_CRUD_Scafolding_04/Homework_CRUD_Scafolding_04.csproj"
COPY . .
WORKDIR "/src/Homework_CRUD_Scafolding_04"
RUN dotnet build "Homework_CRUD_Scafolding_04.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Homework_CRUD_Scafolding_04.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Homework_CRUD_Scafolding_04.dll"]
