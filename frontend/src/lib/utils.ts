import { type ClassValue, clsx } from 'clsx';
import { twMerge } from 'tailwind-merge';

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export function hashCode(str: string) {
  let hash = 0;
  for (let i = 0; i < str.length; i++) {
    const character = str.charCodeAt(i);
    hash = (hash << 5) - hash + character;
    hash |= 0; // Convert to 32bit integer
  }
  return hash;
}

export function formatJson(json?: string | null) {
  if (!json) {
    return null;
  }

  try {
    return JSON.stringify(JSON.parse(json), null, 2);
  } catch (error) {
    return json;
  }
}
