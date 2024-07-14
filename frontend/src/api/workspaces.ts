import { useQuery } from '@tanstack/react-query';

import { axios, parseApiError } from '@/api/axios';
import { WorkspaceOutput } from '@/types/workspaces';
import { ApiErrorOutput } from '@/types/errors';

export function useGetWorkspaceQuery() {
  return useQuery<WorkspaceOutput, ApiErrorOutput>({
    queryKey: ['workspace'],
    queryFn: async () => {
      try {
        const { data } = await axios.get<WorkspaceOutput>(`v1/workspace`);
        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
  });
}
