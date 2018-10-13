require 'spec_helper'

describe 'Whenever Schedule' do
    before do
        load 'Rakefile'
    end

    it 'makes sure rake statements exist' do
        schedule = Whenever::Test::Schedule.new()
        expect(Rake::Task.task_defined?(schedule.jobs[:rake].first[:task])).to eq(true)
    end

    it 'makes sure cron runs daily' do
        schedule = Whenever::Test::Schedule.new()
        expect(schedule.jobs[:rake].first[:every]).to eq([ActiveSupport::Duration.build(86400), {:at => "8:00 am"}])
    end
end
