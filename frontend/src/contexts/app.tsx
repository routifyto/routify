import { createContext, useContext } from 'react';

export interface AppContextProps {
  id: string;
  name: string;
  description?: string;
}

export const AppContext = createContext<AppContextProps>({} as AppContextProps);
export const useApp = () => useContext(AppContext);
