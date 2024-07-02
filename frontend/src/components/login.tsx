import React, { useEffect } from 'react';
import { EmailLogin } from '@/components/accounts/email-login';
import { GoogleLogin } from '@/components/accounts/google-login';
import { LoginPayload } from '@/types/accounts';
import { readToken, saveToken } from '@/lib/storage';
import { EmailRegister } from '@/components/accounts/email-register';

const googleClientId = import.meta.env.VITE_GOOGLE_CLIENT_ID as string;

export function Login() {
  const [showRegister, setShowRegister] = React.useState(false);

  function handleLogin(payload: LoginPayload) {
    saveToken(payload.token);
    window.location.href = '/';
  }

  useEffect(() => {
    const token = readToken();
    if (token) {
      window.location.href = '/';
    }
  }, []);

  return (
    <div className="grid h-screen min-h-screen w-full grid-cols-5">
      <div className="col-span-2 flex items-center justify-center bg-zinc-950">
        <h1 className="font-neotrax text-6xl text-white">routify</h1>
      </div>
      <div className="col-span-3 flex items-center justify-center py-12">
        <div className="w-128 mx-auto grid gap-6">
          <div className="grid gap-2 text-center">
            <h1 className="text-2xl font-semibold tracking-tight">
              Login to Routify
            </h1>
            <p className="text-sm text-muted-foreground">
              Use one of the following methods to login
            </p>
          </div>
          {googleClientId && (
            <>
              <GoogleLogin clientId={googleClientId} onLogin={handleLogin} />
              <div className="relative">
                <div className="absolute inset-0 flex items-center">
                  <span className="w-full border-t" />
                </div>
                <div className="relative flex justify-center text-xs uppercase">
                  <span className="bg-background px-2 text-muted-foreground">
                    Or continue with
                  </span>
                </div>
              </div>
            </>
          )}
          <div className="flex flex-col gap-4">
            {showRegister ? (
              <EmailRegister onRegister={handleLogin} />
            ) : (
              <EmailLogin onLogin={handleLogin} />
            )}
            <p
              className="text-center text-sm text-muted-foreground hover:cursor-pointer hover:underline"
              onClick={() => {
                setShowRegister(!showRegister);
              }}
            >
              {showRegister
                ? 'Already have an account? Login'
                : 'No account yet? Register'}
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
