'use client';

import { BellDot, ChevronsUpDown, LogOut, UserRoundCog } from 'lucide-react';
import * as React from 'react';

import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { Avatar } from '@/components/ui/avatar';
import { deleteToken } from '@/lib/storage';
import { useWorkspace } from '@/contexts/workspace';

export function WorkspaceSidebarUser() {
  const workspace = useWorkspace();

  const handleLogout = () => {
    deleteToken();
    window.location.href = '/login';
  };

  return (
    <DropdownMenu key={workspace.user.id}>
      <DropdownMenuTrigger asChild>
        <div className="mb-1 flex h-14 cursor-pointer items-center justify-between border-t-2 border-gray-100 p-3 text-foreground/80 hover:bg-gray-50">
          <div className="flex items-center gap-2">
            <Avatar
              id={workspace.user.id}
              name={workspace.user.name}
              className="h-7 w-7"
            />
            <span className="flex-grow">{workspace.user.name ?? 'Error'}</span>
          </div>
          <ChevronsUpDown className="h-4 w-4" />
        </div>
      </DropdownMenuTrigger>
      <DropdownMenuContent className="ml-1 w-72">
        <DropdownMenuItem disabled>
          <div className="flex flex-row items-center gap-2">
            <UserRoundCog className="h-4 w-4" />
            <span>Settings</span>
          </div>
        </DropdownMenuItem>
        <DropdownMenuItem disabled>
          <div className="flex flex-row items-center gap-2">
            <BellDot className="h-4 w-4" />
            <span>Notifications</span>
          </div>
        </DropdownMenuItem>
        <DropdownMenuItem onSelect={() => handleLogout()}>
          <div className="flex flex-row items-center gap-2 text-red-800">
            <LogOut className="h-4 w-4" />
            <span>Logout</span>
          </div>
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
