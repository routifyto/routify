import React from 'react';
import { useParams } from 'react-router-dom';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { ConsumerUpdate } from '@/components/consumers/consumer-update';
import { ConsumerDelete } from '@/components/consumers/consumer-delete';

export function Consumer() {
  const { consumerId } = useParams();

  if (!consumerId) {
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
          <ConsumerUpdate consumerId={consumerId} />
        </TabsContent>
        <TabsContent value="manage" className="space-y-4">
          <ConsumerDelete consumerId={consumerId} />
        </TabsContent>
      </div>
    </Tabs>
  );
}
