import React from 'react';
import { useApp } from '@/contexts/app';
import { useNavigate } from 'react-router-dom';
import { ApiKeyForm } from '@/components/api-keys/api-key-form';
import { ApiKeyInput, CreateApiKeyOutput } from '@/types/api-keys';
import { useCreateApiKeyMutation } from '@/api/api-keys';
import { ApiKeyConfirmDialog } from '@/components/api-keys/api-key-confirm-dialog';
import { toast } from '@/components/ui/use-toast';

export function ApiKeyCreate() {
  const app = useApp();
  const navigate = useNavigate();
  const { mutate, isPending } = useCreateApiKeyMutation(app.id);

  const [createdApiKey, setCreatedApiKey] =
    React.useState<CreateApiKeyOutput | null>(null);

  function handleSubmit(input: ApiKeyInput) {
    mutate(input, {
      onSuccess: (data) => {
        setCreatedApiKey(data);
      },
      onError: (error) => {
        toast({
          title: 'Failed to create API Key',
          description: error.message,
          variant: 'destructive',
        });
      },
    });
  }

  return (
    <React.Fragment>
      <ApiKeyForm
        apiKey={null}
        errors={[]}
        isPending={isPending}
        onSubmit={handleSubmit}
      />
      {createdApiKey && (
        <ApiKeyConfirmDialog
          apiKey={createdApiKey}
          open={true}
          onOpenChange={(open) => {
            if (!open) {
              navigate(`/${app.id}/api-keys`);
              setCreatedApiKey(null);
            }
          }}
        />
      )}
    </React.Fragment>
  );
}
