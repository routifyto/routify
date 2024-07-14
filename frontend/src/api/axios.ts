import Axios, { isAxiosError } from 'axios';
import { readToken } from '@/lib/storage';
import { ApiErrorOutput } from '@/types/errors';

export const axios = Axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

const token = readToken();
axios.interceptors.request.use((config) => {
  config.headers['Content-Type'] = 'application/json';
  config.headers.Accept = 'application/json';

  if (token) {
    config.headers.authorization = `Bearer ${token}`;
  }

  return config;
});

export function parseApiError(error: unknown) {
  if (isAxiosError(error) && error.response) {
    if (
      error.response.data &&
      error.response.data.code &&
      error.response.data.message
    ) {
      return error.response.data as ApiErrorOutput;
    }

    if (error.response.status === 401) {
      return {
        code: 'UNAUTHORIZED',
        message: 'You are not authorized to perform this action',
      };
    }

    if (error.response.status === 403) {
      return {
        code: 'FORBIDDEN',
        message: 'You are forbidden from performing this action',
      };
    }

    if (error.response.status === 404) {
      return {
        code: 'NOT_FOUND',
        message: 'Resource not found',
      };
    }

    if (error.response.status === 400) {
      return {
        code: 'BAD_REQUEST',
        message: 'Bad request',
      };
    }
  }

  return {
    code: 'UNKNOWN',
    message: 'An unknown error occurred',
  };
}
