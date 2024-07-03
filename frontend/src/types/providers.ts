export type Provider = {
  id: string;
  name: string;
  description: string;
  logo: string;
};

export const providers: Provider[] = [
  {
    id: 'openai',
    name: 'OpenAI',
    description: 'Description coming soon.',
    logo: '/providers/openai.png',
  },
  {
    id: 'anthropic',
    name: 'Anthropic',
    description: 'Description coming soon.',
    logo: '/providers/anthropic.png',
  },
  {
    id: 'mistral-ai',
    name: 'MistralAI',
    description: 'Description coming soon.',
    logo: '/providers/mistral-ai.png',
  },
  {
    id: 'anyscale',
    name: 'Anyscale',
    description: 'Description coming soon.',
    logo: '/providers/anyscale.png',
  },
  {
    id: 'google',
    name: 'Google',
    description: 'Description coming soon.',
    logo: '/providers/google.png',
  },
  {
    id: 'cohere',
    name: 'Cohere',
    description: 'Description coming soon.',
    logo: '/providers/cohere.png',
  },
  {
    id: 'together-ai',
    name: 'TogetherAI',
    description: 'Description coming soon.',
    logo: '/providers/together-ai.png',
  },
  {
    id: 'workers-ai',
    name: 'Cloudflare WorkersAI',
    description: 'Description coming soon.',
    logo: '/providers/workers-ai.png',
  },
  {
    id: 'azure-openai',
    name: 'Azure OpenAI',
    description: 'Description coming soon.',
    logo: '/providers/azure-openai.png',
  },
  {
    id: 'bedrock',
    name: 'Amazon Bedrock',
    description: 'Description coming soon.',
    logo: '/providers/bedrock.png',
  },
  {
    id: 'perplexity-ai',
    name: 'PerplexityAI',
    description: 'Description coming soon.',
    logo: '/providers/perplexity-ai.png',
  },
  {
    id: 'groq',
    name: 'Groq',
    description: 'Description coming soon.',
    logo: '/providers/groq.png',
  },
];
