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
      setUploadError(false);
      setUploadProgress(0);
      setIsUploading(true);
      var upload = new tus.Upload(file, {
        endpoint: "/upload/",
        retryDelays: [0, 3000, 5000, 10000, 20000],
        metadata: {
          filename: file.name,
          contentType: file.type
        },
        headers: {
          Authorization: "Bearer " + accessToken
        },
        onError: function (error) {
          console.error(error);
          setUploadError(error);
        },
        onProgress: function (bytesUploaded, bytesTotal) {
          var percentage = (bytesUploaded / bytesTotal * 100).toFixed(2)
          setUploadProgress(percentage);
          console.log(bytesUploaded, bytesTotal, percentage + "%")
        },
        onSuccess: function () {
          console.log("Download %s from %s", upload.file.name, upload.url);
          fetchData({page: 0, sort: [], filters: []})
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

    const handleFilesUpload = e => { 
      setIsUploading(true);
      setUploadError(false);
      const formData = new FormData();
      var file = e.target.files[0];
      formData.append(
        "file",
        file
      );
      axios.post("api/v1/images", 
        formData, 
        {
          headers: {
            Authorization: "Bearer " + accessToken,
            "Content-Type": file.type
          }
        }
      )
      .then(response => {fetchData({page: 0, sort: [], filters: []})})
      .catch(error => {setUploadError("P??i nahr??v??n?? souboru do??lo k chyb??.")})
      .then(()=>{setIsUploading(false)})
    }

    const fetchData = useCallback(({page, size = 0, sort, filters})=>{
      console.log(page, size, sort, filters);
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
              setError({text:  "Nezn??m?? chyba", status: ""});
            }
          })
          .then(()=>{
            setIsLoading(false);
          });    
        })();    
      },[accessToken]);

      const columns = useMemo(() => [
        {Header: "N??zev", accessor: "originalName"},      
        {Header: "Typ", accessor: "contentType", disableFilters:true},
        {Header: "V????ka", accessor: "height", disableFilters:true},
        {Header: "??????ka", accessor: "width", disableFilters:true},
        {Header: "Vlastn??k", accessor: "uploader", disableFilters:true, Cell: (data)=>(data.cell.value.email)},
        {Header: "Nahr??no", accessor: "created", disableFilters:true, Cell: (data)=>(<DateTime date={data.cell.value} />)},    
        {Header: "Akce", Cell: (data)=>(<Link to={"" + data.row.original.imageId}>Detail</Link>)}
    ]); 

    return (
        <>
            <h1>Seznam obr??zk??</h1>
            <DataTable
                columns={columns}
                data={data}
                fetchData={fetchData}
                isLoading={isLoading}
                error={error}
                totalPages={totalPages}
            />
            <p>TUS (velk?? soubory)</p>
            <Input type="file" name="file" id="file" onChange={handleFilesSelected} ref={fileUploader} />
            {isUploading 
            ?
              <Progress value={uploadProgress} />
            :
              null
            }
            {uploadError
            ?
              <Alert color="danger">P??i nahr??v??n?? souboru do??lo k chyb??. Soubor mohl b??t p????li?? velk??, nemusel m??t podporovan?? typ (obr??zek) nebo k t??to akci nesta??ila Va??e pr??ve.</Alert>
            :
              null
            }
            <p>Upload</p>
            <Input type="file" name="upload" id="upload" onChange={handleFilesUpload} />
        </>
    );
}

export default List;