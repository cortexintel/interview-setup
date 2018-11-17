class WeatherPresenter
  def initialize(weather)
    @weather = weather
  end

  def call
    <<~MESSAGE
    Weather for #{date}: #{weather.summary}
    Temperature: #{weather.temperature}˚F
    Humidity: #{weather.humidity}%
    Precipitation: #{weather.precip_probability}%
    MESSAGE
  end

  private

  def date
    Time.at(weather.time).strftime('%A, %b %d')
  end

  attr_reader :weather
end
