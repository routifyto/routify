export type RouteType = 'COMPLETION' | 'EMBEDDING';
export type RouteStrategy = 'DEFAULT' | 'LOAD_BALANCE' | 'FALLBACK';

export type RouteOutput = {
  id: string;
  name: string;
  description?: string | null;
  path: string;
  type: RouteType;
  schema: string;
  strategy: RouteStrategy;
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
  strategy: RouteStrategy;
  attrs: Record<string, string>;
  providers: RouteProviderInput[];
};

export type UpdateRouteInput = {
  name: string;
  description?: string | null;
  path: string;
  schema: string;
  strategy: RouteStrategy;
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
