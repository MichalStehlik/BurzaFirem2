import { useAuthContext } from "../../providers/AuthProvider"
import {Link} from "react-router-dom"


export const Dashboard = () => {
    const [{ profile, accessToken }] = useAuthContext();
    return (
        <>
            <Link to="companies">Firmy</Link>
        </>
        
    );
}

export default Dashboard;