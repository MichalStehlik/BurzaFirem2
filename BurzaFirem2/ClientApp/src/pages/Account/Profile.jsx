import { useAuthContext, CLEAR_ACCESS_TOKEN } from "../../providers/AuthProvider"
import { Button } from 'reactstrap'
import { useNavigate } from "react-router-dom"

export const Profile = () => {
    const [{ profile, accessToken }, dispatch] = useAuthContext();
    const navigate = useNavigate();
    return (
        <>
        <div className="text-center">
            <Button onClick={() => {
                dispatch({type: CLEAR_ACCESS_TOKEN}); navigate("/");}}>Odhlásit</Button>
        </div>
        </>
        
    );
}

export default Profile;