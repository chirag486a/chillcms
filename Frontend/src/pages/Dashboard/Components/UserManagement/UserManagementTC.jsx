import Checkbox from "@mui/material/Checkbox";
import PropTypes from "prop-types";
import { format } from "timeago.js";

export default function UserManagementTC({ content }) {
  return (
    <>
      <button className="btn btn-ghost border-base-300 border-0 pl-0 pr-4  rounded-none inline-block h-fit w-full  font-thin even:bg-base-200 even:btn-ghost">
        <div className="flex flex-row w-full justify-between rounded-lg pl-0  py-0 items-center">
          <div className="flex items-center justify-center gap-4">
            <div className="w-fit">
              <Checkbox />
            </div>
            <div className="text-left flex gap-4 items-center w-40">
              <div className="h-8 aspect-square bg-base-300 rounded-sm">
                <img
                  // src={content.Imgsrc}
                  src="https://picsum.photos/200"
                  alt={content.Name + " photo"}
                  className="h-full m-0"
                />
              </div>
              <span className="text-nowrap w-full max-w-96 overflow-ellipsis">
                {content.Name}
              </span>
            </div>
          </div>
          <div className="w-40 text-center">{format(content.CreatedAt)}</div>
          {/* <div className="w-20 text-center">{content.Role}</div> */}
          <div className="w-20 text-center">Admin</div>
          <div className="w-40 text-center">{content.UserName}</div>
          <div className="w-60 text-center">{content.Email}</div>
        </div>
      </button>
    </>
  );
}

UserManagementTC.propTypes = {
  content: PropTypes.shape({
    UserName: PropTypes.string,
    CreatedAt: PropTypes.any,
    Role: PropTypes.string,
    Name: PropTypes.string,
    Email: PropTypes.string,
    Imgsrc: PropTypes.string,
  }),
};
