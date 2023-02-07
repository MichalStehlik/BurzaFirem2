import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuthContext } from "../../providers/AuthProvider";

export const useRequireAuth = (redirectTo = "/unauthorized") => {
    const [{ accessToken }] = useAuthContext();
    let navigate = useNavigate();
    useEffect(() => {
        if (accessToken == null) navigate(redirectTo);
    }, [accessToken, redirectTo, navigate]);
}