#!/bin/sh

# Path to the .env file
ENV_FILE="/usr/share/nginx/html/.env"

# Create or clear the .env file
> $ENV_FILE

# Loop through environment variables prefixed with VITE_
for i in $(env | grep VITE_)
do
    key=$(echo $i | cut -d '=' -f 1)
    value=$(echo $i | cut -d '=' -f 2-)

    echo "Adding $key to $ENV_FILE"

    # Add the environment variable to the .env file
    echo "${key}=${value}" >> $ENV_FILE
done

exec "$@"
