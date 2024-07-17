import React from 'react';
import { useApp } from '@/contexts/app';
import { toast } from '@/components/ui/use-toast';
import { Spinner } from '@/components/ui/spinner';
import { useGetRouteQuery, useUpdateRouteMutation } from '@/api/routes';
import { UpdateRouteInput } from '@/types/routes';
import { RouteNotFound } from '@/components/routes/route-not-found';
import { RouteForm } from '@/components/routes/route-form';

interface RouteUpdateProps {
  routeId: string;
}

export function RouteUpdate({ routeId }: RouteUpdateProps) {
  const app = useApp();
  const { mutate, isPending: isPendingMutation } = useUpdateRouteMutation(
    app.id,
    routeId,
  );

  const { data, isPending: isPendingQuery } = useGetRouteQuery(app.id, routeId);

  function handleSubmit(input: UpdateRouteInput) {
    console.log('update-input', input);
    mutate(input, {
      onSuccess: () => {
        toast({
          title: 'Route updated',
          description: 'The route has been updated successfully.',
          variant: 'default',
        });
      },
      onError: (error) => {
        toast({
          title: 'Failed to update route',
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
    return <RouteNotFound />;
  }

  return (
    <RouteForm
      route={data}
      errors={[]}
      isPending={isPendingMutation}
      onSubmit={handleSubmit}
    />
  );
}
