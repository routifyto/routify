import React from 'react';
import { useParams } from 'react-router-dom';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { RouteUpdate } from '@/components/routes/route-update';
import { RouteDelete } from '@/components/routes/route-delete';

export function Route() {
  const { routeId } = useParams();

  if (!routeId) {
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
          <RouteUpdate routeId={routeId} />
        </TabsContent>
        <TabsContent value="manage" className="space-y-4">
          <RouteDelete routeId={routeId} />
        </TabsContent>
      </div>
    </Tabs>
  );
}
