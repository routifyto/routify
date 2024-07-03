import { Trash } from 'lucide-react';
import React from 'react';

import { useDeleteAppUserMutation } from '@/api/app-users';
import {
  AlertDialog,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from '@/components/ui/alert-dialog';
import { Button } from '@/components/ui/button';
import { Spinner } from '@/components/ui/spinner';
import { toast } from '@/components/ui/use-toast';
import { useApp } from '@/contexts/app';

interface AppUserDeleteProps {
  appUserId: string;
}

export function AppUserDelete({ appUserId }: AppUserDeleteProps) {
  const app = useApp();
  const [open, setOpen] = React.useState(false);
  const [errors, setErrors] = React.useState<string[]>([]);

  const { mutate, isPending } = useDeleteAppUserMutation(app.id, appUserId);

  const handleDeleteClick = () => {
    mutate(undefined, {
      onSuccess: () => {
        toast({
          title: 'User removed',
          description: 'The user was remove successfully',
          variant: 'default',
        });
      },
      onError: () => {
        setErrors(['Error removing user']);
      },
    });
  };

  return (
    <AlertDialog open={open} onOpenChange={setOpen}>
      <AlertDialogTrigger asChild>
        <Trash className="h-4 w-4 cursor-pointer text-muted-foreground hover:text-foreground" />
      </AlertDialogTrigger>
      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>
            Are you sure you want to remove this user?
          </AlertDialogTitle>
          <AlertDialogDescription>
            This action cannot be undone. This user will no longer have access
            to the app.
          </AlertDialogDescription>
        </AlertDialogHeader>
        {errors.length > 0 && (
          <div className="mt-2 flex w-full flex-col gap-1 text-sm">
            {errors.map((error) => (
              <p key={error} className="text-red-500">
                {error}
              </p>
            ))}
          </div>
        )}
        <AlertDialogFooter>
          <AlertDialogCancel>Cancel</AlertDialogCancel>
          <Button variant="destructive" onClick={() => handleDeleteClick()}>
            {isPending && <Spinner className="mr-1" />}
            Remove
          </Button>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  );
}
