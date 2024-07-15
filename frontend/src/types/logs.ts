export type LogRouteOutput = {
  id: string;
  name: string;
  description?: string | null;
  path: string;
};

export type RequestLogOutput = {
  url?: string | null;
  method?: string | null;
  headers?: Record<string, string> | null;
  body?: string | null;
};

export type ResponseLogOutput = {
  statusCode: number;
  headers?: Record<string, string> | null;
  body?: string | null;
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
  consumerId: string | null;
  inputTokens: number;
  outputTokens: number;
  inputCost: number;
  outputCost: number;
  startedAt: string;
  endedAt: string;
  duration: number;

  gatewayRequest: RequestLogOutput;
  providerRequest: RequestLogOutput | null;
  gatewayResponse: ResponseLogOutput | null;
  providerResponse: ResponseLogOutput | null;

  route: LogRouteOutput | null;
  appProvider: LogAppProviderOutput | null;
};
