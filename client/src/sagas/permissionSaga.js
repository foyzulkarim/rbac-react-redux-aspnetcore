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

export function* edit({ payload }) {
    try {
        let output = yield call(api.editPermission, payload);
        yield put({ type: 'EDIT_PERMISSION_SUCCESS', payload: output });
        //yield put({ type: 'FETCH_PERMISSION' });
    } catch (error) {
        console.log('add permission error', error);
    }
}

function* watchEdit() {
    yield takeEvery('EDIT_PERMISSION', edit);
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

export function* fetchDetail({ payload }) {
    try {
        const output = yield call(api.getPermissionDetail, payload);
        yield put({ type: 'FETCH_PERMISSION_DETAIL_SUCCESS', payload: output });
    } catch (error) {
        console.log('fetch permission error', error);
    }
}

function* watchFetchDetail() {
    yield takeEvery('FETCH_PERMISSION_DETAIL', fetchDetail);
}

export default [
    watchAdd(),
    watchFetch(),
    watchFetchDetail(),
    watchEdit()
];