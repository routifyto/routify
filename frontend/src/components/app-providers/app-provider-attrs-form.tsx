import React from 'react';
import { useFormContext } from 'react-hook-form';
import { match } from 'ts-pattern';

import { AppProviderInput } from '@/types/app-providers';
import { OpenaiAppProviderAttrsForm } from '@/components/app-providers/attrs/openai-app-provider-attrs-form';
import { AnthropicAppProviderAttrsForm } from '@/components/app-providers/attrs/anthropic-app-provider-attrs-form';
import { MistralAppProviderAttrsForm } from '@/components/app-providers/attrs/mistral-app-provider-attrs-form';
import { AnyscaleAppProviderAttrsForm } from '@/components/app-providers/attrs/anyscale-app-provider-attrs-form';
import { GoogleAppProviderAttrsForm } from '@/components/app-providers/attrs/google-app-provider-attrs-form';
import { CohereAppProviderAttrsForm } from '@/components/app-providers/attrs/cohere-app-provider-attrs-form';
import { TogetherAiAppProviderAttrsForm } from '@/components/app-providers/attrs/together-ai-app-provider-attrs-form';
import { CloudflareAppProviderAttrsForm } from '@/components/app-providers/attrs/cloudflare-app-provider-attrs-form';
import { AzureOpenaiAppProviderAttrsForm } from '@/components/app-providers/attrs/azure-openai-app-provider-attrs-form';
import { BedrockAppProviderAttrsForm } from '@/components/app-providers/attrs/bedrock-app-provider-attrs-form';
import { GroqAppProviderAttrsForm } from '@/components/app-providers/attrs/groq-app-provider-attrs-form';
import { PerplexityAppProviderAttrsForm } from '@/components/app-providers/attrs/perplexity-app-provider-attrs-form';

export function AppProviderAttrsForm() {
  const form = useFormContext<AppProviderInput>();

  const provider = form.watch('provider');
  return match(provider)
    .with('openai', () => <OpenaiAppProviderAttrsForm />)
    .with('anthropic', () => <AnthropicAppProviderAttrsForm />)
    .with('mistral', () => <MistralAppProviderAttrsForm />)
    .with('anyscale', () => <AnyscaleAppProviderAttrsForm />)
    .with('google', () => <GoogleAppProviderAttrsForm />)
    .with('cohere', () => <CohereAppProviderAttrsForm />)
    .with('together-ai', () => <TogetherAiAppProviderAttrsForm />)
    .with('cloudflare', () => <CloudflareAppProviderAttrsForm />)
    .with('azure-openai', () => <AzureOpenaiAppProviderAttrsForm />)
    .with('bedrock', () => <BedrockAppProviderAttrsForm />)
    .with('perplexity', () => <PerplexityAppProviderAttrsForm />)
    .with('groq', () => <GroqAppProviderAttrsForm />)
    .otherwise(() => null);
}
