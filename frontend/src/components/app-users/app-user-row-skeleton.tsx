import React from 'react';
import { Card, CardHeader } from '@/components/ui/card';
import { Skeleton } from '@/components/ui/skeleton';

export function AppUserRowSkeleton() {
  return (
    <Card className="flex flex-row items-center gap-3 p-4">
      <Skeleton className="h-9 w-9" />
      <CardHeader className="flex-1 p-1">
        <Skeleton className="h-4 w-20" />
        <Skeleton className="h-3 w-40" />
      </CardHeader>
    </Card>
  );
}
