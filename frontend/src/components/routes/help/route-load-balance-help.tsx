import React from 'react';
import { HelpIcon } from '@/components/ui/help-icon';

export function RouteLoadBalanceHelp() {
  return (
    <HelpIcon learnMoreLink="https://routify.to/docs/routes#load">
      <p>
        If enabled, the request is proxied to the providers in a round-robin
        fashion, with custom weights for each provider. This is useful when you
        have multiple providers for a route and want to distribute the load
        evenly.
      </p>
      <p>
        For example, you can have a route that proxies requests to the OpenAI
        API and the Anthropic API, with a 70/30 split between the two.
      </p>
    </HelpIcon>
  );
}
