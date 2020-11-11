import { call, put, takeEvery } from "redux-saga/effects";
import * as api from './api';
import { Constants } from "../constants";

export function* login({ payload }) {
    try {
        let output = yield call(api.login, payload);
        yield put({ type: 'LOGIN_REQUEST_SUCCESS', payload: output });
    } catch (error) {
        console.log('login error', error);
    }
}

function* watchLogin() {
    yield takeEvery('LOGIN_REQUEST', login);
}

export function* logout({ payload }) {
    try {
        let output = yield call(api.logout, payload);
        yield put({ type: 'LOGOUT_REQUEST_SUCCESS', payload: output });
    } catch (error) {
        console.log('login error', error);
    }
}

function* watchLogout() {
    yield takeEvery('LOGOUT_REQUEST', logout);
}

export function* register({ payload }) {
    try {
        let output = yield call(api.register, payload);
        yield put({ type: Constants.REGISTER_SUCCESS, payload: output });
    } catch (error) {
        console.log('register error', error);
        yield put({ type: Constants.REGISTER_FAILURE, payload: error });
    }
}

function* watchRegister() {
    yield takeEvery(Constants.REGISTER_REQUEST, register);
}

export default [
    watchLogin(),
    watchLogout(),
    watchRegister()
];