import React, { Component } from "react";
import { connect } from "react-redux";
import Card from "./Document";
import Document from "./Document";

class AddDocument extends Component {
  constructor(props) {
    super(props);


    this.state = {
      document: {
        id: "",
        title: "",
        content: "",
        isPublic: false,
        link: "",
      },
    };
  }

  render() {
    const {user} = this.props.user;
    return (
      <Document document = {this.state.document} navigate = {this.props.navigate}/>
    );
  }
}

function mapStateToProps(state) {
  const { isLoggedIn, user } = state.auth;
  const { message } = state.message;
  return {
    isLoggedIn,
    message,
    user,
  };
}

export default connect(mapStateToProps)(AddDocument);
