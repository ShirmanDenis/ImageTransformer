import React, { Component } from "react";
import './App.css';
import Axios from 'axios';
import { ComboBox } from "./Components/ComboBox";
import { FileLoader } from "./Components/FileLoader";
export class App extends Component<any, any> {
  private currentFilter: any;

  constructor(props: Readonly<{}>) {
    super(props);
    this.state = { data: [] }
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
  public render() {
    return (
      <div>
        <header>
          Это хедер
        </header>
        <div>
          <ComboBox title={this.state.data[0]} elements={this.state.data}/>
          <FileLoader/>
        </div>
        <footer>
          Это футер
        </footer>
      </div>
    );
  }
}

export default App;
