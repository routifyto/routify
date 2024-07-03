import React from 'react';
import { createBrowserRouter } from 'react-router-dom';

import { NotFound } from '@/components/not-found';
import { Login } from '@/components/login';
import { WorkspaceLayout } from '@/components/workspaces/workspace-layout';
import { AppCreate } from '@/components/apps/app-create';
import { WorkspaceRedirect } from '@/components/workspaces/workspace-redirect';
import { AppLayout } from '@/components/apps/app-layout';
import { AppSettings } from '@/components/apps/app-settings';
import { AppUsers } from '@/components/app-users/app-users';
import { AppProviders } from '@/components/app-providers/app-providers';
import { AppProviderCreate } from '@/components/app-providers/app-provider-create';
import { AppProvider } from '@/components/app-providers/app-provider';

export const router = createBrowserRouter([
  {
    path: '',
    element: <WorkspaceLayout />,
    children: [
      {
        path: '',
        element: <WorkspaceRedirect />,
      },
      {
        path: '/create',
        element: <AppCreate />,
      },
      {
        path: '/:appId',
        element: <AppLayout />,
        children: [
          {
            path: 'providers',
            element: <AppProviders />,
          },
          {
            path: 'providers/create',
            element: <AppProviderCreate />,
          },
          {
            path: 'providers/:appProviderId',
            element: <AppProvider />,
          },
          {
            path: 'users',
            element: <AppUsers />,
          },
          {
            path: 'settings',
            element: <AppSettings />,
          },
        ],
      },
    ],
  },
  {
    path: '/login',
    element: <Login />,
  },
  {
    path: '*',
    element: <NotFound />,
  },
]);
