import {
  useInfiniteQuery,
  useMutation,
  useQuery,
  useQueryClient,
} from '@tanstack/react-query';
import { axios } from '@/api/axios';
import {
  CreateRouteInput,
  UpdateRouteInput,
  RoutePayload,
} from '@/types/routes';
import { PaginatedPayload } from '@/types/common';

export function useGetRoutesQuery(appId: string, limit?: number) {
  return useInfiniteQuery({
    queryKey: ['routes', appId],
    initialPageParam: '',
    queryFn: async ({ pageParam }) => {
      const { data } = await axios.get<PaginatedPayload<RoutePayload>>(
        `v1/apps/${appId}/routes`,
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

export function useGetRouteQuery(appId: string, routeId: string) {
  return useQuery({
    queryKey: ['route', routeId],
    queryFn: async () => {
      const { data } = await axios.get<RoutePayload>(
        `v1/apps/${appId}/routes/${routeId}`,
      );
      return data;
    },
  });
}

export function useCreateRouteMutation(appId: string) {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (input: CreateRouteInput) => {
      const { data } = await axios.post<RoutePayload>(
        `v1/apps/${appId}/routes`,
        input,
      );
      return data;
    },
    onSuccess: async (data) => {
      await queryClient.invalidateQueries({
        queryKey: ['routes', appId],
      });

      queryClient.setQueryData(['route', data.id], data);
    },
  });
}

export function useUpdateRouteMutation(appId: string, routeId: string) {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (input: UpdateRouteInput) => {
      const { data } = await axios.put<RoutePayload>(
        `v1/apps/${appId}/routes/${routeId}`,
        input,
      );
      return data;
    },
    onSuccess: async (data) => {
      await queryClient.invalidateQueries({
        queryKey: ['routes', appId],
      });

      queryClient.setQueryData(['route', routeId], data);
    },
  });
}

export function useDeleteRouteMutation(appId: string, routeId: string) {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async () => {
      await axios.delete(`v1/apps/${appId}/routes/${routeId}`);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['routes', appId],
      });

      await queryClient.invalidateQueries({
        queryKey: ['route', routeId],
      });
    },
  });
}
