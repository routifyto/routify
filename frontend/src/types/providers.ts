export type Provider = {
  id: string;
  name: string;
  description: string;
  logo: string;
  models: Model[];
};

export type Model = {
  id: string;
  name: string;
  description: string;
  contextWindow: number;
};

export const providers: Provider[] = [
  {
    id: 'openai',
    name: 'OpenAI',
    description: 'Description coming soon.',
    logo: '/providers/openai.png',
    models: [
      {
        id: 'gpt-4o',
        name: 'gpt-4o',
        description:
          'Our most advanced, multimodal flagship model that’s cheaper and faster than GPT-4 Turbo. Currently points to gpt-4o-2024-05-13.',
        contextWindow: 128000,
      },
      {
        id: 'gpt-4-turbo',
        name: 'gpt-4-turbo',
        description:
          'The latest GPT-4 Turbo model with vision capabilities. Vision requests can now use JSON mode and function calling. Currently points to gpt-4-turbo-2024-04-09.',
        contextWindow: 128000,
      },
      {
        id: 'gpt-4-turbo-2024-04-09',
        name: 'gpt-4-turbo-2024-04-09',
        description:
          'The latest GPT-4 Turbo model with vision capabilities. Vision requests can now use JSON mode and function calling.',
        contextWindow: 128000,
      },
      {
        id: 'gpt-4-turbo-preview',
        name: 'gpt-4-turbo-preview',
        description:
          'GPT-4 Turbo preview model. Currently points to gpt-4-0125-preview.',
        contextWindow: 128000,
      },
      {
        id: 'gpt-4-0125-preview',
        name: 'gpt-4-0125-preview',
        description:
          'GPT-4 Turbo preview model intended to reduce cases of “laziness” where the model doesn’t complete a task. Returns a maximum of 4,096 output tokens.',
        contextWindow: 128000,
      },
      {
        id: 'gpt-4-1106-preview',
        name: 'gpt-4-1106-preview',
        description:
          'GPT-4 Turbo preview model featuring improved instruction following, JSON mode, reproducible outputs, parallel function calling, and more. Returns a maximum of 4,096 output tokens. This is a preview model.',
        contextWindow: 128000,
      },
      {
        id: 'gpt-4',
        name: 'gpt-4',
        description: 'Currently points to gpt-4-0613.',
        contextWindow: 8192,
      },
      {
        id: 'gpt-4-0613',
        name: 'gpt-4-0613',
        description:
          'Snapshot of gpt-4 from June 13th 2023 with improved function calling support.',
        contextWindow: 8192,
      },
      {
        id: 'gpt-3.5-turbo-0125',
        name: 'gpt-3.5-turbo-0125',
        description:
          'The latest GPT-3.5 Turbo model with higher accuracy at responding in requested formats and a fix for a bug which caused a text encoding issue for non-English language function calls. Returns a maximum of 4,096 output tokens.',
        contextWindow: 16385,
      },
      {
        id: 'gpt-3.5-turbo',
        name: 'gpt-3.5-turbo',
        description: 'Currently points to gpt-3.5-turbo-0125.',
        contextWindow: 16385,
      },
      {
        id: 'gpt-3.5-turbo-1106',
        name: 'gpt-3.5-turbo-1106',
        description:
          'GPT-3.5 Turbo model with improved instruction following, JSON mode, reproducible outputs, parallel function calling, and more. Returns a maximum of 4,096 output tokens.',
        contextWindow: 16385,
      },
    ],
  },
  {
    id: 'anthropic',
    name: 'Anthropic',
    description: 'Description coming soon.',
    logo: '/providers/anthropic.png',
    models: [],
  },
  {
    id: 'mistral-ai',
    name: 'MistralAI',
    description: 'Description coming soon.',
    logo: '/providers/mistral-ai.png',
    models: [],
  },
  {
    id: 'anyscale',
    name: 'Anyscale',
    description: 'Description coming soon.',
    logo: '/providers/anyscale.png',
    models: [],
  },
  {
    id: 'google',
    name: 'Google',
    description: 'Description coming soon.',
    logo: '/providers/google.png',
    models: [],
  },
  {
    id: 'cohere',
    name: 'Cohere',
    description: 'Description coming soon.',
    logo: '/providers/cohere.png',
    models: [],
  },
  {
    id: 'together-ai',
    name: 'TogetherAI',
    description: 'Description coming soon.',
    logo: '/providers/together-ai.png',
    models: [],
  },
  {
    id: 'workers-ai',
    name: 'Cloudflare WorkersAI',
    description: 'Description coming soon.',
    logo: '/providers/workers-ai.png',
    models: [],
  },
  {
    id: 'azure-openai',
    name: 'Azure OpenAI',
    description: 'Description coming soon.',
    logo: '/providers/azure-openai.png',
    models: [],
  },
  {
    id: 'bedrock',
    name: 'Amazon Bedrock',
    description: 'Description coming soon.',
    logo: '/providers/bedrock.png',
    models: [],
  },
  {
    id: 'perplexity-ai',
    name: 'PerplexityAI',
    description: 'Description coming soon.',
    logo: '/providers/perplexity-ai.png',
    models: [],
  },
  {
    id: 'groq',
    name: 'Groq',
    description: 'Description coming soon.',
    logo: '/providers/groq.png',
    models: [],
  },
];
