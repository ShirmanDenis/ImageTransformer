import { apiInstance } from "../api";

export class RootStore{
    constructor(){
        this.FileData = "";
        this.FilterName = "";
        this.Params = [];
    }
    public FileData: String;
    public FilterName: string;
    public Params: any[];

    public setFileData(data: String){
        this.FileData = data;
    }

    public setFilterName(filterName: string){
        this.FilterName = filterName;
    }

    public setParams(params: any){
        this.Params = params;
    }

    public async getFilters() {
        return (await apiInstance.getFilters()).data;
    }
}