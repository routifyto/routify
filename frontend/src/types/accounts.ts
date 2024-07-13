export type EmailLoginInput = {
  email: string;
  password: string;
};

export type EmailRegisterInput = {
  name: string;
  email: string;
  password: string;
};

export type GoogleLoginInput = {
  access_token: string;
  token_type: string;
  expires_in: number;
};

export type LoginOutput = {
  token: string;
};
