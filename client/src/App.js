import React, { } from 'react';
import './App.css';
import { BrowserRouter as Router, Switch, Route, Link, Redirect, useHistory } from "react-router-dom";
import { useSelector, useDispatch } from 'react-redux';
import { PostCreate, Posts, Home, PostDetail, PostEdit, PostDelete } from "./components/Posts";
import { Login } from "./components/Login";
import { Register } from "./components/Register";
import { Constants } from "./constants";
import { checkPermission } from "./utils/permissionManager.js";
import { ResourceCreate, ResourceList } from "./components/Resource";
import { PermissionCreate, PermissionList } from "./components/Permission";
import { RoleCreate, RoleList } from './components/Role';

export const PrivateRoute = ({ component: Component, name: resource, ...rest }) => {

  const userContext = useSelector(state => {
    return state.userContext;
  });

  const isAllowed = checkPermission(resource, userContext);

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
    let data = { jti: userContext.jti };
    dispatch({ type: Constants.LOGOUT_REQUEST, payload: data });
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
    { name: 'link-posts', url: '/posts', text: 'Posts', component: Posts, isRootMenu: true },
    { name: 'link-post-create', url: '/post-create', text: 'Create post', component: PostCreate, isRootMenu: true },
    { name: 'link-post-detail', url: '/post-detail/:id', text: 'Detail post', component: PostDetail },
    { name: 'link-post-edit', url: '/post-edit/:id', text: 'Edit post', component: PostEdit },
    { name: 'link-post-delete', url: '/post-delete/:id', text: 'Delete post', component: PostDelete },
    { name: 'link-permission-create', url: '/permission-create', text: 'Create permission', component: PermissionCreate, isRootMenu: true },
    { name: 'link-permission-list', url: '/permission-list', text: 'List permissions', component: PermissionList, isRootMenu: true },
    { name: 'link-role-create', url: '/role-create', text: 'Create role', component: RoleCreate, isRootMenu: true },
    { name: 'link-role-list', url: '/role-list', text: 'List role', component: RoleList, isRootMenu: true },
    { name: 'link-resource-create', url: '/resource-create', text: 'Create resource', component: ResourceCreate, isRootMenu: true },
    { name: 'link-resource-list', url: '/resource-list', text: 'List resource', component: ResourceList, isRootMenu: true },
  ];

  // let routes = [
  //   { path: '/resource-create', component: ResourceCreate },
  //   { path: '/resource-list', component: ResourceList }
  // ]

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
                      return checkPermission(link.name, userContext) && link.isRootMenu && <Link key={index} to={link.url} className="list-group-item list-group-item-action bg-light">{link.text}</Link>
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
                {/* <PrivateRoute path="/post-detail/:id" component={PostDetail}></PrivateRoute>
                <PrivateRoute path="/post-create" component={PostCreate}></PrivateRoute>
                <PrivateRoute path="/post-edit/:id" component={PostEdit}></PrivateRoute>
                <PrivateRoute path="/post-delete/:id" component={PostDelete}></PrivateRoute>
                <PrivateRoute path="/posts" component={Posts}></PrivateRoute> */}
                {/* <Route path="/permission-create" component={PermissionCreate}></Route>
                <Route path="/role-create" component={RoleCreate}></Route> */}
                {
                  links.map(route => {
                    return <PrivateRoute key={route.name} path={route.url} component={route.component} name={route.name}></PrivateRoute>;
                  })
                }
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