import React from 'react';
import { useApp } from '@/contexts/app';
import { InView } from 'react-intersection-observer';
import { Spinner } from '@/components/ui/spinner';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { Skeleton } from '@/components/ui/skeleton';
import { useGetConsumersQuery } from '@/api/consumers';

interface ConsumersTableProps {
  onSelect: (consumerId: string) => void;
}

export function ConsumersTable({ onSelect }: ConsumersTableProps) {
  const app = useApp();
  const { data, isPending, hasNextPage, isFetchingNextPage, fetchNextPage } =
    useGetConsumersQuery(app.id, 20);

  return (
    <React.Fragment>
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Name</TableHead>
            <TableHead>Alias</TableHead>
            <TableHead>Description</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {isPending ? (
            <React.Fragment>
              {Array.from({ length: 5 }).map((_, index) => (
                <TableRow key={index}>
                  <TableCell colSpan={3}>
                    <Skeleton className="h-10 w-full" />
                  </TableCell>
                </TableRow>
              ))}
            </React.Fragment>
          ) : (
            <React.Fragment>
              {data?.pages.map((page, index) => (
                <React.Fragment key={index}>
                  {page.items.map((consumer) => {
                    return (
                      <TableRow
                        className="hover:cursor-pointer"
                        key={consumer.id}
                        onClick={() => {
                          onSelect(consumer.id);
                        }}
                      >
                        <TableCell>{consumer.name}</TableCell>
                        <TableCell>{consumer.alias}</TableCell>
                        <TableCell>{consumer.description}</TableCell>
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
    </React.Fragment>
  );
}
