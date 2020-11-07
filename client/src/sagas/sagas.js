import { call, put, takeEvery, all } from "redux-saga/effects";
import * as api from './api';
import { Constants } from "../constants";
import posts from "./postSaga";
import resources from "./resourceSaga";
import roles from "./roleSaga";


export default function* rootSaga() {
    let allSagas = [...posts, ...resources, ...roles];
    yield all(allSagas);
};