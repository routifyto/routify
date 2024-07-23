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
import { toast } from '@/components/ui/use-toast';
import { SecretInput } from '@/components/ui/secret-input';

export function AzureOpenaiAppProviderAttrsForm() {
  const form = useFormContext<AppProviderInput>();
  return (
    <React.Fragment>
      <FormField
        control={form.control}
        name="attrs.endpoint"
        render={({ field }) => (
          <FormItem className="flex-1">
            <FormLabel>Endpoint</FormLabel>
            <FormControl>
              <Input placeholder="Endpoint" {...field} />
            </FormControl>
            <FormMessage />
          </FormItem>
        )}
      />
      <FormField
        control={form.control}
        name="attrs.apiKey"
        render={({ field }) => (
          <FormItem className="flex-1">
            <FormLabel>API Key</FormLabel>
            <FormControl>
              <SecretInput
                placeholder="API Key"
                {...field}
                onCopy={() => {
                  navigator.clipboard.writeText(field.value).then(() => {
                    toast({
                      description: 'API Key copied to clipboard',
                    });
                  });
                }}
              />
            </FormControl>
            <FormMessage />
          </FormItem>
        )}
      />
    </React.Fragment>
  );
}
