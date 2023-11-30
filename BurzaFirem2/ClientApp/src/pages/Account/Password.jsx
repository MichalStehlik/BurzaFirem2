import { Card, CardBody, CardHeader, Button, Input, Form, FormGroup, Label } from 'reactstrap';
import axios from "axios"
import { useForm, Controller } from "react-hook-form"
import { useNavigate } from "react-router-dom"
import { Link } from 'react-router-dom'

export const Password = () => {
    const { control, register, handleSubmit, watch, formState: { errors } } = useForm();
    const navigate = useNavigate();
    const onSubmit = data => {
        axios.post("/api/v1/Account/send-password-recovery",
        {
            email: data.email
        }
        )
        .then(response => {
            navigate("/account/password-reset");
        })
        .catch(error => {
            console.error(error);
        })
    };
    return (
        <Card style={{maxWidth: 600, margin: "auto"}}>
            <CardHeader>
            <h1>Obnovení hesla</h1>
            </CardHeader>
            <CardBody>
            <Form onSubmit={handleSubmit(onSubmit)}>
            <FormGroup>
            <Label for="username">Email</Label>
            <Controller name="email" control={control} render={({ field }) => <Input {...field} />} />    
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

export default Password;