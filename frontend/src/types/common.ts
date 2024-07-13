export type PaginatedOutput<T> = {
  items: T[];
  hasNext: boolean;
  nextCursor?: string | null;
};

export type EnumOption = {
  label: string;
  value: string;
};
