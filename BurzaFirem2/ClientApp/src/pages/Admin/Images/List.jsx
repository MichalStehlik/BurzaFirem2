import { useState, useCallback, useMemo, useRef } from "react";
import { Link } from 'react-router-dom'
import { useAuthContext } from "../../../providers/AuthProvider"
import {Badge, Input} from "reactstrap"
import DataTable from "../../../components/DataTable";
import DateTime from "../../../components/DateTime"
import axios from "axios"
import * as tus from 'tus-js-client'

const List = () => {
    const [{ accessToken }] = useAuthContext();
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(false);
    const [data, setData] = useState([]);
    const [totalPages, setTotalPages] = useState(0);
    const fileUploader = useRef(null);

    const handleFilesSelected = e => {
      var files = e.target.files;
      for (let file of files) {
        console.log(file);
        var upload = new tus.Upload(file, {
          endpoint: "https://localhost:44416/upload/",
          retryDelays: [0, 3000, 5000, 10000, 20000],
          metadata: {
            filename: file.name,
            filetype: file.type
          },
          onError: function (error) {
            console.log("Failed because: " + error)
          },
          onProgress: function (bytesUploaded, bytesTotal) {
            var percentage = (bytesUploaded / bytesTotal * 100).toFixed(2)
            console.log(bytesUploaded, bytesTotal, percentage + "%")
          },
          onSuccess: function () {
            console.log("Download %s from %s", upload.file.name, upload.url)
          }
        });
      }
    };

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
          axios.get("/api/v1/images?" + parameters.join("&"), {headers: { Authorization: "Bearer " + accessToken, "Content-Type": "application/json" }})
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
        {Header: "Název", accessor: "originalName"},      
        {Header: "Typ", accessor: "contentType"},
        {Header: "Nahráno", accessor: "created", disableFilters:true, Cell: (data)=>(<DateTime date={data.cell.value} />)},
        {Header: "Akce", Cell: (data)=>(<Link to={"" + data.row.original.id}>Detail</Link>)}
    ]); 

    return (
        <>
            <h1>Seznam obrázků</h1>
            <DataTable
                columns={columns}
                data={data}
                fetchData={fetchData}
                isLoading={isLoading}
                error={error}
                totalPages={totalPages}
            />
            <Input type="file" name="file" id="file" onChange={handleFilesSelected} multiple ref={fileUploader} />
        </>
    );
}

export default List;