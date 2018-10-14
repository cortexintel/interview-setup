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

rtm.on(CLIENT_EVENTS.RTM.AUTHENTICATED, rtmStartData => console.log('slackbot is running..'));

rtm.on(RTM_EVENTS.MESSAGE, message => {
  const wordMap = {};
  if (message.text) {
    const words = message.text.split(' ');
    for (let i = 0; i < words.length; i++) {
      let word = words[i];
      wordMap[word] = true;
    }
  }

  if (wordMap['<@UDD5U0U04>'] && wordMap['Weather']) {    
    axios
      .get(darkSkyUrl)
      .then(response => {
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
        reply = reply ? reply : 'Error retrieving data'

        rtm.sendMessage(reply, message.channel);
      })
      .catch(error => {
        console.log('Error accessing darksky ❌')
        console.log(error)
      })
  }
});

rtm.start();
