FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine
RUN apk add openssh-server fish

ENV SFTP_PASSWORD ""
ENV PUBLIC_KEYS ""

COPY deploy/ /app
COPY scripts/entrypoint.sh /

CMD /entrypoint.sh

