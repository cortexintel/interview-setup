class WeatherPresenter
  def initialize(weather)
    @weather = weather
  end

  def call
    <<~MESSAGE
    Weather for #{date}: #{weather.summary}
    #{temperature}
    Humidity: #{weather.humidity}%
    Precipitation: #{weather.precip_probability}%
    MESSAGE
  end

  private

  def temperature
    if weather.respond_to?(:temperature)
      "Temperature: #{weather.temperature}˚F"
    else
      [
        "Temperature High: #{weather.temperature_high}˚F",
        "Temperature Low: #{weather.temperature_low}˚F"
      ].join("\n")
    end
  end

  def date
    Time.at(weather.time).strftime('%A, %b %d')
  end

  attr_reader :weather
end
