import {
  useInfiniteQuery,
  useMutation,
  useQuery,
  useQueryClient,
} from '@tanstack/react-query';
import { axios } from '@/api/axios';
import { AppProviderInput, AppProviderOutput } from '@/types/app-providers';
import { PaginatedOutput } from '@/types/common';

export function useGetAppProvidersQuery(appId: string, limit?: number) {
  return useInfiniteQuery({
    queryKey: ['app-providers', appId],
    initialPageParam: '',
    queryFn: async ({ pageParam }) => {
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
    },
    getNextPageParam: (lastPage) => lastPage.nextCursor,
  });
}

export function useGetAppProviderQuery(appId: string, appProviderId: string) {
  return useQuery({
    queryKey: ['app-provider', appProviderId],
    queryFn: async () => {
      const { data } = await axios.get<AppProviderOutput>(
        `v1/apps/${appId}/providers/${appProviderId}`,
      );
      return data;
    },
  });
}

export function useCreateAppProviderMutation(appId: string) {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (input: AppProviderInput) => {
      const { data } = await axios.post<AppProviderOutput>(
        `v1/apps/${appId}/providers`,
        input,
      );
      return data;
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
  return useMutation({
    mutationFn: async (input: AppProviderInput) => {
      const { data } = await axios.put<AppProviderOutput>(
        `v1/apps/${appId}/providers/${appProviderId}`,
        input,
      );
      return data;
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
  return useMutation({
    mutationFn: async () => {
      await axios.delete(`v1/apps/${appId}/providers/${appProviderId}`);
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
