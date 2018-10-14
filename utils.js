const axios = require('axios');

const darksky_token = '';
const darkSkyUrl = `https://api.darksky.net/forecast/${darksky_token}/40.7128,-74.0059`;

/**
  Based on DarkSky's data, check if there is precipitation today that there wasn't yesterday, or if there's a 
  > 10 degree difference in temperature
**/
async function checkWeatherDifference()  {
  // Get the time for yesterday to send to DarkSky
  let yesterdayTime = Math.round((new Date().getTime() - 86400000) / 1000);

  let weatherNowPromise = axios.get(darkSkyUrl);
  let weatherYesterdayPromise = axios.get(`${darkSkyUrl},${yesterdayTime}?exclude=currently,flags`);
  // Asynchronously get yesterday's and today's weather, but wait for the response before continuing
  let [nowResponse, yesterdayResponse] = await Promise.all([weatherNowPromise, weatherYesterdayPromise]);
  let response = '';

  if (nowResponse && nowResponse.data && nowResponse.data.daily && nowResponse.data.daily.data && nowResponse.data.daily.data[0]
      && yesterdayResponse && yesterdayResponse.data && yesterdayResponse.data.daily && yesterdayResponse.data.daily.data && yesterdayResponse.data.daily.data[0]) {
    let yesterdayPrecip = yesterdayResponse.data.daily.data[0].precipType;
    let todayPrecip = nowResponse.data.daily.data[0].precipType;
    let yesterdayHigh = yesterdayResponse.data.daily.data[0].temperatureHigh;
    let todayHigh = nowResponse.data.daily.data[0].temperatureHigh;
    // If there is precipitation today that there wasn't yesterday, inform the channel
    if (yesterdayPrecip !== todayPrecip) {
      response += todayPrecip ?  `Today there will be ${todayPrecip}. ` : `Today there will not be ${yesterdayPrecip}. `;
    }
    // Inform the channel if there is a >10 degree difference in temperature
    if (todayHigh && yesterdayHigh && Math.abs(todayHigh - yesterdayHigh) > 10) {
      response += `Today's high will be ${todayHigh} degrees Fahrenheit.`;
    }
  }
  return response;
}

/**
  Check if precipitation is starting or stopping in the next hour
**/
async function checkHourlyPrecipitation() {
  let weather = await axios.get(darkSkyUrl);
  let response;
  if (weather && weather.data && weather.data.hourly && weather.data.hourly.data && weather.data.hourly.data.length > 1) {
    let precipNow = weather.data.hourly.data[0] && weather.data.hourly.data[0].precipType;
    let precipHour = weather.data.hourly.data[1] && weather.data.hourly.data[1].precipType;
    if (precipNow !== precipHour) {
      if (precipHour) {
        response = `It will start ${precipHour}ing in the next hour!`;
      } else if (precipNow) {
        response = `It will stop ${precipNow}ing in the next hour!`;
      }
    }
  }
  return response;
}

module.exports.checkWeatherDifference = checkWeatherDifference;
module.exports.checkHourlyPrecipitation = checkHourlyPrecipitation;