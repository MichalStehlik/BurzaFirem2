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
                  case "userName": parameters.push("userName=" + f.value); break;
                  case "email": parameters.push("email=" + f.value); break;
                  default: break;
              }
            }
            }
          axios.get("/api/v1/users?" + parameters.join("&"), {headers: { Authorization: "Bearer " + accessToken, "Content-Type": "application/json" }})
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
        {Header: "Jméno", accessor: "userName"},      
        {Header: "Email", accessor: "email", disableSortBy: true},
        {Header: "Aktualizace", accessor: "updated", disableFilters:true, Cell: (data)=>(<DateTime date={data.cell.value} />)},
        {Header: "Akce", Cell: (data)=>(<Link to={"" + data.row.original.id}>Detail</Link>)}
    ]); 

    return (
        <>
            <h1>Seznam uživatelů</h1>
            <div><Link className="btn btn-success" to="create">Nový</Link></div>
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