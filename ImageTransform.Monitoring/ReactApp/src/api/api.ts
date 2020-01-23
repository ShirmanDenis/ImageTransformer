import Axios from "axios";
import { IApiResult } from "../Models/IApiResult";

export class Api {
    
    getFilters() {
        return Axios.get<IApiResult<string[]>>("api/filters");
    }

    filtrate(json: string){
        return Axios.post("api/filtrate", json, {
            headers: {
              'Content-Type': 'application/json; charset=utf-8'
            }
          });
    }
}