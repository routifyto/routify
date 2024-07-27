import React from 'react';
import { useFormContext } from 'react-hook-form';
import { CreateRouteInput } from '@/types/routes';
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Switch } from '@/components/ui/switch';

interface MistralRouteProviderAttrsFormProps {
  index: number;
}

export function MistralRouteProviderAttrsForm({
  index,
}: MistralRouteProviderAttrsFormProps) {
  const form = useFormContext<CreateRouteInput>();
  return (
    <div className="flex flex-col gap-4">
      <FormField
        control={form.control}
        name={`providers.${index}.attrs.systemPrompt`}
        render={({ field }) => (
          <FormItem className="flex-1">
            <FormLabel>System prompt</FormLabel>
            <FormControl>
              <Textarea
                placeholder="System prompt (optional)"
                value={field.value ?? ''}
                onChange={(event) => {
                  if (event.target.value === '') {
                    field.onChange(null);
                  } else {
                    field.onChange(event.target.value);
                  }
                }}
              />
            </FormControl>
            <FormMessage />
          </FormItem>
        )}
      />
      <div className="grid grid-cols-2 gap-2">
        <FormField
          control={form.control}
          name={`providers.${index}.attrs.temperature`}
          render={({ field }) => (
            <FormItem className="flex-1">
              <FormLabel>Temperature</FormLabel>
              <FormControl>
                <Input
                  placeholder="1"
                  value={field.value ?? ''}
                  onChange={(event) => {
                    if (event.target.value === '') {
                      field.onChange(null);
                    } else {
                      const number = parseFloat(event.target.value);
                      if (isNaN(number)) {
                        return;
                      }
                      field.onChange(event.target.value);
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
          name={`providers.${index}.attrs.maxTokens`}
          render={({ field }) => (
            <FormItem className="flex-1">
              <FormLabel>Max tokens</FormLabel>
              <FormControl>
                <Input
                  placeholder=""
                  value={field.value ?? ''}
                  onChange={(event) => {
                    if (event.target.value === '') {
                      field.onChange(null);
                    } else {
                      const number = parseInt(event.target.value, 10);
                      if (isNaN(number)) {
                        return;
                      }
                      field.onChange(event.target.value);
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
          name={`providers.${index}.attrs.topP`}
          render={({ field }) => (
            <FormItem className="flex-1">
              <FormLabel>Top P</FormLabel>
              <FormControl>
                <Input
                  placeholder=""
                  value={field.value ?? ''}
                  onChange={(event) => {
                    if (event.target.value === '') {
                      field.onChange(null);
                    } else {
                      const number = parseFloat(event.target.value);
                      if (isNaN(number)) {
                        return;
                      }
                      field.onChange(event.target.value);
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
          name={`providers.${index}.attrs.safePrompt`}
          render={({ field }) => (
            <FormItem className="flex-1">
              <FormLabel>Safe prompt</FormLabel>
              <FormControl>
                <div>
                  <Switch
                    checked={field.value === 'true'}
                    onCheckedChange={(checked) => {
                      if (checked) {
                        field.onChange('true');
                      } else {
                        field.onChange(null);
                      }
                    }}
                  />
                </div>
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
      </div>
    </div>
  );
}
