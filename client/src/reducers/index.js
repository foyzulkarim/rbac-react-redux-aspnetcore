import { combineReducers } from "redux";
import PostsReducer from "./postsReducer";
import UserReducer from "./userReducer";
import ResourceReducer from "./resourceReducer";
import RoleReducer from "./roleReducer";
import PermissionReducer from "./permissionReducer";

const rootReducer = combineReducers({
    posts: PostsReducer,
    userContext: UserReducer,
    resourceContext: ResourceReducer,
    roleContext: RoleReducer,
    permissionContext: PermissionReducer
});

export default rootReducer;