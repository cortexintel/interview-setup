Rails.application.routes.draw do

  get 'api', to: 'slack_events#index'
  post 'api', to: 'slack_events#handle'
end
