import React from 'react';
import { useApp } from '@/contexts/app';
import { useGetCompletionOutgoingLogsQuery } from '@/api/logs';
import { CompletionOutgoingLog } from '@/components/logs/completion-outgoing-log';
import { Spinner } from '@/components/ui/spinner';

interface CompletionOutgoingLogsProps {
  completionLogId: string;
}

export function CompletionOutgoingLogs({
  completionLogId,
}: CompletionOutgoingLogsProps) {
  const app = useApp();
  const { data, isPending } = useGetCompletionOutgoingLogsQuery(
    app.id,
    completionLogId,
  );

  if (isPending) {
    return (
      <div className="flex w-full items-center justify-center py-4">
        <Spinner />
      </div>
    );
  }

  return (
    <React.Fragment>
      <h2 className="pb-2 text-lg font-semibold text-foreground">
        Outgoing requests
      </h2>
      {data?.map((log) => <CompletionOutgoingLog log={log} key={log.id} />)}
    </React.Fragment>
  );
}
