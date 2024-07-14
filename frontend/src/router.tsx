import React from 'react';
import { createBrowserRouter } from 'react-router-dom';

import { NotFound } from '@/components/not-found';
import { Login } from '@/components/login';
import { WorkspaceLayout } from '@/components/workspaces/workspace-layout';
import { AppCreate } from '@/components/apps/app-create';
import { WorkspaceRedirect } from '@/components/workspaces/workspace-redirect';
import { AppLayout } from '@/components/apps/app-layout';
import { AppSettings } from '@/components/apps/app-settings';
import { AppUsers } from '@/components/app-users/app-users';
import { AppProviders } from '@/components/app-providers/app-providers';
import { AppProviderCreate } from '@/components/app-providers/app-provider-create';
import { AppProvider } from '@/components/app-providers/app-provider';
import { Routes } from '@/components/routes/routes';
import { RouteCreate } from '@/components/routes/route-create';
import { Route } from '@/components/routes/route';
import { CompletionLogs } from '@/components/logs/completion-logs';
import { EmbeddingLogs } from '@/components/logs/embedding-logs';
import { AnalyticsDashboard } from '@/components/analytics/analytics-dashboard';
import { ApiKeys } from '@/components/api-keys/api-keys';
import { ApiKeyCreate } from '@/components/api-keys/api-key-create';
import { ApiKey } from '@/components/api-keys/api-key';
import { Consumers } from '@/components/consumers/consumers';
import { ConsumerCreate } from '@/components/consumers/consumer-create';
import { Consumer } from '@/components/consumers/consumer';

export const router = createBrowserRouter([
  {
    path: '',
    element: <WorkspaceLayout />,
    children: [
      {
        path: '',
        element: <WorkspaceRedirect />,
      },
      {
        path: '/create',
        element: <AppCreate />,
      },
      {
        path: '/:appId',
        element: <AppLayout />,
        children: [
          {
            path: '',
            element: <AnalyticsDashboard />,
          },
          {
            path: 'providers',
            children: [
              {
                path: '',
                element: <AppProviders />,
              },
              {
                path: 'create',
                element: <AppProviderCreate />,
              },
              {
                path: ':appProviderId',
                element: <AppProvider />,
              },
            ],
          },
          {
            path: 'routes',
            children: [
              {
                path: '',
                element: <Routes />,
              },
              {
                path: 'create',
                element: <RouteCreate />,
              },
              {
                path: ':routeId',
                element: <Route />,
              },
            ],
          },
          {
            path: 'consumers',
            children: [
              {
                path: '',
                element: <Consumers />,
              },
              {
                path: 'create',
                element: <ConsumerCreate />,
              },
              {
                path: ':consumerId',
                element: <Consumer />,
              },
            ],
          },
          {
            path: 'logs/completions',
            element: <CompletionLogs />,
          },
          {
            path: 'logs/embeddings',
            element: <EmbeddingLogs />,
          },
          {
            path: 'users',
            element: <AppUsers />,
          },
          {
            path: 'api-keys',
            children: [
              {
                path: '',
                element: <ApiKeys />,
              },
              {
                path: 'create',
                element: <ApiKeyCreate />,
              },
              {
                path: ':apiKeyId',
                element: <ApiKey />,
              },
            ],
          },
          {
            path: 'settings',
            element: <AppSettings />,
          },
        ],
      },
    ],
  },
  {
    path: '/login',
    element: <Login />,
  },
  {
    path: '*',
    element: <NotFound />,
  },
]);
