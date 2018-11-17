class DarkskyClient
  include HTTParty
  base_uri "https://api.darksky.net/forecast/#{ENV['DARKSKY_API_KEY']}"

  def self.forecast(*args)
    new(*args).forecast
  end

  def initialize(api_key = nil)
    @latitude = '40.7128'
    @longitude = '74.0060'
  end

  def forecast(epoch_time = nil)
    if epoch_time.present?
      self.class.get("/#{coordinates},#{epoch_time}")
    else
      self.class.get("/#{coordinates}")
    end
  end

  private

  def coordinates
    "#{@latitude},#{@longitude}"
  end
end
