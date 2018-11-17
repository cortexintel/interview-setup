class FetchWeather
  def self.call(*args)
    new(*args).call
  end

  def initialize(darksky_client = nil)
    @darksky_client = darksky_client || DarkskyClient
  end

  def call
    darksky_client.forecast
  end

  private

  attr_reader :darksky_client
end
