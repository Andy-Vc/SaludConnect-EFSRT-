FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Business Layer/API/API.csproj", "Business Layer/API/"]
COPY ["Business Layer/Logic/Logic.csproj", "Business Layer/Logic/"]
COPY ["Data Layer/Data/Data.csproj", "Data Layer/Data/"]
COPY ["Data Layer/Data/Interface/Interface.csproj", "Data Layer/Data/Interface/"]
COPY ["Data Layer/Data/Repository/Repository.csproj", "Data Layer/Data/Repository/"]
COPY ["Data Layer/Models/Models.csproj", "Data Layer/Models/"]
COPY ["Data Layer/Models/DTO/DTO.csproj", "Data Layer/Models/DTO/"]
COPY ["Presentation Layer/Web/Web.csproj", "Presentation Layer/Web/"]

RUN dotnet restore "Business Layer/API/API.csproj"

COPY . .
WORKDIR "/src/Business Layer/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]