import React, {useEffect, useCallback, useState} from 'react'
import { useAuthContext } from "../../../providers/AuthProvider"
import { Spinner, Alert, Button } from 'reactstrap'
import axios from "axios";

const Branches = ({data}) => {
    const [{accessToken}] = useAuthContext();
    const [responseBranches, setResponseBranches] = useState(null);
    const [isLoadingBranches, setIsLoadingBranches] = useState(false);
    const [errorBranches, setErrorBranches] = useState(false);
    const [responseAssignedBranches, setResponseAssignedBranches] = useState(null);
    const [isLoadingAssignedBranches, setIsLoadingAssignedBranches] = useState(false);
    const [errorAssignedBranches, setErrorAssignedBranches] = useState(false);
    const fetchBranches = useCallback(() => {
        setIsLoadingBranches(true);
        setErrorBranches(false);
        axios.get("/api/v1/branches",{
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            } 
        })
        .then(response => {
            setResponseBranches(response.data);
        })
        .catch(error => {
            if (error.response) {
                setErrorBranches({status: error.response.status, text: error.response.statusText});
            }
            else
            {
                setErrorBranches({status: 0, text: "Neznámá chyba"});
            }         
            setResponseBranches([]);
        });
        setIsLoadingBranches(false);
    },[accessToken]);
    const fetchAssignedBranches = useCallback(() => {
        setIsLoadingAssignedBranches(true);
        setErrorAssignedBranches(false);
        axios.get("/api/v1/companies/"+data.companyId+"/branches",{
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            } 
        })
        .then(response => {
            setResponseAssignedBranches(response.data);
        })
        .catch(error => {
            if (error.response) {
                setErrorAssignedBranches({status: error.response.status, text: error.response.statusText});
            }
            else
            {
                setErrorAssignedBranches({status: 0, text: "Neznámá chyba"});
            }         
            setResponseAssignedBranches([]);
        });
        setIsLoadingAssignedBranches(false);
    },[accessToken, data]);
    const addBranch = useCallback((branchId) => {
        axios.post("/api/v1/companies/"+data.companyId+"/branches", {id: branchId}, {
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            } 
        })
        .then(response => {
            fetchAssignedBranches();
        })
    },[accessToken, data, fetchAssignedBranches]);
    const removeBranch = useCallback((branchId) => {
        axios.delete("/api/v1/companies/"+data.companyId+"/branches/" + branchId, {
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            } 
        })
        .then(response => {
            fetchAssignedBranches();
        })
    },[accessToken, data, fetchAssignedBranches]);
    useEffect(()=>{
        fetchBranches();
    },[fetchBranches])
    useEffect(()=>{
        fetchAssignedBranches();
    },[fetchAssignedBranches])
    return (
        <div>
            {
                isLoadingBranches
                    ?
                    <Spinner />
                    :
                    errorBranches
                        ?
                        <Alert color="danger">Při získávání oborů došlo k chybě.</Alert>
                        :
                        responseBranches !== null
                            ?
                            responseBranches.map((item, index) => (
                                    responseAssignedBranches !== null
                                    ?
                                    responseAssignedBranches.filter(e => e.branchId === item.branchId).length > 0
                                        ?
                                        <Button key={ index} onClick={e => {removeBranch(item.branchId)}} className="m-1">{item.name}</Button>
                        :
                                        <Button key={index} onClick={e => {addBranch(item.branchId)}} className="m-1" outline>{item.name}</Button>
                    :
                    null                 
                    ))
                    :
                <p>Nahrávání</p>
        }
        </div>
    )
}

export default Branches;