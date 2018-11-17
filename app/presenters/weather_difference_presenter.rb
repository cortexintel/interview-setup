class WeatherDifferencePresenter
  def initialize(weather_difference)
    @weather_difference = weather_difference
  end

  def call
    <<~MESSAGE
    ALERT: Significant Change In Weather From Yesterday

    Change in temperature high: #{weather_difference.temperature_high.round(2)}˚F
    Change in temperature low: #{weather_difference.temperature_low.round(2)}˚F
    Change in humidity: #{weather_difference.humidity.round(2)}%
    Change in precipitation: #{weather_difference.precip_probability.round(2)}%
    MESSAGE
  end

  private

  attr_reader :weather_difference
end
