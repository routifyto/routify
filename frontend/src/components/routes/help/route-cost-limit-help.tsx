import React from 'react';
import { HelpIcon } from '@/components/ui/help-icon';

export function RouteCostLimitHelp() {
  return (
    <HelpIcon learnMoreLink="https://routify.to/docs/routes#cost-limit">
      <p>
        The cost limit is the maximum cost allowed for a request to the route.
        If the cost of the request exceeds the limit, the request is rejected.
      </p>
    </HelpIcon>
  );
}
