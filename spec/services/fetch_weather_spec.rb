require 'rails_helper'

RSpec.describe 'FetchWeather' do
  let(:darksky_client) { double('darksky_client') }
  let(:response_body) { double('response_body') }
  let(:response) { double('response', body: response_body) }

  it 'requests weather from darksky api' do
    allow(ParseWeatherResponse).to receive(:call)

    expect(darksky_client).to receive(:forecast).with(12345)
      .and_return(response)

    FetchWeather.call({ time: 12345 }, darksky_client)
  end

  it 'parses the response body from darksky' do
    allow(darksky_client).to receive(:forecast).and_return(response)

    expect(ParseWeatherResponse).to receive(:call).with(response_body, 'now')

    FetchWeather.call({ type: 'now' }, darksky_client)
  end
end
