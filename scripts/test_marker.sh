#!/bin/bash
# Test script for Ubuntu CodeDeploy
echo "[Ubuntu] Test script executed at $(date)" | sudo tee /tmp/deployment_test.log
echo "Current directory: $(pwd)" | sudo tee -a /tmp/deployment_test.log
echo "Script path: $(realpath $0)" | sudo tee -a /tmp/deployment_test.log
sudo touch /tmp/deployment_test_success  # Create success marker file