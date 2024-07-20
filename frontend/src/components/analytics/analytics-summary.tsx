import React from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Skeleton } from '@/components/ui/skeleton';
import { AnalyticsSummaryOutput } from '@/types/analytics';
import { formatCost } from '@/lib/utils';

function percentageDifference(current: number, previous: number) {
  if (previous == 0) {
    if (current == 0) {
      return 0;
    }

    return 100;
  }

  return ((current - previous) / previous) * 100;
}

export function AnalyticsSummarySkeleton() {
  return (
    <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">Total requests</CardTitle>
        </CardHeader>
        <CardContent>
          <Skeleton className="h-6 w-20" />
          <Skeleton className="mt-2 h-3 w-32" />
        </CardContent>
      </Card>
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">Total tokens</CardTitle>
        </CardHeader>
        <CardContent>
          <Skeleton className="h-6 w-20" />
          <Skeleton className="mt-2 h-3 w-32" />
        </CardContent>
      </Card>
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">Total cost</CardTitle>
        </CardHeader>
        <CardContent>
          <Skeleton className="h-6 w-20" />
          <Skeleton className="mt-2 h-3 w-32" />
        </CardContent>
      </Card>
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">
            Avg. response time
          </CardTitle>
        </CardHeader>
        <CardContent>
          <Skeleton className="h-6 w-20" />
          <Skeleton className="mt-2 h-3 w-32" />
        </CardContent>
      </Card>
    </div>
  );
}

interface AnalyticsSummaryProps {
  summary: AnalyticsSummaryOutput;
}

export function AnalyticsSummary({ summary }: AnalyticsSummaryProps) {
  const totalRequests = summary.totalRequests;
  const totalRequestsDifference = percentageDifference(
    totalRequests,
    summary.previousTotalRequests,
  );

  const totalTokens = summary.totalTokens;
  const totalTokensDifference = percentageDifference(
    totalTokens,
    summary.previousTotalTokens,
  );

  const totalCost = summary.totalCost;
  const totalCostDifference = percentageDifference(
    totalCost,
    summary.previousTotalCost,
  );

  const averageDuration = summary.averageDuration;
  const averageDurationDifference = percentageDifference(
    averageDuration,
    summary.previousAverageDuration,
  );

  return (
    <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">Total requests</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="text-2xl font-bold">
            {totalRequests.toLocaleString()}
          </div>
          <p className="text-xs text-muted-foreground">
            {totalRequestsDifference > 0 ? '+' : ''}
            {totalRequestsDifference.toFixed(1)}% from previous period
          </p>
        </CardContent>
      </Card>
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">Total tokens</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="text-2xl font-bold">
            {totalTokens.toLocaleString()}
          </div>
          <p className="text-xs text-muted-foreground">
            {totalTokensDifference > 0 ? '+' : ''}
            {totalTokensDifference.toFixed(1)}% from previous period
          </p>
        </CardContent>
      </Card>
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">Total cost</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="text-2xl font-bold">{formatCost(totalCost)}</div>
          <p className="text-xs text-muted-foreground">
            {totalCostDifference > 0 ? '+' : ''}
            {totalCostDifference.toFixed(1)}% from previous period
          </p>
        </CardContent>
      </Card>
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">
            Avg. response time
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="text-2xl font-bold">
            {averageDuration.toFixed(2)} ms
          </div>
          <p className="text-xs text-muted-foreground">
            {averageDurationDifference > 0 ? '+' : ''}
            {averageDurationDifference.toFixed(1)}% from previous period
          </p>
        </CardContent>
      </Card>
    </div>
  );
}
