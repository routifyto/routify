import React from 'react';
import { HelpIcon } from '@/components/ui/help-icon';

export function RouteSchemaHelp() {
  return (
    <HelpIcon learnMoreLink="https://routify.to/docs/routes#load-balancing">
      <p>
        Defines which provider schema to use for the route. This will determine
        the structure of the request and response objects. You can use the
        OpenAI schema for input and output but have the request proxied to the
        Anthropic API or any other provider.
      </p>
      <p className="font-semibold">
        This allows you to switch between providers without changing the
        application code.
      </p>
    </HelpIcon>
  );
}
