class SlackEventsController < ApplicationController
  before_action :check_event_type
  before_action :check_text

  def handle
    weather = FetchWeather.call(type: @match[1])
    message = WeatherPresenter.new(weather).call
    PostSlackMessage.call(message, event_params[:channel])

    head :ok
  end

  private

  def event_params
    params.require(:event).permit(:type, :text, :channel)
  end

  def check_event_type
    head :not_acceptable unless event_params[:type] == 'app_mention'
  end

  def check_text
    @match = /\A.*weather\s(now|tomorrow).*\z/i.match(event_params[:text])

    head :not_acceptable unless @match
  end
end
