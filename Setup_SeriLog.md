# Set up SeriLog

# Install packages
- `Install-Package Serilog.AspNetCore`
  - This will include following transitive packages.
```Install-Package Serilog
    Install-Package Serilog.Settings.Configuration
    Install-Package Serilog.Sinks.Console
    Install-Package Serilog.Sinks.File
```

# Configuration:
- Open appsettings.json and add the following to make use of the Console and File sinks.
```json
{
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
    "MinimumLevel": "Debug",
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/applog-.txt",
          "fileSizeLimitBytes": 16000000,
          "retainedFileCountLimit": 31,
          "rollOnFileSizeLimit": true,
          "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss,fff} {Level:u4}] {Message:lj} ({SourceContext} {RequestId}){NewLine}{Exception}"
        }
      }
    ],
    "Enrich": [ "FromLogContext" ],
    "Properties": {
      "ApplicationName": "TenkiAme"
    }
  }
}
```

# Setting up Program.cs
