class DifferentiateWeather
  def self.call
    new.call
  end

  def call
    differences
  end

  private

  WeatherDifferences = Struct.new(:temperature_high, :temperature_low :humidity, :precip_probability) do
    def significant?
      temperature_high.abs > 10     ||
      temperature_low.abs > 10      ||
      humidity.abs    > 0.25        ||
      precip_probability.abs > 0.25
    end
  end

  def differences
    WeatherDifferences.new(
      weather_today.temperature_high - weather_yesterday.temperature_high,
      weather_today.temperature_low - weather_yesterday.temperature_low,
      weather_today.humidity - weather_yesterday.humidity,
      weather_today.precip_probability - weather_yesterday.precip_probability
    )
  end

  def weather_today
    @weather_today ||= begin
      today = Date.today.to_time.to_i
      FetchWeather.call(type: 'today', time: today)
    end
  end

  def weather_yesterday
    @weather_yesterday ||= begin
      yesterday = 1.day.ago.to_i
      FetchWeather.call(type: 'today', time: yesterday)
    end
  end
end
