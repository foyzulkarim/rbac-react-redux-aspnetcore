import { Constants } from "../constants";
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
                postList: action.payload.data,
                notificationText: ''
            };
        case 'FETCH_RESOURCE_DETAIL_SUCCESS':
            return {
                ...state,
                selectedPost: action.payload.data
            };
        case 'ADD_RESOURCE_SUCCESS':
            return {
                ...state,
                notificationText: 'Comment added successfully'
            };
        case 'FETCH_RESOURCE_SUCCESS':
            if (action.payload.data == null) {
                action.payload.data = [];
            }

            return {
                ...state,
                selectedComments: action.payload.data,
                notificationText: ''
            }
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