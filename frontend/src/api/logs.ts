import { useInfiniteQuery, useQuery } from '@tanstack/react-query';
import { axios, parseApiError } from '@/api/axios';
import { PaginatedOutput } from '@/types/common';
import {
  CompletionLogOutput,
  CompletionLogRowOutput,
  CompletionOutgoingLogOutput,
} from '@/types/logs';
import { ApiErrorOutput } from '@/types/errors';

export function useGetCompletionLogsQuery(appId: string, limit?: number) {
  return useInfiniteQuery<
    PaginatedOutput<CompletionLogRowOutput>,
    ApiErrorOutput
  >({
    queryKey: ['completion-logs', appId],
    initialPageParam: '',
    queryFn: async ({ pageParam }) => {
      try {
        const { data } = await axios.get<
          PaginatedOutput<CompletionLogRowOutput>
        >(`v1/apps/${appId}/logs/completions`, {
          params: {
            after: pageParam,
            limit,
          },
        });
        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
    getNextPageParam: (lastPage) => lastPage.nextCursor,
  });
}

export function useGetCompletionLogQuery(
  appId: string,
  completionLogId: string,
) {
  return useQuery<CompletionLogOutput, ApiErrorOutput>({
    queryKey: ['completion-log', completionLogId],
    queryFn: async () => {
      try {
        const { data } = await axios.get<CompletionLogOutput>(
          `v1/apps/${appId}/logs/completions/${completionLogId}`,
        );
        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
  });
}

export function useGetCompletionOutgoingLogsQuery(
  appId: string,
  completionLogId: string,
) {
  return useQuery<CompletionOutgoingLogOutput[], ApiErrorOutput>({
    queryKey: ['completion-outgoing-logs', completionLogId],
    queryFn: async () => {
      try {
        const { data } = await axios.get<CompletionOutgoingLogOutput[]>(
          `v1/apps/${appId}/logs/completions/${completionLogId}/outgoing`,
        );
        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
  });
}
