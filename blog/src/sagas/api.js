import axios from 'axios';
// const BaseUrl = 'http://localhost:61361/api';
const BaseUrl = 'http://feli-api.azurewebsites.net/api';
const AuthUrl = 'http://localhost:5000'

export const getPosts = () => {
    console.log("getPosts api call.");
    return axios.get(`${BaseUrl}/posts`);
}

export const createPost = (data) => {
    console.log("createPost api call ->", data);
    return axios.post(`${BaseUrl}/posts`, data);
}

export const editPost = (data) => {
    console.log("editPost api call ->", data);
    return axios.put(`${BaseUrl}/posts`, data);
}

export const deletePost = (id) => {
    console.log("deletePost api call ->", id);
    return axios.delete(`${BaseUrl}/posts/${id}`, id);
}

export const getPostDetail = (id) => {
    console.log("getPostDetail api call ->", id);
    return axios.get(`${BaseUrl}/posts/${id}`);
}

export const createComment = ({ postId, data }) => {
    console.log("createComment api call ->", postId, data);
    return axios.post(`${BaseUrl}/comments?postId=${postId}`, data);
}

export const getComments = (postId) => {
    console.log("getComments api call ->", postId);
    return axios.get(`${BaseUrl}/comments?postId=${postId}`);
}


export const login = (data) => {
    console.log("login api call ->", data);
    return axios.post(`${AuthUrl}/api/token`, data);
}