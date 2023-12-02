import { useAuthContext, CLEAR_ACCESS_TOKEN } from "../../providers/AuthProvider"
import { useAppContext } from "../../providers/AppProvider"
import { Button } from 'reactstrap'
import { useNavigate } from "react-router-dom"

export const Profile = () => {
    const [{ profile, accessToken }, dispatch] = useAuthContext();
    const [{ config }, dispatchApp] = useAppContext();
    const navigate = useNavigate();
    return (
        <>
        <div className="text-center">
            <Button onClick={() => {
                dispatch({type: CLEAR_ACCESS_TOKEN}); navigate("/");}}>Odhlásit</Button>
        </div>
        {(config && config.debug === "1") ? (
        <>
        <div>
            <h1>Profil</h1>
            <pre>{JSON.stringify(profile)}</pre>
        </div>
        <div>
            <h1>Token</h1>
            <pre>{JSON.stringify(accessToken)}</pre>
        </div>
        <div>
            <h1>Config</h1>
            <pre>{JSON.stringify(config)}</pre>
        </div>
        </>
        ) : null}
        </>
        
    );
}

export default Profile;