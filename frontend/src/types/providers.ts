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
    models: [
      {
        id: 'claude-3-5-sonnet-20240620',
        name: 'claude-3-5-sonnet-20240620',
        description: 'Our most intelligent model.',
        contextWindow: 200000,
      },
      {
        id: 'claude-3-opus-20240229',
        name: 'claude-3-opus-20240229',
        description: 'Claude 3 Opus.',
        contextWindow: 200000,
      },
      {
        id: 'claude-3-sonnet-20240229',
        name: 'claude-3-sonnet-20240229',
        description: 'Claude 3 Sonnet.',
        contextWindow: 200000,
      },
      {
        id: 'claude-3-haiku-20240307',
        name: 'claude-3-haiku-20240307',
        description: 'Claude 3 Haiku.',
        contextWindow: 200000,
      },
    ],
  },
  {
    id: 'mistral-ai',
    name: 'MistralAI',
    description: 'Description coming soon.',
    logo: '/providers/mistral-ai.png',
    models: [
      {
        id: 'open-mistral-7b',
        name: 'open-mistral-7b',
        description:
          'The first dense model released by Mistral AI, perfect for experimentation, customization, and quick iteration. At the time of the release, it matched the capabilities of models up to 30B parameters.',
        contextWindow: 32000,
      },
      {
        id: 'open-mixtral-8x7b',
        name: 'open-mixtral-8x7b',
        description:
          'A sparse mixture of experts model. As such, it leverages up to 45B parameters but only uses about 12B during inference, leading to better inference throughput at the cost of more vRAM.',
        contextWindow: 32000,
      },
      {
        id: 'open-mixtral-8x22b',
        name: 'open-mixtral-8x22b',
        description:
          'A bigger sparse mixture of experts model. As such, it leverages up to 141B parameters but only uses about 39B during inference, leading to better inference throughput at the cost of more vRAM.',
        contextWindow: 32000,
      },
      {
        id: 'mistral-small-2402',
        name: 'mistral-small-2402',
        description:
          'Suitable for simple tasks that one can do in bulk (Classification, Customer Support, or Text Generation).',
        contextWindow: 32000,
      },
      {
        id: 'codestral-2405',
        name: 'codestral-2405',
        description:
          'State-of-the-art Mistral model trained specifically for code tasks.',
        contextWindow: 32000,
      },
      {
        id: 'mistral-large-2402',
        name: 'mistral-large-2402',
        description:
          "Our flagship model that's ideal for complex tasks that require large reasoning capabilities or are highly specialized (Synthetic Text Generation, Code Generation, RAG, or Agents).",
        contextWindow: 32000,
      },
    ],
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
    models: [
      {
        id: 'command-r-plus',
        name: 'command-r-plus',
        description:
          'Command R+ is an instruction-following conversational model that performs language tasks at a higher quality, more reliably, and with a longer context than previous models. It is best suited for complex RAG workflows and multi-step tool use.',
        contextWindow: 128000,
      },
      {
        id: 'command-r',
        name: 'command-r',
        description:
          'Command R is an instruction-following conversational model that performs language tasks at a higher quality, more reliably, and with a longer context than previous models. It can be used for complex workflows like code generation, retrieval augmented generation (RAG), tool use, and agents.',
        contextWindow: 128000,
      },
      {
        id: 'command',
        name: 'command',
        description:
          'An instruction-following conversational model that performs language tasks with high quality, more reliably and with a longer context than our base generative models.',
        contextWindow: 4000,
      },
      {
        id: 'command-nightly',
        name: 'command-nightly',
        description:
          'Be advised that command-nightly is the latest, most experimental, and (possibly) unstable version of its default counterpart. Nightly releases are updated regularly, without warning, and are not recommended for production use.',
        contextWindow: 128000,
      },
      {
        id: 'command-light',
        name: 'command-light',
        description:
          'A smaller, faster version of command. Almost as capable, but a lot faster.',
        contextWindow: 4000,
      },
      {
        id: 'command-light-nightly',
        name: 'command-light-nightly',
        description:
          'Be advised that command-light-nightly is the latest, most experimental, and (possibly) unstable version of its default counterpart. Nightly releases are updated regularly, without warning, and are not recommended for production use.',
        contextWindow: 4000,
      },
    ],
  },
  {
    id: 'together-ai',
    name: 'TogetherAI',
    description: 'Description coming soon.',
    logo: '/providers/together-ai.png',
    models: [
      {
        id: 'Qwen/Qwen2-72B-Instruct',
        name: 'Qwen/Qwen2-72B-Instruct',
        description:
          'Qwen2 is the new series of Qwen large language models. For Qwen2, we release a number of base language models and instruction-tuned language models ranging from 0.5 to 72 billion parameters, including a Mixture-of-Experts model.',
        contextWindow: 131072,
      },
      {
        id: 'meta-llama/Llama-3-70b-chat-hf',
        name: 'meta-llama/Llama-3-70b-chat-hf',
        description:
          'Llama 3 is an auto-regressive language model that uses an optimized transformer architecture. The tuned versions use supervised fine-tuning (SFT) and reinforcement learning with human feedback (RLHF) to align with human preferences for helpfulness and safety.',
        contextWindow: 8000,
      },
      {
        id: 'Snowflake/snowflake-arctic-instruct',
        name: 'Snowflake/snowflake-arctic-instruct',
        description:
          'Arctic is a dense-MoE Hybrid transformer architecture pre-trained from scratch by the Snowflake AI Research Team',
        contextWindow: 0,
      },
      {
        id: 'meta-llama/Llama-3-8b-chat-hf',
        name: 'meta-llama/Llama-3-8b-chat-hf',
        description:
          'Llama 3 is an auto-regressive language model that uses an optimized transformer architecture. The tuned versions use supervised fine-tuning (SFT) and reinforcement learning with human feedback (RLHF) to align with human preferences for helpfulness and safety.',
        contextWindow: 0,
      },
      {
        id: 'microsoft/WizardLM-2-8x22B',
        name: 'microsoft/WizardLM-2-8x22B',
        description:
          "WizardLM-2 8x22B is Wizard's most advanced model, demonstrates highly competitive performance compared to those leading proprietary works and consistently outperforms all the existing state-of-the-art opensource models.",
        contextWindow: 0,
      },
      {
        id: 'togethercomputer/StripedHyena-Nous-7B',
        name: 'togethercomputer/StripedHyena-Nous-7B',
        description:
          'A hybrid architecture composed of multi-head, grouped-query attention and gated convolutions arranged in Hyena blocks, different from traditional decoder-only Transformers',
        contextWindow: 0,
      },
      {
        id: 'databricks/dbrx-instruct',
        name: 'databricks/dbrx-instruct',
        description:
          'DBRX Instruct is a mixture-of-experts (MoE) large language model trained from scratch by Databricks. DBRX Instruct specializes in few-turn interactions.',
        contextWindow: 0,
      },
      {
        id: 'allenai/OLMo-7B-Instruct',
        name: 'allenai/OLMo-7B-Instruct',
        description: 'The OLMo models are trained on the Dolma dataset.',
        contextWindow: 0,
      },
      {
        id: 'deepseek-ai/deepseek-llm-67b-chat',
        name: 'deepseek-ai/deepseek-llm-67b-chat',
        description:
          'trained from scratch on a vast dataset of 2 trillion tokens in both English and Chinese.',
        contextWindow: 0,
      },
      {
        id: 'google/gemma-7b-it',
        name: 'google/gemma-7b-it',
        description:
          'Gemma is a family of lightweight, state-of-the-art open models from Google, built from the same research and technology used to create the Gemini models.',
        contextWindow: 0,
      },
      {
        id: 'google/gemma-2b-it',
        name: 'google/gemma-2b-it',
        description:
          'Gemma is a family of lightweight, state-of-the-art open models from Google, built from the same research and technology used to create the Gemini models.',
        contextWindow: 0,
      },
      {
        id: 'NousResearch/Nous-Hermes-2-Mistral-7B-DPO',
        name: 'NousResearch/Nous-Hermes-2-Mistral-7B-DPO',
        description:
          "Nous Hermes 2 on Mistral 7B DPO is the new flagship 7B Hermes! This model was DPO'd from Teknium/OpenHermes-2.5-Mistral-7B and has improved across the board on all benchmarks tested - AGIEval, BigBench Reasoning, GPT4All, and TruthfulQA.",
        contextWindow: 0,
      },
      {
        id: 'NousResearch/Nous-Hermes-2-Mixtral-8x7B-SFT',
        name: 'NousResearch/Nous-Hermes-2-Mixtral-8x7B-SFT',
        description:
          'Nous Hermes 2 Mixtral 7bx8 SFT is the new flagship Nous Research model trained over the Mixtral 7bx8 MoE LLM. The model was trained on over 1,000,000 entries of primarily GPT-4 generated data, as well as other high quality data from open datasets across the AI landscape, achieving state of the art performance on a variety of tasks.',
        contextWindow: 0,
      },
      {
        id: 'NousResearch/Nous-Hermes-2-Yi-34B',
        name: 'NousResearch/Nous-Hermes-2-Yi-34B',
        description:
          'Nous Hermes 2 - Yi-34B is a state of the art Yi Fine-tune',
        contextWindow: 0,
      },
      {
        id: 'codellama/CodeLlama-70b-Instruct-hf',
        name: 'codellama/CodeLlama-70b-Instruct-hf',
        description:
          'Code Llama is a family of large language models for code based on Llama 2 providing infilling capabilities, support for large input contexts, and zero-shot instruction following ability for programming tasks.',
        contextWindow: 0,
      },
      {
        id: 'NousResearch/Nous-Hermes-2-Mixtral-8x7B-DPO',
        name: 'NousResearch/Nous-Hermes-2-Mixtral-8x7B-DPO',
        description:
          'Nous Hermes 2 Mixtral 7bx8 DPO is the new flagship Nous Research model trained over the Mixtral 7bx8 MoE LLM. The model was trained on over 1,000,000 entries of primarily GPT-4 generated data, as well as other high quality data from open datasets across the AI landscape, achieving state of the art performance on a variety of tasks',
        contextWindow: 0,
      },
      {
        id: 'snorkelai/Snorkel-Mistral-PairRM-DPO',
        name: 'snorkelai/Snorkel-Mistral-PairRM-DPO',
        description:
          'A state-of-the-art model by Snorkel AI, DPO fine-tuned on Mistral-7B.',
        contextWindow: 0,
      },
      {
        id: 'deepseek-ai/deepseek-coder-33b-instruct',
        name: 'deepseek-ai/deepseek-coder-33b-instruct',
        description:
          'Deepseek Coder is composed of a series of code language models, each trained from scratch on 2T tokens, with a composition of 87% code and 13% natural language in both English and Chinese.',
        contextWindow: 0,
      },
      {
        id: 'zero-one-ai/Yi-34B-Chat',
        name: 'zero-one-ai/Yi-34B-Chat',
        description:
          'The Yi series models are large language models trained from scratch by developers at 01.AI.',
        contextWindow: 0,
      },
      {
        id: 'NousResearch/Nous-Hermes-Llama2-13b',
        name: 'NousResearch/Nous-Hermes-Llama2-13b',
        description:
          'Nous-Hermes-Llama2-13b is a state-of-the-art language model fine-tuned on over 300,000 instructions.',
        contextWindow: 0,
      },
      {
        id: 'NousResearch/Nous-Hermes-Llama2-13b',
        name: 'NousResearch/Nous-Hermes-Llama2-13b',
        description:
          'Nous-Hermes-Llama2-13b is a state-of-the-art language model fine-tuned on over 300,000 instructions.',
        contextWindow: 0,
      },
      {
        id: 'NousResearch/Nous-Hermes-llama-2-7b',
        name: 'NousResearch/Nous-Hermes-llama-2-7b',
        description:
          'Nous-Hermes-Llama2-7b is a state-of-the-art language model fine-tuned on over 300,000 instructions.',
        contextWindow: 0,
      },
      {
        id: 'togethercomputer/Llama-2-7B-32K-Instruct',
        name: 'togethercomputer/Llama-2-7B-32K-Instruct',
        description:
          "Extending LLaMA-2 to 32K context, built with Meta's Position Interpolation and Together AI's data recipe and system optimizations, instruction tuned by Together.",
        contextWindow: 0,
      },
      {
        id: 'meta-llama/Llama-2-70b-chat-hf',
        name: 'meta-llama/Llama-2-70b-chat-hf',
        description:
          'Llama 2-chat leverages publicly available instruction datasets and over 1 million human annotations. Available in three sizes: 7B, 13B and 70B parameters.',
        contextWindow: 0,
      },
      {
        id: 'meta-llama/Llama-2-13b-chat-hf',
        name: 'meta-llama/Llama-2-13b-chat-hf',
        description:
          'Llama 2-chat leverages publicly available instruction datasets and over 1 million human annotations. Available in three sizes: 7B, 13B and 70B parameters.',
        contextWindow: 0,
      },
      {
        id: 'meta-llama/Llama-2-7b-chat-hf',
        name: 'meta-llama/Llama-2-7b-chat-hf',
        description:
          'Llama 2-chat leverages publicly available instruction datasets and over 1 million human annotations. Available in three sizes: 7B, 13B and 70B parameters.',
        contextWindow: 0,
      },
      {
        id: 'codellama/CodeLlama-13b-Instruct-hf',
        name: 'codellama/CodeLlama-13b-Instruct-hf',
        description:
          'Code Llama is a family of large language models for code based on Llama 2 providing infilling capabilities, support for large input contexts, and zero-shot instruction following ability for programming tasks.',
        contextWindow: 0,
      },
      {
        id: 'codellama/CodeLlama-34b-Instruct-hf',
        name: 'codellama/CodeLlama-34b-Instruct-hf',
        description:
          'Code Llama is a family of large language models for code based on Llama 2 providing infilling capabilities, support for large input contexts, and zero-shot instruction following ability for programming tasks.',
        contextWindow: 0,
      },
      {
        id: 'codellama/CodeLlama-7b-Instruct-hf',
        name: 'codellama/CodeLlama-7b-Instruct-hf',
        description:
          'Code Llama is a family of large language models for code based on Llama 2 providing infilling capabilities, support for large input contexts, and zero-shot instruction following ability for programming tasks.',
        contextWindow: 0,
      },
      {
        id: 'NousResearch/Nous-Capybara-7B-V1p9',
        name: 'NousResearch/Nous-Capybara-7B-V1p9',
        description:
          'first Nous collection of dataset and models made by fine-tuning mostly on data created by Nous in-house',
        contextWindow: 0,
      },
      {
        id: 'teknium/OpenHermes-2p5-Mistral-7B',
        name: 'teknium/OpenHermes-2p5-Mistral-7B',
        description:
          'Continuation of OpenHermes 2 Mistral model trained on additional code datasets',
        contextWindow: 0,
      },
      {
        id: 'Open-Orca/Mistral-7B-OpenOrca',
        name: 'Open-Orca/Mistral-7B-OpenOrca',
        description:
          'An OpenOrca dataset fine-tune on top of Mistral 7B by the OpenOrca team.',
        contextWindow: 0,
      },
      {
        id: 'teknium/OpenHermes-2-Mistral-7B',
        name: 'teknium/OpenHermes-2-Mistral-7B',
        description:
          'State of the art Mistral Fine-tuned on extensive public datasets.',
        contextWindow: 0,
      },
      {
        id: 'Austism/chronos-hermes-13b',
        name: 'Austism/chronos-hermes-13b',
        description:
          'This model is a 75/25 merge of Chronos (13B) and Nous Hermes (13B) models resulting in having a great ability to produce evocative storywriting and follow a narrative.',
        contextWindow: 0,
      },
      {
        id: 'garage-bAInd/Platypus2-70B-instruct',
        name: 'garage-bAInd/Platypus2-70B-instruct',
        description:
          'An instruction fine-tuned LLaMA-2 (70B) model by merging Platypus2 (70B) by garage-bAInd and LLaMA-2 Instruct v2 (70B) by upstage.',
        contextWindow: 0,
      },
      {
        id: 'Gryphe/MythoMax-L2-13b',
        name: 'Gryphe/MythoMax-L2-13b',
        description:
          'MythoLogic-L2 and Huginn merge using a highly experimental tensor type merge technique. The main difference with MythoMix is that I allowed more of Huginn to intermingle with the single tensors located at the front and end of a model.',
        contextWindow: 0,
      },
      {
        id: 'togethercomputer/alpaca-7b',
        name: 'togethercomputer/alpaca-7b',
        description:
          'Fine-tuned from the LLaMA 7B model on 52K instruction-following demonstrations.',
        contextWindow: 0,
      },
      {
        id: 'WizardLM/WizardLM-13B-V1.2',
        name: 'WizardLM/WizardLM-13B-V1.2',
        description:
          'This model achieves a substantial and comprehensive improvement on coding, mathematical reasoning and open-domain conversation capacities.',
        contextWindow: 0,
      },
      {
        id: 'upstage/SOLAR-10.7B-Instruct-v1.0',
        name: 'upstage/SOLAR-10.7B-Instruct-v1.0',
        description:
          'Built on the Llama2 architecture, SOLAR-10.7B incorporates the innovative Upstage Depth Up-Scaling.',
        contextWindow: 0,
      },
      {
        id: 'OpenAssistant/llama2-70b-oasst-sft-v10',
        name: 'OpenAssistant/llama2-70b-oasst-sft-v10',
        description: 'An Open-Assistant fine-tuned model from LLaMA-2 70B.',
        contextWindow: 0,
      },
      {
        id: 'openchat/openchat-3.5-1210',
        name: 'openchat/openchat-3.5-1210',
        description:
          'A merge of OpenChat 3.5 was trained with C-RLFT on a collection of publicly available high-quality instruction data, with a custom processing pipeline.',
        contextWindow: 0,
      },
      {
        id: 'Qwen/Qwen1.5-7B-Chat',
        name: 'Qwen/Qwen1.5-7B-Chat',
        description:
          'Qwen1.5 is the beta version of Qwen2, a transformer-based decoder-only language model pretrained on a large amount of data. In comparison with the previous released Qwen.',
        contextWindow: 0,
      },
      {
        id: 'Qwen/Qwen1.5-14B-Chat',
        name: 'Qwen/Qwen1.5-14B-Chat',
        description:
          'Qwen1.5 is the beta version of Qwen2, a transformer-based decoder-only language model pretrained on a large amount of data. In comparison with the previous released Qwen.',
        contextWindow: 0,
      },
      {
        id: 'Qwen/Qwen1.5-1.8B-Chat',
        name: 'Qwen/Qwen1.5-1.8B-Chat',
        description:
          'Qwen1.5 is the beta version of Qwen2, a transformer-based decoder-only language model pretrained on a large amount of data. In comparison with the previous released Qwen.',
        contextWindow: 0,
      },
      {
        id: 'cognitivecomputations/dolphin-2.5-mixtral-8x7b',
        name: 'cognitivecomputations/dolphin-2.5-mixtral-8x7b',
        description:
          'This Dolphin is really good at coding, I trained with a lot of coding data. It is very obedient but it is not DPO tuned - so you still might need to encourage it in the system prompt as I show in the below examples.',
        contextWindow: 0,
      },
      {
        id: 'mistralai/Mixtral-8x22B-Instruct-v0.1',
        name: 'mistralai/Mixtral-8x22B-Instruct-v0.1',
        description:
          'The Mixtral-8x22B-Instruct-v0.1 Large Language Model (LLM) is an instruct fine-tuned version of the Mixtral-8x22B-v0.1.',
        contextWindow: 0,
      },
      {
        id: 'lmsys/vicuna-13b-v1.5',
        name: 'lmsys/vicuna-13b-v1.5',
        description:
          'Vicuna is a chat assistant trained by fine-tuning Llama 2 on user-shared conversations collected from ShareGPT.',
        contextWindow: 0,
      },
      {
        id: 'Qwen/Qwen1.5-0.5B-Chat',
        name: 'Qwen/Qwen1.5-0.5B-Chat',
        description:
          'Qwen1.5 is the beta version of Qwen2, a transformer-based decoder-only language model pretrained on a large amount of data. In comparison with the previous released Qwen.',
        contextWindow: 0,
      },
      {
        id: 'Qwen/Qwen1.5-0.5B-Chat',
        name: 'Qwen/Qwen1.5-0.5B-Chat',
        description:
          'Qwen1.5 is the beta version of Qwen2, a transformer-based decoder-only language model pretrained on a large amount of data. In comparison with the previous released Qwen.',
        contextWindow: 0,
      },
      {
        id: 'Qwen/Qwen1.5-4B-Chat',
        name: 'Qwen/Qwen1.5-4B-Chat',
        description:
          'Qwen1.5 is the beta version of Qwen2, a transformer-based decoder-only language model pretrained on a large amount of data. In comparison with the previous released Qwen.',
        contextWindow: 0,
      },
      {
        id: 'mistralai/Mistral-7B-Instruct-v0.1',
        name: 'mistralai/Mistral-7B-Instruct-v0.1',
        description: 'instruct fine-tuned version of Mistral-7B-v0.1.',
        contextWindow: 0,
      },
      {
        id: 'mistralai/Mistral-7B-Instruct-v0.2',
        name: 'mistralai/Mistral-7B-Instruct-v0.2',
        description:
          'The Mistral-7B-Instruct-v0.2 Large Language Model (LLM) is an improved instruct fine-tuned version of Mistral-7B-Instruct-v0.1.',
        contextWindow: 0,
      },
      {
        id: 'togethercomputer/Pythia-Chat-Base-7B',
        name: 'togethercomputer/Pythia-Chat-Base-7B',
        description:
          'Chat model based on EleutherAI’s Pythia-7B model, and is fine-tuned with data focusing on dialog-style interactions.',
        contextWindow: 0,
      },
      {
        id: 'Qwen/Qwen1.5-32B-Chat',
        name: 'Qwen/Qwen1.5-32B-Chat',
        description:
          'Qwen1.5 is the beta version of Qwen2, a transformer-based decoder-only language model pretrained on a large amount of data. In comparison with the previous released Qwen.',
        contextWindow: 0,
      },
      {
        id: 'Qwen/Qwen1.5-72B-Chat',
        name: 'Qwen/Qwen1.5-72B-Chat',
        description:
          'Qwen1.5 is the beta version of Qwen2, a transformer-based decoder-only language model pretrained on a large amount of data. In comparison with the previous released Qwen.',
        contextWindow: 0,
      },
      {
        id: 'mistralai/Mistral-7B-Instruct-v0.3',
        name: 'mistralai/Mistral-7B-Instruct-v0.3',
        description:
          'The Mistral-7B-Instruct-v0.3 Large Language Model (LLM) is an instruct fine-tuned version of the Mistral-7B-v0.3.',
        contextWindow: 0,
      },
      {
        id: 'Qwen/Qwen1.5-110B-Chat',
        name: 'Qwen/Qwen1.5-110B-Chat',
        description:
          'Qwen1.5 is the beta version of Qwen2, a transformer-based decoder-only language model pretrained on a large amount of data. In comparison with the previous released Qwen.',
        contextWindow: 0,
      },
      {
        id: 'mistralai/Mixtral-8x7B-Instruct-v0.1',
        name: 'mistralai/Mixtral-8x7B-Instruct-v0.1',
        description:
          'The Mixtral-8x7B Large Language Model (LLM) is a pretrained generative Sparse Mixture of Experts.',
        contextWindow: 0,
      },
    ],
  },
  {
    id: 'cloudflare',
    name: 'Cloudflare WorkersAI',
    description: 'Description coming soon.',
    logo: '/providers/workers-ai.png',
    models: [
      {
        id: 'llama-2-7b-chat-fp16',
        name: 'llama-2-7b-chat-fp16',
        description:
          'Full precision (fp16) generative text model with 7 billion parameters from Meta.',
        contextWindow: 0,
      },
      {
        id: 'mistral-7b-instruct-v0.1',
        name: 'mistral-7b-instruct-v0.1',
        description:
          'Instruct fine-tuned version of the Mistral-7b generative text model with 7 billion parameters.',
        contextWindow: 0,
      },
      {
        id: 'deepseek-coder-6.7b-base-awq',
        name: 'deepseek-coder-6.7b-base-awq',
        description:
          'Deepseek Coder is composed of a series of code language models, each trained from scratch on 2T tokens, with a composition of 87% code and 13% natural language in both English and Chinese.',
        contextWindow: 0,
      },
      {
        id: 'deepseek-coder-6.7b-instruct-awq',
        name: 'deepseek-coder-6.7b-instruct-awq',
        description:
          'Deepseek Coder is composed of a series of code language models, each trained from scratch on 2T tokens, with a composition of 87% code and 13% natural language in both English and Chinese.',
        contextWindow: 0,
      },
      {
        id: 'deepseek-math-7b-base',
        name: 'deepseek-math-7b-base',
        description:
          'DeepSeekMath is initialized with DeepSeek-Coder-v1.5 7B and continues pre-training on math-related tokens sourced from Common Crawl, together with natural language and code data for 500B tokens.',
        contextWindow: 0,
      },
      {
        id: 'deepseek-math-7b-instruct',
        name: 'deepseek-math-7b-instruct',
        description:
          'DeepSeekMath-Instruct 7B is a mathematically instructed tuning model derived from DeepSeekMath-Base 7B. DeepSeekMath is initialized with DeepSeek-Coder-v1.5 7B and continues pre-training on math-related tokens sourced from Common Crawl, together with natural language and code data for 500B tokens.',
        contextWindow: 0,
      },
      {
        id: 'discolm-german-7b-v1-awq',
        name: 'discolm-german-7b-v1-awq',
        description:
          'DiscoLM German 7b is a Mistral-based large language model with a focus on German-language applications. AWQ is an efficient, accurate and blazing-fast low-bit weight quantization method, currently supporting 4-bit quantization.',
        contextWindow: 0,
      },
      {
        id: 'falcon-7b-instruct',
        name: 'falcon-7b-instruct',
        description:
          'Falcon-7B-Instruct is a 7B parameters causal decoder-only model built by TII based on Falcon-7B and finetuned on a mixture of chat/instruct datasets.',
        contextWindow: 0,
      },
      {
        id: 'gemma-2b-it-lora',
        name: 'gemma-2b-it-lora',
        description:
          'This is a Gemma-2B base model that Cloudflare dedicates for inference with LoRA adapters. Gemma is a family of lightweight, state-of-the-art open models from Google, built from the same research and technology used to create the Gemini models.',
        contextWindow: 0,
      },
      {
        id: 'gemma-7b-it',
        name: 'gemma-7b-it',
        description:
          'Gemma is a family of lightweight, state-of-the-art open models from Google, built from the same research and technology used to create the Gemini models. They are text-to-text, decoder-only large language models, available in English, with open weights, pre-trained variants, and instruction-tuned variants.',
        contextWindow: 0,
      },
      {
        id: 'gemma-7b-it-lora',
        name: 'gemma-7b-it-lora',
        description:
          'This is a Gemma-7B base model that Cloudflare dedicates for inference with LoRA adapters. Gemma is a family of lightweight, state-of-the-art open models from Google, built from the same research and technology used to create the Gemini models.',
        contextWindow: 0,
      },
      {
        id: 'hermes-2-pro-mistral-7b',
        name: 'hermes-2-pro-mistral-7b',
        description:
          'Hermes 2 Pro on Mistral 7B is the new flagship 7B Hermes! Hermes 2 Pro is an upgraded, retrained version of Nous Hermes 2, consisting of an updated and cleaned version of the OpenHermes 2.5 Dataset, as well as a newly introduced Function Calling and JSON Mode dataset developed in-house.',
        contextWindow: 0,
      },
      {
        id: 'llama-2-13b-chat-awq',
        name: 'llama-2-13b-chat-awq',
        description:
          'Llama 2 13B Chat AWQ is an efficient, accurate and blazing-fast low-bit weight quantized Llama 2 variant.',
        contextWindow: 0,
      },
      {
        id: 'llama-2-7b-chat-hf-lora',
        name: 'llama-2-7b-chat-hf-lora',
        description:
          'This is a Llama2 base model that Cloudflare dedicated for inference with LoRA adapters. Llama 2 is a collection of pretrained and fine-tuned generative text models ranging in scale from 7 billion to 70 billion parameters. This is the repository for the 7B fine-tuned model, optimized for dialogue use cases and converted for the Hugging Face Transformers format.',
        contextWindow: 0,
      },
      {
        id: 'llama-3-8b-instruct',
        name: 'llama-3-8b-instruct',
        description:
          'Generation over generation, Meta Llama 3 demonstrates state-of-the-art performance on a wide range of industry benchmarks and offers new capabilities, including improved reasoning.',
        contextWindow: 0,
      },
      {
        id: 'llama-3-8b-instruct-awq',
        name: 'llama-3-8b-instruct-awq',
        description:
          'Quantized (int4) generative text model with 8 billion parameters from Meta.',
        contextWindow: 0,
      },
      {
        id: 'llamaguard-7b-awq',
        name: 'llamaguard-7b-awq',
        description:
          'Llama Guard is a model for classifying the safety of LLM prompts and responses, using a taxonomy of safety risks.',
        contextWindow: 0,
      },
      {
        id: 'mistral-7b-instruct-v0.1-awq',
        name: 'mistral-7b-instruct-v0.1-awq',
        description:
          'Mistral 7B Instruct v0.1 AWQ is an efficient, accurate and blazing-fast low-bit weight quantized Mistral variant.',
        contextWindow: 0,
      },
      {
        id: 'mistral-7b-instruct-v0.2',
        name: 'mistral-7b-instruct-v0.2',
        description:
          'The Mistral-7B-Instruct-v0.2 Large Language Model (LLM) is an instruct fine-tuned version of the Mistral-7B-v0.2. Mistral-7B-v0.2 has the following changes compared to Mistral-7B-v0.1: 32k context window (vs 8k context in v0.1), rope-theta = 1e6, and no Sliding-Window Attention.',
        contextWindow: 0,
      },
      {
        id: 'mistral-7b-instruct-v0.2-lora',
        name: 'mistral-7b-instruct-v0.2-lora',
        description:
          'The Mistral-7B-Instruct-v0.2 Large Language Model (LLM) is an instruct fine-tuned version of the Mistral-7B-v0.2.',
        contextWindow: 0,
      },
      {
        id: 'neural-chat-7b-v3-1-awq',
        name: 'neural-chat-7b-v3-1-awq',
        description:
          'This model is a fine-tuned 7B parameter LLM on the Intel Gaudi 2 processor from the mistralai/Mistral-7B-v0.1 on the open source dataset Open-Orca/SlimOrca.',
        contextWindow: 0,
      },
      {
        id: 'openchat-3.5-0106',
        name: 'openchat-3.5-0106',
        description:
          'OpenChat is an innovative library of open-source language models, fine-tuned with C-RLFT - a strategy inspired by offline reinforcement learning.',
        contextWindow: 0,
      },
      {
        id: 'openhermes-2.5-mistral-7b-awq',
        name: 'openhermes-2.5-mistral-7b-awq',
        description:
          'OpenHermes 2.5 Mistral 7B is a state of the art Mistral Fine-tune, a continuation of OpenHermes 2 model, which trained on additional code datasets.',
        contextWindow: 0,
      },
      {
        id: 'phi-2',
        name: 'phi-2',
        description:
          'Phi-2 is a Transformer-based model with a next-word prediction objective, trained on 1.4T tokens from multiple passes on a mixture of Synthetic and Web datasets for NLP and coding.',
        contextWindow: 0,
      },
      {
        id: 'qwen1.5-0.5b-chat',
        name: 'qwen1.5-0.5b-chat',
        description:
          'Qwen1.5 is the improved version of Qwen, the large language model series developed by Alibaba Cloud.',
        contextWindow: 0,
      },
      {
        id: 'qwen1.5-1.8b-chat',
        name: 'qwen1.5-1.8b-chat',
        description:
          'Qwen1.5 is the improved version of Qwen, the large language model series developed by Alibaba Cloud.',
        contextWindow: 0,
      },
      {
        id: 'qwen1.5-14b-chat-awq',
        name: 'qwen1.5-14b-chat-awq',
        description:
          'Qwen1.5 is the improved version of Qwen, the large language model series developed by Alibaba Cloud. AWQ is an efficient, accurate and blazing-fast low-bit weight quantization method, currently supporting 4-bit quantization.',
        contextWindow: 0,
      },
      {
        id: 'qwen1.5-7b-chat-awq',
        name: 'qwen1.5-7b-chat-awq',
        description:
          'Qwen1.5 is the improved version of Qwen, the large language model series developed by Alibaba Cloud. AWQ is an efficient, accurate and blazing-fast low-bit weight quantization method, currently supporting 4-bit quantization.',
        contextWindow: 0,
      },
      {
        id: 'sqlcoder-7b-2',
        name: 'sqlcoder-7b-2',
        description:
          'This model is intended to be used by non-technical users to understand data inside their SQL databases.',
        contextWindow: 0,
      },
      {
        id: 'starling-lm-7b-beta',
        name: 'starling-lm-7b-beta',
        description:
          'We introduce Starling-LM-7B-beta, an open large language model (LLM) trained by Reinforcement Learning from AI Feedback (RLAIF). Starling-LM-7B-beta is trained from Openchat-3.5-0106 with our new reward model Nexusflow/Starling-RM-34B and policy optimization method Fine-Tuning Language Models from Human Preferences (PPO).',
        contextWindow: 0,
      },
      {
        id: 'tinyllama-1.1b-chat-v1.0',
        name: 'tinyllama-1.1b-chat-v1.0',
        description:
          'The TinyLlama project aims to pretrain a 1.1B Llama model on 3 trillion tokens. This is the chat model finetuned on top of TinyLlama/TinyLlama-1.1B-intermediate-step-1431k-3T.',
        contextWindow: 0,
      },
      {
        id: 'una-cybertron-7b-v2-bf16',
        name: 'una-cybertron-7b-v2-bf16',
        description:
          'Cybertron 7B v2 is a 7B MistralAI based model, best on it’s series. It was trained with SFT, DPO and UNA (Unified Neural Alignment) on multiple datasets.',
        contextWindow: 0,
      },
      {
        id: 'zephyr-7b-beta-awq',
        name: 'zephyr-7b-beta-awq',
        description:
          'Zephyr 7B Beta AWQ is an efficient, accurate and blazing-fast low-bit weight quantized Zephyr model variant.',
        contextWindow: 0,
      },
    ],
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
    models: [
      {
        id: 'llama3-8b-8192',
        name: 'llama3-8b-8192',
        description: 'Description coming soon..',
        contextWindow: 8192,
      },
      {
        id: 'llama3-70b-8192',
        name: 'llama3-70b-8192',
        description: 'Description coming soon..',
        contextWindow: 8192,
      },
      {
        id: 'mixtral-8x7b-32768',
        name: 'mixtral-8x7b-32768',
        description: 'Description coming soon..',
        contextWindow: 32768,
      },
      {
        id: 'gemma-7b-it',
        name: 'gemma-7b-it',
        description: 'Description coming soon..',
        contextWindow: 8192,
      },
      {
        id: 'gemma2-9b-it',
        name: 'gemma2-9b-it',
        description: 'Description coming soon..',
        contextWindow: 8192,
      },
    ],
  },
];
