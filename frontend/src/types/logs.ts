export type LogRouteOutput = {
  id: string;
  name: string;
  description?: string | null;
  path: string;
};

export type LogAppProviderOutput = {
  id: string;
  name: string;
  alias: string;
  description?: string | null;
};

export type CompletionLogRowOutput = {
  id: string;
  routeId: string;
  path: string;
  provider: string | null;
  model: string | null;
  inputTokens: number;
  outputTokens: number;
  inputCost: number;
  outputCost: number;
  endedAt: string;
  duration: number;
};

export type CompletionLogOutput = {
  id: string;
  routeId: string;
  path: string;
  provider: string | null;
  model: string | null;
  appProviderId: string | null;
  routeProviderId: string;
  apiKeyId: string;
  sessionId: string | null;
  requestBody: string;
  responseStatusCode: number;
  responseBody: string;
  inputTokens: number;
  outputTokens: number;
  inputCost: number;
  outputCost: number;
  startedAt: string;
  endedAt: string;
  duration: number;

  route: LogRouteOutput | null;
  appProvider: LogAppProviderOutput | null;
};
