import { useAuthContext } from "../../providers/AuthProvider"
import {Link} from "react-router-dom"

export const Token = () => {
    const [{ profile, accessToken }] = useAuthContext();
    return (
        <>
            <pre>{accessToken}</pre>
            <pre>{JSON.stringify(profile," ",4)}</pre>
        </>        
    );
}

export default Token;