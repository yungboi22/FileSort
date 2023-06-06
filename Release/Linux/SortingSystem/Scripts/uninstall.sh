#!/bin/bash

sudo rm /etc/systemd/system/sortingsystem.service
sudo rm /opt/SortingSystem/ -R
sudo systemctl daemon-reload
