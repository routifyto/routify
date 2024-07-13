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
import { useGetApiKeysQuery } from '@/api/api-keys';

interface ApiKeysTableProps {
  onSelect: (apiKeyId: string) => void;
}

export function ApiKeysTable({ onSelect }: ApiKeysTableProps) {
  const app = useApp();
  const { data, isPending, hasNextPage, isFetchingNextPage, fetchNextPage } =
    useGetApiKeysQuery(app.id, 20);

  return (
    <React.Fragment>
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Name</TableHead>
            <TableHead>Preview</TableHead>
            <TableHead>Expiration</TableHead>
            <TableHead>Description</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {isPending ? (
            <React.Fragment>
              {Array.from({ length: 5 }).map((_, index) => (
                <TableRow key={index}>
                  <TableCell colSpan={4}>
                    <Skeleton className="h-10 w-full" />
                  </TableCell>
                </TableRow>
              ))}
            </React.Fragment>
          ) : (
            <React.Fragment>
              {data?.pages.map((page, index) => (
                <React.Fragment key={index}>
                  {page.items.map((apiKey) => {
                    return (
                      <TableRow
                        className="hover:cursor-pointer"
                        key={apiKey.id}
                        onClick={() => {
                          onSelect(apiKey.id);
                        }}
                      >
                        <TableCell>{apiKey.name}</TableCell>
                        <TableCell>{`${apiKey.prefix}***********************************${apiKey.suffix}`}</TableCell>
                        <TableCell>
                          {apiKey.expiresAt
                            ? new Date(apiKey.expiresAt).toLocaleString()
                            : 'Never'}
                        </TableCell>
                        <TableCell>{apiKey.description}</TableCell>
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
