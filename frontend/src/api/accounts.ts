import { useMutation } from '@tanstack/react-query';
import {
  EmailLoginInput,
  EmailRegisterInput,
  GoogleLoginInput,
  LoginOutput,
} from '@/types/accounts';
import { axios, parseApiError } from '@/api/axios';
import { ApiErrorOutput } from '@/types/errors';

export function useEmailLoginMutation() {
  return useMutation<LoginOutput, ApiErrorOutput, EmailLoginInput>({
    mutationFn: async (input: EmailLoginInput) => {
      try {
        const { data } = await axios.post<LoginOutput>(
          'v1/accounts/login/email',
          input,
        );

        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
  });
}

export function useEmailRegisterMutation() {
  return useMutation<LoginOutput, ApiErrorOutput, EmailRegisterInput>({
    mutationFn: async (input: EmailRegisterInput) => {
      try {
        const { data } = await axios.post<LoginOutput>(
          'v1/accounts/register/email',
          input,
        );

        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
  });
}

export function useGoogleLoginMutation() {
  return useMutation<LoginOutput, ApiErrorOutput, GoogleLoginInput>({
    mutationFn: async (input: GoogleLoginInput) => {
      try {
        const { data } = await axios.post<LoginOutput>(
          'v1/accounts/login/google',
          input,
        );

        return data;
      } catch (error) {
        const apiError = parseApiError(error);
        return Promise.reject(apiError);
      }
    },
  });
}
