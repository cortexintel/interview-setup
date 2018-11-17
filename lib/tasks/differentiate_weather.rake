desc "Differentiates weather of today from yesterday and posts results to Slack"
task differentiate_weather: :environment  do
  weather_differences = DifferentiateWeather.call
  if weather_differences.significant?
    message = WeatherDifferencePresenter.new(weather_difference).call
    PostSlackMessage.call(message, '#general')
  end
end
