import {
  Card,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import { Skeleton } from '@/components/ui/skeleton';

export function AppUserRowSkeleton() {
  return (
    <Card className="flex flex-row items-center gap-3 p-4">
      <Skeleton className="h-9 w-9" />
      <CardHeader className="flex-1 p-1">
        <CardTitle>
          <Skeleton className="h-4 w-20" />
        </CardTitle>
        <CardDescription>
          <Skeleton className="h-3 w-40" />
        </CardDescription>
      </CardHeader>
    </Card>
  );
}
