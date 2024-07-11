import React from 'react';
import { Provider } from '@/types/providers';
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

interface ModelSelectProps {
  provider: Provider;
  value: string | null;
  onChange: (value: string) => void;
  className?: string;
}

export function ModelSelect({
  provider,
  value,
  onChange,
  className,
}: ModelSelectProps) {
  const [open, setOpen] = React.useState(false);
  const currentModel = provider.models.find((model) => model.id === value);

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          role="combobox"
          aria-expanded={open}
          className={cn('flex w-full flex-row justify-between', className)}
        >
          {currentModel ? currentModel.name : 'No model specified'}
          <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-128 p-0" align="start">
        <Command>
          <CommandInput placeholder="Search model..." />
          <CommandEmpty>No model found.</CommandEmpty>
          <CommandList>
            <CommandGroup>
              {provider.models.map((model) => (
                <CommandItem
                  key={model.id}
                  value={`${model.id} - ${model.name}`}
                  onSelect={() => {
                    onChange(model.id);
                    setOpen(false);
                  }}
                  className="flex flex-row items-center justify-between"
                >
                  <div className="flex flex-col gap-1">
                    <p className="font-semibold">{model.name}</p>
                    <p className="text-xs text-muted-foreground">
                      {model.description}
                    </p>
                    <p className="mt-1 text-xs text-muted-foreground">
                      Context window: {model.contextWindow.toLocaleString()}
                    </p>
                  </div>
                  <Check
                    className={cn(
                      'min-h-4 min-w-4 h-4 w-4',
                      value === model.id ? 'opacity-100' : 'opacity-0',
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
