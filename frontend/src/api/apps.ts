import { useMutation } from '@tanstack/react-query';
import { AppInput, AppPayload } from '@/types/apps';
import { axios } from '@/api/axios';

export function useCreateAppMutation() {
  return useMutation({
    mutationFn: async (input: AppInput) => {
      const { data } = await axios.post<AppPayload>('v1/apps', input);

      return data;
    },
  });
}
