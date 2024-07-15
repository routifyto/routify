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
import { cn, formatJson } from '@/lib/utils';
import { useApp } from '@/contexts/app';
import { providers } from '@/types/providers';
import { Light as SyntaxHighlighter } from 'react-syntax-highlighter';
import json from 'react-syntax-highlighter/dist/esm/languages/hljs/json';
import { docco } from 'react-syntax-highlighter/dist/esm/styles/hljs';

SyntaxHighlighter.registerLanguage('json', json);

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
                  {completionLog.inputCost.toLocaleString('en-US', {
                    maximumFractionDigits: 20,
                  })}
                  $
                </p>
              </div>
              <div className={rowClass}>
                <p className={firstColumnClass}>Generated tokens cost</p>
                <p className={secondColumnClass}>
                  {completionLog.outputCost.toLocaleString('en-US', {
                    maximumFractionDigits: 20,
                  })}
                  $
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
            </CardContent>
          </Card>
          <Card>
            <CardHeader>
              <CardTitle>Original request</CardTitle>
              <CardDescription className="flex flex-row gap-2">
                <span>{completionLog.gatewayRequest.method}</span>
                <span>{completionLog.gatewayRequest.url}</span>
              </CardDescription>
            </CardHeader>
            <CardContent>
              <SyntaxHighlighter
                className="rounded-md bg-white p-3 text-sm"
                language="json"
                style={docco}
              >
                {formatJson(completionLog.gatewayRequest?.body)}
              </SyntaxHighlighter>
            </CardContent>
          </Card>
          <Card>
            <CardHeader>
              <CardTitle>Provider request</CardTitle>
              <CardDescription className="flex flex-row gap-2">
                <span>{completionLog.providerRequest?.method}</span>
                <span>{completionLog.providerRequest?.url}</span>
              </CardDescription>
            </CardHeader>
            <CardContent>
              <SyntaxHighlighter
                className="rounded-md bg-white p-3 text-sm"
                language="json"
                style={docco}
              >
                {formatJson(completionLog.providerRequest?.body)}
              </SyntaxHighlighter>
            </CardContent>
          </Card>
          <Card>
            <CardHeader>
              <CardTitle>Response</CardTitle>
              <CardDescription>
                {completionLog.gatewayResponse?.statusCode}
              </CardDescription>
            </CardHeader>
            <CardContent>
              <SyntaxHighlighter
                className="rounded-md bg-white p-3 text-sm"
                language="json"
                style={docco}
              >
                {formatJson(completionLog.gatewayResponse?.body)}
              </SyntaxHighlighter>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  );
}
