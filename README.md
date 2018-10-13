# DarkSlackBot
A Ruby Slackbot that returns the weather and the phases of the moon. The commands are as follows:

```
@[bot_name] help
# returns a list of commands for the bot
```

```
@[bot_name] weather now
# reports the weather right now
```

```
@[bot_name] weather tomorrow
# reports the weather tomorrow
```

```
@[bot_name] moon phase
# reports the phase of the moon
```

The `weather now` command will also display alerts if the National Weather Service has issued any.

The bot delivers a morning report at 8:00 am when the weather that day will be significantly different from the weather the day before. A "significant difference" is defined as either a greater than 10 degree Fahrenheit difference between the temperatures on the two days, or if one day had inclement weather while the other did not.

## Usage
To start the bot, `cd` into it and run

```
ruby app.rb
```

Open the Slack app in your browser or via the desktop app and use one of the commands above to communicate with the bot.

To run the morning report cron job:

```
whenever --update-crontab
```

## Running Tests
The tests can be run using the `rspec` command.

## Future Updates
Future iterations of this app will allow for forecasts outside of NYC.
