# BApild Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS bApild
WORKDIR /source 
COPY . .

RUN dotnet restore "./MyDemoProjects.Api/MyDemoProjects.Api.csproj" --disable-parallel
RUN dotnet publish "./MyDemoProjects.Api/MyDemoProjects.Api.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY --from=bApild /app ./

EXPOSE 5000

ENTRYPOINT ["dotnet","MyDemoProjects.Api.dll"]


# Create image
#docker bApild --rm -t myprojects-dev/api:latest .
#docker bApild --rm -t myprojects-dev/Api:latest .
#docker run --rm -p 5000:5000 -p 5001:5001 -e ASPNETCORE_HTTP_PORT=https://+:5001 -e ASPNETCORE_URLS=http://+:5000 myprojects-dev/api

#docker container run -itd -p 5000:5000 myprojects-dev/api -e ASPNETCORE_URLS="http://*:5000" ASPNETCORE_ENVIRONMENT=Development
#docker run -d -p 5000:5000 myprojects-dev/api