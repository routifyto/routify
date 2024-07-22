import React from 'react';
import { CompletionLogOutput } from '@/types/logs';
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import { NavLink } from 'react-router-dom';
import { cn, formatCost } from '@/lib/utils';
import { useApp } from '@/contexts/app';
import { providers } from '@/types/providers';

import { CompletionOutgoingLogs } from '@/components/logs/completion-outgoing-logs';
import { JsonFormatter } from '@/components/logs/json-formatter';

interface CompletionLogDetailsProps {
  completionLog: CompletionLogOutput;
}

const rowClass =
  'flex flex-row items-center border-b border-gray-100 p-1 text-xs';
const firstColumnClass = 'w-64 min-w-64';
const secondColumnClass = 'flex-1 min-w-0 overflow-hidden truncate';

export function CompletionLogDetails({
  completionLog,
}: CompletionLogDetailsProps) {
  const app = useApp();
  const provider = providers.find(
    (provider) => provider.id === completionLog.provider,
  );

  return (
    <div className="relative h-full">
      <div className="absolute bottom-0 left-0 right-0 top-0 overflow-y-auto">
        <div className="mt-4 flex flex-col gap-4 px-2 pb-10">
          <Card>
            <CardHeader>
              <CardTitle>Route</CardTitle>
            </CardHeader>
            <CardContent>
              <div className={rowClass}>
                <p className={firstColumnClass}>Name</p>
                <NavLink
                  className={cn(
                    secondColumnClass,
                    'hover:cursor-pointer hover:underline',
                  )}
                  to={`/${app.id}/routes/${completionLog.route?.id}`}
                  target="_blank"
                >
                  {completionLog.route?.name}
                </NavLink>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Description</p>
                <p className={secondColumnClass}>
                  {completionLog.route?.description}
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Path</p>
                <p className={secondColumnClass}>
                  /{completionLog.route?.path}
                </p>
              </div>
            </CardContent>
          </Card>
          <Card>
            <CardHeader>
              <CardTitle>Provider</CardTitle>
            </CardHeader>
            <CardContent>
              <div className={rowClass}>
                <p className={firstColumnClass}>App provider</p>
                <NavLink
                  className={cn(
                    secondColumnClass,
                    'hover:cursor-pointer hover:underline',
                  )}
                  to={`/${app.id}/providers/${completionLog.route?.id}`}
                  target="_blank"
                >
                  {completionLog.appProvider?.name}
                </NavLink>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Description</p>
                <p className={secondColumnClass}>
                  {completionLog.appProvider?.description}
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Alias</p>
                <p className={secondColumnClass}>
                  {completionLog.appProvider?.alias}
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Provider</p>
                <p className={secondColumnClass}>{provider?.name}</p>
              </div>
            </CardContent>
          </Card>
          <Card>
            <CardHeader>
              <CardTitle>Metrics</CardTitle>
            </CardHeader>
            <CardContent>
              <div className={rowClass}>
                <p className={firstColumnClass}>Prompt tokens</p>
                <p className={secondColumnClass}>
                  {completionLog.inputTokens.toLocaleString()}
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Generated tokens</p>
                <p className={secondColumnClass}>
                  {completionLog.outputTokens.toLocaleString()}
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Prompt tokens cost</p>
                <p className={secondColumnClass}>
                  {formatCost(completionLog.inputCost, 4)}
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Generated tokens cost</p>
                <p className={secondColumnClass}>
                  {formatCost(completionLog.outputCost, 4)}
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Started at</p>
                <p className={secondColumnClass}>
                  {new Date(completionLog.startedAt).toLocaleString()}
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Ended at</p>
                <p className={secondColumnClass}>
                  {new Date(completionLog.endedAt).toLocaleString()}
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Duration</p>
                <p className={secondColumnClass}>
                  {Math.floor(completionLog.duration).toLocaleString()} ms
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Cache</p>
                <p className={secondColumnClass}>{completionLog.cacheStatus}</p>
              </div>
            </CardContent>
          </Card>
          <Card>
            <CardHeader>
              <CardTitle>Request</CardTitle>
              <CardDescription className="flex flex-row gap-2">
                <span>{completionLog.requestMethod ?? '-'}</span>
                <span>{completionLog.requestUrl ?? '-'}</span>
              </CardDescription>
            </CardHeader>
            <CardContent>
              <JsonFormatter json={completionLog.requestBody ?? ''} />
            </CardContent>
          </Card>
          <Card>
            <CardHeader>
              <CardTitle>Response</CardTitle>
              <CardDescription>{completionLog.statusCode}</CardDescription>
            </CardHeader>
            <CardContent>
              <JsonFormatter json={completionLog.responseBody ?? ''} />
            </CardContent>
          </Card>
          {completionLog.outgoingRequestsCount > 0 && (
            <CompletionOutgoingLogs completionLogId={completionLog.id} />
          )}
        </div>
      </div>
    </div>
  );
}
