import * as React from 'react';
import { NavLink } from 'react-router-dom';

import { buttonVariants } from '@/components/ui/button';

export function NotFound() {
  return (
    <div className="flex flex-grow items-center justify-center bg-background text-foreground">
      <div className="space-y-4">
        <h2 className="mb-4 text-8xl">404</h2>
        <h1 className="text-3xl font-semibold">Oops! Page not found</h1>
        <p className="text-sm text-muted-foreground">
          We are sorry, but the page you requested was not found
        </p>
        <NavLink to="/" className={buttonVariants()}>
          Back to Home
        </NavLink>
      </div>
    </div>
  );
}
