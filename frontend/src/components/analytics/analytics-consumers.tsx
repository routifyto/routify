import React from 'react';
import { MetricsOutput } from '@/types/analytics';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { Skeleton } from '@/components/ui/skeleton';
import { formatCost } from '@/lib/utils';

export function AnalyticsConsumersSkeleton() {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Models</CardTitle>
      </CardHeader>
      <CardContent>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Consumer</TableHead>
              <TableHead className="text-right">Total requests</TableHead>
              <TableHead className="text-right">Total tokens</TableHead>
              <TableHead className="text-right">Total cost</TableHead>
              <TableHead className="text-right">Avg. response time</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {Array.from({ length: 3 }).map((_, index) => (
              <TableRow key={index}>
                <TableCell colSpan={5}>
                  <Skeleton className="h-8 w-full" />
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </CardContent>
    </Card>
  );
}

interface AnalyticsLConsumersProps {
  consumersMetrics: MetricsOutput[];
}

export function AnalyticsConsumers({
  consumersMetrics,
}: AnalyticsLConsumersProps) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Consumers</CardTitle>
      </CardHeader>
      <CardContent>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Provider</TableHead>
              <TableHead className="text-right">Total requests</TableHead>
              <TableHead className="text-right">Total tokens</TableHead>
              <TableHead className="text-right">Total cost</TableHead>
              <TableHead className="text-right">Avg. response time</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {consumersMetrics.map((consumerMetrics) => {
              return (
                <TableRow key={consumerMetrics.id}>
                  <TableCell>{consumerMetrics.name}</TableCell>
                  <TableCell className="text-right">
                    {consumerMetrics.totalRequests.toLocaleString()}
                  </TableCell>
                  <TableCell className="text-right">
                    {consumerMetrics.totalTokens.toLocaleString()}
                  </TableCell>
                  <TableCell className="text-right">
                    {formatCost(consumerMetrics.totalCost)}
                  </TableCell>
                  <TableCell className="text-right">
                    {consumerMetrics.averageDuration.toFixed(2)}ms
                  </TableCell>
                </TableRow>
              );
            })}
          </TableBody>
        </Table>
      </CardContent>
    </Card>
  );
}
