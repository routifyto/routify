import { useQuery } from '@tanstack/react-query';

import { axios } from '@/api/axios';
import {
  AnalyticsHistogramOutput,
  AnalyticsListsOutput,
  AnalyticsSummaryOutput,
} from '@/types/analytics';

export function useGetAnalyticsSummaryQuery(appId: string, period: string) {
  return useQuery({
    queryKey: ['analytics-summary', appId, period],
    queryFn: async () => {
      const { data } = await axios.get<AnalyticsSummaryOutput>(
        `v1/apps/${appId}/analytics/summary`,
        {
          params: {
            period,
          },
        },
      );

      return data;
    },
  });
}

export function useGetAnalyticsHistogramQuery(appId: string, period: string) {
  return useQuery({
    queryKey: ['analytics-histogram', appId, period],
    queryFn: async () => {
      const { data } = await axios.get<AnalyticsHistogramOutput>(
        `v1/apps/${appId}/analytics/histogram`,
        {
          params: {
            period,
          },
        },
      );

      return data;
    },
  });
}

export function useGetAnalyticsListsQuery(appId: string, period: string) {
  return useQuery({
    queryKey: ['analytics-lists', appId, period],
    queryFn: async () => {
      const { data } = await axios.get<AnalyticsListsOutput>(
        `v1/apps/${appId}/analytics/lists`,
        {
          params: {
            period,
          },
        },
      );

      return data;
    },
  });
}
