import React from 'react';
import { match } from 'ts-pattern';

import { OpenAiRouteProviderAttrsForm } from '@/components/routes/attrs/openai-route-provider-attrs-form';
import { TogetherAiRouteProviderAttrsForm } from '@/components/routes/attrs/together-ai-route-provider-attrs-form';
import { AnthropicRouteProviderAttrsForm } from '@/components/routes/attrs/anthropic-route-provider-attrs-form';
import { MistralRouteProviderAttrsForm } from '@/components/routes/attrs/mistral-route-provider-attrs-form';
import { CohereRouteProviderAttrsForm } from '@/components/routes/attrs/cohere-route-provider-attrs-form';

interface RouteProviderAttrsFormProps {
  provider: string;
  index: number;
}

export function RouteProviderAttrsForm({
  provider,
  index,
}: RouteProviderAttrsFormProps) {
  return match(provider)
    .with('openai', () => <OpenAiRouteProviderAttrsForm index={index} />)
    .with('together-ai', () => (
      <TogetherAiRouteProviderAttrsForm index={index} />
    ))
    .with('anthropic', () => <AnthropicRouteProviderAttrsForm index={index} />)
    .with('mistral', () => <MistralRouteProviderAttrsForm index={index} />)
    .with('cohere', () => <CohereRouteProviderAttrsForm index={index} />)
    .otherwise(() => null);
}
