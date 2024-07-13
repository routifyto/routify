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
import { CreateRouteInput, RouteOutput } from '@/types/routes';
import { RouteProviderForm } from '@/components/routes/route-provider-form';
import { Plus } from 'lucide-react';
import { AppProvidersDialog } from '@/components/app-providers/app-providers-dialog';

const formSchema = z.object({
  name: z.string(),
  path: z.string().regex(/^[a-z0-9-_]+$/, {
    message:
      'Only lowercase alphanumeric characters, hyphens, and underscores are allowed. No spaces or other characters.',
  }),
  description: z.string().optional(),
  attrs: z.record(z.string(), z.string()),
  type: z.enum(['COMPLETION', 'EMBEDDING']),
  providers: z.array(
    z.object({
      id: z.string().nullable().optional(),
      appProviderId: z.string(),
      model: z.string().nullable().optional(),
      attrs: z.record(z.string(), z.string().optional().nullable()),
    }),
  ),
});

interface RouteFormProps {
  route: RouteOutput | null;
  errors: string[];
  isPending: boolean;
  onSubmit: (data: CreateRouteInput) => void;
}

export function RouteForm({
  route,
  errors,
  isPending,
  onSubmit,
}: RouteFormProps) {
  const isUpdate = route != null;
  const form = useForm<CreateRouteInput>({
    resolver: zodResolver(formSchema),
    defaultValues: route ?? {
      name: '',
      description: '',
      path: '',
      attrs: {},
      type: 'COMPLETION',
      schemaType: 'OPENAI',
      providers: [],
    },
  });

  const [openProvidersDialog, setOpenProvidersDialog] = React.useState(false);
  const providers = form.watch('providers');
  console.log('form', form.getValues());

  return (
    <React.Fragment>
      <Form {...form}>
        <form
          className="flex flex-col gap-4"
          onSubmit={form.handleSubmit(onSubmit)}
        >
          {!isUpdate && (
            <h1 className="pb-4 text-2xl font-semibold">Create route</h1>
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
          </div>
          <div className="flex flex-col gap-3 rounded-xl border bg-card p-4 text-card-foreground shadow">
            <FormField
              control={form.control}
              name="path"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Path *</FormLabel>
                  <FormControl>
                    <div className="flex w-full flex-row gap-1 border-b border-b-gray-200 font-mono text-lg font-bold">
                      <span className="text-muted-foreground">/</span>
                      <input
                        className="flex-grow outline-none"
                        placeholder="api/users"
                        autoComplete="off"
                        {...field}
                      />
                    </div>
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>
          {providers.map((_, index) => (
            <div
              key={`${providers.length}-${index}`}
              className="flex flex-col gap-3 rounded-xl border bg-card p-4 text-card-foreground shadow"
            >
              <RouteProviderForm
                index={index}
                onRemove={() => {
                  form.setValue(
                    'providers',
                    providers.filter((_, i) => i !== index),
                  );
                }}
              />
            </div>
          ))}
          <Button
            type="button"
            variant="link"
            size="sm"
            className="mt-1 justify-start rounded hover:bg-gray-50"
            onClick={() => {
              setOpenProvidersDialog(true);
            }}
          >
            <Plus className="h-4 w-4" />
            Add provider
          </Button>
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
      {openProvidersDialog && (
        <AppProvidersDialog
          open={openProvidersDialog}
          onOpenChange={setOpenProvidersDialog}
          onSelect={(appProviderId) => {
            form.setValue('providers', [
              ...providers,
              {
                appProviderId,
                model: null,
                attrs: {},
              },
            ]);

            setOpenProvidersDialog(false);
          }}
        />
      )}
    </React.Fragment>
  );
}
