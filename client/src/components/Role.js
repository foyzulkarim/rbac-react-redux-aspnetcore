import React, { useEffect } from 'react';
import { useHistory } from "react-router-dom";
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import { useSelector, useDispatch } from 'react-redux';
import { Table, Row, Col } from 'react-bootstrap';

const RoleSchema = Yup.object().shape({
    name: Yup.string()
        .min(3, 'Too Short!')
        .max(30, 'Too Long!')
        .required('Hey input the value!')
});

export const RoleCreate = () => {

    let history = useHistory();
    let dispatch = useDispatch();

    return (
        <div>
            <h1>Role entry</h1>
            <Formik
                initialValues={{ name: '' }}
                validationSchema={RoleSchema}
                onSubmit={(values, { setSubmitting }) => {
                    dispatch({
                        type: "ADD_ROLE", payload: values
                    });
                    setSubmitting(false);
                    history.push('/home');
                }}
            >
                {({ isSubmitting }) => (
                    <Form>
                        <div className="form-group row">
                            <label htmlFor="name" className="col-sm-2 col-form-label">Role</label>
                            <Field className="col-sm-8 col-form-label" type="text" name="name" />
                            <ErrorMessage className="col-sm-2 col-form-label text-danger" name="name" component="div" />
                        </div>

                        <div className="form-group row">
                            <label htmlFor="name" className="col-sm-2 col-form-label"></label>
                            <button type="submit" disabled={isSubmitting}>Submit</button>
                        </div>
                    </Form>
                )}
            </Formik>
        </div>
    );
};

export const RoleList = () => {

    let dispatch = useDispatch();

    const roleContext = useSelector(state => {
        return state.roleContext;
    });

    useEffect(() => {
        dispatch({ type: "FETCH_ROLE" });
    }, []);

    console.log("Resources", roleContext.roleList);

    return (
        <>
            <Row>
                <h2>Role List {roleContext.roleList.length}</h2>
            </Row>
            <Row>
                <Col>
                    <Table striped bordered hover>
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Name</th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                roleContext.roleList.map((resource, index) => {
                                    return (
                                        <tr key={index}>
                                            <td>{index + 1}</td>
                                            <td>{resource.name}</td>
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