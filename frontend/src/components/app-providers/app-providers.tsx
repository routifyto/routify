import React from 'react';
import { useApp } from '@/contexts/app';
import { Button } from '@/components/ui/button';
import { Plus } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import { AppProvidersTable } from '@/components/app-providers/app-providers-table';

export function AppProviders() {
  const app = useApp();
  const navigate = useNavigate();

  return (
    <div className="flex flex-col gap-4">
      <div className="flex flex-row items-center justify-between pb-4">
        <h1 className="text-2xl font-semibold">Providers</h1>
        <Button
          variant="outline"
          className="flex flex-row items-center gap-1"
          onClick={() => {
            navigate(`/${app.id}/providers/create`);
          }}
        >
          <Plus className="h-4 w-4" />
          Add provider
        </Button>
      </div>
      <AppProvidersTable
        onSelect={(appProviderId) => {
          navigate(`/${app.id}/providers/${appProviderId}`);
        }}
      />
    </div>
  );
}
