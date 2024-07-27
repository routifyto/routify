import React from 'react';
import { HelpIcon } from '@/components/ui/help-icon';

export function RouteProviderTimeoutHelp() {
  return (
    <HelpIcon learnMoreLink="https://routify.to/docs/routes#timeout">
      <p>
        The timeout is the maximum time in milliseconds that the provider has to
        respond to a request. If the provider does not respond within the
        timeout, the request is considered failed.
      </p>
      <p>
        Keep in mind that network latency and the {"provider's"} API
        geographical location can affect the response time.
      </p>
    </HelpIcon>
  );
}
