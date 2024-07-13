import React from 'react';
import { useParams } from 'react-router-dom';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { ApiKeyUpdate } from '@/components/api-keys/api-key-update';
import { ApiKeyDelete } from '@/components/api-keys/api-key-delete';

export function ApiKey() {
  const { apiKeyId } = useParams();

  if (!apiKeyId) {
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
          <ApiKeyUpdate apiKeyId={apiKeyId} />
        </TabsContent>
        <TabsContent value="manage" className="space-y-4">
          <ApiKeyDelete apiKeyId={apiKeyId} />
        </TabsContent>
      </div>
    </Tabs>
  );
}
