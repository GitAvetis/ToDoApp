#!/bin/bash
docker compose down 2>/dev/null
docker compose up --build -d
sleep 5
xdg-open http://localhost:8080/Dashboard 2>/dev/null || open http://localhost:8080/Dashboard 2>/dev/null || start http://localhost:8080/Dashboard 2>/dev/null
docker compose logs -f web