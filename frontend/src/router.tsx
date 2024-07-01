import React from 'react';
import { createBrowserRouter } from 'react-router-dom';

import { NotFound } from '@/pages/not-found';

export const router = createBrowserRouter([
  {
    path: '*',
    element: <NotFound />,
  },
]);
