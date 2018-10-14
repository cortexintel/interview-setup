# inverview challenge - Colby McBride

This is a weather chatbot for the Cortex interview assignment. All weather is for New York, NY.

To get the chatbot running:
```
npm install
npm start
```

The chatbot accepts commands like
```
@colbym-bot Weather now
@colbym-bot Weather tomorrow
```
and will give hourly precipitation updates (if there's a change). It will also give a morning report if the weather has changed siginificantly from the previous day.