import {useEffect} from "react"
import { useNavigate } from "react-router-dom";
import { useAuthContext } from "../providers/AuthProvider";

export const useRequireAdmin = (redirectTo = "/unauthorized") => {
    const [{ accessToken, profile }] = useAuthContext();
    let navigate = useNavigate();
    useEffect(()=>{
        if (accessToken == null) navigate(redirectTo);
        console.log(profile);
        if (profile.admin !== "1") navigate(redirectTo);
    },[]);
    
    return null;
}

export default useRequireAdmin;