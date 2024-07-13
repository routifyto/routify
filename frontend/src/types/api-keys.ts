import { AppRole } from '@/types/app-users';

export type ApiKeyInput = {
  name: string;
  description?: string;
  canUseGateway: boolean;
  role?: AppRole;
  expiresAt?: Date | null;
};

export type ApiKeyOutput = {
  id: string;
  name: string;
  description?: string;
  canUseGateway: boolean;
  role?: AppRole;
  prefix: string;
  suffix: string;
  expiresAt?: Date | null;
};

export type CreateApiKeyOutput = {
  key: string;
  apiKey: ApiKeyOutput;
};
