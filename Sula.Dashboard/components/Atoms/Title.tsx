import * as React from 'react';

export function Title({ children }: { children: React.ReactNode }): React.ReactElement {
  return <h1 className="text-lg py-5 uppercase font-bold">{children}</h1>;
}
