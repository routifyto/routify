import {
  useInfiniteQuery,
  useMutation,
  useQuery,
  useQueryClient,
} from '@tanstack/react-query';
import { axios, parseApiError } from '@/api/axios';
import { PaginatedOutput } from '@/types/common';
import { ApiErrorOutput } from '@/types/errors';
import { ConsumerInput, ConsumerOutput } from '@/types/consumers';

export function useGetConsumersQuery(appId: string, limit?: number) {
  return useInfiniteQuery<PaginatedOutput<ConsumerOutput>, ApiErrorOutput>({
    queryKey: ['consumers', appId],
    initialPageParam: '',
    queryFn: async ({ pageParam }) => {
      try {
        const { data } = await axios.get<PaginatedOutput<ConsumerOutput>>(
          `v1/apps/${appId}/consumers`,
          {
            params: {
              after: pageParam,
              limit,
            },
          },
        );
        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
    getNextPageParam: (lastPage) => lastPage.nextCursor,
  });
}

export function useGetConsumerQuery(appId: string, consumerId: string) {
  return useQuery<ConsumerOutput, ApiErrorOutput>({
    queryKey: ['consumer', consumerId],
    queryFn: async () => {
      try {
        const { data } = await axios.get<ConsumerOutput>(
          `v1/apps/${appId}/consumers/${consumerId}`,
        );
        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
  });
}

export function useCreateConsumerMutation(appId: string) {
  const queryClient = useQueryClient();
  return useMutation<ConsumerOutput, ApiErrorOutput, ConsumerInput>({
    mutationFn: async (input: ConsumerInput) => {
      try {
        const { data } = await axios.post<ConsumerOutput>(
          `v1/apps/${appId}/consumers`,
          input,
        );
        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
    onSuccess: async (data) => {
      await queryClient.invalidateQueries({
        queryKey: ['consumers', appId],
      });

      queryClient.setQueryData(['consumer', data.id], data);
    },
  });
}

export function useUpdateConsumerMutation(appId: string, consumerId: string) {
  const queryClient = useQueryClient();
  return useMutation<ConsumerOutput, ApiErrorOutput, ConsumerInput>({
    mutationFn: async (input: ConsumerInput) => {
      try {
        const { data } = await axios.put<ConsumerOutput>(
          `v1/apps/${appId}/consumers/${consumerId}`,
          input,
        );
        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
    onSuccess: async (data) => {
      await queryClient.invalidateQueries({
        queryKey: ['consumers', appId],
      });

      queryClient.setQueryData(['consumer', consumerId], data);
    },
  });
}

export function useDeleteConsumerMutation(appId: string, consumerId: string) {
  const queryClient = useQueryClient();
  return useMutation<unknown, ApiErrorOutput, unknown>({
    mutationFn: async () => {
      try {
        await axios.delete(`v1/apps/${appId}/consumers/${consumerId}`);
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['consumers', appId],
      });

      await queryClient.invalidateQueries({
        queryKey: ['consumer', consumerId],
      });
    },
  });
}
