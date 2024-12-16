#!/bin/bash

if [ $# -eq 0 ]; then
    echo "Usage: ./run-sdk.sh <commande dotnet>"
    echo "Exemple: ./run-sdk.sh new webapi"
    exit 1
fi

docker-compose run --rm sdk $@
