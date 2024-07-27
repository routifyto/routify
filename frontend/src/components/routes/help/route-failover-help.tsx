import React from 'react';
import { HelpIcon } from '@/components/ui/help-icon';

export function RouteFailoverHelp() {
  return (
    <HelpIcon learnMoreLink="https://routify.to/docs/routes#failover">
      <p>
        If enabled, the request is retried with the next provider if the current
        provider fails to respond.
      </p>
      <p>
        This is useful when you have multiple providers for a route and want to
        ensure high availability.
      </p>
    </HelpIcon>
  );
}
