class PostSlackMessage
  def self.call(*args)
    new(*args).call
  end

  def initialize(message, channel, client = nil)
    @message = message
    @channel = channel
    @client = client || SlackClient
  end

  def call
    client.post_chat_message(message, channel)
  end

  private

  attr_reader :message, :channel, :client
end
