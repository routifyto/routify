import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import React from 'react';
import { useForm } from 'react-hook-form';

import { Button } from '@/components/ui/button';
import {
  Form,
  FormControl,
  FormDescription,
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
import { ProviderSelect } from '@/components/providers/provider-select';
import { Switch } from '@/components/ui/switch';
import { RoutePathHelp } from '@/components/routes/help/route-path-help';
import { RouteFailoverHelp } from '@/components/routes/help/route-failover-help';
import { RouteLoadBalanceHelp } from '@/components/routes/help/route-load-balance-help';
import { RouteCacheHelp } from '@/components/routes/help/route-cache-help';
import { RouteCostLimitHelp } from '@/components/routes/help/route-cost-limit-help';
import { RouteSchemaHelp } from '@/components/routes/help/route-schema-help';

const formSchema = z.object({
  name: z.string(),
  path: z.string().regex(/^[a-z0-9-_/]+$/, {
    message:
      'Only lowercase alphanumeric characters, hyphens, underscores, and slashes are allowed. No spaces or other characters.',
  }),
  description: z.string().optional(),
  attrs: z.record(z.string(), z.string()),
  type: z.enum(['COMPLETION', 'EMBEDDING']),
  schema: z.string(),
  isLoadBalanceEnabled: z.boolean().default(true),
  isFailoverEnabled: z.boolean().default(false),
  providers: z.array(
    z.object({
      id: z.string().nullable().optional(),
      appProviderId: z.string(),
      model: z.string().nullable().optional(),
      attrs: z.record(z.string(), z.string().optional().nullable()),
      weight: z.number().int().min(1).max(100).default(1),
    }),
  ),
  cacheConfig: z
    .object({
      enabled: z.boolean(),
      expiration: z.number().int().min(1),
    })
    .optional()
    .nullable(),
  costLimitConfig: z
    .object({
      enabled: z.boolean(),
      dailyLimit: z.number().int().min(1).nullable().optional(),
      monthlyLimit: z.number().int().min(1).nullable().optional(),
    })
    .optional()
    .nullable(),
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
      schema: 'openai',
      providers: [],
      isLoadBalanceEnabled: true,
      isFailoverEnabled: false,
      cacheConfig: {
        enabled: false,
        expiration: 60,
      },
      costLimitConfig: {
        enabled: false,
        dailyLimit: null,
        monthlyLimit: null,
      },
    },
  });

  const [openProvidersDialog, setOpenProvidersDialog] = React.useState(false);
  const providers = form.watch('providers');

  const isCacheEnabled = form.watch('cacheConfig.enabled');
  const isCostLimitEnabled = form.watch('costLimitConfig.enabled');

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
                  <FormDescription className="flex flex-row items-center gap-2">
                    Define the path for the route. This is the URL path that the
                    route will be accessible at
                    <RoutePathHelp />
                  </FormDescription>
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
            <FormField
              control={form.control}
              name="schema"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Schema *</FormLabel>
                  <FormDescription className="flex flex-row items-center gap-2">
                    Which provider schema to use for this route. This will
                    determine the structure of the request and response
                    <RouteSchemaHelp />
                  </FormDescription>
                  <FormControl>
                    <ProviderSelect
                      value={field.value}
                      onChange={(provider) => field.onChange(provider)}
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
              name="isFailoverEnabled"
              render={({ field }) => (
                <FormItem className="flex flex-row items-center justify-between">
                  <div className="space-y-0.5">
                    <FormLabel>Enable failover</FormLabel>
                    <FormDescription className="flex flex-row items-center gap-2">
                      Route requests to a backup provider if the primary
                      provider fails
                      <RouteFailoverHelp />
                    </FormDescription>
                  </div>
                  <FormControl>
                    <Switch
                      checked={field.value}
                      onCheckedChange={field.onChange}
                    />
                  </FormControl>
                </FormItem>
              )}
            />
          </div>
          <div className="flex flex-col gap-3 rounded-xl border bg-card p-4 text-card-foreground shadow">
            <FormField
              control={form.control}
              name="isLoadBalanceEnabled"
              render={({ field }) => (
                <FormItem className="flex flex-row items-center justify-between">
                  <div className="space-y-0.5">
                    <FormLabel>Enable load balancing</FormLabel>
                    <FormDescription className="flex flex-row items-center gap-2">
                      Distribute requests across multiple providers
                      <RouteLoadBalanceHelp />
                    </FormDescription>
                  </div>
                  <FormControl>
                    <Switch
                      checked={field.value}
                      onCheckedChange={field.onChange}
                    />
                  </FormControl>
                </FormItem>
              )}
            />
          </div>
          <div className="flex flex-col gap-3 rounded-xl border bg-card p-4 text-card-foreground shadow">
            <FormField
              control={form.control}
              name="cacheConfig.enabled"
              render={({ field }) => (
                <FormItem className="flex flex-row items-center justify-between">
                  <div className="space-y-0.5">
                    <FormLabel>Enable caching</FormLabel>
                    <FormDescription className="flex flex-row items-center gap-2">
                      Cache responses to reduce latency
                      <RouteCacheHelp />
                    </FormDescription>
                  </div>
                  <FormControl>
                    <Switch
                      checked={field.value}
                      onCheckedChange={field.onChange}
                    />
                  </FormControl>
                </FormItem>
              )}
            />
            {isCacheEnabled && (
              <FormField
                control={form.control}
                name="cacheConfig.expiration"
                render={({ field }) => (
                  <FormItem className="flex-1">
                    <FormLabel>Caching expiration (seconds)</FormLabel>
                    <FormControl>
                      <Input
                        placeholder=""
                        value={field.value ?? ''}
                        onChange={(event) => {
                          const number = parseInt(event.target.value, 10);
                          if (isNaN(number)) {
                            return;
                          }
                          field.onChange(number);
                        }}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            )}
          </div>
          <div className="flex flex-col gap-3 rounded-xl border bg-card p-4 text-card-foreground shadow">
            <FormField
              control={form.control}
              name="costLimitConfig.enabled"
              render={({ field }) => (
                <FormItem className="flex flex-row items-center justify-between">
                  <div className="space-y-0.5">
                    <FormLabel>Enable cost limits</FormLabel>
                    <FormDescription className="flex flex-row items-center gap-2">
                      Limit the cost of requests to this route
                      <RouteCostLimitHelp />
                    </FormDescription>
                  </div>
                  <FormControl>
                    <Switch
                      checked={field.value}
                      onCheckedChange={field.onChange}
                    />
                  </FormControl>
                </FormItem>
              )}
            />
            {isCostLimitEnabled && (
              <React.Fragment>
                <FormField
                  control={form.control}
                  name="costLimitConfig.dailyLimit"
                  render={({ field }) => (
                    <FormItem className="flex-1">
                      <FormLabel>Daily limit ($)</FormLabel>
                      <FormControl>
                        <Input
                          placeholder=""
                          value={field.value ?? ''}
                          onChange={(event) => {
                            const number = parseFloat(event.target.value);
                            if (isNaN(number)) {
                              field.onChange(null);
                            } else {
                              field.onChange(number);
                            }
                          }}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="costLimitConfig.monthlyLimit"
                  render={({ field }) => (
                    <FormItem className="flex-1">
                      <FormLabel>Monthly limit ($)</FormLabel>
                      <FormControl>
                        <Input
                          placeholder=""
                          value={field.value ?? ''}
                          onChange={(event) => {
                            const number = parseFloat(event.target.value);
                            if (isNaN(number)) {
                              field.onChange(null);
                            } else {
                              field.onChange(number);
                            }
                          }}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </React.Fragment>
            )}
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
                weight: 1,
              },
            ]);

            setOpenProvidersDialog(false);
          }}
        />
      )}
    </React.Fragment>
  );
}
