import { useEffect } from 'react';

import { useWorkspace } from '@/contexts/workspace';
import { useNavigate } from 'react-router-dom';

export function WorkspaceRedirect() {
  const workspace = useWorkspace();
  const navigate = useNavigate();

  useEffect(() => {
    if (workspace.apps == null || workspace.apps.length == 0) {
      navigate('/create');
      return;
    }

    navigate(`/${workspace.apps[0].id}`);
  }, [workspace.apps, navigate]);

  return null;
}
