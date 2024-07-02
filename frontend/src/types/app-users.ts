import { EnumOption } from '@/types/common';

export type AppUserRole = 'OWNER' | 'ADMIN' | 'MEMBER';

export type AppUserPayload = {
  id: string;
  userId: string;
  name: string;
  email: string;
  avatar?: string | null;
  role: AppUserRole;
  createdAt: Date;
};

export type AppUserInput = {
  role: AppUserRole;
};

export type AppUsersInput = {
  emails: string[];
  role: AppUserRole;
};

export const appUserRoleOptions: EnumOption[] = [
  { label: 'Owner', value: 'OWNER' },
  { label: 'Admin', value: 'ADMIN' },
  { label: 'Member', value: 'MEMBER' },
];
