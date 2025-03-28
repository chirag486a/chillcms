import { useContext, useEffect, useState } from "react";
import axios from "axios";
import { useToast } from "../contexts/ToastContext";
import { AuthContext } from "../contexts/AuthContext";
import { useNavigate } from "react-router-dom";
export default function Register() {
  const [email, setEmail] = useState("");
  const [name, setName] = useState("");
  const [password, setPassword] = useState("");
  const [username, setUsername] = useState("");
  const { addToast } = useToast();
  const { login, currentUser } = useContext(AuthContext);
  const navigate = useNavigate();
  useEffect(() => {
    if (currentUser) {
      navigate("/dashboard");
      addToast("Already Logged in");
    }
  }, [currentUser, navigate, addToast]);
  const handleSubmit = async (e) => {
    try {
      e.preventDefault();
      await axios.post(
        "http://localhost:5235/api/accounts/register",
        { Email: email, Name: name, Password: password, UserName: username }
      );
      const loginResponse = await login(email, password);
      addToast(`Welcome ${loginResponse.data.name}`, "success");
    } catch (err) {
      const errResponse = err.response?.data;
      if (!errResponse) {
        addToast(`${err.message}`, "error");
      }
      console.log(err);
      for (let field in errResponse.errors) {
        errResponse.errors[field].forEach((value) => {
          addToast(value, "error");
        });
      }
    }
  };
  return (
    <div className="register h-full w-full flex items-center justify-center">
      <form
        action=""
        className="shadow-2xl bg-primary-content py-16 px-24 rounded-xl"
        onSubmit={(e) => handleSubmit(e)}
      >
        <div className="prose">
          <h2 className="text-center">Create Account</h2>
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
                <span className="label-text">Username</span>
              </div>
              <input
                type="text"
                placeholder="John"
                className="input input-bordered w-full max-w-xs"
                onChange={(e) => setUsername(e.target.value)}
              />
            </label>
            <label htmlFor="" className="form-control w-full max-w-xs">
              <div className="label">
                <span className="label-text">Name</span>
              </div>
              <input
                type="text"
                placeholder="John Doe"
                className="input input-bordered w-full max-w-xs"
                onChange={(e) => setName(e.target.value)}
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
                className="btn btn-primary font-thin text-base-100 w-full"
              >
                Register
              </button>
              <a>Already have account?</a>
            </div>
          </div>
        </div>
      </form>
    </div>
  );
}
