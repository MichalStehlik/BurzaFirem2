import { useAuthContext } from "../../providers/AuthProvider"
import {Link} from "react-router-dom"


export const Dashboard = () => {
    const [{ profile, accessToken }] = useAuthContext();
    return (
        <ul>
            <li><Link to="companies">Firmy</Link></li>
            <li><Link to="users">Uživatelé</Link></li>
        </ul>
        
    );
}

export default Dashboard;