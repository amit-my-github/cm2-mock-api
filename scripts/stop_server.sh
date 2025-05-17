#!/bin/bash

echo "Stopping ASP.NET Core Web API..."

# Find the process ID of your app
PID=$(pgrep -f "Content.Manager.Core.WebApi.dll")

if [ -n "$PID" ]; then
    echo "Stopping process with PID: $PID"
    kill $PID

    # Wait for process to stop
    sleep 5

    # Force kill if still running
    if ps -p $PID > /dev/null; then
        echo "Force killing process $PID"
        kill -9 $PID
    fi
else
    echo "No running process found for Content.Manager.Core.WebApi.dll"
fi
