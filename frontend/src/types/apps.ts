import { AppRole } from '@/types/app-users';

export type AppInput = {
  name: string;
  description?: string;
};

export type AppOutput = {
  id: string;
  name: string;
  description?: string;
  role: AppRole;
};
