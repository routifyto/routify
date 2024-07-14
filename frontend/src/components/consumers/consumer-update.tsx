import React from 'react';
import { useApp } from '@/contexts/app';
import { toast } from '@/components/ui/use-toast';
import { Spinner } from '@/components/ui/spinner';
import {
  useGetConsumerQuery,
  useUpdateConsumerMutation,
} from '@/api/consumers';
import { ConsumerInput } from '@/types/consumers';
import { ConsumerNotFound } from '@/components/consumers/consumer-not-found';
import { ConsumerForm } from '@/components/consumers/consumer-form';

interface ConsumerUpdateProps {
  consumerId: string;
}

export function ConsumerUpdate({ consumerId }: ConsumerUpdateProps) {
  const app = useApp();
  const { mutate, isPending: isPendingMutation } = useUpdateConsumerMutation(
    app.id,
    consumerId,
  );

  const { data, isPending: isPendingQuery } = useGetConsumerQuery(
    app.id,
    consumerId,
  );

  function handleSubmit(input: ConsumerInput) {
    mutate(input, {
      onSuccess: () => {
        toast({
          title: 'Consumer updated',
          description: 'The consumer has been updated successfully.',
          variant: 'default',
        });
      },
      onError: (error) => {
        toast({
          title: 'Failed to update consumer',
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
    return <ConsumerNotFound />;
  }

  return (
    <ConsumerForm
      consumer={data}
      errors={[]}
      isPending={isPendingMutation}
      onSubmit={handleSubmit}
    />
  );
}
