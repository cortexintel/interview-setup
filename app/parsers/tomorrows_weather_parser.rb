class TomorrowsWeatherParser
  def initialize(body)
    @body = JSON.parse(body)
  end

  def time
    tomorrows_weather['time']
  end

  def summary
    tomorrows_weather['summary']
  end

  def temperature
    tomorrows_weather['temperature']
  end

  def humidity
    tomorrows_weather['humidity']
  end

  def precip_probability
    tomorrows_weather['precipProbability']
  end

  private

  attr_reader :body

  def tomorrows_weather
    body['daily']['data'][1]
  end
end
