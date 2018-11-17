require 'rails_helper'

RSpec.describe 'Slack Event', type: :request do
  it 'posts a message to slack with current weather', vcr: { record: :once } do
    expect(PostSlackMessage).to receive(:call).with(
      "Current Weather: Clear\nTemperature: 1.98˚F\nHumidity: 0.32%\n",
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
end
