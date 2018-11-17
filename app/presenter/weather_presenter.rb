class WeatherPresenter
  def initialize(weather)
    @weather = weather
  end

  def call
    <<~MESSAGE
    Current Weather: #{weather.summary}
    Temperature: #{weather.temperature}˚F
    Humidity: #{weather.humidity}%
    MESSAGE
  end

  private

  attr_reader :weather
end
