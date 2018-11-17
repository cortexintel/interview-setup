class SlackClient
  include HTTParty
  base_uri "https://slack.com/api/chat.postMessage"

  def self.post_chat_message(*args)
    new.post_chat_message(*args)
  end

  def initialize(token = nil)
    @token = token || ENV['SLACK_BOT_TOKEN']
  end

  def post_chat_message(message, channel)
    body = { text: message, channel: channel }

    self.class.post('/', body: body.to_json, headers: headers)
  end

  private

  attr_reader :token

  def headers
    {
      'Content-type' => 'application/json',
      'Authorization' => "Bearer #{token}"
    }
  end
end
