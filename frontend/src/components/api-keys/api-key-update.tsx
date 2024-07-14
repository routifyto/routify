import React from 'react';
import { useApp } from '@/contexts/app';
import { toast } from '@/components/ui/use-toast';
import { Spinner } from '@/components/ui/spinner';
import { AppProviderNotFound } from '@/components/app-providers/app-provider-not-found';
import { useGetApiKeyQuery, useUpdateApiKeyMutation } from '@/api/api-keys';
import { ApiKeyInput } from '@/types/api-keys';
import { ApiKeyForm } from '@/components/api-keys/api-key-form';

interface ApiKeyUpdateProps {
  apiKeyId: string;
}

export function ApiKeyUpdate({ apiKeyId }: ApiKeyUpdateProps) {
  const app = useApp();
  const { mutate, isPending: isPendingMutation } = useUpdateApiKeyMutation(
    app.id,
    apiKeyId,
  );

  const { data, isPending: isPendingQuery } = useGetApiKeyQuery(
    app.id,
    apiKeyId,
  );

  function handleSubmit(input: ApiKeyInput) {
    mutate(input, {
      onSuccess: () => {
        toast({
          title: 'API Key updated',
          description: 'The API Key has been updated successfully.',
          variant: 'default',
        });
      },
      onError: (error) => {
        toast({
          title: 'Failed to update API Key',
          description: error.message,
          variant: 'destructive',
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
    <ApiKeyForm
      apiKey={data}
      errors={[]}
      isPending={isPendingMutation}
      onSubmit={handleSubmit}
    />
  );
}
