import { useQuery } from '@tanstack/react-query';

import { axios } from '@/api/axios';
import { Workspace } from '@/types/workspaces';

export function useGetWorkspaceQuery() {
  return useQuery({
    queryKey: ['workspace'],
    queryFn: async () => {
      const { data } = await axios.get<Workspace>(`v1/workspace`);
      return data;
    },
  });
}
