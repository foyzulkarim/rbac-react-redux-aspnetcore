import { call, put, takeEvery, all } from "redux-saga/effects";
import * as api from './api';
import { Constants } from "../constants";

export function* addPost({ payload }) {
    try {
        let output = yield call(api.createPost, payload);
        yield put({ type: 'ADD_POST_SUCCESS', payload: output });
        yield put({ type: 'FETCH_POSTS' });
    } catch (error) {
        console.log('addPost error', error);
    }
}

function* watchAddPost() {
    yield takeEvery('ADD_POST', addPost);
}

export function* editPost({ payload }) {
    try {
        let output = yield call(api.editPost, payload);
        yield put({ type: 'EDIT_POST_SUCCESS', payload: output });
        yield put({ type: 'FETCH_POSTS' });
    } catch (error) {
        console.log('editPost error', error);
    }
}

function* watchEditPost() {
    yield takeEvery('EDIT_POST', editPost);
}

export function* deletePost({ payload }) {
    try {
        let output = yield call(api.deletePost, payload);
        yield put({ type: 'DELETE_POST_SUCCESS', payload: output });
        yield put({ type: 'FETCH_POSTS' });
    } catch (error) {
        console.log('deletePost error', error);
    }
}

function* watchDeletePost() {
    yield takeEvery('DELETE_POST', deletePost);
}

export function* fetchPosts() {
    try {
        const output = yield call(api.getPosts);
        yield put({ type: 'FETCH_POSTS_SUCCESS', payload: output });
    } catch (error) {
        console.log('fetch posts error', error);
    }
}

function* watchFetchPosts() {
    yield takeEvery('FETCH_POSTS', fetchPosts);
}

export function* fetchPostDetail({ payload }) {
    try {
        const output = yield call(api.getPostDetail, payload);
        yield put({ type: 'FETCH_POST_DETAIL_SUCCESS', payload: output });
    } catch (error) {
        console.log('fetch posts error', error);
    }
}

function* watchFetchPostDetail() {
    yield takeEvery('FETCH_POST_DETAIL', fetchPostDetail);
}

export function* addComment({ payload }) {
    try {
        const output = yield call(api.createComment, payload);
        yield put({ type: 'ADD_COMMENTS_SUCCESS', payload: output });
        yield put({ type: 'FETCH_COMMENTS', payload: payload.postId });
    } catch (error) {
        console.log('fetch posts error', error);
    }
}

function* watchAddComment() {
    yield takeEvery('ADD_COMMENT', addComment);
}

export function* fetchComments({ payload }) {
    try {
        const output = yield call(api.getComments, payload);
        yield put({ type: 'FETCH_COMMENTS_SUCCESS', payload: output });
    } catch (error) {
        console.log('fetch posts error', error);
    }
}

function* watchFetchComments() {
    yield takeEvery('FETCH_COMMENTS', fetchComments);
}

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


export default function* rootSaga() {
    yield all([
        watchAddPost(),
        watchEditPost(),
        watchDeletePost(),
        watchFetchPosts(),
        watchFetchPostDetail(),
        watchAddComment(),
        watchFetchComments(),
        watchLogin(),
        watchRegister()
    ]);
};