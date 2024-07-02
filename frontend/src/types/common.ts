export type PaginatedPayload<T> = {
  items: T[];
  hasNext: boolean;
  nextCursor?: string | null;
};

export type EnumOption = {
  label: string;
  value: string;
};
