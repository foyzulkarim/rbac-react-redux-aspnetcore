import { combineReducers } from "redux";
import PostsReducer from "./postsReducer";
import UserReducer from "./userReducer";
import ResourceReducer from "./resourceReducer";
import RoleReducer from "./roleReducer";

const rootReducer = combineReducers({
    posts: PostsReducer,
    userContext: UserReducer,
    resourceContext: ResourceReducer,
    roleContext: RoleReducer
});

export default rootReducer;