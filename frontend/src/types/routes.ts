export type RouteType = 'COMPLETION' | 'EMBEDDING';
export type RouteSchemaType = 'OPENAI';

export type RouteOutput = {
  id: string;
  name: string;
  description?: string | null;
  path: string;
  type: RouteType;
  schemaType: RouteSchemaType;
  providers: RouteProviderOutput[];
};

export type RouteProviderOutput = {
  id: string;
  appProviderId: string;
  model?: string | null;
};

export type CreateRouteInput = {
  name: string;
  description?: string | null;
  path: string;
  type: RouteType;
  schemaType: RouteSchemaType;
  attrs: Record<string, string>;
  providers: RouteProviderInput[];
};

export type UpdateRouteInput = {
  name: string;
  description?: string | null;
  path: string;
  providers: RouteProviderInput[];
  attrs?: Record<string, string> | null;
};

export type RouteProviderInput = {
  id?: string | null;
  appProviderId: string;
  model?: string | null;
  attrs?: Record<string, string> | null;
};
