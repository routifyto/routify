export type AnalyticsSummaryPayload = {
  totalRequests: number;
  previousTotalRequests: number;
  totalTokens: number;
  previousTotalTokens: number;
  totalCost: number;
  previousTotalCost: number;
  averageDuration: number;
  previousAverageDuration: number;
};

export type AnalyticsHistogramPayload = {
  requests: DateTimeHistogramPayload[];
};

export type AnalyticsListsPayload = {
  providers: MetricsPayload[];
  models: MetricsPayload[];
};

export type DateTimeHistogramPayload = {
  dateTime: Date;
  count: number;
};

export type MetricsPayload = {
  id: string;
  totalRequests: number;
  totalTokens: number;
  totalCost: number;
  averageDuration: number;
};
