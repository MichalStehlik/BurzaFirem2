import {useState} from "react"
import { Button, Form, FormGroup, Label, Input, Alert, Spinner } from 'reactstrap';
import { useForm, Controller } from "react-hook-form"
import { useAuthContext } from "../../../providers/AuthProvider"
import { useNavigate } from "react-router-dom"
import axios from "axios";

const Create = props => {
    const { register, handleSubmit, control, setValue, getValues, formState: { errors } } = useForm({
        defaultValues: {
          userName: "",
          email: "",
          password: "",
          admin: false,
          editor: true
    }})
    const [{ accessToken }] = useAuthContext();
    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(false);
    const [failed, setFailed] = useState(false);
    const [ok, setOk] = useState(false);
    const onSubmit = data => {
        setIsLoading(true);
        setOk(false);
        setFailed(false);
        console.log(data);
        axios.post("/api/v1/users", {
            userName: data.userName.trim(),
            email: data.email.trim(),
            password: data.password.trim(),
            admin: Boolean(data.admin),
            editor: Boolean(data.editor)
        }, {
            headers: {
                Authorization: "Bearer " + accessToken,
                "Content-Type": "application/json"
            }
        })
        .then(response => {
            setOk(true);
            setFailed(false);
            navigate("/admin/users/" + response.data.id)
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
            <h1>Vytvoření nového uživatele</h1>
            <form onSubmit={handleSubmit(onSubmit)}>
                <FormGroup>
                    <Label for="userName">Uživatelské jméno</Label>
                    <Controller name="userName" control={control} rules={{ required: true }} render={({ field }) => <Input {...field} />} />
                    {errors.userName?.type === 'required' && <span className="text-danger">Uživatelské jméno je povinný údaj</span>}
                </FormGroup>
                <FormGroup>
                    <Label for="email">Email</Label>
                    <Controller name="email" control={control} rules={{ required: true }} render={({ field }) => <Input {...field} placeholder="jmeno@firma.cz" type="email" />} />
                    {errors.email?.type === 'required' && <span className="text-danger">Email je povinný údaj</span>}
                </FormGroup>
                <FormGroup>
                    <Label for="password">Heslo</Label>
                    <Controller name="password" control={control} rules={{ required: true }} render={({ field }) => <Input {...field} />} />
                    {errors.password?.type === 'required' && <span className="text-danger">Heslo je povinný údaj</span>}
                </FormGroup>
                <FormGroup>
                    <Controller name="admin" control={control} render={({ field }) => <Input {...field} type="checkbox" defaultChecked={false} />} />{' '}<Label for="admin">Administrátor</Label>
                    {errors.admin?.type === 'required' && <span className="text-danger">Administrátor je povinný údaj</span>}
                </FormGroup>
                <FormGroup>
                    <Controller name="editor" control={control} render={({ field }) => <Input {...field} type="checkbox" defaultChecked={true} />} />{' '}<Label for="editor">Editor</Label>
                    {errors.editor?.type === 'required' && <span className="text-danger">Editor je povinný údaj</span>}
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
                        <Button onClick={e => { navigate("/admin/users")}}>Zpět</Button>
                    </>
                }            
                </div>
            </form>
        </>
    );
}

export default Create;