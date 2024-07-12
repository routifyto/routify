import React from 'react';
import {
  Sheet,
  SheetContent,
  SheetHeader,
  SheetTitle,
} from '@/components/ui/sheet';
import { useApp } from '@/contexts/app';
import { useGetCompletionLogQuery } from '@/api/logs';
import { Spinner } from '@/components/ui/spinner';
import { CompletionLogDetails } from '@/components/logs/completion-log-details';

interface TextLogDetailsSheetProps {
  id: string;
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

export function CompletionLogDetailsSheet({
  id,
  open,
  onOpenChange,
}: TextLogDetailsSheetProps) {
  const app = useApp();
  const { data, isPending } = useGetCompletionLogQuery(app.id, id);

  return (
    <Sheet open={open} onOpenChange={onOpenChange}>
      <SheetContent className="min-w-2/3 max-w-2/3 sm:max-w-2/3 sm:min-w-2/3">
        <SheetHeader>
          <SheetTitle className="pb-2">
            Completion log details - {id}
          </SheetTitle>
        </SheetHeader>
        {isPending ? (
          <div className="flex h-full w-full items-center justify-center">
            <Spinner />
          </div>
        ) : data ? (
          <CompletionLogDetails completionLog={data} />
        ) : (
          <div className="flex h-full w-full items-center justify-center">
            <p>Completion log not found</p>
          </div>
        )}
      </SheetContent>
    </Sheet>
  );
}
