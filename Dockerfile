# BApild Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /source 
COPY . .

RUN dotnet restore "./MyDemoProjects.Api/MyDemoProjects.Api.csproj" --disable-parallel
RUN dotnet publish "./MyDemoProjects.Api/MyDemoProjects.Api.csproj" -c release -o /app --no-restore


# Serve Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000

ENTRYPOINT ["dotnet","MyDemoProjects.Api.dll"]


# Create image
#docker build --rm -t myprojects-dev/api:latest .
#docker build --rm -t myprojects-dev/Ui:latest .
#docker run --rm -p 5000:5000 -p 5001:5001 -e ASPNETCORE_HTTP_PORT=https://+:5001 -e ASPNETCORE_URLS=http://+:5000 myprojects-dev/api

#docker container run -itd -p 5000:5000 myprojects-dev/api -e ASPNETCORE_URLS="http://*:5000" ASPNETCORE_ENVIRONMENT=Development
#docker run -d -p 5000:5000 myprojects-dev/api

#docker ps --format "table {{.ID}}\t{{.Status}}\t{{.Ports}}\t{{.Names}}"

#docker run --rm -p 5000:5000  -e ASPNETCORE_URLS=http://+:5000 -e ASPNETCORE_ENVIRONMENT=Development myprojects-dev/api

#ENV ASPNETCORE_URLS=http://+:5000
#ENV ASPNETCORE_ENVIRONMENT=Development