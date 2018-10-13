require './forecast'

class DarkSlackBot < SlackRubyBot::Bot
    help do
        title 'ifua-bot'
        desc 'This bot will tell you the weather and phases of the moon.'

        command 'weather now' do
            desc 'Reports the weather right now (defaults to NYC).'
        end

        command 'weather tomorrow' do
            desc 'Reports the weather tomorrow (defaults to NYC).'
        end

        command 'moon phase' do
            desc 'Reports the phase of the moon.'
        end
    end

    command /weather now/i do |client, data, match|
        client.say(text: "#{Forecast.new().weather_forecast}", channel: data.channel)
    end

    command /weather tomorrow/i do |client, data, match|
        sec_in_day = 86400
        client.say(text: "#{Forecast.new(Time.now + sec_in_day).weather_forecast}", channel: data.channel)
    end

    command /moon phase/i do |client, data, match|
        client.say(text: "#{Forecast.new(Time.now).moon_phase}", channel: data.channel)
    end
end
