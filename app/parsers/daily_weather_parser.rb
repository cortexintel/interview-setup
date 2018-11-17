class DailyWeatherParser
  def initialize(body)
    @body = JSON.parse(body)
  end

  def time
    weather['time']
  end

  def summary
    weather['summary']
  end

  def temperature_high
    weather['temperatureHigh']
  end

  def temperature_low
    weather['temperatureLow']
  end

  def humidity
    weather['humidity']
  end

  def precip_probability
    weather['precipProbability']
  end

  private

  attr_reader :body

  def weather
    body['daily']['data'][0]
  end
end
