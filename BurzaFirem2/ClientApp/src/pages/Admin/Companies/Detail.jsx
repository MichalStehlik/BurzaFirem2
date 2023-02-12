import React, {useState, useEffect, useCallback} from 'react';
import { useParams } from "react-router-dom";
import { useAuthContext } from "../../../providers/AuthProvider"
import { Spinner, Alert, Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap'
import { Link } from 'react-router-dom'
import { useNavigate } from "react-router-dom"
import axios from "axios"
import Display from "./Display"
import Edit from "./Edit"
import Branches from "./Branches"
import Activities from "./Activities"
import Listings from "./Listings"
import Contacts from "./Contacts"

const Detail = props => {
    const { id } = useParams();
    const [{accessToken}] = useAuthContext();
    const [editing, setEditing] = useState(false);
    const [response, setResponse] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(false);
    const [showDelete, setShowDelete] = useState(false);
    const navigate = useNavigate();
    const fetchData = useCallback(() => {
        setIsLoading(true);
        setError(false);
        axios.get("/api/v1/companies/" + id,{
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
    },[accessToken, id]);

    const deleteData = useCallback(() => {
        setIsLoading(true);
        setError(false);
        axios.delete("/api/v1/companies/" + id,{
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            } 
        })
        .then(response => {
            setResponse(response.data);
            navigate("/admin/companies");
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
    },[accessToken, id, navigate]);

    useEffect(()=>{
        fetchData();
    }, [fetchData, editing]);

    if (isLoading) {
        return <div className="text-center p-3"><Spinner className="m-2"> </Spinner></div>
    }
    else if (error) {
        <Alert color="danger">Při zpracování dat došlo k chybě.</Alert>
    }
    else if (response) {
        return (
            <>
                <div>
                    <Link to="/admin/companies" className="btn btn-secondary">Seznam</Link>
                    {" "}
                    <Button color="primary" onClick={e => {setEditing(!editing)}}>Editace</Button>
                    {" "}
                    <Button color="danger" onClick={e => {setShowDelete(true); /*deleteData(); navigate("/admin/companies"); */}}>Smazání</Button>
                </div>
                <h1 className="mt-2">{response.name}</h1>
                {editing ? <Edit data={response} switchMode={setEditing} fetchdata={fetchData} /> : <Display data={response} />}
                <h2 className="mt-2">Obory</h2>
                <Branches data={response} />  
                <h2 className="mt-2">Aktivity</h2>
                <Activities data={response} />  
                <h2 className="mt-2">Seznamy</h2>
                <Listings data={response} />  
                <h2 className="mt-2">Kontakty</h2>
                <Contacts data={response} />  

                <Modal isOpen={showDelete} toggle={()=>setShowDelete(prev => !prev)}>
                    <ModalHeader toggle={()=>setShowDelete(prev => !prev)}>Smazání firmy</ModalHeader>
                    <ModalBody>
                        Opravdu chcete tuto firmu odstranit z databáze?
                    </ModalBody>
                    <ModalFooter>
                        <Button color="danger" onClick={()=>{setShowDelete(prev => !prev); deleteData();}}>Odstranit</Button>{' '}
                        <Button color="secondary" onClick={()=>setShowDelete(prev => !prev)}>Storno</Button>
                    </ModalFooter>
                </Modal>
            </>
        )
    } 
    else {
        return <div className="text-center p-3"><Spinner className="m-2"> </Spinner></div>
    }
}

export default Detail;