import {
  Boxes,
  Contact,
  Home,
  Pilcrow,
  Route,
  Settings,
  Users,
  WholeWord,
} from 'lucide-react';
import { NavLink, useLocation } from 'react-router-dom';
import React, { ReactNode } from 'react';

import { ScrollArea } from '@/components/ui/scroll-area';
import { WorkspaceSidebarUser } from '@/components/workspaces/workspace-sidebar-user';
import { WorkspaceSidebarApps } from '@/components/workspaces/workspace-sidebar-apps';
import { cn } from '@/lib/utils';
import { useApp } from '@/contexts/app';

interface LinkItemGroup {
  label: string;
  links: LinkItem[];
}

interface LinkItem {
  label: string;
  to: string;
  icon: ReactNode;
  external?: boolean;
}

const iconClass = 'h-5 w-5';
const linkGroups: LinkItemGroup[] = [
  {
    label: 'Main',
    links: [
      {
        label: 'Dashboard',
        to: '',
        icon: <Home className={iconClass} />,
      },
      {
        label: 'Routes',
        to: 'routes',
        icon: <Route className={iconClass} />,
      },
      {
        label: 'Providers',
        to: 'providers',
        icon: <Boxes className={iconClass} />,
      },
      {
        label: 'Customers',
        to: 'customers',
        icon: <Contact className={iconClass} />,
      },
    ],
  },
  {
    label: 'Logs',
    links: [
      {
        label: 'Text',
        to: 'logs/text',
        icon: <Pilcrow className={iconClass} />,
      },
      {
        label: 'Embedding',
        to: 'logs/embedding',
        icon: <WholeWord className={iconClass} />,
      },
    ],
  },
  {
    label: 'Settings',
    links: [
      {
        label: 'Settings',
        to: 'settings',
        icon: <Settings className={iconClass} />,
      },
      {
        label: 'Users',
        to: 'users',
        icon: <Users className={iconClass} />,
      },
    ],
  },
];

export function WorkspaceSidebar() {
  const app = useApp();
  const { pathname } = useLocation();

  return (
    <aside className="flex h-full w-full flex-col border-r">
      <WorkspaceSidebarApps />
      <ScrollArea className="h-full flex-1 px-2">
        {linkGroups.map((group) => (
          <div key={group.label} className="mb-2 flex w-full flex-col">
            <div className="p-1 text-sm font-semibold">{group.label}</div>
            {group.links.map((linkItem) => {
              let link = `/${app.id}`;
              if (linkItem.to.length > 0) link += `/${linkItem.to}`;

              const isActive = pathname === link;
              return (
                <NavLink
                  key={linkItem.to}
                  to={link}
                  target={linkItem.external ? '_blank' : undefined}
                  className={cn(
                    'flex cursor-pointer items-center gap-1 rounded-md p-1.5 px-2 text-sm text-foreground/70 hover:bg-gray-50',
                    isActive && 'bg-gray-50 font-semibold text-primary',
                  )}
                >
                  {linkItem.icon}
                  <span className="line-clamp-1 w-full flex-grow pl-2 text-left">
                    {linkItem.label}
                  </span>
                </NavLink>
              );
            })}
          </div>
        ))}
      </ScrollArea>
      <WorkspaceSidebarUser />
    </aside>
  );
}
