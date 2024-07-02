import { AppUserRole } from '@/types/app-users';

export type Workspace = {
  user: WorkspaceUser;
  apps: WorkspaceApp[];
};

export type WorkspaceUser = {
  id: string;
  name: string;
  email: string;
};

export type WorkspaceApp = {
  id: string;
  name: string;
  role: AppUserRole;
  description?: string;
};
