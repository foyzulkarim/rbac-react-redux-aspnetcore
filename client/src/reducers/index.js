import { combineReducers } from "redux";
import PostsReducer from "./postsReducer";
import UserReducer from "./userReducer";
import ResourceReducer from "./resourceReducer";
import RoleReducer from "./roleReducer";
import PermissionReducer from "./permissionReducer";
import UsersReducer from "./usersReducer";

const rootReducer = combineReducers({
    postContext: PostsReducer,
    userContext: UserReducer,
    usersContext: UsersReducer,
    resourceContext: ResourceReducer,
    roleContext: RoleReducer,
    permissionContext: PermissionReducer
});

export default rootReducer;