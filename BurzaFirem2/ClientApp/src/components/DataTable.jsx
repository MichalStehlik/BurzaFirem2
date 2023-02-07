import { useEffect, useMemo } from "react";
import {Input, Table} from "reactstrap"
import { useTable, usePagination, useSortBy, useFilters, useGlobalFilter, useRowSelect } from 'react-table'

export const TextColumnFilter = ({ column: { filterValue, preFilteredRows, setFilter }}) => {
  return (
    <Input onChange={e => { setFilter(e.target.value || undefined) }} placeholder={`Zadejte text`}
    />
  )
}

export const DataTable = ({columns, data, fetchData, isLoading, error, totalPages, initialState }) => {
  const defaultColumn = useMemo(
    () => ({
        Filter: TextColumnFilter,
    }),
    []
  )
    const {
        getTableProps,
        getTableBodyProps,
        headerGroups,
        prepareRow,
        page,
        canPreviousPage,
        canNextPage,
        pageOptions,
        pageCount,
        gotoPage,
        nextPage,
        previousPage,
        setPageSize,
        setGlobalFilter,
        selectedFlatRows,
        setAllFilters,
        state,
    } = useTable(
        { columns, data, defaultColumn, initialState, manualPagination: true, pageCount: totalPages, manualSortBy: true, disableMultiSort: true, manualFilters: true }, 
        useFilters, useSortBy, usePagination);

    useEffect(() => {
        fetchData({ page: state.pageIndex, size: state.pageSize, sort: state.sortBy, filters: state.filters });
    }, [fetchData, state.pageIndex, state.pageSize, state.sortBy, state.filters]);

    return (
        <Table {...getTableProps()}>
          <thead>
            {headerGroups.map(headerGroup => (
              <tr {...headerGroup.getHeaderGroupProps()}>
                {headerGroup.headers.map(column => (
                  <th {...column.getHeaderProps(column.getSortByToggleProps())}>{column.render('Header')}
                    <span>
                        {column.isSorted
                            ? column.isSortedDesc ? "↑" : "↓"
                            : ""
                        }
                    </span>
                  </th>
                ))}
              </tr>
            ))}
            {headerGroups.map(headerGroup => (
                <tr {...headerGroup.getHeaderGroupProps()}>
                    {headerGroup.headers.map(column => (
                        <th {...column.getHeaderProps()}>
                            {column.canFilter ? column.render('Filter') : null}
                        </th>
                    ))}
                </tr>
            ))}
          </thead>
          <tbody {...getTableBodyProps()}>
            {page.map((row, i) => {
              prepareRow(row)
                return (
                    <tr key={ i } {...row.getRowProps()}>
                  {row.cells.map(cell => {
                    return <td {...cell.getCellProps()}>{cell.render('Cell')}</td>
                  })}
                </tr>
              )
            })}
          </tbody>
        </Table>
    )   
}

export default DataTable;