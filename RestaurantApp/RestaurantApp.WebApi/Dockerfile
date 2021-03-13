#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["RestaurantApp.WebApi/RestaurantApp.WebApi.csproj", "RestaurantApp.WebApi/"]
COPY ["RestaurantApp.BusinessService/RestaurantApp.BusinessService.csproj", "RestaurantApp.BusinessService/"]
COPY ["RestaurantApp.Common/RestaurantApp.Common.csproj", "RestaurantApp.Common/"]
COPY ["RestaurantApp.DataServices/RestaurantApp.DataServices.csproj", "RestaurantApp.DataServices/"]
COPY ["RestaurantApp.DataModel/RestaurantApp.DataModel.csproj", "RestaurantApp.DataModel/"]
COPY ["RestaurantApp.DataContracts/RestaurantApp.DataContracts.csproj", "RestaurantApp.DataContracts/"]
COPY ["RestaurantApp.BusinessEntities/RestaurantApp.BusinessEntities.csproj", "RestaurantApp.BusinessEntities/"]
RUN dotnet restore "RestaurantApp.WebApi/RestaurantApp.WebApi.csproj"
COPY . .
WORKDIR "/src/RestaurantApp.WebApi"
RUN dotnet build "RestaurantApp.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestaurantApp.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestaurantApp.WebApi.dll"]