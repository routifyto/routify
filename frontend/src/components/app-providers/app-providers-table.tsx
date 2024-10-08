import React from 'react';
import { useApp } from '@/contexts/app';
import { InView } from 'react-intersection-observer';
import { Spinner } from '@/components/ui/spinner';
import { useGetAppProvidersQuery } from '@/api/app-providers';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { Skeleton } from '@/components/ui/skeleton';
import { providers } from '@/types/providers';

interface AppProvidersTableProps {
  onSelect: (appProviderId: string) => void;
}

export function AppProvidersTable({ onSelect }: AppProvidersTableProps) {
  const app = useApp();
  const { data, isPending, hasNextPage, isFetchingNextPage, fetchNextPage } =
    useGetAppProvidersQuery(app.id, 20);

  return (
    <React.Fragment>
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead></TableHead>
            <TableHead>Name</TableHead>
            <TableHead>Alias</TableHead>
            <TableHead>Provider</TableHead>
            <TableHead>Description</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {isPending ? (
            <React.Fragment>
              {Array.from({ length: 5 }).map((_, index) => (
                <TableRow key={index}>
                  <TableCell colSpan={5}>
                    <Skeleton className="h-10 w-full" />
                  </TableCell>
                </TableRow>
              ))}
            </React.Fragment>
          ) : (
            <React.Fragment>
              {data?.pages.map((page, index) => (
                <React.Fragment key={index}>
                  {page.items.map((appProvider) => {
                    const provider = providers.find(
                      (provider) => provider.id === appProvider.provider,
                    );

                    if (provider === undefined) {
                      return null;
                    }

                    return (
                      <TableRow
                        className="hover:cursor-pointer"
                        key={appProvider.id}
                        onClick={() => {
                          onSelect(appProvider.id);
                        }}
                      >
                        <TableCell className="w-14">
                          <img
                            src={provider.logo}
                            className="h-10 w-10 rounded p-0.5 shadow"
                            alt={provider.name}
                          />
                        </TableCell>
                        <TableCell>{appProvider.name}</TableCell>
                        <TableCell>{appProvider.alias}</TableCell>
                        <TableCell>{provider.name}</TableCell>
                        <TableCell>{appProvider.description}</TableCell>
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
