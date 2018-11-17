require 'rails_helper'

RSpec.describe 'ParseWeatherResponse' do
  it 'uses the correct parser for now type' do
    expect(CurrentWeatherParser).to receive(:new)
      .with('fake_response_body')

    ParseWeatherResponse.call('fake_response_body', 'now')
  end

  it 'uses the correct parser for tomorrow type' do
    expect(TomorrowsWeatherParser).to receive(:new)
      .with('fake_response_body')

    ParseWeatherResponse.call('fake_response_body', 'tomorrow')
  end
end
