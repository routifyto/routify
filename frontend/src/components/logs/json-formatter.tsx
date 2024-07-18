import React from 'react';
import { formatJson } from '@/lib/utils';
import { Light as SyntaxHighlighter } from 'react-syntax-highlighter';
import json from 'react-syntax-highlighter/dist/esm/languages/hljs/json';
import { docco } from 'react-syntax-highlighter/dist/esm/styles/hljs';

SyntaxHighlighter.registerLanguage('json', json);

interface JsonFormatterProps {
  json: string;
}

export function JsonFormatter({ json }: JsonFormatterProps) {
  return (
    <SyntaxHighlighter
      className="rounded-md bg-white p-3 text-sm"
      language="json"
      style={docco}
    >
      {formatJson(json)}
    </SyntaxHighlighter>
  );
}
