import React from 'react';
import { CreateRouteInput } from '@/types/routes';
import { useFormContext } from 'react-hook-form';
import { useGetAppProviderQuery } from '@/api/app-providers';
import { useApp } from '@/contexts/app';
import { Spinner } from '@/components/ui/spinner';
import { providers } from '@/types/providers';
import { Button } from '@/components/ui/button';
import { Trash } from 'lucide-react';
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { ModelSelect } from '@/components/providers/model-select';
import { RouteProviderAttrsForm } from '@/components/routes/route-provider-attrs-form';
import { Input } from '@/components/ui/input';

interface RouteProviderFormProps {
  index: number;
  onRemove: () => void;
}

export function RouteProviderForm({ index, onRemove }: RouteProviderFormProps) {
  const app = useApp();
  const form = useFormContext<CreateRouteInput>();
  const appProviderId = form.watch(`providers.${index}.appProviderId`);

  const { data: appProvider, isPending } = useGetAppProviderQuery(
    app.id,
    appProviderId,
  );
  const provider = providers.find(
    (provider) => provider.id === appProvider?.provider,
  );

  if (isPending) {
    return (
      <div className="flex h-full w-full items-center justify-center">
        <Spinner />
      </div>
    );
  }

  if (!appProvider || !provider) {
    return (
      <div className="flex h-full w-full items-center justify-center">
        <p>App provider not found or deleted.</p>
      </div>
    );
  }

  return (
    <React.Fragment>
      <div className="mb-3 flex flex-row items-center justify-between">
        <div className="flex flex-row items-center gap-2">
          <div className="flex flex-row items-center gap-3 p-0.5">
            <img
              src={provider.logo}
              className="h-10 w-10 rounded p-0.5 shadow"
              alt={provider.name}
            />
            <div className="flex flex-col gap-1">
              <p className="font-semibold">{appProvider.name}</p>
              <p className="text-xs text-muted-foreground">
                {appProvider.description}
              </p>
            </div>
          </div>
        </div>
        <Button
          type="button"
          variant="outline"
          size="icon"
          className="flex flex-row items-center gap-1"
          onClick={() => {
            onRemove();
          }}
        >
          <Trash className="h-4 w-4" />
        </Button>
      </div>
      <FormField
        control={form.control}
        name={`providers.${index}.weight`}
        render={({ field }) => (
          <FormItem>
            <FormLabel>Weight</FormLabel>
            <FormControl>
              <Input
                placeholder=""
                value={field.value ?? ''}
                onChange={(event) => {
                  const number = parseInt(event.target.value, 10);
                  if (isNaN(number)) {
                    return;
                  }
                  field.onChange(number);
                }}
              />
            </FormControl>
            <FormMessage />
          </FormItem>
        )}
      />
      <FormField
        control={form.control}
        name={`providers.${index}.model`}
        render={({ field }) => (
          <FormItem>
            <FormLabel>Model</FormLabel>
            <FormControl>
              <ModelSelect
                provider={provider}
                value={field.value ?? ''}
                onChange={field.onChange}
              />
            </FormControl>
            <FormMessage />
          </FormItem>
        )}
      />
      <RouteProviderAttrsForm index={index} provider={provider.id} />
    </React.Fragment>
  );
}
