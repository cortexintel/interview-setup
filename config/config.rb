require 'forecast_io'
require 'slack-ruby-bot'

# Darksky API
ForecastIO.configure do |config|
    config.api_key = ENV['DARKSKY_API_KEY']
    config.default_params = { units: 'us'}
end

# Slack API
Slack.configure do |config|
    config.token = ENV['SLACK_API_TOKEN']
end
