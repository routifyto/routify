import React from 'react';
import { cn, hashCode } from '@/lib/utils';

function getAvatarSizeClasses(size?: string) {
  if (size === 'small') {
    return 'w-5 h-5';
  }
  if (size === 'medium') {
    return 'w-9 h-9';
  }
  if (size === 'large') {
    return 'w-12 h-12';
  }
  if (size === 'extra-large') {
    return 'w-16 h-16';
  }

  return 'w-9 h-9';
}

const colors = [
  'rgb(248 113 113)',
  'rgb(74 222 128)',
  'rgb(96 165 250)',
  'rgb(251 146 60)',
  'rgb(244 114 182)',
  'rgb(250 204 21)',
  'rgb(129 140 248)',
  'rgb(192 132 252)',
  'rgb(45 212 191)',
  'rgb(156 163 175)',
];

function getColorForId(id: string) {
  const index = Math.abs(hashCode(id)) % colors.length;
  return colors[index];
}

const Avatar = ({
  id,
  name,
  avatar,
  size,
  className,
}: {
  id: string;
  name: string;
  avatar?: string;
  size?: 'small' | 'medium' | 'large' | 'extra-large';
  className?: string;
}) => {
  if (avatar) {
    return (
      <img
        src={avatar}
        className={cn('rounded shadow', getAvatarSizeClasses(size), className)}
        alt={name}
      />
    );
  }

  const color = getColorForId(id);
  return (
    <div
      className={cn(
        'inline-flex items-center justify-center overflow-hidden rounded text-white shadow',
        getAvatarSizeClasses(size),
        className,
      )}
      style={{ backgroundColor: color }}
    >
      <span className="font-medium">{name[0]?.toLocaleUpperCase()}</span>
    </div>
  );
};

export { Avatar };
