import * as React from 'react';
import { RouterProvider } from 'react-router-dom';

import { ThemeProvider } from '@/contexts/theme';
import { router } from '@/router';

export function App() {
  return (
    <ThemeProvider>
      <RouterProvider router={router} />
    </ThemeProvider>
  );
}
