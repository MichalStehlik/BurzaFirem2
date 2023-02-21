import React, {useState, useEffect, useCallback} from 'react';
import {Alert, Input, Progress, Button} from "reactstrap"
import { useAuthContext } from "../../../providers/AuthProvider"
import axios from "axios"

const Logo = ({data, fetchData}) => {
    const [{ accessToken }] = useAuthContext();
    const [isUploading, setIsUploading] = useState(false);
    const [uploadProgress, setUploadProgress] = useState(0);
    const [uploadError, setUploadError] = useState(false);

    const DeleteLogo = useCallback(()=>{
        axios.delete("api/v1/companies/" + data.companyId + "/logo",
        {
            headers: {
              Authorization: "Bearer " + accessToken,
              "Content-Type": "application/json"
            }
          }
        )
        .then(response => {fetchData()})
    },[accessToken]);

    const handleFilesUpload = e => { 
        setIsUploading(true);
        setUploadError(false);
        const formData = new FormData();
        var file = e.target.files[0];
        formData.append(
          "file",
          file
        );
        axios.post("api/v1/companies/" + data.companyId + "/logo", 
          formData, 
          {
            headers: {
              Authorization: "Bearer " + accessToken,
              "Content-Type": file.type
            }
          }
        )
        .then(response => {fetchData()})
        .catch(error => {setUploadError("Při nahrávání souboru došlo k chybě.")})
        .then(()=>{setIsUploading(false)})
      }

    if (data.logoId) {
        return (
            <div className="m-2">
                <div>
                    <img src={"/api/v1/images/" + data.logoId + "/content"} alt="Logo" />
                </div>
                <div>
                <Button onClick={e => {DeleteLogo()}}>Vymazat</Button>
                </div>      
            </div>
        );
    } else {
        return(
            <div className="m-2">
                <Input type="file" name="upload" id="upload" onChange={handleFilesUpload} />
            </div>
        );
    } 
    
}

export default Logo;