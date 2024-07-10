import { useInfiniteQuery, useQuery } from '@tanstack/react-query';
import { axios } from '@/api/axios';
import { PaginatedPayload } from '@/types/common';
import { CompletionLogPayload, CompletionLogRowPayload } from '@/types/logs';

export function useGetCompletionLogsQuery(appId: string, limit?: number) {
  return useInfiniteQuery({
    queryKey: ['completion-logs', appId],
    initialPageParam: '',
    queryFn: async ({ pageParam }) => {
      const { data } = await axios.get<
        PaginatedPayload<CompletionLogRowPayload>
      >(`v1/apps/${appId}/logs/completions`, {
        params: {
          after: pageParam,
          limit,
        },
      });
      return data;
    },
    getNextPageParam: (lastPage) => lastPage.nextCursor,
  });
}

export function useGetCompletionLogQuery(
  appId: string,
  completionLogId: string,
) {
  return useQuery({
    queryKey: ['completion-log', completionLogId],
    queryFn: async () => {
      const { data } = await axios.get<CompletionLogPayload>(
        `v1/apps/${appId}/logs/completions/${completionLogId}`,
      );
      return data;
    },
  });
}
