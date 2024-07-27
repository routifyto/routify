import React from 'react';
import { HelpIcon } from '@/components/ui/help-icon';

export function RouteCacheHelp() {
  return (
    <HelpIcon learnMoreLink="https://routify.to/docs/routes#caching">
      <p>
        If enabled, the response from the provider is cached for a specified
        duration.
      </p>
      <p>
        Caching is applying on route provider level. If you have multiple
        providers for a route, the response from each provider is cached
        separately.
      </p>
    </HelpIcon>
  );
}
