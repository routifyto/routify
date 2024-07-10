import React from 'react';
import { TextLogPayload } from '@/types/logs';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { NavLink } from 'react-router-dom';
import { cn } from '@/lib/utils';
import { useApp } from '@/contexts/app';
import { providers } from '@/types/providers';

interface TextLogDetailsProps {
  textLog: TextLogPayload;
}

const rowClass =
  'flex flex-row items-center border-b border-gray-100 p-1 text-xs';
const firstColumnClass = 'w-64 min-w-64';
const secondColumnClass = 'flex-1 min-w-0 overflow-hidden truncate';

export function TextLogDetails({ textLog }: TextLogDetailsProps) {
  const app = useApp();
  const provider = providers.find(
    (provider) => provider.id === textLog.provider,
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
                  to={`/${app.id}/routes/${textLog.route?.id}`}
                  target="_blank"
                >
                  {textLog.route?.name}
                </NavLink>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Description</p>
                <p className={secondColumnClass}>
                  {textLog.route?.description}
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Path</p>
                <p className={secondColumnClass}>/{textLog.route?.path}</p>
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
                  to={`/${app.id}/providers/${textLog.route?.id}`}
                  target="_blank"
                >
                  {textLog.appProvider?.name}
                </NavLink>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Description</p>
                <p className={secondColumnClass}>
                  {textLog.appProvider?.description}
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Alias</p>
                <p className={secondColumnClass}>
                  {textLog.appProvider?.alias}
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
                  {textLog.inputTokens.toLocaleString()}
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Generated tokens</p>
                <p className={secondColumnClass}>
                  {textLog.outputTokens.toLocaleString()}
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Prompt tokens cost</p>
                <p className={secondColumnClass}>
                  {textLog.inputCost.toLocaleString('en-US', {
                    maximumFractionDigits: 20,
                  })}
                  $
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Generated tokens cost</p>
                <p className={secondColumnClass}>
                  {textLog.outputCost.toLocaleString('en-US', {
                    maximumFractionDigits: 20,
                  })}
                  $
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Started at</p>
                <p className={secondColumnClass}>
                  {new Date(textLog.startedAt).toLocaleString()}
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Ended at</p>
                <p className={secondColumnClass}>
                  {new Date(textLog.endedAt).toLocaleString()}
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Duration</p>
                <p className={secondColumnClass}>
                  {Math.floor(textLog.duration).toLocaleString()} ms
                </p>
              </div>
            </CardContent>
          </Card>
          <Card>
            <CardHeader>
              <CardTitle>Request</CardTitle>
            </CardHeader>
            <CardContent>
              <code>
                <pre className="whitespace-pre-wrap text-sm">
                  {textLog.requestBody}
                </pre>
              </code>
            </CardContent>
          </Card>
          <Card>
            <CardHeader>
              <CardTitle>Response</CardTitle>
            </CardHeader>
            <CardContent>
              <code>
                <pre className="whitespace-pre-wrap text-sm">
                  {textLog.responseBody}
                </pre>
              </code>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  );
}
