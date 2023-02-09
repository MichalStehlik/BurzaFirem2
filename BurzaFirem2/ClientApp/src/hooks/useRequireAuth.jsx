import { useNavigate } from "react-router-dom";
import { useAuthContext } from "../providers/AuthProvider";

export const useRequireAuth = (redirectTo = "/unauthorized") => {
    const [{ accessToken }] = useAuthContext();
    let navigate = useNavigate();
    if (accessToken == null) navigate(redirectTo);
    return null;
}

export default useRequireAuth;