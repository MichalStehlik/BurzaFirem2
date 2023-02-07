import React, {useEffect, useCallback, useState} from 'react'
import { useAuthContext } from "../../../providers/AuthProvider"
import { Spinner, Alert, Button, Table, FormGroup, Input, Label } from 'reactstrap'
import axios from "axios"
import { useForm, Controller } from "react-hook-form"


const Contacts = ({data}) => {
    const [{accessToken}] = useAuthContext();
    const [response, setResponse] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(false);
    const { register, handleSubmit, control, setValue, getValues, formState: { errors } } = useForm({
        defaultValues: {
          name: "",
          email: "",
          phone: ""
    }})
    const fetchContacts = useCallback(() => {
        setIsLoading(true);
        setError(false);
        axios.get("/api/v1/companies/" + data.companyId + "/contacts",{
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
    const addContact = useCallback((name, email, phone) => {
        axios.post("/api/v1/companies/" + data.companyId + "/contacts", {name, email, phone, companyId: data.companyId}, {
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            } 
        })
        .then(response => {
            fetchContacts();
        })
    },[accessToken, data, fetchContacts]);
    const removeContact = useCallback((contactId) => {
        axios.delete("/api/v1/companies/"+data.companyId+"/contacts/" + contactId, {
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            } 
        })
        .then(response => {
            fetchContacts();
        })
    },[accessToken, data, fetchContacts]);
    const onSubmit = data => {
        addContact(data.name, data.email, data.phone);
        fetchContacts();
    }
    useEffect(()=>{
        fetchContacts();
    },[fetchContacts])
    return (
        <div>
            {
            isLoading 
            ?
            <Spinner />
            :
                error
                ?
                <Alert color="danger">Při získávání kontaktů došlo k chybě.</Alert>
                :
                    response !== null
                    ?
                    <Table>
                        <thead>
                            <tr>
                                <th>Jméno</th>
                                <th>Email</th>
                                <th>Telefon</th>
                                <th>Akce</th>
                            </tr>
                        </thead>
                        <tbody>
                    {response.map((item, index) => (
                        <tr key={index}>
                            <td>{item.name}</td>
                            <td>{item.email}</td>
                            <td>{item.phone}</td>
                            <td><Button color="danger" size="sm" onClick={e => {removeContact(item.contactId)}}>Odstranit</Button></td>
                        </tr>
                    ))}
                        </tbody>
                        <tfoot>
                            <tr>
                                <td>
                                    <form onSubmit={handleSubmit(onSubmit)}>
                                    <FormGroup>
                                        <Label for="name">Jméno</Label>
                                            <Controller name="name" control={control} rules={{ }} render={({ field }) => <Input {...field} placeholder="Kateřina Pokorná" />} />
                                            {errors.name?.type === 'required' && <span className="text-danger">Jméno je povinný údaj</span>}
                                    </FormGroup>
                                    <FormGroup>
                                        <Label for="name">Email</Label>
                                            <Controller name="email" control={control} rules={{ }} render={({ field }) => <Input {...field} placeholder="pokorna@company.cz" />} />
                                            {errors.name?.type === 'required' && <span className="text-danger">Jméno je povinný údaj</span>}
                                    </FormGroup>
                                    <FormGroup>
                                        <Label for="name">Telefon</Label>
                                            <Controller name="phone" control={control} rules={{ }} render={({ field }) => <Input {...field} placeholder="+420111222333" />} />
                                            {errors.name?.type === 'required' && <span className="text-danger">Jméno je povinný údaj</span>}
                                    </FormGroup>
                                    <div className="my-2">
                                        <Button type="submit" color="primary" className="m-1">Uložit</Button>
                                    </div>
                                    </form>
                                </td>
                            </tr>
                        </tfoot>
                    </Table>
                    :
                <p>Nahrávání</p>
        }
        </div>
    )
}

export default Contacts;