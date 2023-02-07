import React, {useEffect, useCallback, useState} from 'react'
import { useAuthContext } from "../../../providers/AuthProvider"
import { Spinner, Alert, Button } from 'reactstrap'
import axios from "axios";

const Activities = ({data}) => {
    const [{accessToken}] = useAuthContext();
    const [response, setResponse] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(false);
    const [responseAssigned, setResponseAssigned] = useState(null);
    const [isLoadingAssigned, setIsLoadingAssigned] = useState(false);
    const [errorAssigned, setErrorAssigned] = useState(false);
    const fetchActivities = useCallback(() => {
        setIsLoading(true);
        setError(false);
        axios.get("/api/v1/activities",{
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            } 
        })
        .then(response => {
            setResponse(response.data);
        })
        .catch(error => {
            if (error.response) {
                setError({status: error.response.status, text: error.response.statusText});
            }
            else
            {
                setError({status: 0, text: "Neznámá chyba"});
            }         
            setResponse([]);
        });
        setIsLoading(false);
    },[accessToken]);
    const fetchAssigned = useCallback(() => {
        setIsLoadingAssigned(true);
        setErrorAssigned(false);
        axios.get("/api/v1/companies/"+data.companyId+"/activities",{
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            } 
        })
        .then(response => {
            setResponseAssigned(response.data);
        })
        .catch(error => {
            if (error.response) {
                setErrorAssigned({status: error.response.status, text: error.response.statusText});
            }
            else
            {
                setErrorAssigned({status: 0, text: "Neznámá chyba"});
            }         
            setResponseAssigned([]);
        });
        setIsLoadingAssigned(false);
    },[accessToken, data]);
    const addActivity = useCallback((activityId) => {
        axios.post("/api/v1/companies/"+data.companyId+"/activities", {id: activityId}, {
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            } 
        })
        .then(response => {
            fetchAssigned();
        })
    },[accessToken, data, fetchAssigned]);
    const removeActivity = useCallback((branchId) => {
        axios.delete("/api/v1/companies/"+data.companyId+"/activities/" + branchId, {
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            } 
        })
        .then(response => {
            fetchAssigned();
        })
    },[accessToken, data, fetchAssigned]);
    useEffect(()=>{
        fetchActivities();
    },[fetchActivities])
    useEffect(()=>{
        fetchAssigned();
    },[fetchAssigned])
    return (
        <div>
            {
            isLoading 
            ?
            <Spinner />
            :
                error
                ?
                <Alert color="danger">Při získávání aktivit došlo k chybě.</Alert>
                :
                    response !== null
                    ?
                    response.map((item, index) => (
                                responseAssigned !== null
                                    ?
                                    responseAssigned.filter(e => e.activityId === item.activityId).length > 0
                                        ?
                                        <Button key={index} onClick={e => { removeActivity(item.activityId) }} className="m-1">{item.name}</Button>
                                        :
                                        <Button key={ index} onClick={e => {addActivity(item.activityId)}} className="m-1" outline>{item.name}</Button>
                    :
                    null               
                    ))
                    :
                <p>Nahrávání</p>
        }
        </div>
    )
}

export default Activities;