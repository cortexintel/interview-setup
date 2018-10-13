require 'spec_helper'

describe DarkSlackBot do
    context "user requests today's weather" do
        it "responds with today's forecast" do
            VCR.use_cassette('weather_now') do
                expect(message: "#{SlackRubyBot.config.user} weather now")
                    .to respond_with_slack_message(/clear/i)
            end
        end
    end

    context "user requests tomorrow's weather" do
        it "responds with tomorrow's forecast" do
            VCR.use_cassette('weather_tomorrow') do
                expect(message: "#{SlackRubyBot.config.user} weather tomorrow")
                    .to respond_with_slack_message(/clear/i)
            end
        end
    end

    context  "user requests phase of the moon" do
        it "responds with the phase of the moon" do
            VCR.use_cassette('moon_phase') do
                expect(message: "#{SlackRubyBot.config.user} moon phase")
                    .to respond_with_slack_message(/waxing/i)
            end
        end
    end
end
