
export class RootStore{
    constructor(){
        this.FilePath = "";
        this.FilterName = "";
        this.Params = [];
    }
    public FilePath: string;
    public FilterName: string;
    public Params: any[];

    public setFileData(data: string){
        this.FilePath = data;
    }

    public setFilterName(filterName: string){
        this.FilterName = filterName;
    }

    public setParams(params: any){
        this.Params = params;
    }
}