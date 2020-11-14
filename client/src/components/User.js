import React from 'react';
import { useHistory, useParams, Link } from "react-router-dom";
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import { useSelector, useDispatch } from 'react-redux';
import { useEffect } from 'react';
import { Table, Row, Col } from 'react-bootstrap';

const UserSchema = Yup.object().shape({
    userName: Yup.string()
        .min(3, 'Too Short!')
        .max(64, 'Too Long!')
        .required('Hey input the value!'),
    password: Yup.string()
        .min(3, 'Too Short!')
        .max(64, 'Too Long!')
        .required('Hey input the value!')
});

export const UserCreate = () => {

    let history = useHistory();
    let dispatch = useDispatch();

    useEffect(() => {
      
    }, []);


    return (
        <div>
            <h1>User entry</h1>
            <Formik
                enableReinitialize={true}
                setFieldValue={(field, value) => console.log(field, value)}
                initialValues={{ firstName: '', lastName: '', userName: '', email: '', phoneNumber: '', password: '' }}
                validationSchema={UserSchema}
                onSubmit={(values, { setSubmitting }) => {
                    console.log(values);
                    dispatch({
                        type: "ADD_USER", payload: values
                    });
                    setSubmitting(false);
                    history.push('/home');
                }}
            >
                {({ isSubmitting }) => (
                    <Form>
                        <div className="form-group row">
                            <label htmlFor="firstName" className="col-sm-2 col-form-label">First Name</label>
                            <Field className="col-sm-8 col-form-label" type="text" name="firstName" />
                            <ErrorMessage className="col-sm-2 col-form-label text-danger" name="name" component="div" />
                        </div>
                        <div className="form-group row">
                            <label htmlFor="lastName" className="col-sm-2 col-form-label">Last Name</label>
                            <Field className="col-sm-8 col-form-label" type="text" name="lastName" />
                            <ErrorMessage className="col-sm-2 col-form-label text-danger" name="name" component="div" />
                        </div>
                        <div className="form-group row">
                            <label htmlFor="userName" className="col-sm-2 col-form-label">User Name</label>
                            <Field className="col-sm-8 col-form-label" type="text" name="userName" />
                            <ErrorMessage className="col-sm-2 col-form-label text-danger" name="userName" component="div" />
                        </div>
                        <div className="form-group row">
                            <label htmlFor="email" className="col-sm-2 col-form-label">Email</label>
                            <Field className="col-sm-8 col-form-label" type="text" name="email" />
                            <ErrorMessage className="col-sm-2 col-form-label text-danger" name="name" component="div" />
                        </div>
                        <div className="form-group row">
                            <label htmlFor="phoneNumber" className="col-sm-2 col-form-label">Phone Number</label>
                            <Field className="col-sm-8 col-form-label" type="text" name="phoneNumber" />
                            <ErrorMessage className="col-sm-2 col-form-label text-danger" name="phoneNumber" component="div" />
                        </div>
                        <div className="form-group row">
                            <label htmlFor="password" className="col-sm-2 col-form-label">Password</label>
                            <Field className="col-sm-8 col-form-label" type="password" name="password" />
                            <ErrorMessage className="col-sm-2 col-form-label text-danger" name="password" component="div" />
                        </div>
                        <div className="form-group row">
                            <label htmlFor="name" className="col-sm-2 col-form-label"></label>
                            <button type="submit" disabled={isSubmitting}>Submit</button>
                        </div>
                    </Form>
                )}
            </Formik>
        </div >
    );
};



export const UserList = () => {

    let dispatch = useDispatch();

    const usersContext = useSelector(state => {
        return state.usersContext;
    });

    useEffect(() => {
        if (usersContext.userList.length === 0 || usersContext.shouldReload) {
            dispatch({ type: "FETCH_USER" });
        }
    }, [usersContext.shouldReload]);

    return (
        <>
            <Row>
                <h2>User List</h2>
            </Row>
            <Row>
                <Col>
                    <Table striped bordered hover>
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Username</th>
                                <th>Phone Number</th>
                                <th>Name</th>
                                <th>Is active</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                usersContext.userList.map((user, index) => {
                                    return (
                                        <tr key={user.id}>
                                            <td>{index + 1}</td>
                                            <td>{user.userName}</td>
                                            <td>{user.phoneNumber}</td>
                                            <td>{user.firstName} {user.lastName}</td>
                                            <td>{user.isActive}</td>
                                            <td><Link to={`/user-edit/${user.id}`} className="">Edit</Link></td>
                                        </tr>
                                    )
                                })
                            }
                        </tbody>
                    </Table>
                </Col>
            </Row>
        </>
    );
}