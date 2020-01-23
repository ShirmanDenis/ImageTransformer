import './Header.css'
import React, { Component } from "react";

export class Header extends Component<any>{

    render(){
        return <div className={"wrapper"}>
            <div>
                <h1>
                    Фильтр изображений
                </h1>
            </div>
        </div>
    }

}