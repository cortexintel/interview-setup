class DarkskyClient
  include HTTParty
  base_uri "https://api.darksky.net/forecast/#{ENV['DARKSKY_API_KEY']}"

  def self.forecast(*args)
    new.forecast(*args)
  end

  def initialize
    @latitude = '40.7128'
    @longitude = '74.0060'
  end

  def forecast(epoch_time = nil)
    if epoch_time.nil?
      self.class.get("/#{coordinates}")
    else
      self.class.get("/#{coordinates},#{epoch_time}")
    end
  end

  private

  def coordinates
    "#{@latitude},#{@longitude}"
  end
end
