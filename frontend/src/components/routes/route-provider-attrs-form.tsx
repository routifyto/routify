import React from 'react';
import { match } from 'ts-pattern';

import { OpenaiRouteProviderAttrsForm } from '@/components/routes/attrs/openai-route-provider-attrs-form';

interface RouteProviderAttrsFormProps {
  provider: string;
  index: number;
}

export function RouteProviderAttrsForm({
  provider,
  index,
}: RouteProviderAttrsFormProps) {
  return match(provider)
    .with('openai', () => <OpenaiRouteProviderAttrsForm index={index} />)
    .otherwise(() => null);
}
