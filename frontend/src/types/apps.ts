import { AppUserRole } from '@/types/app-users';

export type AppInput = {
  name: string;
  description?: string;
};

export type AppPayload = {
  id: string;
  name: string;
  description?: string;
  role: AppUserRole;
};
