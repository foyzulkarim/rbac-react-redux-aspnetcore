import { all } from "redux-saga/effects";
import posts from "./postSaga";
import resources from "./resourceSaga";
import roles from "./roleSaga";
import permissions from "./permissionSaga";

export default function* rootSaga() {
    let allSagas = [...posts, ...resources, ...roles, ...permissions];
    yield all(allSagas);
};