import axios from 'axios';

const BaseUrl = 'https://localhost:5003/api';
const AuthUrl = 'https://localhost:5001';


axios.interceptors.request.use(function (config) {
    let localStorageData = localStorage.getItem('data');
    if (localStorageData) {
        localStorageData = JSON.parse(localStorageData);
        let token = 'Bearer ' + localStorageData.access_token;
        config.headers.Authorization = token;
    }
    console.log(config);
    return config;
});


axios.interceptors.response.use(function (response) {
    return response;
}, function (error) {
    if (error.response.status === 401) {
        localStorage.removeItem('data');
        window.location = '/login';
    } else {
        if (error.response.status === 403) {
            window.location = '/';
        }

        return Promise.reject(error.response);
    }
});

export const getPosts = () => {
    console.log("getPosts api call.");
    console.log(axios.defaults);
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

export const logout = (data) => {
    console.log("logout api call ->", data);
    return axios.post(`${AuthUrl}/api/logout`, data);
}

export const register = (data) => {
    console.log("register api call ->", data);
    return axios.post(`${AuthUrl}/api/user/register`, data);
}

export const createResource = (data) => {
    console.log("createResource api call ->", data);
    return axios.post(`${AuthUrl}/api/ApplicationResources`, data);
}

export const getResources = () => {
    console.log("getResources api call ->");
    return axios.get(`${AuthUrl}/api/ApplicationResources`);
}

export const createRole = (data) => {
    console.log("createRole api call ->", data);
    return axios.post(`${AuthUrl}/api/ApplicationRoles`, data);
}

export const getRoles = () => {
    console.log("getRoles api call ->");
    return axios.get(`${AuthUrl}/api/ApplicationRoles`);
}

export const createPermission = (data) => {
    console.log("createPermission api call ->", data);
    return axios.post(`${AuthUrl}/api/ApplicationPermissions`, data);
}

export const editPermission = (data) => {
    console.log("editPermission api call ->", data);
    return axios.put(`${AuthUrl}/api/ApplicationPermissions/${data.id}`, data);
}

export const getPermissions = () => {
    console.log("getPermissions api call ->");
    return axios.get(`${AuthUrl}/api/ApplicationPermissions`);
}

export const getPermissionDetail = (id) => {
    console.log("getPermissionDetail api call ->", id);
    return axios.get(`${AuthUrl}/api/ApplicationPermissions/${id}`);
}

export const getUsers = () => {
    console.log("getUsers api call ->");
    return axios.get(`${AuthUrl}/api/Users`);
}

export const createUser = (data) => {
    console.log("createUser api call ->", data);
    return axios.post(`${AuthUrl}/api/Users`, data);
}

export const editUser = (data) => {
    console.log("User api call ->", data);
    return axios.put(`${AuthUrl}/api/Users/${data.id}`, data);
}

export const getUserDetail = (id) => {
    console.log("getUserDetail api call ->", id);
    return axios.get(`${AuthUrl}/api/Users/${id}`);
}