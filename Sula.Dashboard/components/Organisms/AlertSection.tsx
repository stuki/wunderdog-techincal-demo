import * as React from 'react';
import { Alert, DataType, Operator } from '../../interfaces';
import useTranslation from 'next-translate/useTranslation';
import { Table } from '../Atoms/Table';
import { Card } from '../Molecules';
import { formatDistanceToNow } from 'date-fns';
import { enUS, fi } from 'date-fns/locale';
import Link from 'next/link';

export function AlertSection({
  data,
  detailed,
  showTitle,
}: {
  data?: Alert[];
  detailed?: boolean;
  showTitle?: boolean;
}) {
  const { t, lang } = useTranslation('alerts');

  const locale = lang === 'en' ? enUS : fi;

  if (!data) {
    return null;
  }

  type CellProps = {
    cell: {
      value: any;
      row: {
        original: Alert;
      };
    };
  };

  const columns = React.useMemo<
    {
      Header: string;
      accessor: string;
      Cell?: ({ cell: { value } }: CellProps) => string;
    }[]
  >(
    () => [
      {
        Header: t('sensor'),
        accessor: 'name',
      },
      {
        Header: t('alertTime'),
        accessor: 'alertTime',
        Cell: ({ cell: { value } }: CellProps) => {
          return `${formatDistanceToNow(new Date(value), { locale })} ${t('ago')}`;
        },
      },
    ],
    []
  );

  if (detailed) {
    columns.push({
      Header: t('reason'),
      accessor: '',
      Cell: ({ cell: { row } }: CellProps) => {
        let dataType;
        let operator;

        switch (row.original.dataType) {
          case DataType.Humidity:
            dataType = t('graph:Humidity');
            break;
          case DataType.Temperature:
            dataType = t('graph:Temperature');
            break;
          case DataType.None:
            dataType = t('none');
            break;
        }
        switch (row.original.operator) {
          case Operator.LessThan:
            operator = '<';
            break;
          case Operator.MoreThan:
            operator = '>';
            break;
          case Operator.Equal:
            operator = '=';
            break;
        }

        return `${dataType} ${operator} ${row.original.value}`;
      },
    });
  }

  const rows = React.useMemo(() => data, []);

  return (
    <Card customClass="p-5">
      {showTitle && (
        <Link href="/alerts">
          <a className="self-start font-semibold hover:opacity-80">{t('common:alerts')}</a>
        </Link>
      )}
      <Table columns={columns} data={rows} detailed={detailed} />
    </Card>
  );
}
