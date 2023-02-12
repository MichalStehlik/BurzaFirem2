import React from 'react';
import {useAuthContext} from "../providers/AuthProvider";
import { Alert, Container } from 'reactstrap'

export const requireAdmin = (WrappedComponent) => props  => {
    const [{accessToken, profile}] = useAuthContext();
    if (accessToken === null) {
        return (
            <Container>
                <Alert variant="danger">Sem nemáte přístup bez přihlášení.</Alert>
            </Container>        
        );
    } else if (profile.admin !== "1") {
        return(
            <Container>
                <Alert variant="danger">Na tohle nemáte oprávnění.</Alert>
            </Container>  
        );
    } else {
        return(
            <WrappedComponent {...props}>
                {props.children}
            </WrappedComponent>
        );        
    }
    
}

export default requireAdmin;