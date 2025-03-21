import { useEffect } from 'react';
import PropTypes from 'prop-types';

const Toast = ({ message, type, duration, onClose }) => {
  useEffect(() => {
    const timer = setTimeout(() => {
      onClose();
    }, duration);

    return () => clearTimeout(timer);
  }, [duration, onClose]); // Include both dependencies

  const typeStyles = {
    success: 'bg-green-500',
    error: 'bg-red-500',
    info: 'bg-blue-500',
  };

  return (
    <div
      className={`p-4 mb-2 rounded-md text-white shadow-lg animate-slide-in ${
        typeStyles[type] || 'bg-gray-500'
      }`}
    >
      {message}
    </div>
  );
};

Toast.propTypes = {
  message: PropTypes.string.isRequired,
  type: PropTypes.oneOf(['success', 'error', 'info']).isRequired,
  duration: PropTypes.number.isRequired,
  onClose: PropTypes.func.isRequired,
};

export default Toast;