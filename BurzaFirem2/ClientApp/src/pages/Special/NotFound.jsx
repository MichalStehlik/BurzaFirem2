import { useNavigate } from 'react-router-dom'

export const NotFound = () => {
    const navigate = useNavigate();
    return (
        <>
            <h1>404</h1>
            <div><button onClick={e => {navigate(-1)}}>Back</button></div>
        </>       
    );
}

export default NotFound;