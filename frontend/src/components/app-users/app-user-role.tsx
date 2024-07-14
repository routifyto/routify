import React from 'react';
import { ChevronDown } from 'lucide-react';

import { useUpdateAppUserMutation } from '@/api/app-users';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { Spinner } from '@/components/ui/spinner';
import { toast } from '@/components/ui/use-toast';
import { useApp } from '@/contexts/app';
import { appUserRoleOptions } from '@/types/app-users';
import { type AppUserOutput } from '@/types/app-users';
import { Button } from '@/components/ui/button';

export function AppUserRole({ appUser }: { appUser: AppUserOutput }) {
  const app = useApp();
  const { mutate, isPending } = useUpdateAppUserMutation(app.id, appUser.id);
  const appUserRole = appUserRoleOptions.find((m) => m.value === appUser.role);

  if (!appUserRole) {
    return null;
  }

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant="outline" className="rounded-md" size="sm">
          <span className="mr-3">{appUserRole.label}</span>
          {isPending ? (
            <Spinner className="h-4 w-4 text-muted-foreground" />
          ) : (
            <ChevronDown className="h-4 w-4" />
          )}
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent className="text-xs">
        {appUserRoleOptions.map((option) => (
          <DropdownMenuItem
            key={option.value}
            disabled={
              isPending ||
              option.value === appUser.role ||
              option.value === 'OWNER'
            }
            onSelect={() => {
              if (option.value !== 'MEMBER' && option.value !== 'ADMIN') {
                return;
              }

              if (option.value === appUser.role) {
                return;
              }

              mutate(
                {
                  role: option.value,
                },
                {
                  onSuccess: () => {
                    toast({
                      title: 'User role updated',
                      description: 'The user role was updated successfully',
                      variant: 'default',
                    });
                  },
                  onError: (error) => {
                    toast({
                      title: 'Failed to update user role',
                      description: error.message,
                      variant: 'destructive',
                    });
                  },
                },
              );
            }}
          >
            {option.label}
          </DropdownMenuItem>
        ))}
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
