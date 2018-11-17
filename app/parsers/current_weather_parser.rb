class CurrentWeatherParser
  def initialize(body)
    @body = JSON.parse(body)
  end

  def time
    current_weather['time']
  end

  def summary
    current_weather['summary']
  end

  def temperature
    current_weather['temperature']
  end

  def humidity
    current_weather['humidity']
  end

  def precip_probability
    current_weather['precipProbability']
  end

  private

  attr_reader :body

  def current_weather
    body['currently']
  end
end
