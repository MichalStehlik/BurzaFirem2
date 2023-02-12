import React, {useState} from 'react';
import { Button, FormGroup, Label, Input, Alert, Spinner } from 'reactstrap';
import { useForm, Controller } from "react-hook-form"
import { useAuthContext } from "../../../providers/AuthProvider"
import axios from "axios"

const Password = ({data, switchMode}) => {
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
        axios.put("/api/v1/users/" + data.id + "/password", {
            id: data.id,
            password: values.password.trim()
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
    return (
        <>
            <form onSubmit={handleSubmit(onSubmit)}>
                <FormGroup>
                    <Label for="password">Nové heslo</Label>
                    <Controller name="password" control={control} rules={{ required: true }} render={({ field }) => <Input {...field} placeholder="" />} />
                    {errors.userName?.type === 'required' && <span className="text-danger">Jméno je povinný údaj</span>}
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

export default Password;