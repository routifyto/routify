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
import { AppProviderInput, AppProviderPayload } from '@/types/app-providers';
import { ProviderSelect } from '@/components/providers/provider-select';
import { AppProviderAttrsForm } from '@/components/app-providers/app-provider-attrs-form';

const formSchema = z.object({
  name: z.string(),
  alias: z.string().regex(/^[a-z0-9-_]+$/, {
    message:
      'Only lowercase alphanumeric characters, hyphens, and underscores are allowed. No spaces or other characters.',
  }),
  provider: z.string(),
  description: z.string().optional(),
  attrs: z.record(z.string(), z.string()),
});

interface AlertProviderProps {
  appProvider: AppProviderPayload | null;
  errors: string[];
  isPending: boolean;
  onSubmit: (data: AppProviderInput) => void;
}

export function AppProviderForm({
  appProvider,
  errors,
  isPending,
  onSubmit,
}: AlertProviderProps) {
  const isUpdate = appProvider != null;
  const form = useForm<AppProviderInput>({
    resolver: zodResolver(formSchema),
    defaultValues: appProvider ?? {
      name: '',
      description: '',
      alias: '',
      provider: 'openai',
      attrs: {},
    },
  });

  return (
    <Form {...form}>
      <form
        className="flex flex-col gap-4"
        onSubmit={form.handleSubmit(onSubmit)}
      >
        {!isUpdate && (
          <h1 className="pb-4 text-2xl font-semibold">Create provider</h1>
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
            name="alias"
            render={({ field }) => (
              <FormItem className="flex-1">
                <FormLabel>Alias</FormLabel>
                <FormControl>
                  <Input placeholder="Alias" {...field} />
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
        </div>
        <div className="flex flex-col gap-3 rounded-xl border bg-card p-4 text-card-foreground shadow">
          <FormField
            control={form.control}
            name="provider"
            render={({ field }) => (
              <FormItem className="flex-1">
                <FormLabel>Provider</FormLabel>
                <FormControl>
                  <ProviderSelect
                    value={field.value}
                    onChange={field.onChange}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <AppProviderAttrsForm />
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
