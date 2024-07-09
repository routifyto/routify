import React from 'react';
import { useApp } from '@/contexts/app';
import { Button } from '@/components/ui/button';
import { Plus } from 'lucide-react';
import { InView } from 'react-intersection-observer';
import { Spinner } from '@/components/ui/spinner';
import { useNavigate } from 'react-router-dom';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { Skeleton } from '@/components/ui/skeleton';
import { useGetRoutesQuery } from '@/api/routes';

export function Routes() {
  const app = useApp();
  const navigate = useNavigate();

  const { data, isPending, hasNextPage, isFetchingNextPage, fetchNextPage } =
    useGetRoutesQuery(app.id, 20);

  return (
    <div className="flex flex-col gap-4">
      <div className="flex flex-row items-center justify-between pb-4">
        <h1 className="text-2xl font-semibold">Routes</h1>
        <Button
          variant="outline"
          className="flex flex-row items-center gap-1"
          onClick={() => {
            navigate(`/${app.id}/routes/create`);
          }}
        >
          <Plus className="h-4 w-4" />
          Add route
        </Button>
      </div>
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Name</TableHead>
            <TableHead>Path</TableHead>
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
                  {page.items.map((route) => {
                    return (
                      <TableRow
                        className="hover:cursor-pointer"
                        key={route.id}
                        onClick={() => {
                          navigate(`/${app.id}/routes/${route.id}`);
                        }}
                      >
                        <TableCell>{route.name}</TableCell>
                        <TableCell>/{route.path}</TableCell>
                        <TableCell>{route.description}</TableCell>
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
    </div>
  );
}
