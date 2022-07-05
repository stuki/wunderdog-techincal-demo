import * as React from 'react';
import Link from 'next/link';
import { ReactElement } from 'react';

export function NavigationItem(props: { target: string; title: string; children: ReactElement }): ReactElement {
  return (
    <Link href={props.target}>
      <a
        className={
          'flex flex-row items-center h-full text-sm hover:opacity-80 mr-10 md:p-0 py-2 px-4 md:active:bg-none active:bg-green-900'
        }>
        {props.children}
        <span className={'pl-2'}>{props.title}</span>
      </a>
    </Link>
  );
}
