class ParseWeatherResponse
  def self.call(*args)
    new(*args).call
  end

  def initialize(response_body, type = nil)
    @response_body = response_body
    @type = type.downcase
  end

  def call
    case type
    when 'now'
      CurrentWeatherParser.new(response_body)
    when 'tomorrow'
      TomorrowsWeatherParser.new(response_body)
    else
      WeatherParser.new(response_body)
    end
  end

  private

  attr_reader :response_body, :type
end
