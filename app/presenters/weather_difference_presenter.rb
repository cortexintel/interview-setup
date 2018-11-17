class WeatherDifferencePresenter
  def initialize(weather_difference)
    @weather_difference = weather_difference
  end

  def call
    <<~MESSAGE
    ALERT: Significant Change In Weather From Yesterday

    Change in temperature high: #{weather_difference.temperature_high}˚F
    Change in temperature low: #{weather_difference.temperature_low}˚F
    Change in humidity: #{weather_difference.humidity}%
    Change in precipitation: #{weather_difference.precip_probability}%
    MESSAGE
  end

  private

  attr_reader :weather_difference
end
