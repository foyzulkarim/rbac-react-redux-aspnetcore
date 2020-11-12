import { call, put, takeEvery } from "redux-saga/effects";
import * as api from './api';

export function* add({ payload }) {
    try {
        let output = yield call(api.createPermission, payload);
        yield put({ type: 'ADD_PERMISSION_SUCCESS', payload: output });
        yield put({ type: 'FETCH_PERMISSION' });
    } catch (error) {
        console.log('add permission error', error);
    }
}

function* watchAdd() {
    yield takeEvery('ADD_PERMISSION', add);
}

export function* fetch({ payload }) {
    try {
        let output = yield call(api.getPermissions, payload);
        yield put({ type: 'FETCH_PERMISSION_SUCCESS', payload: output });
    } catch (error) {
        console.log('fetch PERMISSION error', error);
    }
}

function* watchFetch() {
    yield takeEvery('FETCH_PERMISSION', fetch);
}

export default [
    watchAdd(),
    watchFetch()
];