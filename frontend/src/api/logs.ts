import { useInfiniteQuery, useQuery } from '@tanstack/react-query';
import { axios } from '@/api/axios';
import { PaginatedPayload } from '@/types/common';
import { TextLogPayload, TextLogRowPayload } from '@/types/logs';

export function useGetTextLogsQuery(appId: string, limit?: number) {
  return useInfiniteQuery({
    queryKey: ['text-logs', appId],
    initialPageParam: '',
    queryFn: async ({ pageParam }) => {
      const { data } = await axios.get<PaginatedPayload<TextLogRowPayload>>(
        `v1/apps/${appId}/logs/text`,
        {
          params: {
            after: pageParam,
            limit,
          },
        },
      );
      return data;
    },
    getNextPageParam: (lastPage) => lastPage.nextCursor,
  });
}

export function useGetTextLogQuery(appId: string, textLogId: string) {
  return useQuery({
    queryKey: ['text-log', textLogId],
    queryFn: async () => {
      const { data } = await axios.get<TextLogPayload>(
        `v1/apps/${appId}/logs/text/${textLogId}`,
      );
      return data;
    },
  });
}
