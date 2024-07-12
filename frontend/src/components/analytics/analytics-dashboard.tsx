import React from 'react';
import { useApp } from '@/contexts/app';
import { Spinner } from '@/components/ui/spinner';
import { Button } from '@/components/ui/button';
import { RefreshCcwIcon } from 'lucide-react';
import {
  useGetAnalyticsHistogramQuery,
  useGetAnalyticsListsQuery,
  useGetAnalyticsSummaryQuery,
} from '@/api/analytics';
import {
  AnalyticsSummary,
  AnalyticsSummarySkeleton,
} from '@/components/analytics/analytics-summary';
import { AnalyticsPeriodDropdown } from '@/components/analytics/analytics-period-dropdown';
import {
  AnalyticsHistogram,
  AnalyticsHistogramSkeleton,
} from '@/components/analytics/analytics-histogram';
import {
  AnalyticsProviders,
  AnalyticsProvidersSkeleton,
} from '@/components/analytics/analytics-providers';
import {
  AnalyticsModels,
  AnalyticsModelsSkeleton,
} from '@/components/analytics/analytics-models';

export function AnalyticsDashboard() {
  const app = useApp();
  const [period, setPeriod] = React.useState<string>('today');

  const {
    data: summaryData,
    isLoading: isLoadingSummary,
    isRefetching: isRefetchingSummary,
    refetch: refetchSummary,
  } = useGetAnalyticsSummaryQuery(app.id, period);

  const {
    data: histogramData,
    isLoading: isLoadingHistogram,
    isRefetching: isRefetchingHistogram,
    refetch: refetchHistogram,
  } = useGetAnalyticsHistogramQuery(app.id, period);

  const {
    data: listsData,
    isLoading: isLoadingLists,
    isRefetching: isRefetchingLists,
    refetch: refetchLists,
  } = useGetAnalyticsListsQuery(app.id, period);

  const isReloading =
    isRefetchingSummary || isRefetchingHistogram || isRefetchingLists;

  function reloadAnalytics() {
    Promise.all([refetchSummary(), refetchHistogram(), refetchLists()]).then(
      () => {},
    );
  }

  return (
    <div className="flex flex-col gap-4">
      <div className="flex flex-row items-center justify-end gap-2">
        {isReloading && <Spinner className="h-4 w-4 text-foreground" />}
        <AnalyticsPeriodDropdown value={period} onChange={setPeriod} />
        <Button
          variant="outline"
          size="sm"
          className="justify-start border-dashed text-left font-normal text-muted-foreground hover:text-foreground"
          onClick={() => reloadAnalytics()}
          disabled={isReloading}
        >
          <RefreshCcwIcon className="mr-2 h-4 w-4" />
          <span>Refresh</span>
        </Button>
      </div>
      {isLoadingSummary ? (
        <AnalyticsSummarySkeleton />
      ) : (
        summaryData && <AnalyticsSummary summary={summaryData} />
      )}
      {isLoadingHistogram ? (
        <AnalyticsHistogramSkeleton />
      ) : (
        histogramData && <AnalyticsHistogram histogram={histogramData} />
      )}
      {isLoadingLists ? (
        <AnalyticsProvidersSkeleton />
      ) : (
        listsData && (
          <AnalyticsProviders providersMetrics={listsData.providers} />
        )
      )}
      {isLoadingLists ? (
        <AnalyticsModelsSkeleton />
      ) : (
        listsData && <AnalyticsModels modelsMetrics={listsData.models} />
      )}
    </div>
  );
}
