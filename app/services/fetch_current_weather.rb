class FetchCurrentWeather

  def self.call(darksky_client = nil)
    new(darksky_client).call
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
