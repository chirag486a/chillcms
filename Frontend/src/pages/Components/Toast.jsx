import { useEffect } from "react";
import PropTypes from "prop-types";

const Toast = ({ message, type, duration, timestamp, onClose }) => {
  useEffect(() => {
    let oldTime = timestamp;
    let newTime = Date.now();
    let totalTime = duration;

    let passedTime = newTime - oldTime;
    let leftTime = totalTime - passedTime;
    if (leftTime < 0) {
      onClose();
    }
    const timer = setTimeout(() => {
      onClose();
    }, leftTime);

    return () => clearTimeout(timer);
  }, [duration, timestamp, onClose]); // Include both dependencies

  const typeStyles = {
    success: "bg-green-500",
    error: "bg-red-500",
    info: "bg-blue-500",
  };

  return (
    <div
      className={`p-4 mb-2 rounded-md text-white shadow-lg animate-slide-in text-xs ${
        typeStyles[type] || "bg-gray-500"
      }`}
    >
      {message}
    </div>
  );
};

Toast.propTypes = {
  message: PropTypes.string.isRequired,
  type: PropTypes.oneOf(["success", "error", "info"]).isRequired,
  timestamp: PropTypes.number.isRequired,
  duration: PropTypes.number.isRequired,
  onClose: PropTypes.func.isRequired,
};

export default Toast;
