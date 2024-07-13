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
import { useGetAppUsersQuery } from '@/api/app-users';
import { Avatar } from '@/components/ui/avatar';
import { AppUserRole } from '@/components/app-users/app-user-role';
import { AppUserDelete } from '@/components/app-users/app-user-delete';

export function AppUsersTable() {
  const app = useApp();
  const { data, isPending, hasNextPage, isFetchingNextPage, fetchNextPage } =
    useGetAppUsersQuery(app.id, 20);

  return (
    <React.Fragment>
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead></TableHead>
            <TableHead>Name</TableHead>
            <TableHead>Email</TableHead>
            <TableHead>Role</TableHead>
            <TableHead className="w-10"></TableHead>
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
                  {page.items.map((appUser) => {
                    return (
                      <TableRow key={appUser.id}>
                        <TableCell className="w-14">
                          <Avatar id={appUser.userId} name={appUser.name} />
                        </TableCell>
                        <TableCell>{appUser.name}</TableCell>
                        <TableCell>{appUser.email}</TableCell>
                        <TableCell>
                          <AppUserRole appUser={appUser} />
                        </TableCell>
                        <TableCell className="w-10">
                          <AppUserDelete appUserId={appUser.id} />
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
    </React.Fragment>
  );
}
