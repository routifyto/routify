import React from 'react';
import { Check, ChevronsUpDown } from 'lucide-react';

import { cn } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from '@/components/ui/command';
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover';
import { providers, Provider } from '@/types/providers';

interface ProviderSelectProps {
  value: string;
  onChange: (value: string) => void;
  className?: string;
}

function ProviderCard({ provider }: { provider: Provider }) {
  return (
    <div className="flex flex-row items-center gap-3 p-0.5">
      <img
        src={provider.logo}
        className="h-10 w-10 rounded p-0.5 shadow"
        alt={provider.name}
      />
      <div className="flex flex-col gap-1">
        <p className="font-semibold">{provider.name}</p>
        <p className="text-xs text-muted-foreground">{provider.description}</p>
      </div>
    </div>
  );
}

export function ProviderSelect({
  value,
  onChange,
  className,
}: ProviderSelectProps) {
  const [open, setOpen] = React.useState(false);
  const currentProvider = providers.find((provider) => provider.id === value);

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          role="combobox"
          aria-expanded={open}
          className={cn('flex w-full flex-row justify-between', className)}
        >
          {currentProvider ? currentProvider.name : 'Select provider...'}
          <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-128 p-0" align="start">
        <Command>
          <CommandInput placeholder="Search provider..." />
          <CommandEmpty>No framework found.</CommandEmpty>
          <CommandList>
            <CommandGroup>
              {providers.map((provider) => (
                <CommandItem
                  key={provider.id}
                  value={`${provider.id} - ${provider.name}`}
                  onSelect={() => {
                    onChange(provider.id);
                    setOpen(false);
                  }}
                  className="flex flex-row items-center justify-between"
                >
                  <ProviderCard provider={provider} />
                  <Check
                    className={cn(
                      'mr-2 h-4 w-4',
                      value === provider.id ? 'opacity-100' : 'opacity-0',
                    )}
                  />
                </CommandItem>
              ))}
            </CommandGroup>
          </CommandList>
        </Command>
      </PopoverContent>
    </Popover>
  );
}
