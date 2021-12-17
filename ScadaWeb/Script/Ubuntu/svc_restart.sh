#!/bin/bash
if [ "$(systemctl is-active scadaweb)" == "active" ]; then
    echo "Reload Webstation configration"
    cd "$(dirname "$0")"
    touch ./cmd/webreload
    wget -q -O - "http://localhost:5000/ConfigReload"
    rm -f ./cmd/webreload
else
    echo "Restart Webstation service"
    systemctl restart scadaweb
fi
