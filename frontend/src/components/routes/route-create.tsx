import { useApp } from '@/contexts/app';
import { useNavigate } from 'react-router-dom';
import React from 'react';
import { useCreateRouteMutation } from '@/api/routes';
import { CreateRouteInput } from '@/types/routes';
import { RouteForm } from '@/components/routes/route-form';
import { toast } from '@/components/ui/use-toast';

export function RouteCreate() {
  const app = useApp();
  const navigate = useNavigate();
  const { mutate, isPending } = useCreateRouteMutation(app.id);

  function handleSubmit(input: CreateRouteInput) {
    mutate(input, {
      onSuccess: (data) => {
        navigate(`/${app.id}/routes/${data.id}`);
      },
      onError: (error) => {
        toast({
          title: 'Failed to create route',
          description: error.message,
          variant: 'destructive',
        });
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
