import { useState, useEffect } from "react";
import { useSelector, useDispatch } from 'react-redux';

export const usePermission = (name) => {
    const [isAllowed, setIsAllowed] = useState(false);

    const userContext = useSelector(state => {
        return state.userContext;
    });

    const isOk = () => {
        const ok = userContext.isAuthenticated && userContext.role != null;
        console.log('isOk', ok);
        return ok;
    };

    setIsAllowed(isOk());

    return isAllowed;
}

export const checkPermission = (name, userContext) => {
    const ok = userContext.isAuthenticated;
    console.log('checkPermission', name, userContext, ok);
    return ok;
}