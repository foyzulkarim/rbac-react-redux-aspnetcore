import { call, put, takeEvery, all } from "redux-saga/effects";
import * as api from './api';
import { Constants } from "../constants";

export function* add({ payload }) {
    try {
        let output = yield call(api.createRole, payload);
        yield put({ type: 'ADD_ROLE_SUCCESS', payload: output });
        yield put({ type: 'FETCH_ROLE' });
    } catch (error) {
        console.log('addPost error', error);
    }
}

function* watchAdd() {
    yield takeEvery('ADD_ROLE', add);
}

export function* fetch({ payload }) {
    try {
        let output = yield call(api.getRoles, payload);
        yield put({ type: 'FETCH_ROLE_SUCCESS', payload: output });
    } catch (error) {
        console.log('fetch ROLE error', error);
    }
}

function* watchFetch() {
    yield takeEvery('FETCH_ROLE', fetch);
}

export default [
    watchAdd(),
    watchFetch()
];