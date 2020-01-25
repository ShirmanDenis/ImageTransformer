import React, { Component } from "react";
import "./App.css";
import { Content} from "./../Components/Content/Content";
import { IRootProvider } from "./../Models/IRootStore";
import { Header } from "./../Components/Header/Header";

export class App extends Component<IRootProvider> {
	constructor(props: IRootProvider) {
		super(props);
	}

	public render() {
		return (
			<div>
				<Header />
				<Content root={this.props.Root}/>
				<footer>Это футер</footer>
			</div>
		);
	}
}

export default App;
