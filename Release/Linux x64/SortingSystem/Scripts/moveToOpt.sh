#!/bin/bash

sudo mv ../../SortingSystem /opt/  
sudo cp sortingsystem.service /etc/systemd/system/sortingsystem.service
sudo systemctl daemon-reload
