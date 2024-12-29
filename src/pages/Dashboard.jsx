import { Outlet } from "react-router-dom";
import MenuIcon from "../assets/icons/menu-icon.svg?react";
import SettingIcon from "../assets/icons/setting-icon.svg?react";
import NavBar from "../components/NavBar";
import TopPanal from "../components/TopPanal";

export default function Dashboard() {
  return (
    <div className="w-full">
      <div className="grid grid-cols-[auto_1fr] grid-rows-[auto_1fr]">
        <div className="sticky flex flex-col top-0 h-screen left-0 row-span-2 border-r-[0.5px] border-base-300 ">
          {/* <div className="p-4 w-min">
            <button className="btn btn-circle bg-base-100 border-none shadow-none">
              <MenuIcon className="fill-base-content" />
            </button>
          </div> */}
          <div className="flex-grow flex flex-col justify-between pt-6 p-2">
            <NavBar />
            <div className="p-4 group">
              <button
                className="bg-base-100 border-none btn btn-circle shadow-none flex items-center justify-center tooltip tooltip-right"
                data-tip="Setting"
              >
                <SettingIcon className="fill-base-content transform group-active:transition-transform group-active:rotate-45 duration-300" />
              </button>
            </div>
          </div>
        </div>

        <div>
          <TopPanal />
        </div>
        <div>
          <Outlet />
        </div>
      </div>
    </div>
  );
}
