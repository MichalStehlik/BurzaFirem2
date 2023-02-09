import React, {useState} from 'react';
import { Button, FormGroup, Label, Input, Alert, Spinner } from 'reactstrap';
import { useForm, Controller } from "react-hook-form"
import { useAuthContext } from "../../../providers/AuthProvider"
import axios from "axios"

const Edit = ({data, switchMode}) => {
    const { register, handleSubmit, control, setValue, getValues, formState: { errors } } = useForm({
        defaultValues: {
          userName: data.userName,
          email: data.email
    }})
    const [{ accessToken }] = useAuthContext();
    const [isLoading, setIsLoading] = useState(false);
    const [failed, setFailed] = useState(false);
    const [ok, setOk] = useState(false);
    const onSubmit = values => {
        setIsLoading(true);
        setOk(false);
        setFailed(false);
        axios.put("/api/v1/users/" + data.Id, {
            id: data.Id,
            userName: values.userName.trim(),
            email: values.email
        }, {
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            }
        })
        .then(response => {
            setOk(true);
            setFailed(false);
            switchMode();
        })
        .catch(error => {
            setOk(false);
            setFailed(true);
        })
        .then(() => {
            setIsLoading(false);
        })
    };
    let {description, offer, wanted, logoOrientation, companyBranches} = getValues();
    return (
        <>
            <form onSubmit={handleSubmit(onSubmit)}>
                <FormGroup>
                    <Label for="name">Uživatelské jméno</Label>
                    <Controller name="userName" control={control} rules={{ required: true }} render={({ field }) => <Input {...field} placeholder="jmeno@firma.cz" />} />
                    {errors.userName?.type === 'required' && <span className="text-danger">Jméno je povinný údaj</span>}
                </FormGroup>
                <FormGroup>
                    <Label for="email">Email</Label>
                    <Controller name="email" control={control} render={({ field }) => <Input {...field} placeholder="jmeno@firma.cz" />} />
                    {errors.name?.type === 'required' && <span className="text-danger">Email je povinný údaj</span>}
                </FormGroup>
                {
                failed
                ?
                <Alert color="danger">Při ukládání dat došlo k chybě.</Alert>
                :
                null
                }
                <div className="my-2">
                {
                    isLoading
                    ?
                    <Spinner className="m-2"> </Spinner>
                    :
                    <>
                        <Button type="submit" color="primary" className="m-1">Uložit</Button>
                        <Button onClick={e => {switchMode()}}>Storno</Button>
                    </>
                }            
                </div>
            </form>
        </>
    );
}

export default Edit;