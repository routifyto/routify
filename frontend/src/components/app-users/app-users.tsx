import React from 'react';
import { useApp } from '@/contexts/app';
import { Button } from '@/components/ui/button';
import { Plus } from 'lucide-react';
import { AppUsersCreate } from '@/components/app-users/app-users-create';
import { AppUsersTable } from '@/components/app-users/app-users-table';

export function AppUsers() {
  const app = useApp();
  const [openCreate, setOpenCreate] = React.useState(false);

  return (
    <div className="flex flex-col gap-4">
      <div className="flex flex-row items-center justify-between pb-4">
        <h1 className="text-2xl font-semibold">Users</h1>
        <Button
          variant="outline"
          className="flex flex-row items-center gap-1"
          onClick={() => {
            setOpenCreate(true);
          }}
        >
          <Plus className="h-4 w-4" />
          Add users
        </Button>
      </div>
      <AppUsersTable />
      <AppUsersCreate
        appId={app.id}
        open={openCreate}
        onOpenChange={setOpenCreate}
      />
    </div>
  );
}
