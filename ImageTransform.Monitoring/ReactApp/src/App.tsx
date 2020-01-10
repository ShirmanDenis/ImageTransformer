import React, { Component } from "react";
import './App.css';
import Axios from 'axios';
import { ComboBox } from "./Components/ComboBox";
import { FileLoader } from "./Components/FileLoader";
import { IRootProvider } from "./Models/IRootStore";

export class App extends Component<IRootProvider, any> {
  private currentFilter: any;

  constructor(props: IRootProvider) {
    super(props);
    this.state = { data: [] }
    this.onButtonCLick = this.onButtonCLick.bind(this);
    this.onImageChoosen = this.onImageChoosen.bind(this);
  }
  componentDidMount() {
    Axios.get("api/filters")
      .then(response => {
        this.setState({ data: response.data })
      })
      .catch(error => {
        console.log(error);
      })
  }
  onButtonCLick() {
    const json = JSON.stringify({
      FilterName: this.props.Root.FilterName,
      ImgData: btoa(this.props.Root.FileData.toString())
    });
    Axios
      .post("api/filtrate", json, {
        headers: {
          'Content-Type': 'application/json; charset=utf-8'
        }
      })
      .then(response => {
        const data = response.data;
        const workSpace = document.getElementById("filtered_img") as HTMLElement;
        let firstChild = workSpace.childNodes[0];
        if (firstChild){
          workSpace.removeChild(firstChild);
        }
        const img = document.createElement("img") as HTMLImageElement;
        img.src = `data:image/png;base64, ${data}`; 
        workSpace.appendChild(img);
      })
    .catch(error => {
      console.log(error);
    });
  }
  private onImageChoosen(){
    const workSpace = document.getElementById("loaded_img") as HTMLElement;
    let firstChild = workSpace.childNodes[0];
    if (firstChild){
      workSpace.removeChild(firstChild);
    }
    const img = document.createElement("img") as HTMLImageElement;
    img.src = `data:image/png;base64, ${btoa(this.props.Root.FileData.toString())}`; 
    workSpace.appendChild(img);
  }
  public render() {
    return (
      <div>
        <header>
          Это хедер
        </header>
        <div>
          <ComboBox root={this.props.Root} title={this.state.data[0]} elements={this.state.data}/>
          <FileLoader root={this.props.Root} onLoaded={this.onImageChoosen}/>
          <div id="loaded_img"></div>
          <button onClick={this.onButtonCLick}>Обработка</button>
          <div id="filtered_img"></div>
        </div>
        <footer>
          Это футер
        </footer>
      </div>
    );
  }
}

export default App;
