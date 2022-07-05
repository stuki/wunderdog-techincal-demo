import * as React from 'react';
import classNames from 'classnames';
import Link from 'next/link';

export function SmallCard({
  value,
  title,
  color,
  url,
}: {
  value: number;
  title: string;
  color?: string;
  url?: string;
}) {
  if (url) {
    return (
      <Link href={url}>
        <a className="hover:opacity-90">
          <div className={classNames('color-dark rounded-xl text-4xl flex flex-col h-28 active:bg-green-700', color)}>
            <span className="text-sm p-2">{title}</span>
            <span className="self-center">{value}</span>
          </div>
        </a>
      </Link>
    );
  }
  return (
    <div className={classNames('color-dark rounded-xl text-4xl flex flex-col h-28 active:bg-green-700', color)}>
      <span className="text-sm p-2">{title}</span>
      <span className="self-center">{value}</span>
    </div>
  );
}
