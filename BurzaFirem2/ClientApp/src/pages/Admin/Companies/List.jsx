import { useState, useCallback, useMemo } from "react";
import { Link } from 'react-router-dom'
import { useAuthContext } from "../../../providers/AuthProvider"
import {Badge} from "reactstrap"
import DataTable from "../../../components/DataTable";
import DateTime from "../../../components/DateTime"
import axios from "axios";

const List = props => {
    const [{ accessToken }] = useAuthContext();
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(false);
    const [data, setData] = useState([]);
    const [totalPages, setTotalPages] = useState(0);

    const fetchData = useCallback(({page, size = 0, sort, filters})=>{
        (async () => {
          setIsLoading(true);
          setError(false);
          let parameters = [];
  
          let order = sort[0] ? sort[0].id : undefined;
          if (order) order = order.toLowerCase();
          if (order && sort[0].desc) order = order + "_desc";
  
          if (page) parameters.push("page=" + page);
          if (size) parameters.push("pageSize=" + size);
          if (order) parameters.push("order=" + order);
          if (Array.isArray(filters)) {
            for (let f of filters) {
              switch (f.id) {
                  case "name": parameters.push("name=" + f.value); break;
                  default: break;
              }
            }
            }
          axios.get("/api/v1/companies?" + parameters.join("&"), {headers: { Authorization: "Bearer " + accessToken, "Content-Type": "application/json" }})
          .then(response => {
            setData(response.data.data);
            setTotalPages(response.data.pages);
          })
          .catch(error => {
            if (error.response)
            {
              setError({text:  error.response.statusText, status: error.response.status});
            }
            else
            {
              setError({text:  "Neznámá chyba", status: ""});
            }
          })
          .then(()=>{
            setIsLoading(false);
          });    
        })();    
      },[accessToken]);

      const columns = useMemo(() => [
        {Header: "Název", accessor: "name"},      
        {Header: "Obory", accessor: "targets", disableSortBy: true, Cell: (data) => { if (Array.isArray(data.row.original.branches)) return data.row.original.branches.map((item, index) => (<Badge color="" style={{color: item.textColor, margin: "3px", backgroundColor: item.backgroundColor}} key={index}>{item.name}</Badge>)); else return "ne"}, Filter: (column) => {return []}},
        {Header: "Aktualizace", accessor: "updated", disableFilters:true, Cell: (data)=>(<DateTime date={data.cell.value} />)},
        {Header: "Akce", Cell: (data)=>(<Link to={"" + data.row.original.companyId}>Detail</Link>)}
    ]); 

    return (
        <>
            <h1>Seznam firem</h1>
            <div><Link className="btn btn-success" to="create">Nová</Link></div>
            <DataTable
                columns={columns}
                data={data}
                fetchData={fetchData}
                isLoading={isLoading}
                error={error}
                totalPages={totalPages}
            />
        </>
    );
}

export default List;