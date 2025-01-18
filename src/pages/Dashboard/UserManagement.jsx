import Checkbox from "@mui/material/Checkbox";
import { ChevronLeft } from "@mui/icons-material";
import { ChevronRight } from "@mui/icons-material";
import UserManagementTC from "../../components/UserManagementTC";

export default function UserManagement() {
  const userContent = [
    {
      name: "Chirag Bimali",
      createdAt: Date.now() - 1000000000,
      role: "Admin",
      username: "chirag",
      email: "chirag.486a@gmail.com",
      imgsrc: "https://picsum.photos/200",
    },
  ];

  return (
    <div className="prose max-w-none h-full w-full px-12 py-8 flex flex-col">
      <div className="flex justify-between items-baseline">
        <div>
          <h2 className="mt-0 w-fit">Content Management</h2>
        </div>
        <div className="flex items-center gap-4">
          <span>1-10 of 203</span>
          <div>
            <button className="btn btn-ghost hover:bg-transparent cursor-default rounded-full opacity-50 !p-3 w-fit h-fit">
              <ChevronLeft />
            </button>
            <button className="!h-fit btn btn-ghost hover:bg-base-200 shadow-none rounded-full p-3 w-fit">
              <ChevronRight className="h-2 w-2" />
            </button>
          </div>
        </div>
      </div>
      <div className="w-12/12">
        <div className="pr-4 bg-base-300 pl-0 inline-block h-fit w-full rounded-none font-bold">
          <div className="flex flex-row w-full rounded-sm pl-0 justify-between items-center gap-2">
            <div className="flex gap-4 items-center justify-center">
              <div className="w-fit">
                <Checkbox />
              </div>
              <div className="w-40 text-left">User</div>
            </div>
            <div className="w-40 text-center">Signup</div>
            <div className="w-20 text-center">Role</div>
            <div className="w-40 text-center">Username</div>
            <div className="w-60 text-center">Email</div>
          </div>
        </div>
        <div className="w-full">
          <UserManagementTC content={userContent[0]} />
          <UserManagementTC content={userContent[0]} />
          <UserManagementTC content={userContent[0]} />
          <UserManagementTC content={userContent[0]} />
          <UserManagementTC content={userContent[0]} />
          <UserManagementTC content={userContent[0]} />
          <UserManagementTC content={userContent[0]} />
          <UserManagementTC content={userContent[0]} />
          <UserManagementTC content={userContent[0]} />
        </div>
      </div>
    </div>
  );
}
