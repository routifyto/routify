import React from 'react';
import { MetricsOutput } from '@/types/analytics';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { providers } from '@/types/providers';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { Skeleton } from '@/components/ui/skeleton';

export function AnalyticsProvidersSkeleton() {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Providers</CardTitle>
      </CardHeader>
      <CardContent>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead></TableHead>
              <TableHead>Provider</TableHead>
              <TableHead className="text-right">Total requests</TableHead>
              <TableHead className="text-right">Total tokens</TableHead>
              <TableHead className="text-right">Total cost</TableHead>
              <TableHead className="text-right">Avg. response time</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {Array.from({ length: 3 }).map((_, index) => (
              <TableRow key={index}>
                <TableCell colSpan={6}>
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
  providersMetrics: MetricsOutput[];
}

export function AnalyticsProviders({ providersMetrics }: AnalyticsListsProps) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Providers</CardTitle>
      </CardHeader>
      <CardContent>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead></TableHead>
              <TableHead>Provider</TableHead>
              <TableHead className="text-right">Total requests</TableHead>
              <TableHead className="text-right">Total tokens</TableHead>
              <TableHead className="text-right">Total cost</TableHead>
              <TableHead className="text-right">Avg. response time</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {providersMetrics.map((providerMetrics) => {
              const provider = providers.find(
                (p) => p.id === providerMetrics.id,
              );
              if (!provider) {
                return null;
              }

              return (
                <TableRow key={provider.id}>
                  <TableCell className="w-14">
                    <img
                      src={provider.logo}
                      className="h-10 w-10 rounded p-0.5 shadow"
                      alt={provider.name}
                    />
                  </TableCell>
                  <TableCell>{provider.name}</TableCell>
                  <TableCell className="text-right">
                    {providerMetrics.totalRequests.toLocaleString()}
                  </TableCell>
                  <TableCell className="text-right">
                    {providerMetrics.totalTokens.toLocaleString()}
                  </TableCell>
                  <TableCell className="text-right">
                    {providerMetrics.totalCost.toLocaleString()}$
                  </TableCell>
                  <TableCell className="text-right">
                    {providerMetrics.averageDuration.toFixed(2)}ms
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
