import React from 'react';
import axios from 'axios';
import { DefaultButton, ThemeProvider, Stack, Label, DefaultEffects } from '@fluentui/react';  

class Connect extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      connected: false,
      token: "",
      status: ""
    };
  }

  componentDidMount() {
    this.status();
  }

  create = () => {
    axios.post(`/api/.token/create/${this.props.name}`)
    .then(() => {
      this.setState({ connected: true });
    }).catch((error) => {
      if (error.response
        && error.response.status === 401
        && error.response) {
        // Initiate consent flow with redirect url.
        window.location.replace(error.response.data);
      } else {
        console.log(error);
      }
    });
  }

  delete = () => {
    axios.post(`/api/.token/delete/${this.props.name}`)
      .then(() => {
        this.setState({ connected: false, status: "Not Found!" });
      }).catch((error) => {
        console.log(error);
      });
  }

  status = () => {
    axios.get(`/api/.token/status/${this.props.name}`)
    .then(response => {
      if(response) {
        if(response.data !== undefined) {
          this.setState({ status: response.data });
          if(response.data === 'CONNECTED') {
            this.setState({ connected: true, status: "Created!" });
          }
        }
      }
    }).catch((error) => {
      if(error && error.response && error.response.status){
        if(error.response.status === 404) {
          this.setState({ status: "Not Found!" });
        }

        if(error.response.status === 403) {
          this.setState({ status: "Forbidden!" });
        }

        if(error.response.status === 401) {
          this.setState({ connected: true, status: "Incomplete!" });
        }
      }
      console.log(error);
    });
  }

  render() { 
    return (<div style={{boxShadow: DefaultEffects.elevation4}}>
      <ThemeProvider>
      <Label style={{marginLeft: "10px"}}>{this.props.name} : {this.state.status}</Label>
         <Stack verticalAlign tokens={{
               childrenGap: "l2",
               padding: "l2"
             }}>
               
               <DefaultButton iconProps={{ iconName: 'PlugConnected' }} onClick={this.create} disabled={this.state.connected === true} >
                 Create
               </DefaultButton>

               <DefaultButton iconProps={{ iconName: 'PlugDisconnected' }} onClick={this.delete}  disabled={this.state.connected === false}>
                 Delete
               </DefaultButton>

               <DefaultButton iconProps={{ iconName: 'StatusCircleSync' }} onClick={this.status}>
                 Status
               </DefaultButton>

             </Stack>
       </ThemeProvider>
       </div>);
    }
  }

export default Connect;
