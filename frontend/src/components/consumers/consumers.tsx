import React from 'react';
import { useApp } from '@/contexts/app';
import { Button } from '@/components/ui/button';
import { Plus } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import { ConsumersTable } from '@/components/consumers/consumers-table';

export function Consumers() {
  const app = useApp();
  const navigate = useNavigate();

  return (
    <div className="flex flex-col gap-4">
      <div className="flex flex-row items-center justify-between pb-4">
        <h1 className="text-2xl font-semibold">Consumers</h1>
        <Button
          variant="outline"
          className="flex flex-row items-center gap-1"
          onClick={() => {
            navigate(`/${app.id}/consumers/create`);
          }}
        >
          <Plus className="h-4 w-4" />
          Add consumer
        </Button>
      </div>
      <ConsumersTable
        onSelect={(consumerId) => {
          navigate(`/${app.id}/consumers/${consumerId}`);
        }}
      />
    </div>
  );
}
