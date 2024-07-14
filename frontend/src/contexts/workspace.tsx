import { createContext, useContext } from 'react';

import { WorkspaceOutput } from '@/types/workspaces';

export const WorkspaceContext = createContext<WorkspaceOutput>(
  {} as WorkspaceOutput,
);

export const useWorkspace = () => useContext(WorkspaceContext);
