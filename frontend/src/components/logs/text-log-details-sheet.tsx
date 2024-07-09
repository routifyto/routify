import React from 'react';
import {
  Sheet,
  SheetContent,
  SheetHeader,
  SheetTitle,
} from '@/components/ui/sheet';
import { useApp } from '@/contexts/app';
import { useGetTextLogQuery } from '@/api/logs';
import { Spinner } from '@/components/ui/spinner';
import { TextLogDetails } from '@/components/logs/text-log-details';

interface TextLogDetailsSheetProps {
  id: string;
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

export function TextLogDetailsSheet({
  id,
  open,
  onOpenChange,
}: TextLogDetailsSheetProps) {
  const app = useApp();
  const { data, isPending } = useGetTextLogQuery(app.id, id);

  return (
    <Sheet open={open} onOpenChange={onOpenChange}>
      <SheetContent className="min-w-2/3 max-w-2/3 sm:max-w-2/3 sm:min-w-2/3">
        <SheetHeader>
          <SheetTitle className="pb-2">Text log details - {id}</SheetTitle>
        </SheetHeader>
        {isPending ? (
          <div className="flex h-full w-full items-center justify-center">
            <Spinner />
          </div>
        ) : data ? (
          <TextLogDetails textLog={data} />
        ) : (
          <div className="flex h-full w-full items-center justify-center">
            <p>Text log not found</p>
          </div>
        )}
      </SheetContent>
    </Sheet>
  );
}
