<div align="center">

<img src="https://static.routify.to/landing/cover.png" alt="Routify"><br>

# Open-source AI Gateway
#### Route your LLM requests to different LLM providers with no code changes

</div>

Routify streamlines requests to 150+ open & closed source models with a unified API and easy-to-use UI. It is also supports caching, fallbacks, retries, timeouts, loadbalancing, and can be self-hosted.

- **Fast**: Routify is built on top of .NET8 and is designed to be fast and efficient.
- **Easy to use**: Routify provides an easy-to-use interface where you can manage all requests and responses.
- **Schema mapping**: Routify can map requests and responses to different provider schemas.
- **Observability**: Routify provides observability features to help you monitor your AI requests.
- **Cost control**: Routify can help you control costs by estimating the cost of each request and applying cost limits.
- **Load balancing**: Routify can load balance requests across multiple providers.
- **Failover**: Routify can automatically failover to a different provider if the primary provider is down.

## How it works

Routify uses routes which allow you to have different behavior and logic for different use cases. Each route is defined by a path and can have one or more providers to proxy the requests.

For example, you can have a route that matches `/openai` to proxy requests to the OpenAI API and another route that matches `/anthropic` to proxy requests to the Anthropic API.

Each route has a schema that defines which provider schema to use for the route. This will determine the structure of the request and response objects. You can use the OpenAI schema for input and output but have the request proxied to the Anthropic API or any other provider. This allows you to switch providers without changing your application code.

Also you can override request parameters, such as `system prompt`, `temperature`, `max_tokens`, `top_p`, etc. for each provider, through the user interface, on runtime.

## Upcoming features

- 🔲 Streaming response for chat completion
- 🔲 Automatic and configurable retries
- 🔲 Filtering and sorting of logs

## Test with Routify cloud

- Create an account and app at [cloud.routify.to](https://cloud.routify.to)
- Create a route, for example: `v1/chat/completion` if you are using OpenAI
- Add the OpenAI provider and the necessary credentials
- Make the request to Routify cloud gateway

```bash
curl --request POST \
     --url https://gateway.routify.to/v1/chat/completions \
     --header 'Authorization: Bearer {ROUTIFY_API_KEY}' \
     --header 'accept: application/json' \
     --header 'content-type: application/json' \
     --header 'routify-app: {ROUTIFY_APP_ID}' \
```

## Components

- **Fronend** - React SPA for managing routes, providers, logs etc
- **API** - .NET8 API for storing configuration in postgres
- **Gateway** - .NET8 gateway for proxying requests to different providers
- **Postgres** - main database for configurations and logs
- **Redis** - in memory database for caching and cost limits

## Self host

We are working on the documentation for self hosting and running Routify locally.