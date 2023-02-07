import { useAuthContext } from "../../providers/AuthProvider"
import {Link} from "react-router-dom"


export const Dashboard = () => {
    const [{ profile, accessToken }] = useAuthContext();
    return (
        <>
            <p>Profile</p>
            <pre>{accessToken}</pre>
            <pre>{JSON.stringify(profile," ",4)}</pre>
            <Link to="companies">Firmy</Link>
        </>
        
    );
}

export default Dashboard;