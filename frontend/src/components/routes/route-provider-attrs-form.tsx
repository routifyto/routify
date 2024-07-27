import React from 'react';
import { match } from 'ts-pattern';

import { OpenAiRouteProviderAttrsForm } from '@/components/routes/attrs/openai-route-provider-attrs-form';
import { TogetherAiRouteProviderAttrsForm } from '@/components/routes/attrs/together-ai-route-provider-attrs-form';

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
    .otherwise(() => null);
}
