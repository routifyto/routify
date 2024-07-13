import { EnumOption } from '@/types/common';

export type AppRole = 'OWNER' | 'ADMIN' | 'MEMBER';

export type AppUserOutput = {
  id: string;
  userId: string;
  name: string;
  email: string;
  avatar?: string | null;
  role: AppRole;
  createdAt: Date;
};

export type AppUserInput = {
  role: AppRole;
};

export type AppUsersInput = {
  emails: string[];
  role: AppRole;
};

export const appUserRoleOptions: EnumOption[] = [
  { label: 'Owner', value: 'OWNER' },
  { label: 'Admin', value: 'ADMIN' },
  { label: 'Member', value: 'MEMBER' },
];
