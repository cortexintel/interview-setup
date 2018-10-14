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
  console.log(message);
})

rtm.start();
