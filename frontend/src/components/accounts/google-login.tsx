import React from 'react';
import { Button } from '@/components/ui/button';
import { Spinner } from '@/components/ui/spinner';
import { GoogleIcon } from '@/components/ui/icons';
import { LoginOutput } from '@/types/accounts';
import { GoogleOAuthProvider, useGoogleLogin } from '@react-oauth/google';
import { useGoogleLoginMutation } from '@/api/accounts';

interface GoogleLoginProps {
  onLogin: (output: LoginOutput) => void;
  clientId: string;
}

function GoogleLoginButton({ onLogin }: GoogleLoginProps) {
  const { mutate, isPending } = useGoogleLoginMutation();
  const login = useGoogleLogin({
    onSuccess: async (response) => {
      mutate(response, {
        onSuccess: (data) => {
          onLogin(data);
        },
      });
    },
    flow: 'implicit',
  });

  return (
    <Button
      variant="outline"
      type="button"
      disabled={isPending}
      className="w-full"
      onClick={() => login()}
    >
      {isPending ? (
        <Spinner className="mr-2 h-4 w-4 animate-spin" />
      ) : (
        <GoogleIcon className="mr-2 h-4 w-4" />
      )}{' '}
      Google
    </Button>
  );
}

export function GoogleLogin({ onLogin, clientId }: GoogleLoginProps) {
  return (
    <GoogleOAuthProvider clientId={clientId}>
      <GoogleLoginButton onLogin={onLogin} clientId={clientId} />
    </GoogleOAuthProvider>
  );
}
