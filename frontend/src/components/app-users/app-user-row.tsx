import { ChevronDown } from 'lucide-react';

import { useUpdateAppUserMutation } from '@/api/app-users';
import { AppUserDelete } from '@/components/app-users/app-user-delete';
import {
  Card,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { Spinner } from '@/components/ui/spinner';
import { toast } from '@/components/ui/use-toast';
import { Avatar } from '@/components/ui/avatar';
import { useApp } from '@/contexts/app';
import { appUserRoleOptions } from '@/types/app-users';
import { type AppUserPayload } from '@/types/app-users';

export function AppUserRow({ appUser }: { appUser: AppUserPayload }) {
  const app = useApp();
  const { mutate, isPending } = useUpdateAppUserMutation(app.id, appUser.id);
  const appUserRole = appUserRoleOptions.find((m) => m.value === appUser.role);

  if (!appUserRole) {
    return null;
  }

  return (
    <Card className="flex flex-row items-center gap-3 p-4">
      <Avatar id={appUser.userId} name={appUser.name} />
      <CardHeader className="flex-1 p-1">
        <CardTitle>{appUser.name}</CardTitle>
        <CardDescription>{appUser.email}</CardDescription>
      </CardHeader>
      <DropdownMenu>
        <DropdownMenuTrigger asChild>
          <p className="flex cursor-pointer flex-row items-center gap-1 rounded-md p-2 text-sm text-foreground/80 hover:bg-gray-50">
            <span>{appUserRole.label}</span>
            {isPending ? (
              <Spinner className="h-4 w-4 text-muted-foreground" />
            ) : (
              <ChevronDown className="h-4 w-4" />
            )}
          </p>
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
                        title: 'User updated',
                        description: 'The user was updated successfully',
                        variant: 'default',
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
      <AppUserDelete appUserId={appUser.id} />
    </Card>
  );
}
