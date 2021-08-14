#!/bin/zsh

docker build -t switch_native_devkit . && docker-compose up
cp HvcProxy/HvcProxy.kip ../AnchorNX