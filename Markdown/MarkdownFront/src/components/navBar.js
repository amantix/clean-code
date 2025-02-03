import { Component } from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import { logout } from "../actions/auth";
import EventBus from "../common/EventBus";
import { history } from "../helpers/history";
import { clearMessage } from "../actions/message";

class NavBar extends Component {
  constructor(props) {
    super(props);
    this.logOut = this.logOut.bind(this);
    this.state = {
      currentUser: undefined,
    };

    history.listen((location) => {
      props.dispatch(clearMessage());
    });
  }

  componentDidMount() {
    const user = this.props.user;

    if (user) {
      this.setState({
        currentUser: user,
      });
    }

    EventBus.on("logout", () => {
      this.logOut();
    });
  }

  componentWillUnmount() {
    EventBus.remove("logout");
  }

  logOut() {
    this.props.dispatch(logout());
    this.setState({
      currentUser: undefined,
    });
  }

  render() {
    const { currentUser } = this.state;

    return (
      <nav
        className="navbar navbar-expand-lg sticky-top bg-dark"
        data-bs-theme="dark"
      >
        <div className="container-fluid">
          <a className="navbar-brand" href="/">
            Markdown
          </a>
          <button
            className="navbar-toggler"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#navbarText"
            aria-controls="navbarText"
            aria-expanded="false"
            aria-label="Toggle navigation"
          >
            <span className="navbar-toggler-icon"></span>
          </button>
          <div className="collapse navbar-collapse" id="navbarText">
            {currentUser && (
              <ul className="navbar-nav me-auto mb-2 mb-lg-0">
                <li className="nav-item">
                  <Link to={"/documents"} className="nav-link">
                    Документы
                  </Link>
                </li>

              </ul>
            )}
            <span>
              {currentUser ? (
                <ul className="navbar-nav me-auto mb-2 mb-lg-0">
                  <li className="nav-item">
                    <Link to={"/profile"} className="nav-link">
                      Профиль
                    </Link>
                  </li>
                  <li className="nav-item ml-auto mb-2 mb-lg-0">
                    <Link
                      to="/login"
                      className="nav-link"
                      onClick={this.logOut}
                    >
                      Выйти
                    </Link>
                  </li>
                </ul>
              ) : (
                <ul className="navbar-nav me-auto mb-2 mb-lg-0">
                  <li className="nav-item">
                    <Link to={"/login"} className="nav-link">
                      Войти
                    </Link>
                  </li>

                  <li className="nav-item mb-2 mb-lg-0">
                    <Link to={"/register"} className="nav-link">
                      Зарегестрироваться
                    </Link>
                  </li>
                </ul>
              )}
            </span>
          </div>
        </div>
      </nav>
    );
  }
}

function mapStateToProps(state) {
  const { user } = state.auth;

  return {
    user,
  };
}
export default connect(mapStateToProps)(NavBar);
