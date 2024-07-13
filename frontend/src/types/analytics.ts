export type AnalyticsSummaryOutput = {
  totalRequests: number;
  previousTotalRequests: number;
  totalTokens: number;
  previousTotalTokens: number;
  totalCost: number;
  previousTotalCost: number;
  averageDuration: number;
  previousAverageDuration: number;
};

export type AnalyticsHistogramOutput = {
  requests: DateTimeHistogramOutput[];
};

export type AnalyticsListsOutput = {
  providers: MetricsOutput[];
  models: MetricsOutput[];
};

export type DateTimeHistogramOutput = {
  dateTime: Date;
  count: number;
};

export type MetricsOutput = {
  id: string;
  totalRequests: number;
  totalTokens: number;
  totalCost: number;
  averageDuration: number;
};
