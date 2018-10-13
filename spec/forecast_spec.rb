require 'spec_helper'

describe Forecast do
    context "weather today is different from yesterday" do
        it "responds with today's forecast" do
            VCR.use_cassette('morning_report') do
                expect(Forecast.morning_report["message"]["text"])
                    .to match(/forecast/i)
            end
        end
    end
end
