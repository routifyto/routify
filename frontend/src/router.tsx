import React from 'react';
import { createBrowserRouter } from 'react-router-dom';

import { NotFound } from '@/pages/not-found';
import { Login } from '@/pages/login';
import { WorkspaceLayout } from '@/components/workspaces/workspace-layout';
import { CreateApp } from '@/pages/create-app';
import { WorkspaceRedirect } from '@/components/workspaces/workspace-redirect';
import { AppLayout } from '@/pages/app-layout';

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
        element: <CreateApp />,
      },
      {
        path: '/:appId',
        element: <AppLayout />,
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
