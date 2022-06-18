import React from 'react';
import axios from 'axios';
import { Dropdown, TextField, PrimaryButton, ThemeProvider, Stack, Pivot, PivotItem, Separator, DefaultEffects} from '@fluentui/react';

const dropdownStyles = {
  dropdown: { width: 100 }
};

const httpMethods = [
  { key: 'GET', text: 'GET' },
  { key: 'POST', text: 'POST' },
  { key: 'PUT', text: 'PUT' },
  { key: 'PATCH', text: 'PATCH' },
  { key: 'DELETE', text: 'DELETE' },
];

const defaultMethod = "GET";
const defaultHeaders = `{"X-MS-TOKENPROVIDER-ID":"graph","X-MS-PROXY-BACKEND-HOST":"https://graph.microsoft.com/v1.0"}`;
const defaultApiOperation = "me";

class Proxy extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      method: defaultMethod,
      headers: defaultHeaders,
      apiOperation: defaultApiOperation,
      body: undefined,
      response: undefined
    };
  }

  methodChanged = (ev, option) => { this.setState({ method: option.text }); };

  operationChange = (ev, text) => { this.setState({ apiOperation: text }); };

  headersChange = (ev, text) => { this.setState({ headers: text }); };

  bodyChange = (ev, text) => { this.setState({ body: text }); };

  sendRequest = () => {
    const headers = JSON.parse(this.state.headers);
    const tokenProviderId = headers["X-MS-TOKENPROVIDER-ID"];
    const proxyBackend = headers["X-MS-PROXY-BACKEND-HOST"];

    if (tokenProviderId === undefined
      || tokenProviderId === ""
      || proxyBackend === undefined
      || proxyBackend === "") {
      this.setState({ response: "Headers X-MS-TOKENPROVIDER-ID and X-MS-PROXY-BACKEND-HOST must be supplied." });
    } else {
      axios.get(`/api/.token/status/${tokenProviderId}`)
        .then(response => {
          if (response && response.data === 'CONNECTED') {
            let config = null;
            if (this.state.body) {
              config = {
                url: `/api/proxy/${this.state.apiOperation}`,
                method: this.state.method,
                headers: headers,
                data: JSON.parse(this.state.body)
              };
            } else {
              config = {
                url: `/api/proxy/${this.state.apiOperation}`,
                method: this.state.method,
                headers: headers,
              };
            }

            axios.request(config).then((response) => {
              this.setState({ response: JSON.stringify(response.data) });
            }).catch((error) => {
              this.setState({ response: error });
            });
          } else {
            this.setState({ response: `Token State:${response.data}. Try create the token again.` });
          }
        }).catch((error) => {
          this.setState({ response: error });
        });
    }
  };

  render() {
    return (<div style={{ boxShadow: DefaultEffects.elevation8, paddingLeft: "200px", paddingRight: "200px", paddingTop: "30px", paddingBottom: "30px"}}>
      <ThemeProvider>
        <Separator>Request</Separator>
        <Stack horizontal tokens={{
          childrenGap: "5",
          padding: "5",
        }}>
          <Dropdown label="Method" required defaultSelectedKey={defaultMethod} onChange={this.methodChanged} options={httpMethods} styles={dropdownStyles} />
          <TextField label="API Operation" required defaultValue={defaultApiOperation} onChange={this.operationChange} width="300" />
        </Stack>
        <Stack verticalAlign tokens={{
          childrenGap: "10",
          padding: "10"
        }}>
        </Stack>
        <TextField label="Headers" defaultValue={defaultHeaders} multiline onChange={this.headersChange} required />
        <TextField label="Body" height="300" multiline onChange={this.bodyChange} width="300" />
        <br />
        <Separator>Response</Separator>
        <Pivot>
          <PivotItem headerText="Body">
            <TextField readOnly multiline value={this.state.response} width="100" />
          </PivotItem>
        </Pivot>
        <br />
        <PrimaryButton text="Send Request" label="Send" onClick={this.sendRequest} />
      </ThemeProvider>
    </div>)
  }
}

export default Proxy;

