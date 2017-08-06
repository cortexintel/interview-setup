# Weather Slack Bot

## Overview 

Slack weather bot that uses DarkSky API to get forecasts. The bot supports location based weather requests ()
To support location based weather requests, we need to persist user locations. 

It currently only supports small set of locations from demonstration purposes, 1000 cities in the United States only. Data source for cities from this Gist: https://gist.github.com/Miserlou/c5cd8364bf9b2420bb29

## Features

* Responds to `weather now`, `weather tomorrow`, and `what should I wear?`
* Responds to `weather now <location>`, `weather tomorrow <location>`
  * <location> needs to be a city state pair, where state is a two letter abbreviation e.g. New York, NY
* User can set location with `set me to <location>`

## Development Setup

* Visual Studio 2017
* Windows 10 (.NET 4.5.2)

## External Dependencies

* DarkSky API -- provides the weather info
* Slack API -- uses RTM inferface 
* SQLite database

## Setup Instructions

1. Create app.config file in deploy folder
```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <startup>
      <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.2" />
  </startup>
  <appSettings>
      <add key="SLACK_API_TOKEN" value="TOKEN" />
      <add key="DARK_SKY_TOKEN" value="TOKEN" />      
  </appSettings>
  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="Newtonsoft.Json" publicKeyToken="30ad4fe6b2a6aeed" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-10.0.0.0" newVersion="10.0.0.0" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>
```
1. Set token for DarkSky API `DARK_SKY_TOKEN` to DarkSky secret in App.config
1. Set token for Slack API bot user `SLACK_API_TOKEN` to Slack bot user secret in App.config
1. Run executable `./DBPreparer.exe`, this creates a `SlackBotDb.db` file that is required for persistence.
1. Copy DB file to deploy folder.
1. Run executable via commandline `./SlackBotPrototype.exe`

## Troubleshooting

* `SQLite.dll` is not found -- quirk with library means that sometimes a full `rebuid solution` is required




