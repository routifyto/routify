import React from 'react';
import { useApp } from '@/contexts/app';
import {
  useGetAppProviderQuery,
  useUpdateAppProviderMutation,
} from '@/api/app-providers';
import { AppProviderForm } from '@/components/app-providers/app-provider-form';
import { AppProviderInput } from '@/types/app-providers';
import { toast } from '@/components/ui/use-toast';
import { Spinner } from '@/components/ui/spinner';
import { AppProviderNotFound } from '@/components/app-providers/app-provider-not-found';

interface AppProviderUpdateProps {
  appProviderId: string;
}

export function AppProviderUpdate({ appProviderId }: AppProviderUpdateProps) {
  const app = useApp();
  const { mutate, isPending: isPendingMutation } = useUpdateAppProviderMutation(
    app.id,
    appProviderId,
  );

  const { data, isPending: isPendingQuery } = useGetAppProviderQuery(
    app.id,
    appProviderId,
  );

  function handleSubmit(input: AppProviderInput) {
    mutate(input, {
      onSuccess: () => {
        toast({
          title: 'Provider updated',
          description: 'The provider has been updated successfully.',
          variant: 'default',
        });
      },
    });
  }

  if (isPendingQuery) {
    return <Spinner />;
  }

  if (!data) {
    return <AppProviderNotFound />;
  }

  return (
    <AppProviderForm
      appProvider={data}
      errors={[]}
      isPending={isPendingMutation}
      onSubmit={handleSubmit}
    />
  );
}
