import React, {Component} from "react";
import {Navigate} from "react-router-dom";
import Form from "react-validation/build/form";
import Input from "react-validation/build/input";
import CheckButton from "react-validation/build/button";
import {connect} from "react-redux";
import Textarea from "react-validation/build/textarea";
import {createDocument, updateDocument} from "../../actions/documents";
import documentsService from "../../services/documents.service";
import {FloatingLabel} from "react-bootstrap";
import markdownService from "../../services/markdown.service";


const required = (value) => {
    if (!value) {
        return (
            <div className="alert alert-danger" role="alert">
                Это поле обязательно!
            </div>
        );
    }
};

const contentValidator = (value) => {
    if (value.length < 5 || value.length > 1280) {
        return (
            <div className="alert alert-danger" role="alert">
                Контент должно быть от 5 до 250 символов.
            </div>
        );
    }
};

const titleValidator = (value) => {
    if (value.length < 5) {
        return (
            <div className="alert alert-danger" role="alert">
                Название должно быть от 5.
            </div>
        );
    }
};


class Document extends Component {
    constructor(props) {
        super(props);
        this.handleAdded = this.handleAdded.bind(this);
        this.onChangeTitle = this.onChangeTitle.bind(this);
        this.onChangeContent = this.onChangeContent.bind(this);
        this.onChangeLink = this.onChangeLink.bind(this);
        this.onChangeIsPublic = this.onChangeIsPublic.bind(this);

        this.handleDelete = this.handleDelete.bind(this);
        this.handleUpdate = this.handleUpdate.bind(this);
        this.setUp = this.setUp.bind(this);

        let document = this.props.document === undefined ?? null;
        this.state = this.setUp(document);
    }

    setUp(document) {
        console.info(document);
        return {
            id: document.id ?? "",
            userId: document.userID ?? 0,
            title: document.title ?? "",
            content: document.content ?? "",
            isPublic: document.isPublic ?? false,
            link: document.link ?? "",
            loading: false,
            html: document.html ?? ""
        };
    }

    componentDidMount() {
        if (!this.props.document) {
            documentsService.getById(this.props.params.id).then(
                (response) => {
                    let document = response.data.document;
                    document.content = response.data.content;
                    if (document) {
                        markdownService.convert(response.data.content).then(
                            (response) => {
                                document.html = response.data.html;
                                this.setState(this.setUp(document));

                            }
                        )
                    }
                },
                (error) => {
                }
            );
        }
    }

    handleReload = () => {
        window.location.reload();
    };

    handleDelete(id) {

        this.setState({});
    }

    handleAdded(e) {
        e.preventDefault();
        this.setState({
            loading: true,
        });

        this.form.validateAll();

        const {dispatch} = this.props;
        if (this.checkBtn.context._errors.length === 0) {
            console.info({
                title: this.state.title,
                content: this.state.content,
                isPublic: this.state.isPublic,
                link: this.state.link,
            });
            this.props
                .dispatch(
                    createDocument({
                        title: this.state.title,
                        content: this.state.content,
                        isPublic: this.state.isPublic,
                        link: this.state.link,
                    })
                )
                .then(() => {
                    this.props.navigate("/documents", {replace: true});
                })
                .catch(() => {
                    this.setState({
                        loading: false,
                    });
                });
        } else {
            this.setState({
                loading: false,
            });
        }
    }

    handleUpdate(e) {
        e.preventDefault();
        this.setState({
            loading: true,
        });
        this.form.validateAll();
        const {dispatch} = this.props;
        if (this.checkBtn.context._errors.length === 0) {
            console.info(this.state.isPublic);
            dispatch(
                updateDocument({
                    id: this.state.id,
                    title: this.state.title,
                    content: this.state.content,
                    isPublic: this.state.isPublic,
                    link: this.state.link,
                })
            )
                .then(() => {
                    this.props.navigate("/documents", {replace: true});
                })
                .catch(() => {
                    this.setState({
                        loading: false,
                    });
                });
        } else {
            this.setState({
                loading: false,
            });
        }
    }

    onChangeTitle(e) {
        this.setState({
            title: e.target.value,
        });
    }

    onChangeIsPublic(e) {
        console.info(e.target.checked);
        this.setState({
            isPublic: e.target.checked,
        });
    }

    onChangeContent(e) {
        markdownService.convert(e.target.value).then(
            (response) => {
                this.setState({
                    html: response.data.html,
                    content: e.target.value,
                });
            }
        )
        this.setState({
            content: e.target.value,
        });
    }

    onChangeLink(e) {
        this.setState({
            link: e.target.value,
        });
    }

    render() {
        const {isLoggedIn, user: currentUser, message} = this.props;

        if (!isLoggedIn) {
            return <Navigate to="/"/>;
        }

        if (this.state.userId !== 0 && currentUser.user.id !== this.state.userId &&
            !this.state.isPublic) {
            return <Navigate to="/"/>;
        }


        return (
            <div className="container">
                <div className="row">
                    <div className="col-md-6">
                        <div className="card bg-light text-dark">
                            <h1>
                                <center>Добавление документа</center>
                            </h1>
                            <Form
                                onSubmit={!this.state.id ? this.handleAdded : this.handleUpdate}
                                ref={(c) => {
                                    this.form = c;
                                }}
                            >
                                <div className="form-group">
                                    <label htmlFor="description">Название: </label>
                                    <Input
                                        className="form-control small-12 medium-12 columns"
                                        name="title"
                                        value={this.state.title}
                                        rows="14"
                                        onChange={this.onChangeTitle}
                                        validations={[required, titleValidator]}
                                    />
                                </div>
                                <hr></hr>
                                <div className="form-group">
                                    <label htmlFor="description">Описание: </label>
                                    <Textarea
                                        className="form-control small-12 medium-12 columns"
                                        name="description"
                                        value={this.state.content}
                                        rows="14"
                                        onChange={this.onChangeContent}
                                        validations={[required, contentValidator]}
                                    />
                                </div>
                                <div className="form-group">
                                    <label htmlFor="description">Публичный: </label>
                                    <Input
                                        type="checkbox"
                                        checked={this.state.isPublic}
                                        rows="14"
                                        onChange={this.onChangeIsPublic}
                                    />
                                </div>
                                {(this.state.userId === 0 || currentUser.user.id === this.state.userId) && (

                                <div className="form-group">
                                    <button
                                        className="btn btn-dark btn-block"
                                        disabled={this.state.loading}
                                    >
                                        {this.state.loading && (
                                            <span className="spinner-border spinner-border-sm"></span>
                                        )}
                                        {!this.state.id ? (
                                            <span>Добавить</span>
                                        ) : (
                                            <span>Сохранить</span>
                                        )}
                                    </button>
                                </div>)
                                }

                                {message && (
                                    <div className="form-group">
                                        <div className="alert alert-danger" role="alert">
                                            {message}
                                        </div>
                                    </div>
                                )}
                                <CheckButton
                                    style={{display: "none"}}
                                    ref={(c) => {
                                        this.checkBtn = c;
                                    }}
                                />
                            </Form>
                        </div>
                    </div>

                    <div className="col-md-6">
                        <div className="card bg-light text-dark">
                            <h2 className="text-center">Предпросмотр</h2>
                            <div className="p-3">
                                <h4>{this.state.title || "Название документа"}</h4>
                                <pre>
                                    <div
                                        dangerouslySetInnerHTML={{
                                            __html: this.state.html || "Контент будет отображаться здесь.",
                                        }}
                                    />
                                </pre>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

function mapStateToProps(state) {
    const {isLoggedIn, user} = state.auth;
    const {message} = state.message;

    return {
        isLoggedIn,
        user,
        message,
    };
}

export default connect(mapStateToProps)(Document);
