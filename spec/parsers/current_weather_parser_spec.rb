require 'spec_helper'

RSpec.describe 'CurrentWeatherParser' do

  let(:body) do
    <<~JSON
    {
      "latitude": 42.3601,
      "longitude": -71.0589,
      "timezone": "America/New_York",
      "currently": {
        "time": 1509993277,
        "summary": "Drizzle",
        "icon": "rain",
        "nearestStormDistance": 0,
        "precipIntensity": 0.0089,
        "precipIntensityError": 0.0046,
        "precipProbability": 0.9,
        "precipType": "rain",
        "temperature": 66.1,
        "apparentTemperature": 66.31,
        "dewPoint": 60.77,
        "humidity": 0.83,
        "pressure": 1010.34,
        "windSpeed": 5.59,
        "windGust": 12.03,
        "windBearing": 246,
        "cloudCover": 0.7,
        "uvIndex": 1,
        "visibility": 9.84,
        "ozone": 267.44
      }
    }
    JSON
  end

  it 'parses the rsponse body' do
    result = CurrentWeatherParser.new(body)

    expect(result).to have_attributes(
      time: 1509993277,
      summary: 'Drizzle',
      temperature: 66.1,
      humidity: 0.83,
      wind_speed: 5.59,
      cloud_cover: 0.7
    )
  end
end
