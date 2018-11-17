require 'rails_helper'

RSpec.describe 'PostSlackMessage' do
  it 'posts the provided message' do
    http_client = double('http_party')
    text = 'Weather!'
    channel = 'cool channel'

    expect(http_client).to receive(:post_chat_message).with(text, channel)

    PostSlackMessage.new(text, channel, http_client).call
  end
end
