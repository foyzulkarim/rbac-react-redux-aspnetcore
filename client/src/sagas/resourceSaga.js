import { call, put, takeEvery, all } from "redux-saga/effects";
import * as api from './api';
import { Constants } from "../constants";

export function* add({ payload }) {
    try {
        let output = yield call(api.createResource, payload);
        yield put({ type: 'ADD_POST_SUCCESS', payload: output });
        yield put({ type: 'FETCH_POSTS' });
    } catch (error) {
        console.log('addPost error', error);
    }
}

function* watchAdd() {
    yield takeEvery('ADD_RESOURCE', add);
}

export default [
    watchAdd()
];