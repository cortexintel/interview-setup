require 'rails_helper'

RSpec.describe 'DifferentiateWeather' do
  context 'when weather change from yesterday to today is significant' do
    let(:weather_today) { double('weather_today', temperature: 30, humidity: 0.00, precip_probability: 0.75) }
    let(:weather_yesterday) { double('weather_today', temperature: 30, humidity: 0.15, precip_probability: 0.34) }

    it 'returns the differences' do
      allow(FetchWeather).to receive(:call).and_return(weather_today, weather_yesterday)

      result = DifferentiateWeather.new.call

      expect(result).to have_attributes(temperature: 0, humidity: -0.15, precip_probability: 0.41)
    end

    it 'reports significance' do
      allow(FetchWeather).to receive(:call).and_return(weather_today, weather_yesterday)

      result = DifferentiateWeather.new.call

      expect(result.significant?).to be true
    end
  end

  context 'when weather change from yesterday to today is NOT significant' do
    let(:weather_today) { double('weather_today', temperature: 30, humidity: 0.00, precip_probability: 0.75) }
    let(:weather_yesterday) { weather_today }


    it 'reports significance' do
      allow(FetchWeather).to receive(:call).and_return(weather_today, weather_yesterday)

      result = DifferentiateWeather.new.call

      expect(result.significant?).to be false
    end
  end
end
