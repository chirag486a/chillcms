import { useState, useContext, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useToast } from "../contexts/ToastContext";

import { AuthContext } from "../contexts/AuthContext";

export default function Login() {
  const { login, currentUser, isLoggedIn } = useContext(AuthContext);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const { addToast } = useToast();
  var navigate = useNavigate();
  useEffect(() => {
    async function checkLoggedIn() {
      await isLoggedIn()
      if (currentUser) {
        navigate("/dashboard");
        addToast("Already Logged in.");
      }
    }
    checkLoggedIn();
  }, [navigate, currentUser, addToast, isLoggedIn]);
  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await login(email, password);
    } catch (err) {
      if (!err.errors) addToast("Server: Something went wrong", "error", 3000);
      for (const key in err.errors) {
        addToast(`${key}: ${err.errors[key]}`, "error", 3000);
      }
    }
  };

  return (
    <div className="register h-full w-full flex items-center justify-center">
      <form
        action=""
        className="shadow-2xl bg-primary-content py-16 px-24 rounded-xl"
        onSubmit={handleSubmit}
      >
        <div className="prose">
          <h2 className="text-center">Login</h2>
          <div className="form-control gap-4 w-80">
            <label className="form-control w-full max-w-xs">
              <div className="label">
                <span className="label-text">Email</span>
              </div>
              <input
                type="text"
                placeholder="John@gmail.com"
                className="input input-bordered w-full max-w-xs"
                onChange={(e) => setEmail(e.target.value)}
              />
            </label>
            <label htmlFor="" className="form-control w-full max-w-xs">
              <div className="label">
                <span className="label-text">Password</span>
              </div>
              <input
                type="password"
                name=""
                id=""
                placeholder="Password"
                className="input input-bordered w-full max-w-xs"
                onChange={(e) => setPassword(e.target.value)}
              />
            </label>
            <div className="form-control gap-3 items-center justify-center my-4">
              <button
                type="submit"
                className="btn btn-primary font-thin btn-base-100 w-full"
              >
                Login
              </button>
              <a>Create a new account</a>
            </div>
          </div>
        </div>
      </form>
    </div>
  );
}
