import React, { useEffect } from 'react';
import { readToken } from '@/lib/storage';
import { useGetWorkspaceQuery } from '@/api/workspaces';
import { WorkspaceSkeleton } from '@/components/workspaces/workspace-skeleton';
import { WorkspaceContext } from '@/contexts/workspace';
import { Outlet } from 'react-router-dom';

export function WorkspaceLayout() {
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

  // to do: handle error state
  if (!data) {
    return null;
  }

  return (
    <WorkspaceContext.Provider value={data}>
      <div className="max-h-screen min-h-screen font-sans">
        <Outlet />
      </div>
    </WorkspaceContext.Provider>
  );
}
