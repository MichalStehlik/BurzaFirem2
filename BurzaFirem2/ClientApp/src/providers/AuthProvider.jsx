import React, { createContext, useReducer, useContext, useEffect } from "react";

export const SET_ACCESS_TOKEN = "SET_ACCESS_TOKEN";
export const CLEAR_ACCESS_TOKEN = "CLEAR_ACCESS_TOKEN";

const TOKEN_KEY = "TOKEN_KEY";

const parseJwt = (token) => {
    const base64Url = token.split(".")[1];
    const base64 = base64Url.replace("-", "+").replace("_", "/");
    return JSON.parse(window.atob(base64));
};

const initialState = {
    accessToken: null,
    userId: null,
    profile: null
}

const reducer = (state, action) => {
    switch (action.type) {
        case SET_ACCESS_TOKEN:
            let tokenData = parseJwt(action.payload);
            return { ...state, accessToken: action.payload, userId: tokenData.sub, profile: tokenData }
        case CLEAR_ACCESS_TOKEN:
            return { ...state, accessToken: null, userId: null, profile: null }
        default: return state;
    }
}

export const AuthContext = createContext(initialState);
export const AuthConsumer = AuthContext.Consumer;
export const AuthProvider = props => {
    const store = useReducer(
        reducer,
        initialState
    );
    const [state, dispatch] = store;
    useEffect(()=>{
        let data = sessionStorage.getItem(TOKEN_KEY);
        if (data) {
            dispatch({type: SET_ACCESS_TOKEN, payload: data});
        }
    },[dispatch]);
    useEffect(()=>{
        if (state.accessToken === null) {
            sessionStorage.clear();
        }
        else
        {
            sessionStorage.setItem(TOKEN_KEY, state.accessToken);
        }     
    },[state]);
    return (
        <AuthContext.Provider value={store}>
            {props.children}
        </AuthContext.Provider>
    );
}

export const useAuthContext = () => useContext(AuthContext);