#!/bin/bash
APP_PATH="/var/www/myapp/Content.Manager.Core.WebApi.dll"
LOG_FILE="/var/log/myapp.log"
PID_FILE="/var/run/myapp.pid"

echo "Starting ASP.NET Core Web API...."
echo "Running as: $(whoami)"
if [ -f "$PID_FILE" ] && kill -0 $(cat "$PID_FILE") 2>/dev/null; then
    echo "Application is already running with PID $(cat $PID_FILE)."
    exit 0
fi

nohup dotnet "$APP_PATH" > "$LOG_FILE" 2>&1 &

echo $! > "$PID_FILE"
echo "Application started with PID $(cat $PID_FILE). Logs at $LOG_FILE"
