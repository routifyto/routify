import React from 'react';
import { useApp } from '@/contexts/app';
import { useCreateAppProviderMutation } from '@/api/app-providers';
import { AppProviderForm } from '@/components/app-providers/app-provider-form';
import { AppProviderInput } from '@/types/app-providers';
import { useNavigate } from 'react-router-dom';

export function AppProviderCreate() {
  const app = useApp();
  const navigate = useNavigate();
  const { mutate, isPending } = useCreateAppProviderMutation(app.id);

  function handleSubmit(input: AppProviderInput) {
    mutate(input, {
      onSuccess: (data) => {
        navigate(`/${app.id}/providers/${data.id}`);
      },
    });
  }

  return (
    <AppProviderForm
      appProvider={null}
      errors={[]}
      isPending={isPending}
      onSubmit={handleSubmit}
    />
  );
}
