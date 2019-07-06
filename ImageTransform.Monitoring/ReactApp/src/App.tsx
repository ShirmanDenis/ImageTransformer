import React, { Component } from "react";
import ReactDOM from "react-dom";
import './App.css';
import Axios from 'axios';

export class App extends Component<any, any> {
  constructor(props: Readonly<{}>) {
    super(props);
    this.state = { data: "not_yet" }
  }
  componentDidMount() {
    Axios.get("api/test")
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
          Контент
          {this.state.data}
        </div>
        <footer>
          Это футер
        </footer>
      </div>
    );
  }
}

export default App;
