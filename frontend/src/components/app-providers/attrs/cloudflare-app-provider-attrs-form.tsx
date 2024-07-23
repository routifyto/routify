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

export function CloudflareAppProviderAttrsForm() {
  const form = useFormContext<AppProviderInput>();
  return (
    <React.Fragment>
      <FormField
        control={form.control}
        name="attrs.accountId"
        render={({ field }) => (
          <FormItem className="flex-1">
            <FormLabel>Account ID</FormLabel>
            <FormControl>
              <Input placeholder="Account ID" {...field} />
            </FormControl>
            <FormMessage />
          </FormItem>
        )}
      />
      <FormField
        control={form.control}
        name="attrs.apiToken"
        render={({ field }) => (
          <FormItem className="flex-1">
            <FormLabel>API Token</FormLabel>
            <FormControl>
              <Input placeholder="API Token" {...field} />
              <SecretInput
                placeholder="API Token"
                {...field}
                onCopy={() => {
                  navigator.clipboard.writeText(field.value).then(() => {
                    toast({
                      description: 'API Token copied to clipboard',
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
