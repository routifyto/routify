import React from 'react';
import { CompletionOutgoingLogOutput } from '@/types/logs';
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import { JsonFormatter } from '@/components/logs/json-formatter';

interface CompletionOutgoingLogsProps {
  log: CompletionOutgoingLogOutput;
}

export function CompletionOutgoingLog({ log }: CompletionOutgoingLogsProps) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>{log.appProvider?.name}</CardTitle>
        <CardDescription>
          {log.requestMethod} - {log.requestUrl} - {log.statusCode}
        </CardDescription>
      </CardHeader>
      <CardContent className="flex flex-col gap-3">
        <p className="text-sm text-muted-foreground">Request</p>
        <JsonFormatter json={log.requestBody ?? ''} />
        <p className="text-sm text-muted-foreground">Response</p>
        <JsonFormatter json={log.responseBody ?? ''} />
      </CardContent>
    </Card>
  );
}
