Rails.application.routes.draw do

  post 'api', to: 'slack_events#handle'
end
