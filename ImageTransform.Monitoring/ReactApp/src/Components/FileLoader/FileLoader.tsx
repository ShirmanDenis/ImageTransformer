import React, { Component } from 'react';
import "./FileLoader.css"
import { RootStore } from '../../Stores/RootStore';

interface IFileLoaderProps {
    root: RootStore;
    onLoaded?: () => void;
}

export class FileLoader extends Component<IFileLoaderProps, any>{
    constructor(props: IFileLoaderProps){
        super(props);
        this.onChange = this.onChange.bind(this);
    }
    
    onChange(c: Component<IFileLoaderProps, any>, e: React.ChangeEvent<HTMLInputElement>){
        if (e.currentTarget.files === null) return;
        const curFile =  e.currentTarget.files[0];
        const fileReader= new FileReader();
        fileReader.readAsBinaryString(curFile);
        fileReader.onloadend = () => {
            console.log(curFile.size);
            const s = fileReader.result as String;
            c.props.root.setFileData(s);
            if (this.props.onLoaded)
                this.props.onLoaded();
        }
    }

    render(){
        return (
            <div className="block">
                <form>
                    <div className="form-group">
                        {/* <label htmlFor="exampleFormControlFile1">Выберите изображение</label> */}
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