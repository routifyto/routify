import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import React from 'react';
import { useForm } from 'react-hook-form';

import { Button } from '@/components/ui/button';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Spinner } from '@/components/ui/spinner';
import { Textarea } from '@/components/ui/textarea';
import { ApiKeyInput, ApiKeyOutput } from '@/types/api-keys';
import { ApiKeyExpirationCalendar } from '@/components/api-keys/api-key-expiration-calendar';

const formSchema = z.object({
  name: z.string(),
  description: z.string().optional().nullable(),
  expiresAt: z.date().optional().nullable(),
  canUseGateway: z.boolean(),
});

interface ApiKeyProps {
  apiKey: ApiKeyOutput | null;
  errors: string[];
  isPending: boolean;
  onSubmit: (data: ApiKeyInput) => void;
}

export function ApiKeyForm({
  apiKey,
  errors,
  isPending,
  onSubmit,
}: ApiKeyProps) {
  const isUpdate = apiKey != null;
  const form = useForm<ApiKeyInput>({
    resolver: zodResolver(formSchema),
    defaultValues: apiKey ?? {
      name: '',
      description: '',
      expiresAt: null,
      canUseGateway: true,
    },
  });

  return (
    <Form {...form}>
      <form
        className="flex flex-col gap-4"
        onSubmit={form.handleSubmit(onSubmit)}
      >
        {!isUpdate && (
          <h1 className="pb-4 text-2xl font-semibold">Create API Key</h1>
        )}
        <div className="flex flex-col gap-3 rounded-xl border bg-card p-4 text-card-foreground shadow">
          <FormField
            control={form.control}
            name="name"
            render={({ field }) => (
              <FormItem className="flex-1">
                <FormLabel>Name</FormLabel>
                <FormControl>
                  <Input placeholder="Name" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name="description"
            render={({ field }) => (
              <FormItem className="flex-1">
                <FormLabel>Description</FormLabel>
                <FormControl>
                  <Textarea
                    placeholder="Write an optional description about the route."
                    value={field.value ?? ''}
                    onChange={field.onChange}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name="expiresAt"
            render={({ field }) => (
              <FormItem className="flex-1">
                <FormLabel>Expiration date</FormLabel>
                <FormControl>
                  <div className="w-full">
                    <ApiKeyExpirationCalendar
                      value={field.value}
                      onChange={field.onChange}
                    />
                  </div>
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
        </div>
        <div className="flex flex-col gap-2 pb-2">
          <div className="flex flex-row items-center justify-end gap-2">
            <Button type="submit" disabled={isPending}>
              {isPending && <Spinner className="mr-1 h-4 w-4" />}
              {isUpdate ? 'Update' : 'Create'}
            </Button>
          </div>
          {errors.length > 0 && (
            <div className="mt-2 flex w-full flex-col items-end gap-2 text-sm">
              {errors.map((error) => (
                <p key={error} className="text-red-500">
                  {error}
                </p>
              ))}
            </div>
          )}
        </div>
      </form>
    </Form>
  );
}
