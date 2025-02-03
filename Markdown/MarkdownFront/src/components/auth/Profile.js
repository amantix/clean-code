import React, { Component } from "react";
import { connect } from "react-redux";
import EventBus from "../../common/EventBus";

class Profile extends Component {
  constructor(props) {
    super(props);
    this.state = {
      tests: "",
    };
  }




  render() {
    const { user: currentUser } = this.props;
    console.info(currentUser);

    return (
      <div>
        <div className="card bg-light text-dark w-100">
        <h1>{currentUser.user.name}</h1>
        <br />
        <p>
          <strong>Имя:</strong> {currentUser.user.username}
        </p>
        <p>
          <strong>Почта:</strong> {currentUser.user.email}
        </p>
        </div>
      </div>
    );
  }
}

function mapStateToProps(state) {
  const { user } = state.auth;
  return {
    user,
  };
}

export default connect(mapStateToProps)(Profile);
