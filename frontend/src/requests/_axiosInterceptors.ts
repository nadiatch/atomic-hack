import {AxiosResponse} from "axios";

export function createInterceptorResponse() {
    return function interceptorResponse(response: AxiosResponse) {
        if (response.status === 401 || response.status === 403) {
            localStorage.removeItem("token");
            window.location.href="/";
        }
        return response;
    };
}

export function createInterceptorBeforeRequest() {
    return function interceptorBeforeRequest(request: any) {
        let token = localStorage.getItem('token');
        if (token) request.headers["Authorization"] = `Bearer ${token}`;
        return request;
    };
}
