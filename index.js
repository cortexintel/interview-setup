const {RtmClient, CLIENT_EVENTS, RTM_EVENTS} = require('@slack/client');
const axios = require('axios');
const {checkWeatherDifference, checkHourlyPrecipitation} = require('./utils')

/**
  Setup variables
**/
const bot_token = '';
const darksky_token = '';

if (!bot_token) {
  console.log("Missing SLACK_BOT_TOKEN environment variable ❌");
  process.exit(1);
}

if (!darksky_token) {
  console.log("Missing DARKSKY_TOKEN environment variable ❌");
  process.exit(1);
}

const darkSkyUrl = `https://api.darksky.net/forecast/${darksky_token}/40.7128,-74.0059`;
const channel = '';

const rtm = new RtmClient(bot_token);

// Verify slackbot is connected and authenticated
rtm.on(CLIENT_EVENTS.RTM.AUTHENTICATED, rtmStartData => console.log('slackbot is running..'));

/**
  Slackbot should analyze each message to check if a response is expected
**/
rtm.on(RTM_EVENTS.MESSAGE, message => {
  // Create a map of words in the message
  const wordMap = {};
  if (message.text) {
    const words = message.text.split(' ');
    for (let i = 0; i < words.length; i++) {
      let word = words[i];
      wordMap[word] = true;
    }
  }

  // Check if slackbot was mentioned and word "weather" was included
  if (wordMap['<@UDD5U0U04>'] && wordMap['Weather'] || wordMap['eather']) {    
    axios
      .get(darkSkyUrl)
      .then(response => {
        // Construct message based on response from DarkSky
        let reply;
        if (wordMap['now']) {
          reply = response && response.data && response.data.currently && response.data.currently.summary;
          if (reply) {
            let temp = response.data.currently.temperature;
            reply += temp ? `\nTemperature is ${temp} degrees Fahrenheit.` : '';
          } 
        } else if (wordMap['tomorrow']) {
          reply = response && response.data && response.data.daily && response.data.daily.data && response.data.daily.data[1] && response.data.daily.data[1].summary;
          if (reply) {
            let temp = response.data.daily.data[1].temperatureHigh;
            reply += temp ? `\nHigh will be ${temp} degrees Fahrenheit.` : '';
          } 
        } else {
          reply = 'Weather now or Weather tomorrow?'
        }
        // If reply was not set, something has gone wrong
        reply = reply ? reply : 'Error retrieving data'

        rtm.sendMessage(reply, message.channel);
      })
      .catch(error => {
        console.log('Error accessing darksky ❌')
        console.log(error)
      })
  }
});

/**
  Slackbot should give a morning update at 6 AM if the weather has changed.
  It should also check every hour if precipitation is starting or stopping. 
**/
rtm.on(CLIENT_EVENTS.RTM.RTM_CONNECTION_OPENED, () => {
  setInterval(() => { 
    var hour = new Date().getHours();
    if (hour === 6) {
      checkWeatherDifference().then(response => {
        if (response) {
          rtm.sendMessage(response, channel);
        }
      }).catch(error => {
        console.log('Error accessing darksky ❌')
        console.log(error)
      })
    }

    checkHourlyPrecipitation().then(response => {
      if (response) {
        rtm.sendMessage(response, channel);
      }
    }).catch(error => {
      console.log('Error accessing darksky ❌')
      console.log(error)
    })
  } , 1000 * 60 * 60);
});

rtm.start();
