# weather-bot-api

This is the api that serves the weather bot. It uses Rails 5 api.

It has been deployed to heroku. The endpoint that handles the Slack Bot event requests is:
```
https://weather-slack-bot-api.herokuapp.com/api
```

## Usage

On Slack API for the `thomasc-bot` app:
1) Enable events
2) Add the above mentioned url as the event request url
3) Subscribe to `app_mention` Bot Event

In the Slack Application
1) Invite `@thomasc-bot` to a Slack channel
2) Type the following to interact with the bot:
```
@thomasc-bot weather now          # for current weather
@thoamsc-bot weather tomorrow # for tomorrow's forecast
```

In addition to the above a daily task is scheduled to run on Heroku that diffs yesterday's weather to today's and posts the results to the `#general` channel if there is a "significant" change.

## Testing

This app is tested with rspec. To run tests:
```
$ bundle exec rspec
```
