import PropTypes from "prop-types";
import { NavLink, useLocation } from "react-router-dom";

const camalCaser = function (string) {
  return string
    .split("-")
    .map((str) => {
      return str.charAt(0).toUpperCase() + str.slice(1);
      
    })
    .join(" ");
};
export default function NavItem({ children, to }) {
  const location = useLocation();
  const className = ({ isPending }) => {
    const normalizePath = location.pathname.replace(/\/+$/, "");
    const isMatch = to === normalizePath;
    return isPending ? "pending  p-4" : isMatch ? "active p-4" : "p-4";
  };

  return (
    <li key={to} className="tooltip tooltip-right" data-tip={camalCaser(to.split("/").at(-1))}>
      <NavLink to={to} className={className} end>
        {children}
      </NavLink>
    </li>
  );
}

NavItem.propTypes = {
  to: PropTypes.string,
  children: PropTypes.element,
  active: PropTypes.bool,
};
