var RtmClient = require('@slack/client').RtmClient;
var CLIENT_EVENTS = require('@slack/client').CLIENT_EVENTS;
var http = require('http');
var axios = require('axios');

var bot_token = process.env.SLACK_BOT_TOKEN;
var darksky_token = process.env.DARKSKY_TOKEN;

if (!bot_token) {
  console.log("Missing SLACK_BOT_TOKEN environement variable ❌")
  process.exit(1)
}

if (!darksky_token) {
  console.log("Missing DARKSKY_TOKEN environement variable ❌")
  process.exit(1)
}

var rtm = new RtmClient(bot_token);

rtm.on(CLIENT_EVENTS.RTM.AUTHENTICATED, function (rtmStartData) {
  console.log("Slack Successfully Authenticated ✅");
  
  axios
    .get("https://api.darksky.net/forecast/" +  darksky_token + "/40.7128,-74.0059")
    .then(function (response) {
      console.log('Darksky Successfully Authenticated ✅')
      process.exit()
    })
    .catch(function (error) {
      console.log('Error accessing darksky ❌')
      console.log(error)
      process.exit(1)
    })
});

rtm.start();
