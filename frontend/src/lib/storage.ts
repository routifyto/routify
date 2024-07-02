const TOKEN_KEY = 'routify-token';

export function readToken() {
  return localStorage.getItem(TOKEN_KEY);
}

export function saveToken(token: string) {
  localStorage.setItem(TOKEN_KEY, token);
}

export function deleteToken() {
  localStorage.removeItem(TOKEN_KEY);
}
