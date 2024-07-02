import { ChevronDown } from 'lucide-react';
import { useEffect } from 'react';

import { Button } from '@/components/ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { cn } from '@/lib/utils';
import { EnumOption } from '@/types/common';

const EnumDropdown = ({
  options,
  value,
  onValueChange,
  className,
}: {
  options: EnumOption[];
  value: string;
  onValueChange: (value: string) => void;
  className?: string;
}) => {
  const selectedOption = options.find((m) => m.value === value);

  useEffect(() => {
    if (!selectedOption && options.length > 0) {
      onValueChange(options[0].value);
    }
  });

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button
          variant="outline"
          size="sm"
          className="h-9 w-full justify-between"
        >
          <span>{selectedOption?.label ?? ''}</span>
          <ChevronDown className="h-4 w-4" />
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent className={cn('text-xs', className)} align="start">
        {options.map((option) => (
          <DropdownMenuItem
            key={option.value}
            onSelect={() => {
              onValueChange(option.value);
            }}
          >
            {option.label}
          </DropdownMenuItem>
        ))}
      </DropdownMenuContent>
    </DropdownMenu>
  );
};

export { EnumDropdown };
