import { useEffect, useMemo } from "react";
import {Input, Table, Alert, Spinner, Row, Col, Button, ButtonGroup} from "reactstrap"
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
            {error ? (
              <tr>
                <td colSpan={1000}><Alert color="danger">Došlo k chybě</Alert></td>
              </tr>
            ) : (
              isLoading ? (
               <tr><td align="center" colSpan={1000}><Spinner /></td></tr>
            ) : 
            page.map((row, i) => {
              prepareRow(row)
                return (
                    <tr key={ i } {...row.getRowProps()}>
                  {row.cells.map(cell => {
                    return <td {...cell.getCellProps()}>{cell.render('Cell')}</td>
                  })}
                </tr>
              )
            }))}
          </tbody>
          <tfoot>
            <tr>
              <td colSpan={1000}>
                <Row className="w-100 d-flex justify-content-between">
                  <Col>
                  <ButtonGroup className="me-2">
                    <Button variant="secondary" size="sm" onClick={() => gotoPage(0)} disabled={!canPreviousPage}>⇤</Button>{' '}
                    <Button variant="secondary" size="sm" onClick={() => previousPage()} disabled={!canPreviousPage}>←</Button>{' '}
                    <Button variant="secondary" size="sm" onClick={() => nextPage()} disabled={!canNextPage} >→</Button>{' '}
                    <Button variant="secondary" size="sm" onClick={() => gotoPage(pageCount - 1)} disabled={!canNextPage}>⭲</Button>{' '}
                    <Button variant="secondary" size="sm" onClick={() => setAllFilters([])}>Reset</Button>{' '}
                  </ButtonGroup>
                  </Col>
                  <Col>
                  <ButtonGroup>
                  {[...Array(pageCount).keys()].map((num) => (<Button size="sm" variant="outline-secondary" key={num} onClick={() => { gotoPage(num) }}>{num + 1}</Button>))}
                  </ButtonGroup>
                  </Col>
                  <Col>
                    <ButtonGroup className="me-2">
                      {[10, 50, 100, 500].map(ps => (
                        <Button variant={(state.pageSize === ps) ? "primary" : "secondary"} size="sm" onClick={() => setPageSize(ps)}>{ps}</Button>
                      ))}
                    </ButtonGroup>
                  </Col>
                </Row>
              </td>
            </tr>
          </tfoot>
        </Table>
    )   
}

DataTable.defaultProps = {
  initialState: { pageIndex: 0, pageSize: 50 }
}

export default DataTable;