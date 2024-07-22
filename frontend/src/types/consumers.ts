import { CostLimitConfig } from '@/types/configs';

export type ConsumerInput = {
  name: string;
  alias?: string | null;
  description?: string | null;
  costLimitConfig?: CostLimitConfig | null;
};

export type ConsumerOutput = {
  id: string;
  name: string;
  alias?: string | null;
  description?: string | null;
  costLimitConfig?: CostLimitConfig | null;
};
