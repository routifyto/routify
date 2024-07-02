import { createContext, useContext } from 'react';

import { Workspace } from '@/types/workspaces';

export const WorkspaceContext = createContext<Workspace>({} as Workspace);

export const useWorkspace = () => useContext(WorkspaceContext);
