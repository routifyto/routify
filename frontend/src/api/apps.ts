import { useMutation, useQueryClient } from '@tanstack/react-query';
import { AppInput, AppOutput } from '@/types/apps';
import { axios } from '@/api/axios';
import { Workspace } from '@/types/workspaces';

export function useCreateAppMutation() {
  return useMutation({
    mutationFn: async (input: AppInput) => {
      const { data } = await axios.post<AppOutput>('v1/apps', input);
      return data;
    },
  });
}

export function useUpdateAppMutation(appId: string) {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (input: AppInput) => {
      const { data } = await axios.put<AppOutput>(`v1/apps/${appId}`, input);
      return data;
    },
    onSuccess: (data) => {
      const workspace = queryClient.getQueryData<Workspace>(['workspace']);
      if (!workspace) {
        return;
      }

      queryClient.setQueryData<Workspace>(['workspace'], {
        ...workspace,
        apps: workspace.apps.map((app) => (app.id === data.id ? data : app)),
      });
    },
  });
}

export function useDeleteAppMutation(appId: string) {
  return useMutation({
    mutationFn: async () => {
      await axios.delete(`v1/apps/${appId}`);
    },
  });
}
