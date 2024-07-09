import React from 'react';
import { providers } from '@/types/providers';
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover';
import { Button } from '@/components/ui/button';
import { cn } from '@/lib/utils';
import { Check, ChevronsUpDown } from 'lucide-react';
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from '@/components/ui/command';
import { useGetAppProvidersQuery } from '@/api/app-providers';
import { useApp } from '@/contexts/app';
import { Spinner } from '@/components/ui/spinner';

interface AppProviderSelectProps {
  value: string | null;
  onChange: (value: string) => void;
  className?: string;
}

export function AppProviderSelect({
  value,
  onChange,
  className,
}: AppProviderSelectProps) {
  const app = useApp();
  const [open, setOpen] = React.useState(false);
  const { data, isPending } = useGetAppProvidersQuery(app.id, 100);

  const currentAppProvider = data?.pages
    .flatMap((page) => page.items)
    .find((provider) => provider.id === value);

  let text = 'Select provider...';
  if (isPending) {
    text = 'Loading providers...';
  } else if (currentAppProvider) {
    text = currentAppProvider.name;
  }

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          role="combobox"
          aria-expanded={open}
          className={cn('flex w-full flex-row justify-between', className)}
        >
          {text}
          <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-128 p-0" align="start">
        <Command>
          {isPending ? (
            <CommandEmpty>
              <Spinner />
            </CommandEmpty>
          ) : (
            <React.Fragment>
              <CommandInput placeholder="Search provider..." />
              <CommandEmpty>No provider found.</CommandEmpty>
              <CommandList>
                <CommandGroup>
                  {data?.pages.map((page, index) => (
                    <React.Fragment key={index}>
                      {page.items.map((appProvider) => {
                        const provider = providers.find(
                          (provider) => provider.id === appProvider.provider,
                        );

                        if (!provider) {
                          return null;
                        }

                        return (
                          <CommandItem
                            key={appProvider.id}
                            value={`${appProvider.id} - ${appProvider.name}`}
                            onSelect={() => {
                              onChange(appProvider.id);
                              setOpen(false);
                            }}
                            className="flex flex-row items-center justify-between"
                          >
                            <div className="flex flex-row items-center gap-3 p-0.5">
                              <img
                                src={provider.logo}
                                className="h-10 w-10 rounded p-0.5 shadow"
                                alt={provider.name}
                              />
                              <div className="flex flex-col gap-1">
                                <p className="font-semibold">
                                  {appProvider.name}
                                </p>
                                <p className="text-xs text-muted-foreground">
                                  {appProvider.description}
                                </p>
                              </div>
                            </div>
                            <Check
                              className={cn(
                                'mr-2 h-4 w-4',
                                value === provider.id
                                  ? 'opacity-100'
                                  : 'opacity-0',
                              )}
                            />
                          </CommandItem>
                        );
                      })}
                    </React.Fragment>
                  ))}
                </CommandGroup>
              </CommandList>
            </React.Fragment>
          )}
        </Command>
      </PopoverContent>
    </Popover>
  );
}
