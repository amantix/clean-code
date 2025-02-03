import React, { Component } from "react";
import { connect } from "react-redux";
import EventBus from "../../common/EventBus";
import DocumentsService from "../../services/documents.service";

class DocumentList extends Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: false,
    };
  }

  handleReload() {
    window.location.reload(false);
  }

  handleDelete(e) {
    DocumentsService.deleteById(e.id).then(
      () => {
        this.handleReload();
      },
      (error) => {
        this.setState({
          cards:
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
    const { test, documents } = this.props;
    if (documents) {
      const items = documents.map((document, i) => (
        <div className="col">
          <div className="card" key={document.id}>
            <div className="card-body">
              <div key={document.id}>
                {!test && (
                    <div className="d-flex my-3">
                      <strong>Ссылка: </strong>
                      <a href={document.link} className="mx-2">
                        {document.link}
                      </a>
                    </div>
                )}
                <p className="card-text">
                  <strong>Название: </strong>
                  {document.title}
                </p>
              </div>
              <hr></hr>
              <div className="d-flex justify-content-between">
                  <button
                    type="button"
                    className="btn btn-success"
                    disabled={this.state.loading}
                    onClick={() => this.props.navigate("/documents/" + document.id)}
                  >
                    <i className="bi bi-pen" style={{ fontSize: 16 }}></i>
                    &nbsp;Изменить
                  </button>
                  <button
                    type="button"
                    className="btn btn-danger"
                    disabled={this.state.loading}
                    onClick={() => this.handleDelete(document)}
                  >
                    {this.state.loading && (
                      <span className="spinner-border spinner-border-sm"></span>
                    )}
                    <i className="bi bi-trash" style={{ fontSize: 16 }}></i>
                    &nbsp;Удалить
                  </button>
              </div>
            </div>
          </div>
        </div>
      ));
      return <div className="row row-cols-3 g-3">{items}</div>;
    }
  }
}

function mapStateToProps(state) {
  const { isLoggedIn } = state.auth;
  return {
    isLoggedIn,
  };
}

export default connect(mapStateToProps)(DocumentList);
