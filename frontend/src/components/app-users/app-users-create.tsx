import { zodResolver } from '@hookform/resolvers/zod';
import React, { useState } from 'react';
import { useForm } from 'react-hook-form';

import { useCreateAppUsersMutation } from '@/api/app-users';
import { Button } from '@/components/ui/button';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { EnumDropdown } from '@/components/ui/enum-dropdown';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { Spinner } from '@/components/ui/spinner';
import { TagInput } from '@/components/ui/tag-input';
import { toast } from '@/components/ui/use-toast';
import { appUserRoleOptions, AppUsersInput } from '@/types/app-users';
import { z } from 'zod';

const formSchema = z.object({
  emails: z.array(z.string()),
  role: z.enum(['MEMBER', 'ADMIN', 'OWNER']),
});

interface AppUsersCreateProps {
  appId: string;
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

export function AppUsersCreate({
  appId,
  open,
  onOpenChange,
}: AppUsersCreateProps) {
  const [errors, setErrors] = useState<string[]>([]);
  const { mutate, isPending } = useCreateAppUsersMutation(appId);

  const form = useForm<AppUsersInput>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      emails: [],
      role: 'MEMBER',
    },
  });

  const handleSubmit = (values: AppUsersInput) => {
    mutate(values, {
      onSuccess() {
        toast({
          title: 'Users added',
          description: `Users have been added to the app`,
        });
        onOpenChange(false);
        form.reset();
      },
      onError() {
        setErrors(['Error adding users to app']);
      },
      onSettled() {
        form.reset();
      },
    });
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-1/2 w-1/3">
        <Form {...form}>
          <form onSubmit={form.handleSubmit(handleSubmit)}>
            <DialogHeader>
              <DialogTitle>Add users to your app</DialogTitle>
              <DialogDescription>
                Invite users to collaborate on your app by adding their email
                addresses.
              </DialogDescription>
            </DialogHeader>
            <FormField
              control={form.control}
              name="emails"
              render={({ field }) => (
                <FormItem className="mt-4">
                  <FormLabel>Emails</FormLabel>
                  <FormControl>
                    <TagInput
                      placeholder="john@example.com (press enter to add)"
                      values={field.value}
                      onChange={(values) => {
                        field.onChange(values);
                      }}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="role"
              render={({ field }) => {
                return (
                  <FormItem className="mt-2">
                    <FormLabel>Role</FormLabel>
                    <FormControl>
                      <EnumDropdown
                        options={appUserRoleOptions}
                        value={field.value}
                        onValueChange={field.onChange}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                );
              }}
            />
            {errors.length > 0 && (
              <div className="mb-2 mt-2 flex w-full flex-col items-end gap-2 text-sm">
                {errors.map((error) => (
                  <p key={error} className="text-red-500">
                    {error}
                  </p>
                ))}
              </div>
            )}
            <DialogFooter className="mt-4">
              <Button type="submit" disabled={isPending}>
                {isPending && <Spinner className="mr-1 h-4 w-4" />}
                Add
              </Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
}
