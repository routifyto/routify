import React from 'react';
import { HelpIcon } from '@/components/ui/help-icon';

export function RouteProviderModelHelp() {
  return (
    <HelpIcon learnMoreLink="https://routify.to/docs/routes#model">
      <p>
        The provider model to be used for chat completion. This model will
        override the value of the request body.
      </p>
      <p>
        You can change the model here without having to change the request body.
      </p>
    </HelpIcon>
  );
}
