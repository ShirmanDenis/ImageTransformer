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
    Axios
    .post("api/filtrate", {
      FilterName: this.props.Root.FilterName,
      ImgData: this.props.Root.FilePath
    }, {
      headers: {
        'Content-Type': 'application/json'
      }
    }
    )
    .then(response => {
      const data = response.data;
      const workSpace = document.getElementById("work_space") as HTMLElement;
      const img = document.createElement("img") as HTMLImageElement;
      img.src = data;
      workSpace.appendChild(img)
    })
    .catch(error => {
      console.log(error);
    });
  }
  public render() {
    return (
      <div>
        <header>
          Это хедер
        </header>
        <div id="work_space">
          <ComboBox root={this.props.Root} title={this.state.data[0]} elements={this.state.data}/>
          <FileLoader Root={this.props.Root}/>
          <button onClick={this.onButtonCLick}>Обработка</button>
        </div>
        <footer>
          Это футер
        </footer>
      </div>
    );
  }
}

export default App;
