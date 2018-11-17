require 'rails_helper'

RSpec.describe 'FetchCurrentWeather' do
  it 'requests the current weather from darksky api' do
    darksky_client = double('darksky_client')

    expect(darksky_client).to receive(:forecast)

    FetchCurrentWeather.call(darksky_client)
  end
end
