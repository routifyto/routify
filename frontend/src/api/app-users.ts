import {
  useInfiniteQuery,
  useMutation,
  useQueryClient,
} from '@tanstack/react-query';
import { axios } from '@/api/axios';
import { PaginatedPayload } from '@/types/common';
import { AppUserInput, AppUserPayload, AppUsersInput } from '@/types/app-users';

export function useGetAppUsersQuery(appId: string, limit?: number) {
  return useInfiniteQuery({
    queryKey: ['app-users', appId],
    initialPageParam: '',
    queryFn: async ({ pageParam }) => {
      const { data } = await axios.get<PaginatedPayload<AppUserPayload>>(
        `v1/apps/${appId}/users`,
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

export function useCreateAppUsersMutation(appId: string) {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (input: AppUsersInput) => {
      const { data } = await axios.post<AppUserPayload>(
        `v1/apps/${appId}/users`,
        input,
      );
      return data;
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ['app-users', appId] });
    },
  });
}

export function useUpdateAppUserMutation(appId: string, appUserId: string) {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (input: AppUserInput) => {
      const { data } = await axios.put<AppUserPayload>(
        `v1/apps/${appId}/users/${appUserId}`,
        input,
      );
      return data;
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ['app-users', appId] });
    },
  });
}

export function useDeleteAppUserMutation(appId: string, appUserId: string) {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async () => {
      await axios.delete(`v1/apps/${appId}/users/${appUserId}`);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ['app-users', appId] });
    },
  });
}
