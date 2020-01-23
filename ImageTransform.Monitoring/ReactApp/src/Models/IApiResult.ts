export interface IApiResult<T>{
    result: T;
    errorMsg: string;
    isSuccessful: boolean;
}