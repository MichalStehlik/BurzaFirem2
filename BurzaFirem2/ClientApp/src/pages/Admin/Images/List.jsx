import { useState, useCallback, useMemo, useRef } from "react";
import { Link } from 'react-router-dom'
import { useAuthContext } from "../../../providers/AuthProvider"
import {Alert, Input, Progress} from "reactstrap"
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
    const [isUploading, setIsUploading] = useState(false);
    const [uploadProgress, setUploadProgress] = useState(0);
    const [uploadError, setUploadError] = useState(false);

    const handleFilesSelected = e => {
      var file = e.target.files[0];
      setIsUploading(true);
      var upload = new tus.Upload(file, {
        endpoint: "https://localhost:44416/upload/",
        retryDelays: [0, 3000, 5000, 10000, 20000],
        metadata: {
          filename: file.name,
          filetype: file.type
        },
        onError: function (error) {
          setUploadError(error);
        },
        onProgress: function (bytesUploaded, bytesTotal) {
          var percentage = (bytesUploaded / bytesTotal * 100).toFixed(2)
          setUploadProgress(percentage);
          console.log(bytesUploaded, bytesTotal, percentage + "%")
        },
        onSuccess: function () {
          console.log("Download %s from %s", upload.file.name, upload.url)
        }
      });
      upload.findPreviousUploads().then(function (previousUploads) {
        if (previousUploads.length) {
            upload.resumeFromPreviousUpload(previousUploads[0])
        }
        upload.start()
      });
      setIsUploading(false);
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
            <Input type="file" name="file" id="file" onChange={handleFilesSelected} ref={fileUploader} />
            {isUploading 
            ?
              <Progress value={uploadProgress} />
            :
              null
            }
            {uploadError
            ?
              <Alert color="danger">{uploadError}</Alert>
            :
              null
            }
        </>
    );
}

export default List;