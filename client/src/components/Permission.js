import React from 'react';
import { useHistory } from "react-router-dom";
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import { useSelector, useDispatch } from 'react-redux';
import { useEffect } from 'react';
import { Table, Row, Col } from 'react-bootstrap';

const PermissionSchema = Yup.object().shape({
    resourceId: Yup.string()
        .min(3, 'Too Short!')
        .max(64, 'Too Long!')
        .required('Hey input the value!')
});

export const PermissionCreate = () => {

    let history = useHistory();
    let dispatch = useDispatch();

    const resourceContext = useSelector(state => {
        return state.resourceContext;
    });

    const roleContext = useSelector(state => {
        return state.roleContext;
    });


    useEffect(() => {
        dispatch({ type: "FETCH_RESOURCE" });
        dispatch({ type: "FETCH_ROLE" });
    }, []);


    return (
        <div>
            <h1>Resource entry</h1>
            <Formik
                setFieldValue={(field, value) => console.log(field, value)}
                initialValues={{ resourceId: '', roleId: '', isAllowed: false }}
                validationSchema={PermissionSchema}
                onSubmit={(values, { setSubmitting }) => {
                    console.log(values);
                    dispatch({
                        type: "ADD_PERMISSION", payload: values
                    });
                    setSubmitting(false);
                    history.push('/home');
                }}
            >
                {({ isSubmitting }) => (
                    <Form>
                        <div className="form-group row">
                            <label htmlFor="resourceId" className="col-sm-2 col-form-label">Resource</label>
                            <Field as="select" name="resourceId" className="col-sm-8">
                                <option value="" key=""></option>
                                {
                                    resourceContext.resourceList.map(({ id, name }) => {
                                        return <option value={id} key={id}>{name}</option>;
                                    })
                                }
                            </Field>
                            <ErrorMessage className="col-sm-2 col-form-label text-danger" name="name" component="div" />
                        </div>
                        <div className="form-group row">
                            <label htmlFor="roleId" className="col-sm-2 col-form-label">Roles</label>
                            <Field as="select" name="roleId" className="col-sm-8">
                                <option value="" key=""></option>
                                {
                                    roleContext.roleList.map(({ id, name }) => {
                                        return <option value={id} key={id}>{name}</option>;
                                    })
                                }
                            </Field>
                            <ErrorMessage className="col-sm-2 col-form-label text-danger" name="name" component="div" />
                        </div>
                        <div className="form-group row">
                            <label htmlFor="isAllowed" className="col-sm-2 col-form-label">Is allowed</label>
                            <label><Field type="checkbox" name="isAllowed" /></label>
                            <ErrorMessage className="col-sm-2 col-form-label text-danger" name="isAllowed" component="div" />
                        </div>
                        <div className="form-group row">
                            <label htmlFor="isDisabled" className="col-sm-2 col-form-label">Is disabled</label>
                            <label><Field type="checkbox" name="isDisabled" /></label>
                            <ErrorMessage className="col-sm-2 col-form-label text-danger" name="isDisabled" component="div" />
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


export const PermissionList = () => {

    let dispatch = useDispatch();

    const permissionContext = useSelector(state => {
        return state.permissionContext;
    });

    useEffect(() => {
        dispatch({ type: "FETCH_PERMISSION" });
    }, []);

    return (
        <>
            <Row>
                <h2>Permission List {permissionContext.permissionList.length}</h2>
            </Row>
            <Row>
                <Col>
                    <Table striped bordered hover>
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Resource</th>
                                <th>Role</th>
                                <th>Is allowed</th>
                                <th>Is disabled</th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                permissionContext.permissionList.map((resource, index) => {
                                    return (
                                        <tr>
                                            <td>{index + 1}</td>
                                            <td>{resource.resourceName}</td>
                                            <td>{resource.roleName}</td>
                                            <td>{resource.isAllowed.toString()}</td>
                                            <td>{resource.isDisabled.toString()}</td>
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