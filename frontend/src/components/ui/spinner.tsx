import * as React from 'react';

import { cn } from '@/lib/utils';
import { Loader2 } from 'lucide-react';

export interface SpinnerProps {
  className?: string;
  size?: string | number | undefined;
}

const Spinner = React.forwardRef<SVGSVGElement, SpinnerProps>(
  ({ className, ...props }, ref) => (
    <Loader2 className={cn('animate-spin', className)} ref={ref} {...props} />
  ),
);
Spinner.displayName = 'Spinner';

export { Spinner };
