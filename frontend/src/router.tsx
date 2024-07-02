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
