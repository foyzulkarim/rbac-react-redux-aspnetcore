import { call, put, takeEvery, all } from "redux-saga/effects";
import * as api from './api';
import { Constants } from "../constants";

export function* add({ payload }) {
    try {
        let output = yield call(api.createResource, payload);
        yield put({ type: 'ADD_RESOURCE_SUCCESS', payload: output });
        yield put({ type: 'FETCH_RESOURCE' });
    } catch (error) {
        console.log('addPost error', error);
    }
}

function* watchAdd() {
    yield takeEvery('ADD_RESOURCE', add);
}

export function* fetch({ payload }) {
    try {
        let output = yield call(api.getResources, payload);
        yield put({ type: 'FETCH_RESOURCE_SUCCESS', payload: output });
    } catch (error) {
        console.log('fetch resource error', error);
    }
}

function* watchFetch() {
    yield takeEvery('FETCH_RESOURCE', fetch);
}

export default [
    watchAdd(),
    watchFetch()
];