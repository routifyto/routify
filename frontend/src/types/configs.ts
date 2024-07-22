export type CacheConfig = {
  enabled: boolean;
  expiration: number;
};

export type CostLimitConfig = {
  enabled: boolean;
  dailyLimit?: number | null;
  monthlyLimit?: number | null;
};
