import React, { Component } from 'react';
import { IRootProvider } from '../Models/IRootStore';
import Axios from 'axios';


export class FileLoader extends Component<IRootProvider, any>{
    constructor(props: IRootProvider){
        super(props);
        this.onChange = this.onChange.bind(this);
    }
    
    onChange(c: Component<IRootProvider, any>, e: React.ChangeEvent<HTMLInputElement>){
        if (e.currentTarget.files === null) return;
        const curFile =  e.currentTarget.files[0];
        const fileReader= new FileReader();
        fileReader.readAsBinaryString(curFile);
        fileReader.onloadend = () => {
            c.props.Root.setFileData(fileReader.result as string);
        }
    }

    render(){
        return (
            <div>
                <form>
                    <div className="form-group">
                        <label htmlFor="exampleFormControlFile1">Выберите изображение</label>
                        <input 
                            type="file" 
                            className="form-control-file" 
                            id="exampleFormControlFile1" 
                            onChange={(e) => this.onChange(this, e)}>
                            
                        </input>
                    </div>
                </form>
            </div>
        );
    }
}