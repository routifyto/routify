import { useQuery } from '@tanstack/react-query';

import { axios, parseApiError } from '@/api/axios';
import {
  AnalyticsHistogramOutput,
  AnalyticsListsOutput,
  AnalyticsSummaryOutput,
} from '@/types/analytics';
import { ApiErrorOutput } from '@/types/errors';

export function useGetAnalyticsSummaryQuery(appId: string, period: string) {
  return useQuery<AnalyticsSummaryOutput, ApiErrorOutput>({
    queryKey: ['analytics-summary', appId, period],
    queryFn: async () => {
      try {
        const { data } = await axios.get<AnalyticsSummaryOutput>(
          `v1/apps/${appId}/analytics/summary`,
          {
            params: {
              period,
            },
          },
        );

        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
  });
}

export function useGetAnalyticsHistogramQuery(appId: string, period: string) {
  return useQuery<AnalyticsHistogramOutput, ApiErrorOutput>({
    queryKey: ['analytics-histogram', appId, period],
    queryFn: async () => {
      try {
        const { data } = await axios.get<AnalyticsHistogramOutput>(
          `v1/apps/${appId}/analytics/histogram`,
          {
            params: {
              period,
            },
          },
        );

        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
  });
}

export function useGetAnalyticsListsQuery(appId: string, period: string) {
  return useQuery<AnalyticsListsOutput, ApiErrorOutput>({
    queryKey: ['analytics-lists', appId, period],
    queryFn: async () => {
      try {
        const { data } = await axios.get<AnalyticsListsOutput>(
          `v1/apps/${appId}/analytics/lists`,
          {
            params: {
              period,
            },
          },
        );

        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
  });
}
