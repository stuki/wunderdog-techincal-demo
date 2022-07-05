import classNames from 'classnames';
import React, { ReactElement } from 'react';
import { useTable } from 'react-table';

export function Table({ columns, data, detailed }: { columns: any[]; data: any[]; detailed?: boolean }): ReactElement {
  const { getTableProps, getTableBodyProps, headerGroups, rows, prepareRow } = useTable({
    columns,
    data,
  });

  return (
    <table {...getTableProps()} className="w-full divide-y divide-gray-200">
      <thead>
        {headerGroups.map((headerGroup) => (
          <tr {...headerGroup.getHeaderGroupProps()}>
            {headerGroup.headers.map((column) => (
              <th
                {...column.getHeaderProps()}
                className="py-4 text-left text-xs font-medium text-gray-200 uppercase tracking-wider">
                {column.render('Header')}
              </th>
            ))}
          </tr>
        ))}
      </thead>
      <tbody
        {...getTableBodyProps()}
        className={classNames('divide-y divide-gray-200', detailed ? 'text-xs md:text-sm' : 'text-sm')}>
        {rows.map((row) => {
          prepareRow(row);
          return (
            <tr {...row.getRowProps()}>
              {row.cells.map((cell) => {
                return (
                  <td {...cell.getCellProps()} className="py-4 whitespace-nowrap text-gray-200">
                    {cell.render('Cell')}
                  </td>
                );
              })}
            </tr>
          );
        })}
      </tbody>
    </table>
  );
}
