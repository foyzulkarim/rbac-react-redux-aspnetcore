const initialState = {
    userList: [],
    selectedUser: {},
    shouldReload: false,
    isSuccess: false,
    errors: []
};


export default (state = initialState, action) => {

    switch (action.type) {
        case 'ADD_USER_SUCCESS':
            return {
                ...state,
                shouldReload: true,
                isSuccess: true,
                errors: []
            };
        case 'USER_ERROR':
            return {
                ...state,
                errors: action.payload.data,
                isSuccess: false,
                shouldReload: true,
            };
        case 'FETCH_USER_SUCCESS':
            return {
                ...state,
                shouldReload: false,
                userList: action.payload.data,
                isSuccess: false,
                errors: []
            };
        case 'FETCH_USER_DETAIL_SUCCESS':
            return {
                ...state,
                shouldReload: false,
                selectedUser: action.payload.data,
                errors: []
            }
        case 'EDIT_USER_SUCCESS':
            return {
                ...state,
                shouldReload: true,
                selectedUser: {},
                errors: [],
                isSuccess: true,
            };
        default:
            return state;
    }
}