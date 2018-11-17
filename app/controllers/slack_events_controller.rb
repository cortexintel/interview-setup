class SlackEventsController < ApplicationController

  def handle
    if event_params[:type] == 'app_mention' && event_params[:text] == 'Weather now'
      response = FetchCurrentWeather.call
      weather  = CurrentWeatherParser.new(response.body)
      message  = WeatherPresenter.new(weather).call
      PostSlackMessage.call(message, event_params[:channel])

      head :ok
    else
      head :not_acceptable
    end
  end

  def event_params
    params.require(:event).permit(:type, :text, :channel)
  end
end
