const initialState = {
    permissionList: [],
    selectedPermission: {},
    shouldReload: false
};


export default (state = initialState, action) => {

    switch (action.type) {
        case 'ADD_PERMISSION_SUCCESS':
            return {
                ...state,
                shouldReload: true,
            };
        case 'FETCH_PERMISSION_SUCCESS':
            return {
                ...state,
                shouldReload: false,
                permissionList: action.payload.data
            };
        case 'FETCH_PERMISSION_DETAIL_SUCCESS':
            return {
                ...state,
                shouldReload: false,
                selectedPermission: action.payload.data
            }
        case 'EDIT_PERMISSION_SUCCESS':
            return {
                ...state,
                shouldReload: true,
                selectedPermission: {}
            };
        default:
            return state;
    }
}