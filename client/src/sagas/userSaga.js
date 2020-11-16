import { call, put, takeEvery } from "redux-saga/effects";
import * as api from './api';

export function* add({ payload }) {
    try {
        let output = yield call(api.createUser, payload);
        yield put({ type: 'ADD_USER_SUCCESS', payload: output });
    } catch (error) {
        console.log('add user error', error);
        yield put({ type: 'USER_ERROR', payload: error });
    }
}

function* watchAdd() {
    yield takeEvery('ADD_USER', add);
}

export function* edit({ payload }) {
    try {
        let output = yield call(api.editUser, payload);
        yield put({ type: 'EDIT_USER_SUCCESS', payload: output });     
    } catch (error) {
        console.log('edit user error', error);
        yield put({ type: 'USER_ERROR', payload: error });
    }
}

function* watchEdit() {
    yield takeEvery('EDIT_USER', edit);
}

export function* fetch({ payload }) {
    try {
        let output = yield call(api.getUsers, payload);
        yield put({ type: 'FETCH_USER_SUCCESS', payload: output });
    } catch (error) {
        console.log('fetch USER error', error);
    }
}

function* watchFetch() {
    yield takeEvery('FETCH_USER', fetch);
}

export function* fetchDetail({ payload }) {
    try {
        const output = yield call(api.getUserDetail, payload);
        yield put({ type: 'FETCH_USER_DETAIL_SUCCESS', payload: output });
    } catch (error) {
        console.log('fetch user error', error);
    }
}

function* watchFetchDetail() {
    yield takeEvery('FETCH_USER_DETAIL', fetchDetail);
}

export default [
    watchAdd(),
    watchFetch(),
    watchFetchDetail(),
    watchEdit()
];