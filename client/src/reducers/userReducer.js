import { Constants } from "../constants";

const initialState = {
    isAuthenticated: false,
    user: null,
    token: null,
    isRegistered: false,
    error: null,
    role: null,
    resources: null
}

export default (state = initialState, action) => {
    switch (action.type) {
        case Constants.LOGIN_SUCCESS:
            const data = action.payload.data;
            localStorage.setItem('data', JSON.stringify(data));
            return {
                ...state,
                isAuthenticated: true,
                user: { username: data.userName },
                token: data.access_token,
                role: data.role,
                resources: data.resources
            };
        case Constants.LOGOUT_REQUEST:
            localStorage.removeItem('data');
            return {
                isAuthenticated: initialState.isAuthenticated,
                user: initialState.user,
                token: initialState.token,
            };
        case Constants.PERMISSION_SUCCESS:
            return {
                ...state,
                resources: data.resources
            };
        case Constants.REGISTER_SUCCESS:
            console.log('REGISTER_SUCCESS', action.payload);
            return {
                ...state,
                isRegistered: true,
            };
        case Constants.REGISTER_FAILURE:
            const error = action.payload;
            let message = JSON.stringify(error.data);
            return {
                ...state,
                isRegistered: false,
                error: { status: error.status, message: message }
            };
        default:
            let localStorageData = localStorage.getItem('data');
            if (localStorageData) {
                localStorageData = JSON.parse(localStorageData);
                return {
                    ...state,
                    isAuthenticated: true,
                    user: { username: localStorageData.userName },
                    token: localStorageData.access_token,
                }
            }

            return state;
    }
}