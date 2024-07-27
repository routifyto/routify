import React from 'react';

const Code = ({ children }: { children: React.ReactNode }) => {
  return (
    <code className="relative rounded bg-muted px-[0.3rem] py-[0.2rem] font-mono text-sm">
      {children}
    </code>
  );
};

export { Code };
