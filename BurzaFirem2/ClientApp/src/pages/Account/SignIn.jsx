import { useForm, Controller } from "react-hook-form"
import axios from "axios"
import { Card, CardBody, CardHeader, Button, Input, Form, FormGroup, Label } from 'reactstrap';
import { useAuthContext, SET_ACCESS_TOKEN } from "../../providers/AuthProvider"
import { useNavigate } from "react-router-dom"

export const SignIn = () => {
    const { control, register, handleSubmit, watch, formState: { errors } } = useForm();
    const [, dispatch] = useAuthContext();
    const navigate = useNavigate();
    const onSubmit = data => {
        axios.post("/api/v1/Account/login",
        {
            username: data.username,
            password: data.password
        }
        )
        .then(response => {
            dispatch({type: SET_ACCESS_TOKEN, payload: response.data.value});
            navigate("/");
        })
        .catch(error => {
            console.error(error);
        })
    };
    return (
        <Card style={{maxWidth: 600, margin: "auto"}}>
            <CardHeader>
            <h1>Přihlášení</h1>
            </CardHeader>
            <CardBody>
            <Form onSubmit={handleSubmit(onSubmit)}>
            <FormGroup>
            <Label for="username">Přihlašovací jméno</Label>
            <Controller name="username" control={control} render={({ field }) => <Input type="email" placeholder="jmeno@firma.cz" {...field} />} />    
            </FormGroup>
            <FormGroup>
            <Label for="password">Heslo</Label>
            <Controller name="password" control={control} render={({ field }) => <Input type="password" {...field} />} />    
            </FormGroup>
            <FormGroup>
                <Button type="submit" color="primary">Přihlásit</Button>
            </FormGroup>
            </Form>
            </CardBody>
        </Card>
    );
}

export default SignIn;