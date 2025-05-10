import { useState, useEffect } from "react";
import { useToast } from "../contexts/ToastContext";
import axios from "axios";
import { AuthContext } from "../contexts/AuthContext";

import PropTypes from "prop-types";

export const AuthProvider = ({ children }) => {
  let { addToast } = useToast();
  const [currentUser, setCurrentUser] = useState(null);
  const [loading, setLoading] = useState(true);
  useEffect(() => {
    const token = localStorage.getItem("token");
    async function checkLoggedIn() {
      try {
        await axios.get("http://localhost:5235/api/accounts/", {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        setLoading(false);
        setCurrentUser({ token });
      } catch (err) {
        console.log(err);
        localStorage.removeItem("token");
        setCurrentUser(null);
        setLoading(false);
        throw err;
      }
    }
    if (token) {
      checkLoggedIn();
    } else {
      setLoading(false);
    }
  }, [addToast]);

  const login = async (email, password) => {
    try {
      const response = await axios.post(
        "http://localhost:5235/api/accounts/login",
        { Email: email, Password: password }
      );
      const {
        data: {
          data: { token },
        },
      } = response;
      localStorage.setItem("token", token);
      setCurrentUser({ token });
      return response.data;
    } catch (err) {
      throw err.response.data;
    }
  };
  const isLoggedIn = async () => {
    try {
      if (!currentUser?.token) {
        return false;
      }
      await axios.get("http://localhost:5235/api/accounts/", {
        headers: {
          Authorization: `Bearer ${currentUser.token}`,
        },
      });
      return true;
      // eslint-disable-next-line no-unused-vars
    } catch (err) {
      setCurrentUser(null);
      localStorage.removeItem("token");
      return false;
    }
  };
  const logout = () => {
    try {
      localStorage.removeItem("token");
      setCurrentUser(null);
    } catch (err) {
      throw new Error(err);
    }
  };
  const value = { currentUser, login, logout, isLoggedIn };
  return (
    <AuthContext.Provider value={value}>
      {!loading && children}
    </AuthContext.Provider>
  );
};

AuthProvider.propTypes = { children: PropTypes.element };
