# syntax=docker/dockerfile:1

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build

COPY . /source

WORKDIR /source/EnergyCounter

# This is the architecture you’re building for, which is passed in by the builder.
ARG TARGETARCH

# Define a default architecture in case TARGETARCH is not provided.
ENV ARCH=x64

# Set ARCH based on TARGETARCH if it is "amd64".
RUN if [ "$TARGETARCH" = "amd64" ]; then export ARCH=x64; fi

# Build the application.
# Leverage a cache mount to /root/.nuget/packages
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -a $ARCH --use-current-runtime --self-contained false -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Copy everything needed to run the app from the "build" stage.
COPY --from=build /app .

ENTRYPOINT ["dotnet", "EnergyCounter.dll"]
