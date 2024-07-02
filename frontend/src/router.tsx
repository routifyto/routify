import React from 'react';
import { createBrowserRouter } from 'react-router-dom';

import { NotFound } from '@/pages/not-found';
import { Login } from '@/pages/login';
import { Workspace } from '@/components/workspaces/workspace';

export const router = createBrowserRouter([
  {
    path: '/',
    element: <Workspace />,
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
