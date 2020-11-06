import React from 'react';
import { BrowserRouter as Router, Switch, Route, Link, useRouteMatch, useParams, useHistory } from "react-router-dom";
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import { useSelector, useDispatch } from 'react-redux';

const ResourceSchema = Yup.object().shape({
    name: Yup.string()
        .min(3, 'Too Short!')
        .max(30, 'Too Long!')
        .required('Hey input the value!')
});

const Basic = () => {

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
                    history.push('/home');
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


export default Basic;