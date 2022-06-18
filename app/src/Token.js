import React from 'react';
import axios from 'axios';
import {ThemeProvider, Stack, Label, TextField, PrimaryButton, DefaultButton , DefaultEffects} from '@fluentui/react';  

class Token extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      token: "",
      status: "", 
      connected: false
    };
  }

  componentDidMount() {
    this.status();
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

  getToken = () => {
    axios.get(`/api/.token/status/${this.props.name}`)
    .then(response => {
      if(response && response.data === 'CONNECTED') {
        axios.post(`/api/.token/${this.props.name}`)
        .then((token) => {
          this.setState({token: token });
        }).catch((error) => {
          console.log(error);
        });
       }
    }).catch((error) => {
      console.log(error);
    });
  }

  clear = () => {
    this.setState({ token: "" });
  }

  render() { 
    return (<div style={{boxShadow: DefaultEffects.elevation4}}>
      <ThemeProvider>
      <Label style={{marginLeft: "10px"}}>{this.props.name} : {this.state.status}</Label>
         <Stack verticalAlign tokens={{
               childrenGap: "l2",
               padding: "l2"
             }}>
               <PrimaryButton iconProps={{ iconName: 'AzureKeyVault' }} onClick={this.getToken} disabled={this.state.connected === false}>
                 Display
               </PrimaryButton>
               <DefaultButton iconProps={{ iconName: 'Cancel' }} onClick={this.clear}  disabled={this.state.connected === false}>
                 Clear
               </DefaultButton>
             </Stack>
             <TextField readOnly label="Token" multiline resizable={true} value={this.state.token.data} />
       </ThemeProvider>
       </div>);
    }
  }

export default Token;
