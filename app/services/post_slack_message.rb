class PostSlackMessage

  def self.call(*args)
    new.call(*args)
  end

  def initialize(message, channel, http_client = nil)
    @message = message
    @channel = channel
    @http_client = http_client || SlackClient
  end

  def call
    http_client.post_chat_message(message, channel)
  end

  private

  attr_reader :message, :channel, :http_client
end
