import { useAuthContext } from "../../providers/AuthProvider"
import {Link} from "react-router-dom"

export const Dashboard = () => {
    const [{ profile, accessToken }] = useAuthContext();
    return (
        <ul>
            <li><Link to="companies">Firmy</Link></li>
            {profile !== null && profile.admin === "1"
            ?
            <>
            <li><Link to="users">Uživatelé</Link></li>
            <li><Link to="images">Obrázky</Link></li>
            </>
            :
            null
            }
            
        </ul>
        
    );
}

export default Dashboard;