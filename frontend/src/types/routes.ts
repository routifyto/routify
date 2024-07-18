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
};

export type RouteProviderInput = {
  id?: string | null;
  appProviderId: string;
  model?: string | null;
  attrs?: Record<string, string> | null;
  weight: number;
};
