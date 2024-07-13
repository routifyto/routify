import React from 'react';
import { useApp } from '@/contexts/app';
import { Button } from '@/components/ui/button';
import { Plus } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import { ApiKeysTable } from '@/components/api-keys/api-keys-table';

export function ApiKeys() {
  const app = useApp();
  const navigate = useNavigate();

  return (
    <div className="flex flex-col gap-4">
      <div className="flex flex-row items-center justify-between pb-4">
        <h1 className="text-2xl font-semibold">API Keys</h1>
        <Button
          variant="outline"
          className="flex flex-row items-center gap-1"
          onClick={() => {
            navigate(`/${app.id}/api-keys/create`);
          }}
        >
          <Plus className="h-4 w-4" />
          Add API Key
        </Button>
      </div>
      <ApiKeysTable
        onSelect={(apiKeyId) => {
          navigate(`/${app.id}/api-keys/${apiKeyId}`);
        }}
      />
    </div>
  );
}
