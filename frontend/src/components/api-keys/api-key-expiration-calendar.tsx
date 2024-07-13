import React from 'react';
import { format } from 'date-fns';
import { CalendarIcon } from 'lucide-react';

import { Button } from '@/components/ui/button';
import { Calendar } from '@/components/ui/calendar';
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover';
import { cn } from '@/lib/utils';

interface ApiKeyExpirationCalendarProps {
  value: Date | null | undefined;
  onChange: (date: Date | null | undefined) => void;
}

export function ApiKeyExpirationCalendar({
  value,
  onChange,
}: ApiKeyExpirationCalendarProps) {
  return (
    <Popover>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          size="sm"
          className={cn(
            'w-full justify-start border-input text-left font-normal',
            !value && 'text-muted-foreground',
          )}
        >
          <CalendarIcon className="mr-2 h-4 w-4" />
          {value ? format(value, 'LLL dd, y') : <span>Never</span>}
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-auto p-0" align="start">
        <Calendar
          initialFocus
          mode="single"
          defaultMonth={value ?? new Date()}
          selected={value ?? undefined}
          onSelect={(date) => {
            if (date) {
              const dateOnly = new Date(
                Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()),
              );
              onChange(dateOnly);
            } else {
              onChange(null);
            }
          }}
          numberOfMonths={1}
        />
      </PopoverContent>
    </Popover>
  );
}
