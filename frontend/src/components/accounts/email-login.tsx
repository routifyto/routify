import React from 'react';
import { Input } from '@/components/ui/input';
import { useEmailLoginMutation } from '@/api/accounts';
import { Button } from '@/components/ui/button';
import { Spinner } from '@/components/ui/spinner';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormMessage,
} from '@/components/ui/form';
import { z } from 'zod';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Mail } from 'lucide-react';
import { LoginPayload } from '@/types/accounts';

const formSchema = z.object({
  email: z.string().min(2).email(),
  password: z.string().min(8),
});

interface EmailLoginProps {
  onLogin: (payload: LoginPayload) => void;
}

export function EmailLogin({ onLogin }: EmailLoginProps) {
  const { mutate, isPending } = useEmailLoginMutation();

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: '',
      password: '',
    },
  });

  function handleSubmit(values: z.infer<typeof formSchema>) {
    mutate(values, {
      onSuccess: (data) => {
        onLogin(data);
      },
    });
  }

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(handleSubmit)} className="space-y-3">
        <FormField
          control={form.control}
          name="email"
          render={({ field }) => (
            <FormItem>
              <FormControl>
                <Input placeholder="Email" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="password"
          render={({ field }) => (
            <FormItem>
              <FormControl>
                <Input type="password" placeholder="Password" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <Button
          type="submit"
          variant="outline"
          className="w-full"
          disabled={isPending}
        >
          {isPending ? (
            <Spinner className="mr-2 h-4 w-4" />
          ) : (
            <Mail className="mr-2 h-4 w-4" />
          )}
          Login
        </Button>
      </form>
    </Form>
  );
}
