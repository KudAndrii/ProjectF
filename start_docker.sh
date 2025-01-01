#!/bin/bash

# Source the .env file to get $SECRETS_FILE_PROJECT_F
source .env

# Load secrets from secrets.json
if [[ -f "$SECRETS_FILE_PROJECT_F" ]]; then
    OMDB_API_KEY=$(jq -r '.OmdbConfiguration.ApiKey' "$SECRETS_FILE_PROJECT_F")
    export OMDB_API_KEY
else
    echo "Secrets file not found at $SECRETS_FILE_PROJECT_F"
    exit 1
fi

# Drop existing containers and images related to the yaml file
docker-compose down --rmi local

# Run named docker-compose with loaded variables
docker-compose -p project-f up --build -d