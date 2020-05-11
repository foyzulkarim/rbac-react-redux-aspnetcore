import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Switch, Route, Link, Redirect, useRouteMatch, useParams, useHistory } from "react-router-dom";
import { useForm } from 'react-hook-form';
import { useSelector, useDispatch } from 'react-redux';
import { Constants } from "../constants";

export const Register = () => {
    let history = useHistory();
    let dispatch = useDispatch();
    const { register, handleSubmit, watch, errors, setError } = useForm();

    let submitData = (data) => {
        dispatch({
            type: Constants.REGISTER_REQUEST, payload: data
        });
    }

    const onSubmit = data => {
        if (data.password != data.confirmpassword) {
            setError("password", "notMatch", "Password and Confirm Password are mismatched");
            return;
        }

        submitData(data);
    };

    const userContext = useSelector(state => {
        return state.userContext;
    });

    let shouldRedirect = userContext.isAuthenticated || userContext.isRegistered;
    if (userContext.isRegistered) {
        history.push('/');
    }

    if (!userContext.isRegistered && userContext.error) {
        console.log(userContext.error);
    }

    return (shouldRedirect) ? <Redirect to={{ pathname: '/' }} /> :
        <>
            <div className="col-md-6 col-md-offset-3">
                <h2>Login</h2>
                {userContext.error &&
                    <div className="alert alert-danger  alert-dismissible fade show" role="alert">
                        {userContext.error.message}
                        <button type="button" className="close" data-dismiss="alert" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                }
                <form name="form" onSubmit={handleSubmit(onSubmit)}>
                    <div className="form-group">
                        <label htmlFor="email">Email</label>
                        <input type="text" className="form-control" name="email" ref={register({ required: true })} />
                        <span>{errors.email && 'Email is required'}</span>
                    </div>
                    <div className="form-group">
                        <label htmlFor="phone">Phone</label>
                        <input type="text" className="form-control" name="phone" ref={register({ required: true })} />
                        <span>{errors.phone && 'Phone is required'}</span>
                    </div>
                    <div className='form-group'>
                        <label htmlFor="password">Password</label>
                        <input type="password" className="form-control" name="password" ref={register({ required: true })} />
                        <span>{errors.password && <p>{errors.password.message}</p>}</span>
                    </div>
                    <div className='form-group'>
                        <label htmlFor="confirmpassword">Confirm password</label>
                        <input type="password" className="form-control" name="confirmpassword" ref={register({ required: true })} />
                        <span>{errors.confirmpassword && 'Confirm password is required'}</span>
                    </div>
                    <div className="form-group">
                        <input type="submit" className="btn btn-primary" />
                    </div>
                </form>
            </div>
        </>;
};