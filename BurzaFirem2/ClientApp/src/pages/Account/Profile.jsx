import { useAuthContext } from "../../providers/AuthProvider";

export const Profile = () => {
    const [{ profile, accessToken }] = useAuthContext();
    return (
        <>
            <p>Profile</p>
            <pre>{accessToken}</pre>
            <pre>{JSON.stringify(profile," ",4)}</pre>
        </>
        
    );
}

export default Profile;