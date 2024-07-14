import { AppRole } from '@/types/app-users';

export type WorkspaceOutput = {
  user: WorkspaceUserOutput;
  apps: WorkspaceAppOutput[];
};

export type WorkspaceUserOutput = {
  id: string;
  name: string;
  email: string;
};

export type WorkspaceAppOutput = {
  id: string;
  name: string;
  role: AppRole;
  description?: string;
};
