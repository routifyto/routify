import React from 'react';
import { useApp } from '@/contexts/app';
import { useNavigate } from 'react-router-dom';
import { toast } from '@/components/ui/use-toast';
import { useCreateConsumerMutation } from '@/api/consumers';
import { ConsumerInput } from '@/types/consumers';
import { ConsumerForm } from '@/components/consumers/consumer-form';

export function ConsumerCreate() {
  const app = useApp();
  const navigate = useNavigate();
  const { mutate, isPending } = useCreateConsumerMutation(app.id);

  function handleSubmit(input: ConsumerInput) {
    mutate(input, {
      onSuccess: (data) => {
        navigate(`/${app.id}/consumers/${data.id}`);
      },
      onError: (error) => {
        toast({
          title: 'Failed to create consumer',
          description: error.message,
          variant: 'destructive',
        });
      },
    });
  }

  return (
    <ConsumerForm
      consumer={null}
      errors={[]}
      isPending={isPending}
      onSubmit={handleSubmit}
    />
  );
}
