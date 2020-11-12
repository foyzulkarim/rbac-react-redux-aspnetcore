const initialState = {
    roleList: [],
    selectedRole: {},
    notificationText: ''
};


export default (state = initialState, action) => {

    switch (action.type) {
        case 'ADD_ROLE_SUCCESS':
            return {
                ...state,
                notificationText: 'ROLE added successfully'
            };
        case 'FETCH_ROLE_SUCCESS':
            return {
                ...state,
                roleList: action.payload.data,
                notificationText: ''
            };
        default:
            return state;
    }
}