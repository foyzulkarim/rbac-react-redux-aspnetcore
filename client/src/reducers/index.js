import { combineReducers } from "redux";
import PostsReducer from "./postsReducer";
import UserReducer from "./userReducer";

const rootReducer = combineReducers({
    posts: PostsReducer,
    userContext: UserReducer,
});

export default rootReducer;