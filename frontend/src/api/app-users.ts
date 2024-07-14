import {
  useInfiniteQuery,
  useMutation,
  useQueryClient,
} from '@tanstack/react-query';
import { axios, parseApiError } from '@/api/axios';
import { PaginatedOutput } from '@/types/common';
import {
  AppUserInput,
  AppUserOutput,
  AppUsersInput,
  AppUsersOutput,
} from '@/types/app-users';
import { ApiErrorOutput } from '@/types/errors';

export function useGetAppUsersQuery(appId: string, limit?: number) {
  return useInfiniteQuery<PaginatedOutput<AppUserOutput>, ApiErrorOutput>({
    queryKey: ['app-users', appId],
    initialPageParam: '',
    queryFn: async ({ pageParam }) => {
      try {
        const { data } = await axios.get<PaginatedOutput<AppUserOutput>>(
          `v1/apps/${appId}/users`,
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

export function useCreateAppUsersMutation(appId: string) {
  const queryClient = useQueryClient();
  return useMutation<AppUsersOutput, ApiErrorOutput, AppUsersInput>({
    mutationFn: async (input: AppUsersInput) => {
      try {
        const { data } = await axios.post<AppUsersOutput>(
          `v1/apps/${appId}/users`,
          input,
        );
        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ['app-users', appId] });
    },
  });
}

export function useUpdateAppUserMutation(appId: string, appUserId: string) {
  const queryClient = useQueryClient();
  return useMutation<AppUserOutput, ApiErrorOutput, AppUserInput>({
    mutationFn: async (input: AppUserInput) => {
      try {
        const { data } = await axios.put<AppUserOutput>(
          `v1/apps/${appId}/users/${appUserId}`,
          input,
        );
        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ['app-users', appId] });
    },
  });
}

export function useDeleteAppUserMutation(appId: string, appUserId: string) {
  const queryClient = useQueryClient();
  return useMutation<unknown, ApiErrorOutput, unknown>({
    mutationFn: async () => {
      try {
        await axios.delete(`v1/apps/${appId}/users/${appUserId}`);
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ['app-users', appId] });
    },
  });
}
