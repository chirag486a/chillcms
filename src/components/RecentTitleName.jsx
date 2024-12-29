import PropTypes from "prop-types";


export default function RecentTitleName({ children }) {
  return (
    <div >
      <div
        id="username"
        className="text-sm text-left text-base-content relative before:absolute before:bottom-0 before:left-0 before:h-[1px] before:w-0 before:bg-base-content/50 group-hover:before:w-full before:duration-300 text-nowrap text-ellipsis w-fit max-w-52 block overflow-hidden whitespace-nowrap"
      >
        {children}
      </div>
    </div>
  );
}

RecentTitleName.propTypes = {
  children: PropTypes.string,
};
