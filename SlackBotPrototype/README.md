# Weather Slack Bot

## Overview 

Slack weather bot that uses DarkSky API to get forecasts. If bot is running, it'll notify every morning of the current weather if the today's forecast is different from yesterday's. The bot is currently intended to run a console executable but can be modified to run as a windows service.

## Development Setup

* Visual Studio 2017
* Windows 10 (.NET 4.5.2)

## External Dependencies

* DarkSky API -- provides the weather info
* Slack API -- uses RTM inferface 
* Stanford NLP library -- used to tokenization and labeling of messages
  * Download model data package from http://nlp.stanford.edu/software/stanford-postagger-full-2016-10-31.zip

## Setup Instructioons

* Download Stanford NLP model data from http://nlp.stanford.edu/software/stanford-postagger-full-2016-10-31.zip
* Extract package contents
* Set environment variable for Stanford NLP model folder `STANFORD_NLP_FOLDER` to the location of the extracted package
* Set environment variable for DarkSky API `DARK_SKY_TOKEN` to DarkSky secret
* Set environment variable for Slack API bot user `SLACK_API_TOKEN` to Slack bot user secret
* Run executable via commandline `./SlackBotProtoType.exe`





