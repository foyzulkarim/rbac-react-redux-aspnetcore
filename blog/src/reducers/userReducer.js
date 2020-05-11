import { Constants } from "../constants";

const initialState = {
    isAuthenticated: false,
    user: {},
    token: '',
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
            };
        case Constants.LOGOUT_REQUEST:
            localStorage.removeItem('data');
            return {
                isAuthenticated: initialState.isAuthenticated,
                user: initialState.user,
                token: initialState.token,
            };
        default:
            let localStorageData = localStorage.getItem('data');
            console.log('localStorageData', localStorageData);
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