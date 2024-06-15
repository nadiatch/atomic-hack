import axios from "axios";
import {CONFIG} from "../CONFIG";
import qs from "qs";
import {createInterceptorBeforeRequest, createInterceptorResponse,} from "./_axiosInterceptors";

const AXIOS = axios.create({
    baseURL: CONFIG.BASE_URL,
    timeout: 20 * 60 * 1000,
    validateStatus: (status) => {
        return true;
    },
    headers: {
        'Content-Type': 'application/json',
    },
});

AXIOS.defaults.paramsSerializer = function (params) {
    return qs.stringify(params);
};

AXIOS.interceptors.response.use(createInterceptorResponse(), (error) => Promise.reject(error));
AXIOS.interceptors.request.use(createInterceptorBeforeRequest(), (error) => Promise.reject(error));

export default AXIOS;
