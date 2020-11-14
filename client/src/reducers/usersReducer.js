const initialState = {
    userList: [],
    selectedUser: {},
    shouldReload: false
};


export default (state = initialState, action) => {

    switch (action.type) {
        case 'ADD_USER_SUCCESS':
            return {
                ...state,
                shouldReload: true,
            };
        case 'FETCH_USER_SUCCESS':
            return {
                ...state,
                shouldReload: false,
                userList: action.payload.data
            };
        case 'FETCH_USER_DETAIL_SUCCESS':
            return {
                ...state,
                shouldReload: false,
                selectedUser: action.payload.data
            }
        case 'EDIT_USER_SUCCESS':
            return {
                ...state,
                shouldReload: true,
                selectedUser: {}
            };
        default:
            return state;
    }
}