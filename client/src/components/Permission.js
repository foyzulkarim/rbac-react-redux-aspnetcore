import React from 'react';
import { useHistory, useParams, Link } from "react-router-dom";
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import { useSelector, useDispatch } from 'react-redux';
import { useEffect } from 'react';
import { Table, Row, Col } from 'react-bootstrap';

const PermissionSchema = Yup.object().shape({
    resourceId: Yup.string()
        .min(3, 'Too Short!')
        .max(64, 'Too Long!')
        .required('Hey input the value!'),
    roleId: Yup.string()
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


export const PermissionEdit = () => {

    let history = useHistory();
    let dispatch = useDispatch();
    let { id } = useParams();

    const resourceContext = useSelector(state => {
        return state.resourceContext;
    });

    const roleContext = useSelector(state => {
        return state.roleContext;
    });

    const permissionContext = useSelector(state => {
        return state.permissionContext;
    })

    useEffect(() => {
        dispatch({ type: "FETCH_RESOURCE" });
        dispatch({ type: "FETCH_ROLE" });
        dispatch({ type: "FETCH_PERMISSION_DETAIL", payload: id });
    }, []);


    return (
        <div>
            <h1>Permission edit</h1>
            <Formik
                enableReinitialize={true}
                setFieldValue={(field, value) => console.log(field, value)}
                initialValues={{
                    resourceId: permissionContext.selectedPermission.resourceId,
                    roleId: permissionContext.selectedPermission.roleId,
                    isAllowed: permissionContext.selectedPermission.isAllowed
                }}
                validationSchema={PermissionSchema}
                onSubmit={(values, { setSubmitting }) => {
                    values.id = id;
                    console.log('EDIT_PERMISSION', values);
                    dispatch({
                        type: "EDIT_PERMISSION", payload: values
                    });
                    setSubmitting(false);
                    history.push('/permission-list');
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
        if (permissionContext.permissionList.length === 0 || permissionContext.shouldReload) {
            dispatch({ type: "FETCH_PERMISSION" });
        }
    }, [permissionContext.shouldReload]);

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
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                permissionContext.permissionList.map((permission, index) => {
                                    return (
                                        <tr key={permission.id}>
                                            <td>{index + 1}</td>
                                            <td>{permission.resourceName}</td>
                                            <td>{permission.roleName}</td>
                                            <td>{permission.isAllowed.toString()}</td>
                                            <td>{permission.isDisabled.toString()}</td>
                                            <td><Link to={`/permission-edit/${permission.id}`} className="">Edit</Link></td>
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