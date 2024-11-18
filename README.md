# TenkiAme

TenkiAme is a weather forecasting application that integrates with various APIs to provide weather and UV index information. You can access it at [tenkiame.org](https://tenkiame.org).

## Table of Contents

- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Usage](#usage)
- [Deployment](#deployment)
- [Licence](#licence)

## Features

- Fetches weather data from the MetOcean API, SunriseSunset API, NIWA API
- Uses Serilog for logging
- Serves the application using Nginx

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio](https://visualstudio.microsoft.com/downloads/)
- [Azure Account](https://azure.microsoft.com/en-us/free/)

## Installation

1. **Clone the repository:**

```sh
git clone https://github.com/your-username/TenkiAme.git
```

## Usage
1. Restore dependencies
```sh
dotnet restore
```
2. Build the project
```sh
dotnet build
```
3. Run the application locally
```sh
dotnet run --project TenkiAme
```
Or just open up the solution in Visual Studio and navigate the GUI.

## Deployment
### Using Azure VM
1. Set up your Azure VM and install Nginx.
2. Deploy your application to the Azure VM.
3. Configure Nginx to reverse proxy to your application:
```nginx
server {
    listen 80;

    location / {
        proxy_pass http://localhost:5000;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```
4. Reload Nginx:
```sh
sudo nginx -s reload
```
5. Set up your application as a service to ensure it runs continuously. Create a service file at /etc/systemd/system/tenkiame.service with the following content:
```ini
[Unit]
Description=TenkiAme

[Service]
WorkingDirectory=/var/www/app/
ExecStart=/usr/bin/dotnet /var/www/app/TenkiAme.dll
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=dotnet-TenkiAme
User=$USERNAMEHERE$
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```
6. Start and enable the service:
```sh
sudo systemctl start tenkiame
sudo systemctl enable tenkiame
```

## Licence
This project is licensed under the MIT License. See the LICENSE tab for details.
