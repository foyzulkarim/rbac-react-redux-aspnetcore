const initialState = {
    resourceList: [],
    selectedResource: {},
    notificationText: ''
};


export default (state = initialState, action) => {

    switch (action.type) {

        case 'ADD_RESOURCE_SUCCESS':
            return {
                ...state,
                notificationText: 'Resource added successfully'
            };
        case 'FETCH_RESOURCE_SUCCESS':
            return {
                ...state,
                resourceList: action.payload.data,
                notificationText: ''
            };
        case 'FETCH_RESOURCE_DETAIL_SUCCESS':
            return {
                ...state,
                selectedPost: action.payload.data
            };
        case 'CLEAR_SELECTION':
            return {
                ...state,
                selectedPost: initialState.selectedPost,
                selectedComments: initialState.selectedComments,
            };
        default:
            return state;
    }
}