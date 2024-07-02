import React from 'react';
import { useApp } from '@/contexts/app';
import { useGetAppUsersQuery } from '@/api/app-users';
import { Button } from '@/components/ui/button';
import { Plus } from 'lucide-react';
import { AppUserRow } from '@/components/app-users/app-user-row';
import { AppUsersCreate } from '@/components/app-users/app-users-create';
import { AppUserRowSkeleton } from '@/components/app-users/app-user-row-skeleton';
import { InView } from 'react-intersection-observer';
import { Spinner } from '@/components/ui/spinner';

export function AppUsers() {
  const app = useApp();
  const [openCreate, setOpenCreate] = React.useState(false);
  const { data, isPending, hasNextPage, isFetchingNextPage, fetchNextPage } =
    useGetAppUsersQuery(app.id, 20);

  return (
    <div className="flex flex-col gap-4">
      <div className="flex flex-row items-center justify-between pb-4">
        <h1 className="text-2xl font-semibold">Users</h1>
        <Button
          variant="outline"
          className="flex flex-row items-center gap-1"
          onClick={() => {
            setOpenCreate(true);
          }}
        >
          <Plus className="h-4 w-4" />
          Add users
        </Button>
      </div>
      <div className="flex flex-col gap-4">
        {isPending ? (
          <React.Fragment>
            {Array.from({ length: 5 }).map((_, index) => (
              <AppUserRowSkeleton key={index} />
            ))}
          </React.Fragment>
        ) : (
          <React.Fragment>
            {data?.pages.map((page) => (
              <React.Fragment key={page.nextCursor}>
                {page.items.map((appUser) => (
                  <AppUserRow key={appUser.id} appUser={appUser} />
                ))}
              </React.Fragment>
            ))}
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
          </React.Fragment>
        )}
      </div>
      <AppUsersCreate
        appId={app.id}
        open={openCreate}
        onOpenChange={setOpenCreate}
      />
    </div>
  );
}
