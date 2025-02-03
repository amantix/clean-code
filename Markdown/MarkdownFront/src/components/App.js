import React, { Component } from "react";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";

import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-icons/font/bootstrap-icons.css";
import "../css/App.css";
import Login from "./auth/Login";
import Register from "./auth/Registration";
import Profile from "./auth/Profile";
import NavigateComponent from "../helpers/navigate";
import { ToastContainer } from "react-toastify";
import NavBar from "./navBar";
import Document from "./documents/Document";
import Documents from "./documents/Documents";
import AddDocument from "./documents/AddDocument";

class App extends Component {
  render() {
    return (
      <BrowserRouter navigate={Navigate}>
        <NavBar />
        
        <div className="container mt-3">
          <Routes>
            <Route
              exact
              path="/"
              element={<NavigateComponent Component={Register} />}
            />
            <Route
              exact
              path="/login"
              element={<NavigateComponent Component={Login} />}
            />
            <Route
              exact
              path="/register"
              element={<NavigateComponent Component={Register} />}
            />
            <Route
              exact
              path="/profile"
              element={<NavigateComponent Component={Profile} />}
            />

            <Route
              exact
              path="/documents"
              element={<NavigateComponent Component={Documents} />}
            />
            <Route
              exact
              path="/documents/add"
              element={<NavigateComponent Component={AddDocument} />}
            />
            <Route
              exact
              path="/documents/:id"
              element={<NavigateComponent Component={Document} />}
            />
          </Routes>
        </div>
        <ToastContainer
          position="top-center"
          autoClose={5000}
          hideProgressBar={false}
          newestOnTop={false}
          closeOnClick
          rtl={false}
          pauseOnFocusLoss
          draggable
          pauseOnHover
          theme="dark"
        />
      </BrowserRouter>
    );
  }
}

export default App;
