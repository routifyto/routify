import {
  useInfiniteQuery,
  useMutation,
  useQuery,
  useQueryClient,
} from '@tanstack/react-query';
import { axios, parseApiError } from '@/api/axios';
import { PaginatedOutput } from '@/types/common';
import {
  ApiKeyInput,
  ApiKeyOutput,
  CreateApiKeyOutput,
} from '@/types/api-keys';
import { ApiErrorOutput } from '@/types/errors';

export function useGetApiKeysQuery(appId: string, limit?: number) {
  return useInfiniteQuery<PaginatedOutput<ApiKeyOutput>, ApiErrorOutput>({
    queryKey: ['api-keys', appId],
    initialPageParam: '',
    queryFn: async ({ pageParam }) => {
      try {
        const { data } = await axios.get<PaginatedOutput<ApiKeyOutput>>(
          `v1/apps/${appId}/api-keys`,
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

export function useGetApiKeyQuery(appId: string, apiKeyId: string) {
  return useQuery<ApiKeyOutput, ApiErrorOutput>({
    queryKey: ['api-key', apiKeyId],
    queryFn: async () => {
      try {
        const { data } = await axios.get<ApiKeyOutput>(
          `v1/apps/${appId}/api-keys/${apiKeyId}`,
        );
        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
  });
}

export function useCreateApiKeyMutation(appId: string) {
  const queryClient = useQueryClient();
  return useMutation<CreateApiKeyOutput, ApiErrorOutput, ApiKeyInput>({
    mutationFn: async (input: ApiKeyInput) => {
      try {
        const { data } = await axios.post<CreateApiKeyOutput>(
          `v1/apps/${appId}/api-keys`,
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
        queryKey: ['api-keys', appId],
      });

      if (data.apiKey) {
        queryClient.setQueryData(['api-key', data.apiKey.id], data.apiKey);
      }
    },
  });
}

export function useUpdateApiKeyMutation(appId: string, apiKeyId: string) {
  const queryClient = useQueryClient();
  return useMutation<ApiKeyOutput, ApiErrorOutput, ApiKeyInput>({
    mutationFn: async (input: ApiKeyInput) => {
      try {
        const { data } = await axios.put<ApiKeyOutput>(
          `v1/apps/${appId}/api-keys/${apiKeyId}`,
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
        queryKey: ['api-keys', appId],
      });

      queryClient.setQueryData(['api-key', apiKeyId], data);
    },
  });
}

export function useDeleteApiKeyMutation(appId: string, apiKeyId: string) {
  const queryClient = useQueryClient();
  return useMutation<unknown, ApiErrorOutput, unknown>({
    mutationFn: async () => {
      try {
        await axios.delete(`v1/apps/${appId}/api-keys/${apiKeyId}`);
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['api-keys', appId],
      });

      await queryClient.invalidateQueries({
        queryKey: ['api-key', apiKeyId],
      });
    },
  });
}
