import React from 'react';

import { AppNotFound } from '@/components/apps/app-not-found';
import { AppContext } from '@/contexts/app';
import { useWorkspace } from '@/contexts/workspace';
import { Outlet, useParams } from 'react-router-dom';
import { WorkspaceSidebar } from '@/components/workspaces/workspace-sidebar';

export function AppLayout() {
  const { appId } = useParams();
  const workspace = useWorkspace();
  const app = workspace.apps.find((app) => app.id === appId);

  if (app == null) {
    return <AppNotFound />;
  }

  return (
    <AppContext.Provider value={app}>
      <div className="grid h-screen min-h-screen w-full grid-cols-[280px_minmax(0,1fr)] items-start gap-2">
        <WorkspaceSidebar />
        <div className="relative h-screen w-full overflow-hidden">
          <div className="absolute bottom-0 left-0 right-0 top-0 h-screen overflow-y-scroll px-10 py-6">
            <div className="container">
              <Outlet />
            </div>
          </div>
        </div>
      </div>
    </AppContext.Provider>
  );
}
