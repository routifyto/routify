import { CacheConfig, CostLimitConfig } from '@/types/configs';

export type RouteType = 'COMPLETION' | 'EMBEDDING';

export type RouteOutput = {
  id: string;
  name: string;
  description?: string | null;
  path: string;
  type: RouteType;
  schema: string;
  isLoadBalanceEnabled: boolean;
  isFailoverEnabled: boolean;
  providers: RouteProviderOutput[];
  cacheConfig?: CacheConfig | null;
  costLimitConfig?: CostLimitConfig | null;
};

export type RouteProviderOutput = {
  id: string;
  appProviderId: string;
  model?: string | null;
  weight: number;
};

export type CreateRouteInput = {
  name: string;
  description?: string | null;
  path: string;
  type: RouteType;
  schema: string;
  isLoadBalanceEnabled: boolean;
  isFailoverEnabled: boolean;
  attrs: Record<string, string>;
  providers: RouteProviderInput[];
  cacheConfig?: CacheConfig | null;
  costLimitConfig?: CostLimitConfig | null;
};

export type UpdateRouteInput = {
  name: string;
  description?: string | null;
  path: string;
  schema: string;
  isLoadBalanceEnabled: boolean;
  isFailoverEnabled: boolean;
  providers: RouteProviderInput[];
  attrs?: Record<string, string> | null;
  cacheConfig?: CacheConfig | null;
  costLimitConfig?: CostLimitConfig | null;
};

export type RouteProviderInput = {
  id?: string | null;
  appProviderId: string;
  model?: string | null;
  attrs?: Record<string, string> | null;
  weight: number;
};
