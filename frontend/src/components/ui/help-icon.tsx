import React from 'react';
import {
  HoverCard,
  HoverCardContent,
  HoverCardTrigger,
} from '@/components/ui/hover-card';
import { ExternalLink, Info } from 'lucide-react';

interface HelpIconProps {
  children: React.ReactNode;
  learnMoreLink?: string;
}

const HelpIcon = ({ children, learnMoreLink }: HelpIconProps) => {
  return (
    <HoverCard>
      <HoverCardTrigger asChild>
        <Info className="h-4 w-4 hover:cursor-pointer" />
      </HoverCardTrigger>
      <HoverCardContent className="flex w-128 flex-col gap-2 text-foreground/90">
        {children}
        {learnMoreLink && (
          <a
            href={learnMoreLink}
            target="_blank"
            className="mt-2 flex flex-row items-center gap-2 font-semibold text-blue-500 hover:underline"
            rel="noreferrer"
          >
            <span>Learn more</span>
            <ExternalLink className="h-4 w-4" />
          </a>
        )}
      </HoverCardContent>
    </HoverCard>
  );
};

export { HelpIcon };
