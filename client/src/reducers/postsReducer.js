const initialState = {
    postList: [],
    selectedPost: {},
    selectedComments: [],
    notificationText: ''
};

export default (state = initialState, action) => {

    switch (action.type) {

        case 'ADD_POST_SUCCESS':
            return {
                ...state,
                notificationText: 'Post added successfully'
            };
        case 'FETCH_POSTS_SUCCESS':
            return {
                ...state,
                postList: action.payload.data,
                notificationText: ''
            };
        case 'FETCH_POST_DETAIL_SUCCESS':
            return {
                ...state,
                selectedPost: action.payload.data
            };
        case 'ADD_COMMENT_SUCCESS':
            return {
                ...state,
                notificationText: 'Comment added successfully'
            };
        case 'FETCH_COMMENTS_SUCCESS':
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