import { Card, CardBody, CardHeader, Button, Input, Form, FormGroup, Label } from 'reactstrap';
import axios from "axios"
import { useForm, Controller } from "react-hook-form"
import { useNavigate } from "react-router-dom"
import { Link, useSearchParams } from 'react-router-dom'

export const PasswordReset = () => {
    let [searchParams, setSearchParams] = useSearchParams();
    let code = searchParams.get("code");
    let email = searchParams.get("email");
    const { control, register, handleSubmit, watch, formState: { errors } } = useForm({
        defaultValues: {
            email: email,
            code: code
        }
    });
    const navigate = useNavigate();
    const onSubmit = data => {
        if (data.password === data.password2) {
            axios.post("/api/v1/Account/password-reset",
            {
                email: data.email,
                code: code,
                password: data.password,
            }
            )
            .then(response => {
                navigate("/account/sign-in");
            })
            .catch(error => {
                console.error(error);
            })
        };
    };
    return (
        <Card style={{maxWidth: 600, margin: "auto"}}>
            <CardHeader>
            <h1>Nastavení hesla</h1>
            </CardHeader>
            <CardBody>
            <Form onSubmit={handleSubmit(onSubmit)}>
            <FormGroup>
            <Label for="email">Email</Label>
            <Controller name="email" control={control} render={({ field }) => <Input {...field} required />} />    
            </FormGroup>
            <FormGroup>
            <Label for="password">Heslo</Label>
            <Controller name="password" control={control} render={({ field }) => <Input type="password" {...field} />} />    
            </FormGroup>
            <FormGroup>
            <Label for="password2">Potvrzení hesla</Label>
            <Controller name="password2" control={control} render={({ field }) => <Input type="password" {...field} />} />    
            </FormGroup>
            <FormGroup>
                <Button type="submit" color="primary">Odeslat</Button>{" "}
                <Link to="/account/sign-in"><Button color="link">Přihlášení</Button></Link>
            </FormGroup>
            </Form>
            </CardBody>
        </Card>
    );
}

export default PasswordReset;