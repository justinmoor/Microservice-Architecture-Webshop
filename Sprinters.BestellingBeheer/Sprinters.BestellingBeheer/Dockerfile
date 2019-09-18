FROM microsoft/dotnet:2.2-aspnetcore-runtime
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-./obj/docker/publish} .
ENTRYPOINT [ "dotnet", "Sprinters.BestellingBeheer.dll" ]