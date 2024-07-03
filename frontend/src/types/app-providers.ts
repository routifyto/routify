export type AppProviderPayload = {
  id: string;
  name: string;
  alias: string;
  provider: string;
  description?: string;
  attrs: Record<string, string>;
};

export type AppProviderInput = {
  name: string;
  alias: string;
  provider: string;
  description?: string;
  attrs: Record<string, string>;
};
