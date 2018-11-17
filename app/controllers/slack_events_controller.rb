class SlackEventsController < ApplicationController
  before_action :handle_url_verification, only: :handle
  before_action :check_event_type, only: :handle
  before_action :check_text, only: :handle

  def index
    head :ok
  end

  def handle
    weather = FetchWeather.call(type: @match[1])
    message = WeatherPresenter.new(weather).call
    PostSlackMessage.call(message, event_params[:channel])

    head :ok
  end

  private

  def handle_url_verification
    if params[:type] == 'url_verification'
      render plain: params[:challenge], status: :ok
    end
  end

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
