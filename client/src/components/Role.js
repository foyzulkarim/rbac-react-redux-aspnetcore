import React from 'react';
import { useHistory } from "react-router-dom";
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import { useDispatch } from 'react-redux';

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
