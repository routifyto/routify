import { useMutation } from '@tanstack/react-query';
import {
  EmailLoginInput,
  EmailRegisterInput,
  GoogleLoginInput,
  LoginPayload,
} from '@/types/accounts';
import { axios } from '@/api/axios';

export function useEmailLoginMutation() {
  return useMutation({
    mutationFn: async (input: EmailLoginInput) => {
      const { data } = await axios.post<LoginPayload>(
        'v1/accounts/login/email',
        input,
      );

      return data;
    },
  });
}

export function useEmailRegisterMutation() {
  return useMutation({
    mutationFn: async (input: EmailRegisterInput) => {
      const { data } = await axios.post<LoginPayload>(
        'v1/accounts/register/email',
        input,
      );

      return data;
    },
  });
}

export function useGoogleLoginMutation() {
  return useMutation({
    mutationFn: async (input: GoogleLoginInput) => {
      const { data } = await axios.post<LoginPayload>(
        'v1/accounts/login/google',
        input,
      );

      return data;
    },
  });
}
