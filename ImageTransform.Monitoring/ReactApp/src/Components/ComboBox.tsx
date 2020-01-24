import React, { Component } from 'react';
import { IRootProvider } from '../Models/IRootStore';
import { RootStore } from '../Stores/RootStore';

interface IComboBoxProps {
    title?: string;
    root: RootStore;
    elements: string[];
    onClick?: any;
}

interface IComboBoxState {
    currentElement: any;
}

export class ComboBox extends Component<IComboBoxProps, IComboBoxState> {
    constructor(props: Readonly<IComboBoxProps>) {
        super(props)
        this.state = {currentElement: props.title}
        this.onElemClick = this.onElemClick.bind(this);
    }
    componentWillMount(){
        this.setState({currentElement: this.props.elements[0]});
    }
    onElemClick(c: Component<IComboBoxProps, any>, e: React.MouseEvent<HTMLAnchorElement, MouseEvent>){
        const content = e.currentTarget.innerHTML;
        // todo: remove logic from combobox
        this.props.root.setFilterName(content);
        c.setState({currentElement: content});
    }
    
    render() {
        return (
            <div className="dropdown">
                <button
                    className="btn btn-secondary dropdown-toggle"
                    type="button"
                    id="dropdownMenuButton"
                    data-toggle="dropdown"
                    aria-haspopup="true"
                    aria-expanded="false">
                    {this.state.currentElement}
                </button>
                <div className="dropdown-menu" aria-labelledby="dropdownMenuButton">
                    {
                        this.props.elements.map((e) =>
                            <a
                                className="dropdown-item"
                                href="#"
                                onClick = {(event) => this.onElemClick(this, event)}
                                >
                                {e}
                            </a>
                        )
                    }
                </div>
            </div>
        );
    }
}