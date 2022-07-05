import React from 'react';

export function Card({
  customClass = '',
  children,
}: {
  customClass?: string;
  children: React.ReactNode;
}): React.ReactElement {
  return (
    <div className={`rounded-xl items-center flex flex-col md:px-6 md:py-4 ${customClass} color-dark`}>{children}</div>
  );
}
