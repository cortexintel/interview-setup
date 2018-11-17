require 'rails_helper'

RSpec.describe 'Slack Event', type: :request do
  let(:headers) do
    { "Content-Type" => "application/json" }
  end

  context 'Request url verification' do
    let(:params) do
      {
        token: '12345',
        challenge: '555',
        type: 'url_verification'
      }.to_json
    end

    it 'returns 200' do
      post '/api', params: params, headers: headers

      expect(response).to have_http_status('200')
    end

    it 'returns the challenge token' do
      post '/api', params: params, headers: headers

      expect(response.body).to eq('555')
    end

    it 'is a plain text response' do
      post '/api', params: params, headers: headers

      expect(response.content_type).to eq('text/plain')
    end
  end

  context 'Invalid request' do
    it 'returns 406 if event type is unacceptable' do
      post '/api', params: {
        event:
          {
            type: 'message',
            text: 'Weather now',
            channel: 'cool channel'
          }
        }.to_json,
        headers: headers

      expect(response).to have_http_status('406')
    end

    it 'returns 406 if text is unacceptable' do
      post '/api', params: {
        event:
          {
            type: 'app_mention',
            text: 'Weather next friday',
            channel: 'cool channel'
          }
        }.to_json,
        headers: headers

      expect(response).to have_http_status('406')
    end
  end

  it 'posts a message to slack with current weather', vcr: { record: :once } do
    expect(PostSlackMessage).to receive(:call).with(
      "Weather for Friday, Nov 16: Clear\nTemperature: 0.84˚F\nHumidity: 0.34%\nPrecipitation: 0%\n",
      "cool channel"
    )

    post '/api', params: {
      event:
        {
          type: 'app_mention',
          text: 'Weather now',
          channel: 'cool channel'
        }
      }.to_json,
      headers: headers
  end

  it 'posts a message to slack with weather for tomorrow', vcr: { record: :once } do
    expect(PostSlackMessage).to receive(:call).with(
      "Weather for Saturday, Nov 17: Mostly cloudy until afternoon.\nTemperature High: 21.7˚F\nTemperature Low: 5.3˚F\nHumidity: 0.69%\nPrecipitation: 0%\n",
      "cool channel"
    )

    post '/api', params: {
      event:
        {
          type: 'app_mention',
          text: 'Weather tomorrow',
          channel: 'cool channel'
        }
      }.to_json,
      headers: headers
  end
end
