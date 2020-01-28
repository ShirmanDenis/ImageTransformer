import React, { Component } from "react";
import { ComboBox } from "../ComboBox";
import { FileLoader } from "../FileLoader/FileLoader";
import { RootStore } from "../../Stores/RootStore";

export interface IContentProps{
	root: RootStore;
}

export interface IContentState{
	filters: string[];
}

export class Content extends Component<IContentProps, IContentState> {
	constructor(props: IContentProps){
		super(props);
		this.state = {filters: []}
		this.onButtonCLick = this.onButtonCLick.bind(this);
		this.onImageChoosen = this.onImageChoosen.bind(this);
	}

	componentDidMount() {
		this.props.root.getFilters().then(getFiltersResult => {
			if (getFiltersResult.isSuccessful) {
				this.setState(() => ({ filters: getFiltersResult.result }));
			} else {
				console.log(getFiltersResult.errorMsg);
			}
		});
	}

	render() {
		return (
			<div>
				<table>
					<tr>
						<td>
							Фильтр:
						</td>
						<td>
							<FileLoader
								root={this.props.root}
								onLoaded={this.onImageChoosen}
							/>
						</td>
						<td>
							<button onClick={this.onButtonCLick}>Применить фильтр</button>
						</td>
					</tr>
					<tr>
						<td>
							<ComboBox
								root={this.props.root}
								elements={[...this.state.filters]}
							/>
						</td>
						<td>
							<div id="loaded_img" className="block"></div>
						</td>
						<td>
							<div id="filtered_img" className="block"></div>
						</td>
					</tr>
				</table>
			</div>
		);
	}

	private onImageChoosen() {
		const workSpace = document.getElementById("loaded_img") as HTMLElement;
		let firstChild = workSpace.childNodes[0];
		if (firstChild) {
			workSpace.removeChild(firstChild);
		}
		const img = document.createElement("img") as HTMLImageElement;
		img.src = `data:image/png;base64, ${btoa(
			this.props.root.FileData.toString()
		)}`;
		workSpace.appendChild(img);
	}

	onButtonCLick() {
		const json = JSON.stringify({
			FilterName: this.props.root.FilterName,
			ImgData: btoa(this.props.root.FileData.toString()),
			Area: {
				X: 0,
				Y: 0,
				Width: 100,
				Height: 100
			},
			Params: [
				{
					Field: "some_val"
				}
			]
		});
		this.props.root.filtrate(json)
			.then(response => {
				if (!response.isSuccessful) {
					console.log(response.errorMsg);
					return;
				}
				const imgData = response.result;
				const workSpace = document.getElementById(
					"filtered_img"
				) as HTMLElement;
				let firstChild = workSpace.childNodes[0];
				if (firstChild) {
					workSpace.removeChild(firstChild);
				}
				const img = document.createElement("img") as HTMLImageElement;
				img.src = `data:image/png;base64, ${imgData}`;
				workSpace.appendChild(img);
			})
			.catch(error => {
				console.log(error);
			});
	}
}
