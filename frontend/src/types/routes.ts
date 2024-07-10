export type RouteType = 'COMPLETION' | 'EMBEDDING';

export type RoutePayload = {
  id: string;
  name: string;
  description?: string | null;
  path: string;
  type: RouteType;
  providers: RouteProviderPayload[];
};

export type RouteProviderPayload = {
  id: string;
  appProviderId: string;
  model?: string | null;
};

export type CreateRouteInput = {
  name: string;
  description?: string | null;
  path: string;
  type: RouteType;
  attrs: Record<string, string>;
  providers: RouteProviderInput[];
};

export type UpdateRouteInput = {
  name: string;
  description?: string | null;
  path: string;
  providers: RouteProviderInput[];
};

export type RouteProviderInput = {
  id?: string | null;
  appProviderId: string;
  model?: string | null;
};
