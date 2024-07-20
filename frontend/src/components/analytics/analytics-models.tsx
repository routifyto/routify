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

export function AnalyticsModelsSkeleton() {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Models</CardTitle>
      </CardHeader>
      <CardContent>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Model</TableHead>
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

interface AnalyticsListsProps {
  modelsMetrics: MetricsOutput[];
}

export function AnalyticsModels({ modelsMetrics }: AnalyticsListsProps) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Models</CardTitle>
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
            {modelsMetrics.map((modelMetrics) => {
              return (
                <TableRow key={modelMetrics.id}>
                  <TableCell>{modelMetrics.id}</TableCell>
                  <TableCell className="text-right">
                    {modelMetrics.totalRequests.toLocaleString()}
                  </TableCell>
                  <TableCell className="text-right">
                    {modelMetrics.totalTokens.toLocaleString()}
                  </TableCell>
                  <TableCell className="text-right">
                    {formatCost(modelMetrics.totalCost)}
                  </TableCell>
                  <TableCell className="text-right">
                    {modelMetrics.averageDuration.toFixed(2)}ms
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
