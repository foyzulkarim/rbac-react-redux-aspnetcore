const initialState = {
    permissionList: []    
};


export default (state = initialState, action) => {

    switch (action.type) {
        case 'ADD_PERMISSION_SUCCESS':
            return {
                ...state                
            };
        case 'FETCH_PERMISSION_SUCCESS':
            return {
                ...state,
                permissionList: action.payload.data
            };
        default:
            return state;
    }
}