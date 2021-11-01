FROM mcr.microsoft.com/mssql/server:2019-latest AS base

USER root

ENV ACCEPT_EULA=Y
ENV MSSQL_TCP_PORT=1433
EXPOSE 1433

WORKDIR /src
COPY ./docker/build-script/SetupDatabases.sh /docker/SetupDatabases.sh
RUN chmod -R 777 /docker/.

ENTRYPOINT /docker/SetupDatabases.sh & /opt/mssql/bin/sqlservr
