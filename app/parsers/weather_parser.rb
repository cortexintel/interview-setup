class WeatherParser
  def initialize(body)
    @body = JSON.parse(body)
  end

  def time
    weather['time']
  end

  def summary
    weather['summary']
  end

  def temperature
    weather['temperature']
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
