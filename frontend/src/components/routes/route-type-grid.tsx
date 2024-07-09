import React from 'react';
import { RouteType } from '@/types/routes';
import {
  Card,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import { cn } from '@/lib/utils';
import { Check } from 'lucide-react';

interface RouteTypeOption {
  value: RouteType;
  label: string;
  description: string;
}

const routeTypesData: RouteTypeOption[] = [
  {
    value: 'TEXT',
    label: 'Text',
    description: 'Generate text.',
  },
  {
    value: 'EMBEDDING',
    label: 'Embedding',
    description: 'Generate text embeddings.',
  },
];

interface RouteTypeSelectProps {
  value: RouteType;
  onChange: (value: RouteType) => void;
}

export function RouteTypeGrid({ value, onChange }: RouteTypeSelectProps) {
  return (
    <div className="grid grid-cols-2 gap-3">
      {routeTypesData.map((routeType) => {
        const isSelected = value === routeType.value;
        return (
          <Card
            key={routeType.value}
            className={cn(
              'hover:cursor-pointer hover:bg-gray-50',
              isSelected && 'border-gray-400',
            )}
            onClick={() => {
              onChange(routeType.value);
            }}
          >
            <div className="flex flex-row items-center gap-1">
              <CardHeader className="flex-1">
                <CardTitle>{routeType.label}</CardTitle>
                <CardDescription>{routeType.description}</CardDescription>
              </CardHeader>
              {isSelected && (
                <div className="mr-2 flex h-6 w-6 items-center justify-center rounded-full border border-gray-400">
                  <Check className="h-4 w-4" />
                </div>
              )}
            </div>
          </Card>
        );
      })}
    </div>
  );
}
