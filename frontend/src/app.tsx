import * as React from 'react';
import { RouterProvider } from 'react-router-dom';
import { QueryClient } from '@tanstack/query-core';
import {
  MutationCache,
  QueryCache,
  QueryClientProvider,
} from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';

import { router } from '@/router';
import { Toaster } from '@/components/ui/toaster';
import { TooltipProvider } from '@/components/ui/tooltip';
import { deleteToken } from '@/lib/storage';
import { ApiErrorOutput } from '@/types/errors';

const queryClient = new QueryClient({
  queryCache: new QueryCache({
    onError: (error) => {
      const apiError = error as unknown as ApiErrorOutput;
      if (apiError.code && apiError.code === 'UNAUTHORIZED') {
        deleteToken();
        window.location.href = '/login';
      }
    },
  }),
  mutationCache: new MutationCache({
    onError: (error) => {
      const apiError = error as unknown as ApiErrorOutput;
      if (apiError.code && apiError.code === 'UNAUTHORIZED') {
        deleteToken();
        window.location.href = '/login';
      }
    },
  }),
});

export function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <TooltipProvider>
        <RouterProvider router={router} />
      </TooltipProvider>
      <ReactQueryDevtools initialIsOpen={false} />
      <Toaster />
    </QueryClientProvider>
  );
}
