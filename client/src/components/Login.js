import React, { useEffect} from 'react';
import { Redirect, useHistory } from "react-router-dom";
import { useForm } from 'react-hook-form';
import { useSelector, useDispatch } from 'react-redux';
import { Constants } from "../constants";

export const Login = () => {
    let history = useHistory();
    let dispatch = useDispatch();
    const { register, handleSubmit, errors } = useForm();

    let submitData = (data) => {
        dispatch({
            type: Constants.LOGIN_REQUEST, payload: data
        })
    }

    const onSubmit = data => {
        submitData(data);
    };

    const userContext = useSelector(state => {
        return state.userContext;
    });

    useEffect(() => {
        console.log('userContext', userContext);
        if (userContext.isSuccess)
            history.push('/');
    }, [userContext.isSuccess]);

    return (userContext.isAuthenticated) ? <Redirect to={{ pathname: '/' }} /> :
        <>
            <div className="col-md-6 col-md-offset-3">
                <h2>Login</h2>
                <form name="form" onSubmit={handleSubmit(onSubmit)}>
                    <div className="form-group">
                        <label htmlFor="username">Username</label>
                        <input type="text" className="form-control" name="username" ref={register({ required: true })} />
                        <span>{errors.username && 'Username is required'}</span>
                    </div>
                    <div className='form-group'>
                        <label htmlFor="password">Password</label>
                        <input type="password" className="form-control" name="password" ref={register({ required: true })} />
                        <span>{errors.password && 'Password is required'}</span>
                    </div>
                    <div className="form-group">
                        <input type="submit" className="btn btn-primary" />
                    </div>
                    <div className="form-group">
                        <label htmlFor="error"></label>                        
                        <span className="col-form-label text-danger">{userContext.error}</span>
                    </div>
                </form>
            </div>
        </>;
};