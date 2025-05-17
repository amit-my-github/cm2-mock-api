#!/bin/bash
echo "Starting ASP.NET Core Web API..."
nohup dotnet /var/www/myapp/Content.Manager.Core.WebApi.dll > /dev/null 2>&1 &
