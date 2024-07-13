'use client';

import { Copy, Eye, EyeOff } from 'lucide-react';
import React, { useState } from 'react';

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
import { Input } from '@/components/ui/input';
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import { toast } from '@/components/ui/use-toast';
import { CreateApiKeyOutput } from '@/types/api-keys';

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
  const [displayKey, setDisplayKey] = useState(false);

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
          <div className="flex flex-row items-center gap-1">
            <Input
              type={displayKey ? 'text' : 'password'}
              className="flex-1 whitespace-nowrap"
              value={apiKey.key}
              readOnly
            />
            <Tooltip delayDuration={100}>
              <TooltipTrigger asChild>
                <Button
                  variant="outline"
                  size="icon"
                  onClick={() => {
                    setDisplayKey(!displayKey);
                  }}
                >
                  {displayKey ? (
                    <EyeOff className="h-4 w-4" />
                  ) : (
                    <Eye className="h-4 w-4" />
                  )}
                </Button>
              </TooltipTrigger>
              <TooltipContent>
                <p>{displayKey ? 'Hide' : 'Show'} API key</p>
              </TooltipContent>
            </Tooltip>
            <Tooltip delayDuration={100}>
              <TooltipTrigger asChild>
                <Button
                  variant="outline"
                  size="icon"
                  onClick={() => {
                    navigator.clipboard.writeText(apiKey.key).then(() => {
                      toast({
                        description: 'API Key copied to clipboard',
                      });
                    });
                  }}
                >
                  <Copy className="h-4 w-4" />
                </Button>
              </TooltipTrigger>
              <TooltipContent>
                <p>Copy to clipboard</p>
              </TooltipContent>
            </Tooltip>
          </div>
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
