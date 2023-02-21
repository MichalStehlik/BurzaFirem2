import {useState, useEffect, useCallback} from 'react';
import { useParams } from "react-router-dom";
import { useAuthContext } from "../../providers/AuthProvider"
import { Spinner, Alert, Button, Badge, Table } from 'reactstrap'
import { Link } from 'react-router-dom'
import { useNavigate } from "react-router-dom"
import axios from "axios"

const Detail = props => {
    const { id } = useParams();
    const [{accessToken}] = useAuthContext();
    const [response, setResponse] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(false);
    const navigate = useNavigate();
    const fetchData = useCallback((id) => {
        setIsLoading(true);
        setResponse(null);
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
                setError({ status: error.response.status, text: error.response.statusText })
            } else {
                setError({ status: 0, text: "??" });
            }
            setResponse(null);               
        })
        .then(() => {
            setIsLoading(false);
        })
    }, []);

    useEffect(()=>{
        fetchData(id);
    }, [fetchData, id]);

    if (isLoading) {
        return <div className="text-center p-3"><Spinner className="m-2"> </Spinner></div>
    }
    else if (error.status === 404) {
        return <Alert color="danger">Taková firma v databázi není.</Alert>
    }
    else if (error) {
        return <Alert color="danger">Při zpracování dat došlo k chybě.</Alert>
    }
    else if (response) {
        return (
            <>
                <div className="border-bottom pb-1 mb-2">
                    <h1 className="mt-1 display-4">{response.name}</h1>
                    {response.branches.map((item, index) => (<Badge key={index} className="m-1" color="" style={{backgroundColor: item.backgroundColor}}>{item.name}</Badge>))}
                    {response.activities.map((item, index) => (<Badge key={index} className="m-1" style={{ backgroundColor: "gray" }}>{item.name}</Badge>))}
                </div>
            {response.description ? <div dangerouslySetInnerHTML={{__html: response.description }} /> : null}
            {response.offer
                ?
                <>
                <h2>Nabízíme</h2>
                <div dangerouslySetInnerHTML={{__html: response.offer }} />
                </>
                :
                null
            }
            {response.wanted
                ?
                <>
                <h2>Požadujeme</h2>
                <div dangerouslySetInnerHTML={{__html: response.wanted }} />
                </>
                :
                null
            }
            {response.municipality || response.addressStreet
            ?
            <>
            <h2>Sídlo</h2>
            <address className="p-1 bg-light text-dark">
                {response.addressStreet}<br />{response.municipality}
            </address>
            </>
            :
            null
            }
            {response.companyBranches
            ?
            <>
            <h2>Pobočky</h2>
            <div dangerouslySetInnerHTML={{__html: response.companyBranches }} />
            </>
            :
            null
                }
                <div className="text-center">
                {response.companyUrl
                    ?
                    <Button tag="a" href={response.companyUrl.startsWith("http") ? response.companyUrl : ("https://" + response.companyUrl) } className="m-1" color="primary">Firemní web</Button>
                    :
                    null
                }
                {response.presentationUrl
                ?
                <Button tag="a" href={response.presentationUrl.startsWith("http") ? response.presentationUrl : ("https://" + response.presentationUrl) } className="m-1" >Firemní prezentace</Button>
                :
                null
                }
                </div>
            {response.contacts.length > 0
            ?
            <>
            <h2>Kontakty</h2>
            <Table striped={true} responsive={ true }>
                <tbody>
                {response.contacts.map((item, index) => (
                    <tr key={index}>
                        <td>{item.name}</td>
                        <td><a href={"mailto:" + item.email}>{item.email}</a></td>
                        <td><a href={"tel:" + item.phone}>{item.phone}</a></td>
                    </tr>
                ))}
                </tbody>
            </Table>
            </>
            :
            null
                }
                {response.logoId
                    ?
                    <div className="text-center m-3">
                        <a href={response.companyUrl.startsWith("http") ? response.companyUrl : ("https://" + response.companyUrl)}>
                            <img src={"/api/v1/images/" + response.logoId + "/content"} alt={response.name} className="mb-3" style={{ maxHeight: "128px" }} />
                        </a>
                    </div>
                    :
                    null
                }
            </>
        )
    } 
    else {
        return <div className="text-center p-3"><Spinner className="m-2"> </Spinner></div>
    }
}

export default Detail;