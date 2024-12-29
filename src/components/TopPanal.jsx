import FilterAltOutlinedIcon from "@mui/icons-material/FilterAltOutlined";
import ArchiveOutlinedIcon from '@mui/icons-material/ArchiveOutlined';
import DeleteOutlineOutlinedIcon from '@mui/icons-material/DeleteOutlineOutlined';
import AccountCircleIcon from "../assets/icons/account-circle.svg?react";
import { useLocation } from "react-router-dom";
import PropTypes from "prop-types";
function SearchToolBar({ isDashboard }) {
  if (!isDashboard)
    return (
      <div className="flex justify-start w-5/6 gap-24">
        <label className="input input-bordered flex items-center gap-2 w-1/2">
          <input type="text" className="grow" placeholder="Search" />
          <kbd className="kbd kbd-sm">⌘</kbd>
          <kbd className="kbd kbd-sm">K</kbd>
        </label>
        <div className="flex gap-3">
          <button className="btn hover:btn-neutral tooltip tooltip-top" data-tip="Filter">
            <FilterAltOutlinedIcon />
          </button>
          {/* <select className="select select-bordered">
            <option defaultValue="">Action</option>
            <option value="delete"> Delete </option>
            <option value="archive">Archive</option> 
          </select> */}
          <button className="btn hover:btn-error tooltip tooltip-top tooltip-error" data-tip="Delete">
            <DeleteOutlineOutlinedIcon />
          </button>
          <button className="btn hover:btn-neutral tooltip tooltip-top" data-tip="Archive">
            <ArchiveOutlinedIcon />
          </button>

        </div>
      </div>
    );
  else return <></>;
}

SearchToolBar.propTypes = {
  isDashboard: PropTypes.bool,
};

export default function TopPanal() {
  const normalizePath = useLocation().pathname.replace(/\/+$/, "");
  let isDashboard = false;
  if (normalizePath === "/dashboard") {
    isDashboard = true;
    return <></>;
  }
  return (
    <div className="px-12 py-4 flex border-b-[0.5px] border-base-300">
      <div className="flex w-full items-center">
        <SearchToolBar isDashboard={isDashboard} />
        <button className="flex flex-col ml-auto items-center justify-center p-4 btn btn-link no-underline text-base-content font-thin  bg-base-100 h-max">
          <div className="flex justify-center">
            <AccountCircleIcon className="h-10 w-10 fill-base-content" />
          </div>
          <a>Admin</a>
        </button>
      </div>
    </div>
  );
}
