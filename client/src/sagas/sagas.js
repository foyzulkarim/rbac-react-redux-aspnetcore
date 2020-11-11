import { all } from "redux-saga/effects";
import posts from "./postSaga";
import logins from "./loginSaga";
import resources from "./resourceSaga";
import roles from "./roleSaga";
import permissions from "./permissionSaga";

export default function* rootSaga() {
    let allSagas = [...posts, ...logins, ...resources, ...roles, ...permissions];
    yield all(allSagas);
};