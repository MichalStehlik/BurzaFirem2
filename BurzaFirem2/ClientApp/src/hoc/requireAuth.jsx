import React from 'react';
import {useAuthContext} from "../providers/AuthProvider";
import { Alert, Container } from 'reactstrap'

export const requireAuth = (WrappedComponent) => props  => {
    const [{accessToken}] = useAuthContext();
    if (accessToken === null) {
        return (
            <Container>
                <Alert variant="info">Musíte se přihlásit.</Alert>
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

export default requireAuth;