import React from 'react';
import { Area, AreaChart, CartesianGrid } from 'recharts';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import {
  ChartConfig,
  ChartContainer,
  ChartTooltip,
  ChartTooltipContent,
} from '@/components/ui/chart';
import { Skeleton } from '@/components/ui/skeleton';
import { AnalyticsHistogramOutput } from '@/types/analytics';

export function AnalyticsHistogramSkeleton() {
  return (
    <Card className="w-full">
      <CardHeader>
        <CardTitle>Requests histogram</CardTitle>
      </CardHeader>
      <CardContent className="flex h-[330px] flex-row items-end justify-center gap-4 p-10 pl-2">
        <Skeleton className="h-6 w-128" />
      </CardContent>
    </Card>
  );
}

const chartConfig = {
  count: {
    label: 'Requests',
    color: 'hsl(var(--chart-1))',
  },
} satisfies ChartConfig;

interface AnalyticsHistogramProps {
  histogram: AnalyticsHistogramOutput;
}

export function AnalyticsHistogram({ histogram }: AnalyticsHistogramProps) {
  const requestsHistogram = histogram.requests.map((item) => ({
    name: new Date(item.dateTime).toLocaleString(),
    count: item.count,
  }));

  return (
    <Card className="w-full">
      <CardHeader>
        <CardTitle>Requests histogram</CardTitle>
      </CardHeader>
      <CardContent className="pl-2">
        <ChartContainer config={chartConfig} className="h-[350px] w-full">
          <AreaChart
            accessibilityLayer
            data={requestsHistogram}
            margin={{
              left: 12,
              right: 12,
            }}
          >
            <CartesianGrid vertical={false} />
            <ChartTooltip
              labelFormatter={(label, payload) => {
                return payload[0].payload.name;
              }}
              cursor={true}
              content={<ChartTooltipContent />}
            />
            <defs>
              <linearGradient id="fillCount" x1="0" y1="0" x2="0" y2="1">
                <stop
                  offset="5%"
                  stopColor="var(--color-count)"
                  stopOpacity={0.8}
                />
                <stop
                  offset="95%"
                  stopColor="var(--color-count)"
                  stopOpacity={0.1}
                />
              </linearGradient>
            </defs>
            <Area
              dataKey="count"
              type="natural"
              fill="url(#fillCount)"
              fillOpacity={0.4}
              stroke="var(--color-count)"
              stackId="a"
            />
          </AreaChart>
        </ChartContainer>
      </CardContent>
    </Card>
  );
}
