import PropTypes from "prop-types";
import { format } from "timeago.js";
import RecentTitleName from "./RecentTitleName";

export default function RecentPost({ details }) {

  return (
    <button className="btn btn-ghost py-3 group h-fit w-full">
      <div className="w-full flex justify-between items-center cursor-pointer gap-12">
        <div className="flex gap-2">
          <div
            id="userphoto"
            className="h-10 w-10 aspect-square bg-base-content/80 rounded-sm"
          ></div>
          <div className="flex flex-col">
            <RecentTitleName>{details.ContentTitle}</RecentTitleName>
            <span id="active-time" className="text-xs font-thin w-fit">
              {format(details.CreatedAt)}
            </span>
          </div>
        </div>
        <div className="group-hover:opacity-100 opacity-0 transition-all duration-300">
          <div className="btn btn-sm btn-ghost font-thin rounded-full">
            Details
          </div>
        </div>
      </div>
    </button>
  );
}
RecentPost.propTypes = {
  details: PropTypes.shape({
    ContentTitle: PropTypes.string,
    CreatedAt: PropTypes.string,
  }),
};
