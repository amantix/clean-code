import AuthService from "../services/auth.service";

export default async function authHeader() {
  let user = JSON.parse(localStorage.getItem("user"));

  if (user && user.token) {
      return {
          Authorization: "Bearer " + user.token,
          baseURL: "http://localhost:3000",
          withCredentials: true,
      };
  } else {
    return {};
  }
}
