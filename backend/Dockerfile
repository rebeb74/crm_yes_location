FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["YesLocation.Api/YesLocation.Api.csproj", "YesLocation.Api/"]
RUN dotnet restore "YesLocation.Api/YesLocation.Api.csproj"
COPY . .
WORKDIR "/src/YesLocation.Api"
RUN dotnet build "YesLocation.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YesLocation.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "YesLocation.Api.dll"]