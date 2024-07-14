import { useMutation, useQueryClient } from '@tanstack/react-query';
import { AppInput, AppOutput } from '@/types/apps';
import { axios, parseApiError } from '@/api/axios';
import { WorkspaceOutput } from '@/types/workspaces';
import { ApiErrorOutput } from '@/types/errors';

export function useCreateAppMutation() {
  return useMutation<AppOutput, ApiErrorOutput, AppInput>({
    mutationFn: async (input: AppInput) => {
      try {
        const { data } = await axios.post<AppOutput>('v1/apps', input);
        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
  });
}

export function useUpdateAppMutation(appId: string) {
  const queryClient = useQueryClient();
  return useMutation<AppOutput, ApiErrorOutput, AppInput>({
    mutationFn: async (input: AppInput) => {
      try {
        const { data } = await axios.put<AppOutput>(`v1/apps/${appId}`, input);
        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
    onSuccess: (data) => {
      const workspace = queryClient.getQueryData<WorkspaceOutput>([
        'workspace',
      ]);
      if (!workspace) {
        return;
      }

      queryClient.setQueryData<WorkspaceOutput>(['workspace'], {
        ...workspace,
        apps: workspace.apps.map((app) => (app.id === data.id ? data : app)),
      });
    },
  });
}

export function useDeleteAppMutation(appId: string) {
  return useMutation<unknown, ApiErrorOutput, unknown>({
    mutationFn: async () => {
      try {
        await axios.delete(`v1/apps/${appId}`);
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
  });
}
