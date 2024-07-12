import React from 'react';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { Button } from '@/components/ui/button';
import { ChevronDown } from 'lucide-react';
import { EnumOption } from '@/types/common';

const periodOptions: EnumOption[] = [
  {
    label: 'Today',
    value: 'today',
  },
  {
    label: 'Yesterday',
    value: 'yesterday',
  },
  {
    label: 'Last 7 days',
    value: '7days',
  },
  {
    label: 'Last 30 days',
    value: '30days',
  },
];

interface AnalyticsPeriodDropdownProps {
  value: string;
  onChange: (value: string) => void;
}

export function AnalyticsPeriodDropdown({
  value,
  onChange,
}: AnalyticsPeriodDropdownProps) {
  const selectedOption = periodOptions.find((m) => m.value === value);

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button
          variant="outline"
          size="sm"
          className="justify-start border-dashed text-left font-normal text-muted-foreground hover:text-foreground"
        >
          <span>{selectedOption?.label ?? ''}</span>
          <ChevronDown className="ml-2 h-4 w-4" />
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent className="text-xs" align="center">
        {periodOptions.map((option) => (
          <DropdownMenuItem
            key={option.value}
            onSelect={() => {
              onChange(option.value);
            }}
          >
            {option.label}
          </DropdownMenuItem>
        ))}
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
