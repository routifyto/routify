import React from 'react';
import { ChevronsUpDown, Plus } from 'lucide-react';

import { Avatar } from '@/components/ui/avatar';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { useWorkspace } from '@/contexts/workspace';
import { useApp } from '@/contexts/app';
import { NavLink } from 'react-router-dom';

export function WorkspaceSidebarApps() {
  const workspace = useWorkspace();
  const app = useApp();

  return (
    <DropdownMenu key={app.id}>
      <DropdownMenuTrigger asChild>
        <div className="mb-1 flex h-14 cursor-pointer items-center justify-between border-b-2 border-gray-100 p-3 text-foreground/80 hover:bg-gray-50">
          <div className="flex items-center gap-2">
            <Avatar id={app.id} name={app.name} className="h-7 w-7" />
            <span className="flex-grow font-bold">{app.name}</span>
          </div>
          <ChevronsUpDown className="h-4 w-4" />
        </div>
      </DropdownMenuTrigger>
      <DropdownMenuContent className="ml-1 w-72">
        <DropdownMenuLabel>Apps</DropdownMenuLabel>
        <DropdownMenuGroup className="max-h-128 overflow-auto">
          {workspace.apps.map((app) => {
            return (
              <DropdownMenuItem key={app.id}>
                <NavLink
                  to={`/${app.id}`}
                  className="flex w-full flex-row items-center gap-3"
                >
                  <Avatar id={app.id} name={app.name} size="medium" />
                  <span className="flex-grow">{app.name}</span>
                </NavLink>
              </DropdownMenuItem>
            );
          })}
        </DropdownMenuGroup>
        <DropdownMenuSeparator />
        <DropdownMenuItem>
          <NavLink
            to="/create"
            className="flex w-full flex-row items-center gap-2"
          >
            <Plus className="h-4 w-4" />
            <span>Create app</span>
          </NavLink>
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
