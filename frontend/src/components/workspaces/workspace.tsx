import React, { useEffect } from 'react';
import { readToken } from '@/lib/storage';
import { useGetWorkspaceQuery } from '@/api/workspaces';
import { WorkspaceSkeleton } from '@/components/workspaces/workspace-skeleton';

export function Workspace() {
  const { data, isPending } = useGetWorkspaceQuery();

  useEffect(() => {
    const token = readToken();
    if (!token) {
      window.location.href = '/login';
    }
  }, []);

  if (isPending) {
    return <WorkspaceSkeleton />;
  }

  return (
    <div className="min-w-screen flex h-full min-h-screen w-full items-center justify-center">
      <div className="flex flex-col items-center gap-8 text-center">
        <p>Welcome to Routify, {data?.user.name}!</p>
      </div>
    </div>
  );
}
