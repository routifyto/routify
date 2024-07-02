import React from 'react';
import { Spinner } from '@/components/ui/spinner';

export function WorkspaceSkeleton() {
  return (
    <div className="min-w-screen flex h-full min-h-screen w-full items-center justify-center">
      <div className="flex flex-col items-center gap-8 text-center">
        <h2 className="font-neotrax text-shadow-lg text-4xl text-gray-800">
          loading your workspace
        </h2>
        <div>
          <Spinner />
        </div>
      </div>
    </div>
  );
}
