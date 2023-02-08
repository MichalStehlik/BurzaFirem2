import React, {useEffect, useCallback, useState} from 'react'
import { useAuthContext } from "../../../providers/AuthProvider"
import { Spinner, Alert, Button } from 'reactstrap'
import axios from "axios";

const Listings = ({data}) => {
    const [{accessToken}] = useAuthContext();
    const [responseListings, setResponseListings] = useState([]);
    const [isLoadingListings, setIsLoadingListings] = useState(false);
    const [errorListings, setErrorListings] = useState(false);
    const [responseAssignedListings, setResponseAssignedListings] = useState(null);
    const [isLoadingAssignedListings, setIsLoadingAssignedListings] = useState(false);
    const [errorAssignedListings, setErrorAssignedListings] = useState(false);
    const fetchListings = useCallback(() => {
        setIsLoadingListings(true);
        setErrorListings(false);
        axios.get("/api/v1/listings",{
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            } 
        })
        .then(response => {
            setResponseListings(response.data.data);
        })
        .catch(error => {
            if (error.response) {
                setErrorListings({status: error.response.status, text: error.response.statusText});
            }
            else
            {
                setErrorListings({status: 0, text: "Neznámá chyba"});
            }         
            setResponseListings([]);
        });
        setIsLoadingListings(false);
    },[accessToken]);
    const fetchAssignedListings = useCallback(() => {
        setIsLoadingAssignedListings(true);
        setErrorAssignedListings(false);
        axios.get("/api/v1/companies/"+data.companyId+"/listings",{
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            } 
        })
        .then(response => {
            setResponseAssignedListings(response.data);
        })
        .catch(error => {
            if (error.response) {
                setErrorAssignedListings({status: error.response.status, text: error.response.statusText});
            }
            else
            {
                setErrorAssignedListings({status: 0, text: "Neznámá chyba"});
            }         
            setResponseAssignedListings([]);
        });
        setIsLoadingAssignedListings(false);
    },[accessToken, data]);
    const addListing = useCallback((listingId) => {
        axios.post("/api/v1/companies/"+data.companyId+"/listings", {id: listingId}, {
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            } 
        })
        .then(response => {
            fetchAssignedListings();
        })
    },[accessToken, data, fetchAssignedListings]);
    const removeListing = useCallback((listingId) => {
        axios.delete("/api/v1/companies/"+data.companyId+"/listings/" + listingId, {
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            } 
        })
        .then(response => {
            fetchAssignedListings();
        })
    },[accessToken, data, fetchAssignedListings]);
    useEffect(()=>{
        fetchListings();
    },[fetchListings])
    useEffect(()=>{
        fetchAssignedListings();
    },[fetchAssignedListings])
    console.log(responseListings);
    return (
        <div>
            {
                isLoadingListings
                    ?
                    <Spinner />
                    :
                    errorListings
                        ?
                        <Alert color="danger">Při získávání seznamů došlo k chybě.</Alert>
                        :
                        responseListings !== null
                            ?
                            responseListings.map((item, index) => (
                                    responseAssignedListings !== null
                                    ?
                                    responseAssignedListings.filter(e => e.listingId === item.listingId).length > 0
                                        ?
                                        <Button key={ index} onClick={e => {removeListing(item.listingId)}} className="m-1">{item.name}</Button>
                        :
                                        <Button key={index} onClick={e => {addListing(item.listingId)}} className="m-1" outline>{item.name}</Button>
                    :
                    null                 
                    ))
                    :
                <p>Nahrávání</p>
        }
        </div>
    )
}

export default Listings;