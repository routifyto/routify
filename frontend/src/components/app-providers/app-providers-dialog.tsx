import React from 'react';
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { AppProvidersTable } from '@/components/app-providers/app-providers-table';

interface AppProvidersDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onSelect: (appProviderId: string) => void;
}

export function AppProvidersDialog({
  open,
  onOpenChange,
  onSelect,
}: AppProvidersDialogProps) {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="md:min-h-3/4 md:max-h-3/4 flex flex-col gap-4 overflow-auto pr-12 md:h-3/4 md:w-3/4 md:max-w-full">
        <DialogHeader>
          <DialogTitle>App providers</DialogTitle>
        </DialogHeader>
        <AppProvidersTable onSelect={onSelect} />
      </DialogContent>
    </Dialog>
  );
}
