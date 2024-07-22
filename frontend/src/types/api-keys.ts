import { AppRole } from '@/types/app-users';
import { CostLimitConfig } from '@/types/configs';

export type ApiKeyInput = {
  name: string;
  description?: string;
  canUseGateway: boolean;
  role?: AppRole;
  expiresAt?: Date | null;
  costLimitConfig?: CostLimitConfig | null;
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
  costLimitConfig?: CostLimitConfig | null;
};

export type CreateApiKeyOutput = {
  key: string;
  apiKey: ApiKeyOutput;
};
