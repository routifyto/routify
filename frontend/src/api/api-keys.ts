import {
  useInfiniteQuery,
  useMutation,
  useQuery,
  useQueryClient,
} from '@tanstack/react-query';
import { axios } from '@/api/axios';
import { PaginatedOutput } from '@/types/common';
import {
  ApiKeyInput,
  ApiKeyOutput,
  CreateApiKeyOutput,
} from '@/types/api-keys';

export function useGetApiKeysQuery(appId: string, limit?: number) {
  return useInfiniteQuery({
    queryKey: ['api-keys', appId],
    initialPageParam: '',
    queryFn: async ({ pageParam }) => {
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
    },
    getNextPageParam: (lastPage) => lastPage.nextCursor,
  });
}

export function useGetApiKeyQuery(appId: string, apiKeyId: string) {
  return useQuery({
    queryKey: ['api-key', apiKeyId],
    queryFn: async () => {
      const { data } = await axios.get<ApiKeyOutput>(
        `v1/apps/${appId}/api-keys/${apiKeyId}`,
      );
      return data;
    },
  });
}

export function useCreateApiKeyMutation(appId: string) {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (input: ApiKeyInput) => {
      const { data } = await axios.post<CreateApiKeyOutput>(
        `v1/apps/${appId}/api-keys`,
        input,
      );
      return data;
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
  return useMutation({
    mutationFn: async (input: ApiKeyInput) => {
      const { data } = await axios.put<ApiKeyOutput>(
        `v1/apps/${appId}/api-keys/${apiKeyId}`,
        input,
      );
      return data;
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
  return useMutation({
    mutationFn: async () => {
      await axios.delete(`v1/apps/${appId}/api-keys/${apiKeyId}`);
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
