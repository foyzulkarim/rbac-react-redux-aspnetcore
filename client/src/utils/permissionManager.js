export const checkPermission = (resource, userContext) => {
    console.log('checkPermission', resource, userContext.resources);
    return userContext.isAuthenticated && userContext.resources.includes(resource);
}