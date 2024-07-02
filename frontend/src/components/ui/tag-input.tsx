import { X } from 'lucide-react';
import React, { useState } from 'react';

interface TagInputProps {
  placeholder?: string;
  values: string[];
  onChange?: (values: string[]) => void;
  readOnly?: boolean;
}

const TagInput = ({
  values,
  onChange,
  placeholder,
  readOnly,
}: TagInputProps) => {
  const [tempValue, setTempValue] = useState('');
  return (
    <div className="flex h-9 w-full flex-row gap-2 rounded-md border border-input bg-background p-1 text-sm shadow-sm transition-colors placeholder:text-muted-foreground">
      {values.map((value) => (
        <p
          key={value}
          className="flex h-full flex-row items-center gap-1 border border-gray-200 bg-gray-100 p-0.5 px-1 text-primary shadow"
        >
          <span>{value}</span>
          <X
            className="h-4 w-4 text-muted-foreground hover:cursor-pointer hover:text-primary"
            onClick={() => {
              onChange?.(values.filter((v) => v !== value));
            }}
          />
        </p>
      ))}
      {!readOnly && (
        <input
          value={tempValue}
          className="flex-grow px-1 focus-visible:outline-none"
          onChange={(e) => {
            setTempValue(e.currentTarget.value);
          }}
          placeholder={placeholder}
          onKeyDown={(e) => {
            if (e.key === 'Enter') {
              e.preventDefault();
              e.stopPropagation();
              onChange?.([...values, tempValue]);
              setTempValue('');
            }
          }}
          onBlur={() => {
            if (tempValue) {
              onChange?.([...values, tempValue]);
              setTempValue('');
            }
          }}
        />
      )}
    </div>
  );
};

export { TagInput };
