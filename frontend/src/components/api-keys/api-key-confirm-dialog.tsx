import React from 'react';

import { Button } from '@/components/ui/button';
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { toast } from '@/components/ui/use-toast';
import { CreateApiKeyOutput } from '@/types/api-keys';
import { SecretInput } from '@/components/ui/secret-input';

interface ApiKeyConfirmDialogProps {
  apiKey: CreateApiKeyOutput;
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

export function ApiKeyConfirmDialog({
  apiKey,
  open,
  onOpenChange,
}: ApiKeyConfirmDialogProps) {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="w-128">
        <DialogHeader>
          <DialogTitle>API Key</DialogTitle>
          <DialogDescription>
            Please copy this API key and store it securely. You will not be able
            to see it again.
          </DialogDescription>
        </DialogHeader>
        <div className="py-4">
          <SecretInput
            className="flex-1 whitespace-nowrap"
            value={apiKey.key}
            readOnly
            onCopy={() => {
              navigator.clipboard.writeText(apiKey.key).then(() => {
                toast({
                  description: 'API Key copied to clipboard',
                });
              });
            }}
          />
        </div>
        <DialogFooter>
          <DialogClose asChild>
            <Button type="button" variant="secondary">
              Close
            </Button>
          </DialogClose>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
