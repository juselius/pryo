FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
RUN apk add fish

COPY deploy/ /app
COPY scripts/entrypoint.sh /

CMD /entrypoint.sh

