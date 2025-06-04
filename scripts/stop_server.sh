#!/bin/bash
set -e

echo "Stopping ASP.NET Core Web API...."

PID=$(pgrep -f "Content.Manager.Core.WebApi.dll")

if [ -n "$PID" ]; then
    echo "Stopping process with PID: $PID"
    kill $PID

    # Wait up to 10 seconds for graceful shutdown
    for i in {1..10}; do
        if ! ps -p $PID > /dev/null; then
            echo "Process stopped gracefully."
            # Remove PID file if exists
            [ -f /var/run/myapp.pid ] && rm /var/run/myapp.pid
            exit 0
        fi
        sleep 1
    done

    echo "Process still running, force killing PID $PID"
    kill -9 $PID

    # Cleanup PID file
    [ -f /var/run/myapp.pid ] && rm /var/run/myapp.pid
else
    echo "No running process found for Content.Manager.Core.WebApi.dll"
fi
