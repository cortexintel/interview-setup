class FetchWeather
  def self.call(*args)
    new(*args).call
  end

  def initialize(args = {}, client = nil)
    @type = args.fetch(:type, nil)
    @time = args.fetch(:time, nil)
    @client = client || DarkskyClient
  end

  def call
    response = client.forecast(time)
    ParseWeatherResponse.call(response.body, type)
  end

  private

  attr_reader :client, :time, :type
end
