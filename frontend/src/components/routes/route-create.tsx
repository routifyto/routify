import { useApp } from '@/contexts/app';
import { useNavigate } from 'react-router-dom';
import React from 'react';
import { useCreateRouteMutation } from '@/api/routes';
import { CreateRouteInput } from '@/types/routes';
import { RouteForm } from '@/components/routes/route-form';

export function RouteCreate() {
  const app = useApp();
  const navigate = useNavigate();
  const { mutate, isPending } = useCreateRouteMutation(app.id);

  function handleSubmit(input: CreateRouteInput) {
    mutate(input, {
      onSuccess: (data) => {
        navigate(`/${app.id}/routes/${data.id}`);
      },
    });
  }

  return (
    <RouteForm
      route={null}
      errors={[]}
      isPending={isPending}
      onSubmit={handleSubmit}
    />
  );
}
