import axios from "axios";

const API_URL = "http://localhost:41301/Auth/";

class AuthService {
  login(email, password) {
    return axios
      .post(API_URL + "login", { email: email, password: password })
      .then((response) => {
        
        if (response.data.token) {
          localStorage.setItem("user", JSON.stringify(response.data));
        }
        return response.data;
      });
  }

  logout() {
    localStorage.removeItem("user");
  }


  register(name, email, password, anotherPassword) {
    return axios.post(API_URL + "register", {
      Username: name,
      Email: email,
      Password: password,
      AnotherPassword: anotherPassword,
    });
  }
}

const authService = new AuthService();

export default authService;
