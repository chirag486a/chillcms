import Checkbox from "@mui/material/Checkbox";

export default function TodoTC() {
  return (
    <>
      <button className="btn btn-ghost border-base-300 border-0 pl-0 pr-4  rounded-none inline-block h-fit w-full  font-thin even:bg-base-200 even:btn-ghost">
        <div className="flex flex-row w-full justify-between rounded-lg pl-0  py-0 items-center">
          <div className="flex items-center justify-center gap-4">
            <div className="w-fit">
              <Checkbox />
            </div>
            <div className="text-center flex gap-4 min-w-96 max-w-96 items-center">
              <span className="max-w-96 text-ellipsis text-nowrap overflow-hidden h-4">I am going to be the best version of me from now and I will conqore the the world</span>
            </div>
          </div>
          <div className="w-40 text-center">Hi</div>
          <div className="w-20 text-center">what</div>
          <div className="w-40 text-center">why</div>
        </div>
      </button>
    </>
  );
}
