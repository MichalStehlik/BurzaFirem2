import { useForm } from "react-hook-form"
import axios from "axios"
import { useAuthContext, SET_ACCESS_TOKEN } from "../../providers/AuthProvider"
import { useNavigate } from "react-router-dom"

export const SignIn = () => {
    const { register, handleSubmit, watch, formState: { errors } } = useForm();
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
            console.log(response.data);
            dispatch({type: SET_ACCESS_TOKEN, payload: response.data.value});
            navigate("/");
        })
        .catch(error => {
            console.error(error);
        })
    };
    return (
        <>
        <h1>Přihlášení</h1>
        <form onSubmit={handleSubmit(onSubmit)}>
            <div>
                <label>Přihlašovací jméno</label>
                <input defaultValue="" {...register("username")} />
            </div>
            <div>
                <label>Heslo</label>
                <input type="password" {...register("password")} />
            </div>
            <div>
                <button type="submit">Login</button>
            </div>
        </form>
        </>
    );
}

export default SignIn;