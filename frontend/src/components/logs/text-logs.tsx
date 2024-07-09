import { useApp } from '@/contexts/app';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import React from 'react';
import { Skeleton } from '@/components/ui/skeleton';
import { InView } from 'react-intersection-observer';
import { Spinner } from '@/components/ui/spinner';
import { useGetTextLogsQuery } from '@/api/logs';
import { providers } from '@/types/providers';
import { TextLogDetailsSheet } from '@/components/logs/text-log-details-sheet';

export function TextLogs() {
  const app = useApp();

  const { data, isPending, hasNextPage, isFetchingNextPage, fetchNextPage } =
    useGetTextLogsQuery(app.id, 50);

  const [logDetailsId, setLogDetailsId] = React.useState<string | null>(null);

  return (
    <div className="flex flex-col gap-4">
      <div className="flex flex-row items-center justify-between pb-4">
        <h1 className="text-2xl font-semibold">Text logs</h1>
      </div>
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead></TableHead>
            <TableHead>Path</TableHead>
            <TableHead>Model</TableHead>
            <TableHead>Date & time</TableHead>
            <TableHead className="w-20 text-right">Tokens</TableHead>
            <TableHead className="w-20 text-right">Cost</TableHead>
            <TableHead className="w-20 text-right">Duration</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {isPending ? (
            <React.Fragment>
              {Array.from({ length: 5 }).map((_, index) => (
                <TableRow key={index}>
                  <TableCell colSpan={7}>
                    <Skeleton className="h-10 w-full" />
                  </TableCell>
                </TableRow>
              ))}
            </React.Fragment>
          ) : (
            <React.Fragment>
              {data?.pages.map((page, index) => (
                <React.Fragment key={index}>
                  {page.items.map((textLog) => {
                    const provider = providers.find(
                      (provider) => provider.id === textLog.provider,
                    );

                    if (provider === undefined) {
                      return null;
                    }

                    return (
                      <TableRow
                        className="hover:cursor-pointer"
                        key={textLog.id}
                        onClick={() => {
                          setLogDetailsId(textLog.id);
                        }}
                      >
                        <TableCell className="w-14">
                          <img
                            src={provider.logo}
                            className="h-10 w-10 rounded p-0.5 shadow"
                            alt={provider.name}
                          />
                        </TableCell>
                        <TableCell>/{textLog.path}</TableCell>
                        <TableCell>{textLog.model}</TableCell>
                        <TableCell>
                          {new Date(textLog.endedAt).toLocaleString()}
                        </TableCell>
                        <TableCell className="w-20 text-right">
                          {(
                            textLog.inputTokens + textLog.outputTokens
                          ).toLocaleString()}
                        </TableCell>
                        <TableCell className="w-20 text-right">
                          {(
                            textLog.inputCost + textLog.outputCost
                          ).toLocaleString()}
                          $
                        </TableCell>
                        <TableCell className="w-20 text-right">
                          {Math.floor(textLog.duration).toLocaleString()} ms
                        </TableCell>
                      </TableRow>
                    );
                  })}
                </React.Fragment>
              ))}
            </React.Fragment>
          )}
        </TableBody>
      </Table>
      {!isPending && !isFetchingNextPage && hasNextPage && (
        <InView
          rootMargin="200px"
          onChange={async (inView) => {
            if (inView && hasNextPage && !isFetchingNextPage) {
              await fetchNextPage();
            }
          }}
        >
          <div className="flex w-full items-center justify-center py-4">
            {isFetchingNextPage && <Spinner />}
          </div>
        </InView>
      )}
      {logDetailsId !== null && (
        <TextLogDetailsSheet
          id={logDetailsId}
          open={true}
          onOpenChange={() => {
            setLogDetailsId(null);
          }}
        />
      )}
    </div>
  );
}
