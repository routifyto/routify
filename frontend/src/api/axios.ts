import Axios from 'axios';
import { readToken } from '@/lib/storage';

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
