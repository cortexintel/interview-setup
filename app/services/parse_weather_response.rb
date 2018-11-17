class ParseWeatherResponse

  def self.call(*args)
    new(*args).call
  end

  def initialize(weather_response, type)
    @weather_response = weather_response
    @type = type.downcase
  end

  def call
    case type
    when 'now'
      CurrentWeatherParser.new(weather_response)
    when 'tomorrow'
      TomorrowsWeatherParser.new(weather_response)
    end
  end

  private

  attr_reader :weather_response, :type
end
