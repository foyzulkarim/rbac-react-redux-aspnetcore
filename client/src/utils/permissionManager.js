const getElement = (resource, userContext) => {
    return userContext.resources
        && userContext.resources.length > 0
        && userContext.resources.find(element => element.name === resource);
}

export const checkPermission = (resource, userContext) => {
    console.log('checkPermission', resource, userContext.resources);
    const element = getElement(resource, userContext);
    return userContext.isAuthenticated && element != null && element.isAllowed;
}

export const checkIsDisabled = (resource, userContext) => {
    console.log('isDisabled', resource, userContext.resources);
    const element = getElement(resource, userContext);
    return userContext.isAuthenticated && element != null && element.isDisabled;
}