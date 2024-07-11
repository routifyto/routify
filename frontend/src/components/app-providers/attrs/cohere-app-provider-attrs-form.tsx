import React from 'react';
import { useFormContext } from 'react-hook-form';
import { AppProviderInput } from '@/types/app-providers';
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';

export function CohereAppProviderAttrsForm() {
  const form = useFormContext<AppProviderInput>();
  return (
    <React.Fragment>
      <FormField
        control={form.control}
        name="attrs.apiKey"
        render={({ field }) => (
          <FormItem className="flex-1">
            <FormLabel>API Key</FormLabel>
            <FormControl>
              <Input placeholder="API Key" {...field} />
            </FormControl>
            <FormMessage />
          </FormItem>
        )}
      />
    </React.Fragment>
  );
}
