import React from 'react';
import { HelpIcon } from '@/components/ui/help-icon';

export function RouteProviderWeightHelp() {
  return (
    <HelpIcon learnMoreLink="https://routify.to/docs/routes#weight">
      <p>
        Weights are only used when you have multiple providers for a route and
        load balancing is enabled.
      </p>
      <p>
        The weight is used to determine how often a provider is selected when a
        request is made to the route. You can assign any positive integer value
        to the weight.
      </p>
    </HelpIcon>
  );
}
