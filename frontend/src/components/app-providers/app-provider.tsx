import React from 'react';
import { useParams } from 'react-router-dom';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { AppProviderUpdate } from '@/components/app-providers/app-provider-update';
import { AppProviderDelete } from '@/components/app-providers/app-provider-delete';

export function AppProvider() {
  const { appProviderId } = useParams();

  if (!appProviderId) {
    return null;
  }

  return (
    <Tabs defaultValue="configuration" className="space-y-4">
      <TabsList>
        <TabsTrigger value="configuration">Configuration</TabsTrigger>
        <TabsTrigger value="manage">Manage</TabsTrigger>
      </TabsList>
      <div className="px-1">
        <TabsContent value="configuration" className="space-y-4">
          <AppProviderUpdate appProviderId={appProviderId} />
        </TabsContent>
        <TabsContent value="manage" className="space-y-4">
          <AppProviderDelete appProviderId={appProviderId} />
        </TabsContent>
      </div>
    </Tabs>
  );
}
