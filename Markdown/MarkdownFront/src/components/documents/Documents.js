import React, { Component } from "react";

import { Navigate } from "react-router-dom";
import { connect } from "react-redux";
import { history } from "../../helpers/history";
import EventBus from "../../common/EventBus";
import DocumentsList from "./DocumentsList";
import DocumentsService from "../../services/documents.service";

class Documents extends Component {
  constructor(props) {
    super(props);
    
    this.state = {
      documents: "",
    };

  }

  nextPath(path) {
    history.push(path);
    window.location.reload(false);
  }

  componentDidMount() {
    DocumentsService.getAll().then(
      (response) => {
        console.info(response);

        this.setState({
          documents: response.data.documents,
        });

      },
      (error) => {
        this.setState({
          error:
            (error.response &&
              error.response.data &&
              error.response.data.message) ||
            error.message ||
            error.toString(),
        });

        if (error.response && error.response.status === 401) {
          EventBus.dispatch("logout");
        }
      }
    );
    
  }

  render() {
    const { isLoggedIn, isTestExists } = this.props;
    if (!isLoggedIn && !this.state.content) {
      return <Navigate to="/" />;
    }
    if (isTestExists) {
      return <Navigate to="/test" />;
    }
    return (
      <div className="card bg-light text-dark ">
        <button
          type="button"
          className="btn btn-success"
          onClick={() => this.nextPath("/documents/add")}
        >
          Добавить документ
        </button>
        <DocumentsList documents={this.state.documents} navigate={this.props.navigate} />
      </div>
    );
  }
}

function mapStateToProps(state) {
  const { isLoggedIn } = state.auth;
  return {
    isLoggedIn,
  };
}

export default connect(mapStateToProps)(Documents);
