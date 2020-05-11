import React, { useState, useEffect } from 'react';
import './App.css';
import { BrowserRouter as Router, Switch, Route, Link, Redirect, useRouteMatch, useParams, useHistory } from "react-router-dom";
import { useForm } from 'react-hook-form';
import { useSelector, useDispatch } from 'react-redux';
import { PostDetail, PostCreate, PostEdit, PostDelete, Posts, Home } from "./components/Posts";
import { Login } from "./components/Login";
import { Register } from "./components/Register";
import { Constants } from "./constants";

export const PrivateRoute = ({ component: Component, ...rest }) => {

  const userContext = useSelector(state => {
    return state.userContext;
  });

  return (
    <Route {...rest} render={props => {
      return (userContext.isAuthenticated)
        ? <Component {...props} />
        : <Redirect to={{ pathname: '/login', state: { from: props.location } }} />;
    }} />
  )
}

export const Navigation = () => {

  const userContext = useSelector(state => {
    return state.userContext;
  });

  let dispatch = useDispatch();
  let history = useHistory();

  const logOut = () => {
    dispatch({ type: Constants.LOGOUT_REQUEST });
  }

  let login = () => {
    history.push('/login');
  }

  let register = () => {
    history.push('/register');
  }

  return (
    <>
      <h3>hello {userContext.user && <><span>{userContext.user.username}</span></>}</h3>
      {userContext.isAuthenticated && <button onClick={logOut}>Log Out</button>}
      {!userContext.isAuthenticated &&
        <>
          <button onClick={login}>Login</button> <br />
          <button onClick={register}>Register</button>
        </>
      }
    </>
  );
}


const App = () => {

  const userContext = useSelector(state => {
    return state.userContext;
  });

  return (
    <Router>
      <div className="App">
        <div className="d-flex" id="wrapper">
          <div className="bg-light border-right" id="sidebar-wrapper">
            <div className="sidebar-heading">Code with me</div>
            <div className="list-group list-group-flush">
              <Link to="/" className="list-group-item list-group-item-action bg-light">Home</Link>
              {userContext.isAuthenticated &&
                <>
                  <Link to="/posts" className="list-group-item list-group-item-action bg-light">Posts</Link>
                  <Link to="/post-create" className="list-group-item list-group-item-action bg-light">Create Post</Link>
                </>
              }
            </div>
          </div>

          <div id="page-content-wrapper">
            <nav className="navbar navbar-expand-lg navbar-light bg-light border-bottom">
              <Navigation />
            </nav>

            <div className="container-fluid">
              <Switch>
                <PrivateRoute path="/post-detail/:id" component={PostDetail}></PrivateRoute>
                <PrivateRoute path="/post-create" component={PostCreate}></PrivateRoute>
                <PrivateRoute path="/post-edit/:id" component={PostEdit}></PrivateRoute>
                <PrivateRoute path="/post-delete/:id" component={PostDelete}></PrivateRoute>
                <PrivateRoute path="/posts" component={Posts}></PrivateRoute>
                <Route path="/login" component={Login}></Route>
                <Route path="/register" component={Register}></Route>
                <Route path="/"><Home /></Route>
              </Switch>
            </div>
          </div>
        </div>
      </div>
    </Router>
  );
}

export default App;