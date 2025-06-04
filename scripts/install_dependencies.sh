#!/bin/bash
set -e

echo "Installing .NET 7 runtime...."

# Import Microsoft package signing key and repo
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

sudo apt-get update

# Install .NET runtime 7.0 (for running apps)
sudo apt-get install -y dotnet-runtime-7.0

echo ".NET 7 runtime installed."

# Ensure app directory exists
mkdir -p /var/www/myapp

echo "Dependencies installed successfully."
