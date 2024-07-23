import path from 'path';

import react from '@vitejs/plugin-react';
import { defineConfig } from 'vite';
import fs from 'fs';
import dotenv from 'dotenv';

export default defineConfig(() => {
  // Load .env file if it exists
  const envFilePath = './.env';
  if (fs.existsSync(envFilePath)) {
    dotenv.config({ path: envFilePath });
  }
  return {
    plugins: [react()],
    resolve: {
      alias: {
        '@': path.resolve(__dirname, './src'),
      },
    },
    define: {
      VITE_API_URL: JSON.stringify(process.env.VITE_API_URL),
      VITE_GOOGLE_CLIENT_ID: JSON.stringify(process.env.VITE_GOOGLE_CLIENT_ID),
    },
  };
});
