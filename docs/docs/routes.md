---
sidebar_position: 2
---

# Routes

Routify allows you to create custom routes to implement your own logic. You can create routes to route requests to different providers, models, or services.

Routes are created through the interface or by using the API. 

Each route contains a name, a path and one or more providers. Some examples of route paths:

- `/v1/chat/completions`: The standard path for OpenAI chat completions.
- `/v1/openai`: A custom path for OpenAI requests.
- `/v1/gpt3`: A custom path for GPT-3 requests.
- `/v1/support`: A custom route for support requests.

## Providers

Each route can have one or more providers. Providers are the services that will handle the requests. Some examples of providers: OpenAI, Mistral, Anthropic, etc.

Before adding a provider to a route, you need to create a provider in the interface and provide the necessary credentials.

Each provider can have a custom weight, which determines the percentage of requests that will be routed to that provider. For example, if you have two providers with weights of 50 and 50, then each provider will receive 50% of the requests.