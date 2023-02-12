import React, {useState, useEffect, useCallback} from 'react';
import { useParams } from "react-router-dom";
import { useAuthContext } from "../../../providers/AuthProvider"
import { Spinner, Alert, Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap'
import { Link } from 'react-router-dom'
import { useNavigate } from "react-router-dom"
import axios from "axios"

import Display from "./Display"
import Edit from "./Edit"
import Password from "./Password"

export const DISPLAY = "display";
export const EDIT = "edit";
export const PASSWORD = "password";

const Detail = props => {
    const { id } = useParams();
    const [{accessToken}] = useAuthContext();
    const [editing, setEditing] = useState(DISPLAY);
    const [response, setResponse] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(false);
    const [showDelete, setShowDelete] = useState(false);
    const navigate = useNavigate();
    const fetchData = useCallback(() => {
        setIsLoading(true);
        setError(false);
        axios.get("/api/v1/users/" + id,{
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
        axios.delete("/api/v1/users/" + id,{
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            } 
        })
        .then(response => {
            setResponse(response.data);
            navigate("/admin/users/");
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
                    <Link to="/admin/users" className="btn btn-secondary">Seznam</Link>
                    {" "}
                    <Button color="primary" onClick={e => {setEditing(prev => (prev === EDIT) ? DISPLAY : EDIT)}}>Editace</Button>
                    {" "}
                    <Button color="secondary" onClick={e => {setEditing(prev => (prev === PASSWORD) ? DISPLAY : PASSWORD)}}>Heslo</Button>
                    {" "}
                    <Button color="danger" onClick={e => { setShowDelete(true); }}>Smazání</Button>
                </div>
                <h1 className="mt-2">{response.userName}</h1>
                {(editing === EDIT) 
                ? 
                <Edit data={response} switchMode={() => setEditing(DISPLAY)} /> 
                : 
                (editing === PASSWORD)
                ?
                <Password data={response} switchMode={() => setEditing(DISPLAY)} /> 
                :
                <Display data={response} />}
                <Modal isOpen={showDelete} toggle={()=>setShowDelete(prev => !prev)}>
                    <ModalHeader toggle={()=>setShowDelete(prev => !prev)}>Smazání uživatele</ModalHeader>
                    <ModalBody>
                        Opravdu chcete tohoto uživatele odstranit z databáze?
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