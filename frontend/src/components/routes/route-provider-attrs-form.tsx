import React from 'react';
import { match } from 'ts-pattern';

import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from '@/components/ui/collapsible';

import { OpenaiRouteProviderAttrsForm } from '@/components/routes/attrs/openai-route-provider-attrs-form';
import { ChevronDown, ChevronUp } from 'lucide-react';

interface RouteProviderAttrsFormProps {
  provider: string;
  index: number;
}

export function RouteProviderAttrsForm({
  provider,
  index,
}: RouteProviderAttrsFormProps) {
  const [open, setOpen] = React.useState(false);
  return (
    <Collapsible open={open} onOpenChange={setOpen}>
      <CollapsibleTrigger className="flex flex-row items-center gap-1 text-xs text-muted-foreground hover:underline">
        <span>{open ? 'Hide' : 'Show'} advanced configuration</span>
        {open ? (
          <ChevronUp className="h-3 w-3" />
        ) : (
          <ChevronDown className="h-3 w-3" />
        )}
      </CollapsibleTrigger>
      <CollapsibleContent className="mt-4">
        {match(provider)
          .with('openai', () => <OpenaiRouteProviderAttrsForm index={index} />)
          .otherwise(() => null)}
      </CollapsibleContent>
    </Collapsible>
  );
}
