require './forecast'
require 'rake'

namespace :morning_report do
    desc "Sends a morning report to Slack"
    task :send do
        Forecast.morning_report
    end
end
