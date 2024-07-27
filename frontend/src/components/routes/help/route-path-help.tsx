import React from 'react';
import { HelpIcon } from '@/components/ui/help-icon';
import { Code } from '@/components/ui/code';

export function RoutePathHelp() {
  return (
    <HelpIcon learnMoreLink="https://routify.to/docs/routes#path">
      <p>
        The path is the URL that the route will match. It can contain only
        alphanumeric characters, hyphens, underscores, and slashes.
      </p>
      <p>
        You can use paths to have different behavior for different use cases.
        For example, you can have a route that matches <Code>/openai</Code> to
        proxy requests to the OpenAI API and another route that matches
        <Code>/anthropic</Code> to proxy requests to the Anthropic API.
      </p>
    </HelpIcon>
  );
}
