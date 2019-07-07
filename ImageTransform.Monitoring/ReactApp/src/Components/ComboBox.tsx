import React, { Component } from 'react';

interface IComboBoxProps {
    title?: string;
    elements: any[];
    onClick?: any;
}

export class ComboBox extends Component<IComboBoxProps, any> {
    constructor(props: Readonly<IComboBoxProps>) {
        super(props)
        this.state = {currentElement: null}
        this.onElemClick = this.onElemClick.bind(this);
    }
    onElemClick(c: Component<IComboBoxProps, any>, e: React.MouseEvent<HTMLAnchorElement, MouseEvent>){
        const content = e.currentTarget.innerHTML;
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