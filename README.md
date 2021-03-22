This ASP.NET Core application provides a simple gateway to translate Octopus web hook events to Slack posts.

Define the `API_TOKEN` environment variable to a random string. Each request to the gateway must include an 
`API-TOKEN` header with the same value.

Define the `SLACK_URI_APIKEY` environment variable to the Slack web hook URL created through the console
at [https://api.slack.com/apps/](https://api.slack.com/apps/).

This application has been push to Docker Hub as [octopussamples/octopustoslack](https://hub.docker.com/r/octopussamples/octopustoslack).

Run locally with:

```
docker run \
    -e SLACK_URI_APIKEY=https://hooks.slack.com/services/randomchars/morerandomchars/yetmorerandomchars \
    -e API_TOKEN=thetokentoaccessthegamewaywith \
    -p 8080:80 \
    octopussamples/octopustoslack
```