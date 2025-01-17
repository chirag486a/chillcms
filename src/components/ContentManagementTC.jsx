import Checkbox from "@mui/material/Checkbox";
import PropTypes from "prop-types";
import { format } from "timeago.js";

// Content Management Table Content
export default function ContentManagementTC({ content }) {
  return (
    <button className="btn btn-ghost border-base-300 border-0 border-b first:border-t pl-0 pr-4  rounded-none  inline-block h-fit w-full  font-thin even:bg-base-200 even:hover:bg-base-300">
      <div className="flex flex-row w-full rounded-lg pl-0  justify-between  py-0 items-center gap-2">
        <div className="w-fit">
          <Checkbox />
        </div>
        <div className="text-left max-w-96 min-w-96 overflow-ellipsis overflow-hidden">
          <span className="text-nowrap max-w-96 overflow-ellipsis">
            {content.title}
          </span>
        </div>
        <div className="w-40 text-center">{format(content.createdAt)}</div>
        <div className="w-20 text-right">{content.status}</div>
      </div>
    </button>
  );
}

ContentManagementTC.propTypes = {
  content: PropTypes.shape({
    title: PropTypes.string,
    createdAt: PropTypes.any,
    status: PropTypes.string,
  }),
};
