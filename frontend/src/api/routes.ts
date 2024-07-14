import {
  useInfiniteQuery,
  useMutation,
  useQuery,
  useQueryClient,
} from '@tanstack/react-query';
import { axios, parseApiError } from '@/api/axios';
import {
  CreateRouteInput,
  UpdateRouteInput,
  RouteOutput,
} from '@/types/routes';
import { PaginatedOutput } from '@/types/common';
import { ApiErrorOutput } from '@/types/errors';

export function useGetRoutesQuery(appId: string, limit?: number) {
  return useInfiniteQuery<PaginatedOutput<RouteOutput>, ApiErrorOutput>({
    queryKey: ['routes', appId],
    initialPageParam: '',
    queryFn: async ({ pageParam }) => {
      try {
        const { data } = await axios.get<PaginatedOutput<RouteOutput>>(
          `v1/apps/${appId}/routes`,
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

export function useGetRouteQuery(appId: string, routeId: string) {
  return useQuery<RouteOutput, ApiErrorOutput>({
    queryKey: ['route', routeId],
    queryFn: async () => {
      try {
        const { data } = await axios.get<RouteOutput>(
          `v1/apps/${appId}/routes/${routeId}`,
        );
        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
  });
}

export function useCreateRouteMutation(appId: string) {
  const queryClient = useQueryClient();
  return useMutation<RouteOutput, ApiErrorOutput, CreateRouteInput>({
    mutationFn: async (input: CreateRouteInput) => {
      try {
        const { data } = await axios.post<RouteOutput>(
          `v1/apps/${appId}/routes`,
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
        queryKey: ['routes', appId],
      });

      queryClient.setQueryData(['route', data.id], data);
    },
  });
}

export function useUpdateRouteMutation(appId: string, routeId: string) {
  const queryClient = useQueryClient();
  return useMutation<RouteOutput, ApiErrorOutput, UpdateRouteInput>({
    mutationFn: async (input: UpdateRouteInput) => {
      try {
        const { data } = await axios.put<RouteOutput>(
          `v1/apps/${appId}/routes/${routeId}`,
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
        queryKey: ['routes', appId],
      });

      queryClient.setQueryData(['route', routeId], data);
    },
  });
}

export function useDeleteRouteMutation(appId: string, routeId: string) {
  const queryClient = useQueryClient();
  return useMutation<unknown, ApiErrorOutput, unknown>({
    mutationFn: async () => {
      try {
        await axios.delete(`v1/apps/${appId}/routes/${routeId}`);
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
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
