import React, { createContext, useReducer, useContext } from "react";

export const SET_MODE = "SET_MODE";
export const SET_CONFIG = "SET_CONFIG";
export const MODE_LIGHT = "MODE_LIGHT";
export const MODE_DARK = "MODE_DARK";

const initialState = {
    mode: MODE_LIGHT,
    config: {name: "Burza firem"}
}

const reducer = (state, action) => {
    switch (action.type) {
        case SET_MODE:
            return { ...state, mode: action.payload }
        case SET_CONFIG:
            return { ...state, config: action.payload }
        default: return state;
    }
}

export const AppContext = createContext(initialState);
export const AppConsumer = AppContext.Consumer;
export const AppProvider = props => {
    const store = useReducer(
        reducer,
        initialState
    );
    return (
        <AppContext.Provider value={store}>
            {props.children}
        </AppContext.Provider>
    );
}

export const useAppContext = () => useContext(AppContext);