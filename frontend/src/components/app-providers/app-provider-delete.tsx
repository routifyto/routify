import React, { useState } from 'react';
import { BadgeX, Trash } from 'lucide-react';
import { Button } from '@/components/ui/button';
import {
  AlertDialog,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from '@/components/ui/alert-dialog';
import { useApp } from '@/contexts/app';
import { Spinner } from '@/components/ui/spinner';
import { useDeleteAppProviderMutation } from '@/api/app-providers';
import { useNavigate } from 'react-router-dom';
import { toast } from '@/components/ui/use-toast';

interface AppProviderDeleteProps {
  appProviderId: string;
}

export function AppProviderDelete({ appProviderId }: AppProviderDeleteProps) {
  const app = useApp();
  const navigate = useNavigate();
  const [open, setOpen] = useState(false);
  const { mutate, isPending } = useDeleteAppProviderMutation(
    app.id,
    appProviderId,
  );

  return (
    <div className="flex flex-row items-center justify-between rounded-xl border bg-card p-4 text-card-foreground shadow">
      <div className="flex flex-col gap-2">
        <h2 className="flex flex-row items-center gap-1 font-semibold">
          <BadgeX className="h-4 w-4" />
          Delete Provider
        </h2>
        <p className="text-sm text-muted-foreground">
          Delete this provider permanently
        </p>
      </div>
      <Button
        type="button"
        variant="destructive"
        className="flex flex-row items-center gap-1"
        onClick={() => {
          setOpen(true);
        }}
      >
        <Trash className="h-4 w-4" />
        Delete
      </Button>
      <AlertDialog open={open} onOpenChange={setOpen}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>
              Are you sure you want to delete this provider?
            </AlertDialogTitle>
            <AlertDialogDescription>
              This action cannot be undone and will permanently delete the
              provider.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <Button
              variant="destructive"
              onClick={() => {
                mutate(undefined, {
                  onSuccess: () => {
                    navigate(`/${app.id}/providers`);
                  },
                  onError: (error) => {
                    toast({
                      title: 'Failed to delete provider',
                      description: error.message,
                      variant: 'destructive',
                    });
                  },
                });
              }}
            >
              {isPending && <Spinner className="mr-1" />}
              Delete
            </Button>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </div>
  );
}
