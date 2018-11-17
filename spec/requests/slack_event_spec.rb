require 'rails_helper'

RSpec.describe 'Slack Event', type: :request do

  context 'Invalid request' do
    it 'returns 406 if event type is unacceptable' do
      post '/api', params: {
        event:
          {
            type: 'message',
            text: 'Weather now',
            channel: 'cool channel'
          }
        }

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
        }

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
      }
  end

  it 'posts a message to slack with weather for tomorrow', vcr: { record: :once } do
    expect(PostSlackMessage).to receive(:call).with(
      "Weather for Saturday, Nov 17: Mostly cloudy until afternoon.\nTemperature: ˚F\nHumidity: 0.69%\nPrecipitation: 0%\n",
      "cool channel"
    )

    post '/api', params: {
      event:
        {
          type: 'app_mention',
          text: 'Weather tomorrow',
          channel: 'cool channel'
        }
      }
  end
end
