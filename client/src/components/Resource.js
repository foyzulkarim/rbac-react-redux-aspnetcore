import React from 'react';
import { useHistory } from "react-router-dom";
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import { useSelector, useDispatch } from 'react-redux';
import { Table, Row, Col } from 'react-bootstrap';
import { useEffect } from 'react';

const ResourceSchema = Yup.object().shape({
    name: Yup.string()
        .min(3, 'Too Short!')
        .max(30, 'Too Long!')
        .required('Hey input the value!')
});

export const ResourceCreate = () => {

    let history = useHistory();
    let dispatch = useDispatch();

    return (
        <div>
            <h1>Resource entry</h1>
            <Formik
                initialValues={{ name: '' }}
                validationSchema={ResourceSchema}
                onSubmit={(values, { setSubmitting }) => {
                    dispatch({
                        type: "ADD_RESOURCE", payload: values
                    });
                    setSubmitting(false);
                    history.push('/resource-list');
                }}
            >
                {({ isSubmitting }) => (
                    <Form>
                        <div className="form-group row">
                            <label htmlFor="name" className="col-sm-2 col-form-label">Resource</label>
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

export const ResourceList = () => {

    let dispatch = useDispatch();

    const resourceContext = useSelector(state => {
        return state.resourceContext;
    });

    useEffect(() => {
        dispatch({ type: "FETCH_RESOURCE" });
    }, []);

    console.log("Resources", resourceContext.resourceList);

    return (
        <>
            <Row>
                <h2>Resource List {resourceContext.resourceList.length}</h2>
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
                                resourceContext.resourceList.map((resource, index) => {
                                    return (
                                        <tr>
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