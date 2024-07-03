import React from 'react';
import { useFormContext } from 'react-hook-form';
import { match } from 'ts-pattern';

import { AppProviderInput } from '@/types/app-providers';
import { OpenAiConfigForm } from '@/components/app-providers/configs/openai-config-form';
import { AnthropicConfigForm } from '@/components/app-providers/configs/anthropic-config-form';
import { MistralAiConfigForm } from '@/components/app-providers/configs/mistral-ai-config-form';
import { AnyscaleConfigForm } from '@/components/app-providers/configs/anyscale-config-form';
import { GoogleConfigForm } from '@/components/app-providers/configs/google-config-form';
import { CohereConfigForm } from '@/components/app-providers/configs/cohere-config-form';
import { TogetherAiConfigForm } from '@/components/app-providers/configs/together-ai-config-form';
import { WorkersAiConfigForm } from '@/components/app-providers/configs/workers-ai-config-form';
import { AzureOpenAiConfigForm } from '@/components/app-providers/configs/azure-openai-config-form';
import { BedrockConfigForm } from '@/components/app-providers/configs/bedrock-config-form';
import { GroqConfigForm } from '@/components/app-providers/configs/groq-config-form';
import { PerplexityAiConfigForm } from '@/components/app-providers/configs/perplexity-ai-config-form';

export function AppProviderConfigForm() {
  const form = useFormContext<AppProviderInput>();

  const provider = form.watch('provider');
  return match(provider)
    .with('openai', () => <OpenAiConfigForm />)
    .with('anthropic', () => <AnthropicConfigForm />)
    .with('mistral-ai', () => <MistralAiConfigForm />)
    .with('anyscale', () => <AnyscaleConfigForm />)
    .with('google', () => <GoogleConfigForm />)
    .with('cohere', () => <CohereConfigForm />)
    .with('together-ai', () => <TogetherAiConfigForm />)
    .with('workers-ai', () => <WorkersAiConfigForm />)
    .with('azure-openai', () => <AzureOpenAiConfigForm />)
    .with('bedrock', () => <BedrockConfigForm />)
    .with('perplexity-ai', () => <PerplexityAiConfigForm />)
    .with('groq', () => <GroqConfigForm />)
    .otherwise(() => null);
}
