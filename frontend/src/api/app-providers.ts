import {
  useInfiniteQuery,
  useMutation,
  useQuery,
  useQueryClient,
} from '@tanstack/react-query';
import { axios, parseApiError } from '@/api/axios';
import { AppProviderInput, AppProviderOutput } from '@/types/app-providers';
import { PaginatedOutput } from '@/types/common';
import { ApiErrorOutput } from '@/types/errors';

export function useGetAppProvidersQuery(appId: string, limit?: number) {
  return useInfiniteQuery<PaginatedOutput<AppProviderOutput>, ApiErrorOutput>({
    queryKey: ['app-providers', appId],
    initialPageParam: '',
    queryFn: async ({ pageParam }) => {
      try {
        const { data } = await axios.get<PaginatedOutput<AppProviderOutput>>(
          `v1/apps/${appId}/providers`,
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

export function useGetAppProviderQuery(appId: string, appProviderId: string) {
  return useQuery<AppProviderOutput, ApiErrorOutput>({
    queryKey: ['app-provider', appProviderId],
    queryFn: async () => {
      try {
        const { data } = await axios.get<AppProviderOutput>(
          `v1/apps/${appId}/providers/${appProviderId}`,
        );
        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
  });
}

export function useCreateAppProviderMutation(appId: string) {
  const queryClient = useQueryClient();
  return useMutation<AppProviderOutput, ApiErrorOutput, AppProviderInput>({
    mutationFn: async (input: AppProviderInput) => {
      try {
        const { data } = await axios.post<AppProviderOutput>(
          `v1/apps/${appId}/providers`,
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
        queryKey: ['app-providers', appId],
      });

      queryClient.setQueryData(['app-provider', data.id], data);
    },
  });
}

export function useUpdateAppProviderMutation(
  appId: string,
  appProviderId: string,
) {
  const queryClient = useQueryClient();
  return useMutation<AppProviderOutput, ApiErrorOutput, AppProviderInput>({
    mutationFn: async (input: AppProviderInput) => {
      try {
        const { data } = await axios.put<AppProviderOutput>(
          `v1/apps/${appId}/providers/${appProviderId}`,
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
        queryKey: ['app-providers', appId],
      });

      queryClient.setQueryData(['app-provider', appProviderId], data);
    },
  });
}

export function useDeleteAppProviderMutation(
  appId: string,
  appProviderId: string,
) {
  const queryClient = useQueryClient();
  return useMutation<unknown, ApiErrorOutput, unknown>({
    mutationFn: async () => {
      try {
        await axios.delete(`v1/apps/${appId}/providers/${appProviderId}`);
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['app-providers', appId],
      });

      await queryClient.invalidateQueries({
        queryKey: ['app-provider', appProviderId],
      });
    },
  });
}
