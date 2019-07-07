import React, { Component } from 'react';

export class FileLoader extends Component<any, any>{
    render(){
        return (
            <div>
                <form>
                    <div className="form-group">
                        <label htmlFor="exampleFormControlFile1">Выберите изображение</label>
                        <input type="file" className="form-control-file" id="exampleFormControlFile1"></input>
                    </div>
                </form>
            </div>
        );
    }
}