import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { AuthContext } from "../../contexts/AuthContext";
import PropTypes from "prop-types";
import { useToast } from "../../contexts/ToastContext";
const ProtectedRoute = ({ children }) => {
  const { currentUser } = useContext(AuthContext);
  const [checkAuth, setCheckAuth] = useState(true);
  const navigate = useNavigate();
  const { addToast } = useToast();

  useEffect(() => {
    if (currentUser?.token === undefined || currentUser?.token === null) {
      navigate("/login");
      addToast("Login to get access");
    } else setCheckAuth(false);
  }, [currentUser, navigate, addToast]);

  if (checkAuth) return <div></div>;
  return children;
};

ProtectedRoute.propTypes = {
  children: PropTypes.node,
};

export default ProtectedRoute;
