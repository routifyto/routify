import React from 'react';
import { AppUpdate } from '@/components/apps/app-update';
import { AppDelete } from '@/components/apps/app-delete';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';

export function AppSettings() {
  return (
    <Tabs defaultValue="general" className="space-y-4">
      <TabsList>
        <TabsTrigger value="general">General</TabsTrigger>
        <TabsTrigger value="manage">Manage</TabsTrigger>
      </TabsList>
      <div className="px-1">
        <TabsContent value="general" className="space-y-4">
          <AppUpdate />
        </TabsContent>
        <TabsContent value="manage" className="space-y-4">
          <AppDelete />
        </TabsContent>
      </div>
    </Tabs>
  );
}
