import * as React from 'react';

import { cn } from '@/lib/utils';
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import { Button } from '@/components/ui/button';
import { Copy, Eye, EyeOff } from 'lucide-react';

export interface SecretInputProps
  extends React.InputHTMLAttributes<HTMLInputElement> {
  onCopy?: () => void;
}

const SecretInput = React.forwardRef<HTMLInputElement, SecretInputProps>(
  ({ className, onCopy, ...props }, ref) => {
    const [displayInput, setDisplayInput] = React.useState(false);

    return (
      <div className="flex flex-row items-center gap-1">
        <input
          autoComplete="off"
          type={displayInput ? 'text' : 'password'}
          className={cn(
            'flex h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 text-sm shadow-sm transition-colors file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:cursor-not-allowed disabled:opacity-50',
            className,
          )}
          ref={ref}
          {...props}
          value={props.value ?? ''}
        />
        <Tooltip delayDuration={100}>
          <TooltipTrigger asChild>
            <Button
              variant="outline"
              size="icon"
              type="button"
              onClick={() => {
                setDisplayInput(!displayInput);
              }}
            >
              {displayInput ? (
                <EyeOff className="h-4 w-4" />
              ) : (
                <Eye className="h-4 w-4" />
              )}
            </Button>
          </TooltipTrigger>
          <TooltipContent>
            <p>{displayInput ? 'Hide' : 'Show'}</p>
          </TooltipContent>
        </Tooltip>
        <Tooltip delayDuration={100}>
          <TooltipTrigger asChild>
            <Button
              variant="outline"
              size="icon"
              type="button"
              onClick={() => {
                onCopy?.();
              }}
            >
              <Copy className="h-4 w-4" />
            </Button>
          </TooltipTrigger>
          <TooltipContent>
            <p>Copy to clipboard</p>
          </TooltipContent>
        </Tooltip>
      </div>
    );
  },
);
SecretInput.displayName = 'SecretInput';

export { SecretInput };
