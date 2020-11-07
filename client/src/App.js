import React, { useState, useEffect } from 'react';
import './App.css';
import { BrowserRouter as Router, Switch, Route, Link, Redirect, useRouteMatch, useParams, useHistory } from "react-router-dom";
import { useForm } from 'react-hook-form';
import { useSelector, useDispatch } from 'react-redux';
import { PostDetail, PostCreate, PostEdit, PostDelete, Posts, Home } from "./components/Posts";
import { Login } from "./components/Login";
import { Register } from "./components/Register";
import { Constants } from "./constants";
import { usePermission, checkPermission } from "./hooks/usePermission.js";
import { ResourceCreate, ResourceList } from "./components/Resource";
import Permission from "./components/Permission";
import Role from './components/Role';

export const PrivateRoute = ({ component: Component, ...rest }) => {

  const userContext = useSelector(state => {
    return state.userContext;
  });

  const isAllowed = checkPermission(Component.name, userContext);

  return (
    <Route {...rest} render={props => {
      return (isAllowed)
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

  //console.log('userContext', userContext);

  let links = [
    { name: 'link-posts', url: '/posts', text: 'Posts' },
    { name: 'link-post-create', url: '/post-create', text: 'Create post' },
    { name: 'link-permission-create', url: '/permission-create', text: 'Create permission' },
    { name: 'link-role-create', url: '/role-create', text: 'Create role' },
    { name: 'link-resource-create', url: '/resource-create', text: 'Create resource' },
    { name: 'link-resource-list', url: '/resource-list', text: 'List resource' },
  ];

  let routes = [
    { path: '/resource-create', component: ResourceCreate },
    { path: '/resource-list', component: ResourceList }
  ]

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
                  {
                    links.map((link, index) => {
                      return <Link key={index} to={link.url} className="list-group-item list-group-item-action bg-light">{link.text}</Link>
                    })
                  }
                </>
              }
            </div>
          </div>

          <div id="page-content-wrapper">
            <nav className="navbar navbar-expand-lg navbar-light bg-light border-bottom">
              <Navigation />
            </nav>

            <div className="container">
              <Switch>
                <PrivateRoute path="/post-detail/:id" component={PostDetail}></PrivateRoute>
                <PrivateRoute path="/post-create" component={PostCreate}></PrivateRoute>
                <PrivateRoute path="/post-edit/:id" component={PostEdit}></PrivateRoute>
                <PrivateRoute path="/post-delete/:id" component={PostDelete}></PrivateRoute>
                <PrivateRoute path="/posts" component={Posts}></PrivateRoute>
                <Route path="/login" component={Login}></Route>
                <Route path="/register" component={Register}></Route>
                <Route path="/basic" component={ResourceCreate}></Route>
                <Route path="/permission-create" component={Permission}></Route>
                <Route path="/role-create" component={Role}></Route>
                {
                  routes.map(route => {
                    return <Route path={route.path} component={route.component}></Route>;
                  })
                }
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