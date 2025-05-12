import ContentManagementTC from "./Components/ContentManagement/ContentManagementTC";
import Checkbox from "@mui/material/Checkbox";
import { ChevronLeft } from "@mui/icons-material";
import { ChevronRight } from "@mui/icons-material";
import FilterAltOutlinedIcon from "@mui/icons-material/FilterAltOutlined";
import ArchiveOutlinedIcon from "@mui/icons-material/ArchiveOutlined";
import DeleteOutlineOutlinedIcon from "@mui/icons-material/DeleteOutlineOutlined";
import { useContext, useEffect, useState } from "react";
import { ApiContext } from "../../contexts/ApiContext";

function SearchToolBar() {
  return (
    <>
      <div className="flex justify-start w-5/6 gap-24">
        <label className="input input-bordered flex items-center gap-2 w-1/2">
          <input type="text" className="grow" placeholder="Search" />
          <kbd className="kbd kbd-sm">⌘</kbd>
          <kbd className="kbd kbd-sm">K</kbd>
        </label>
        <div className="flex gap-3">
          <button
            className="btn hover:btn-neutral tooltip tooltip-top"
            data-tip="Filter"
          >
            <FilterAltOutlinedIcon />
          </button>
          <button
            className="btn hover:btn-error tooltip tooltip-top tooltip-error"
            data-tip="Delete"
          >
            <DeleteOutlineOutlinedIcon />
          </button>
          <button
            className="btn hover:btn-neutral tooltip tooltip-top"
            data-tip="Archive"
          >
            <ArchiveOutlinedIcon />
          </button>
        </div>
      </div>
    </>
  );
}

export default function ContentManagement() {
  const [page, setPage] = useState(1);
  const [contents, setContents] = useState(null);
  const [totals, setTotals] = useState(0);
  const pageSize = 8;
  const { getAllContentMeta } = useContext(ApiContext);
  const [loading, setLoading] = useState(true);

  const [activateLeft, setActivateLeft] = useState(false);
  const [activateRight, setActivateRight] = useState(false);

  function handleLeft() {
    if (activateLeft) {
      setPage(page - 1);
    }
  }
  function handleRight() {
    if (activateRight) {
      setPage(page + 1);
    }
  }

  useEffect(() => {
    async function loadContents() {
      try {
        const data = await getAllContentMeta({
          isDescending: true,
          shortBy: ["CreatedAt"],
          page: page,
          pageSize: pageSize,
          fields: "CreatedAt,ContentTitle,Id",
        });
        if (!data.data) {
          throw new Error("Data is undefined");
        }
        setContents(data.data);
        setTotals(data?.total);

        if (page * pageSize < data.total) {
          setActivateRight(true);
        } else setActivateRight(false);
        if (page > 1) {
          setActivateLeft(true);
        } else setActivateLeft(false);

        setLoading(false);
      } catch (err) {
        console.log(err);
      }
    }
    loadContents();
  }, [getAllContentMeta, page]);
  return (
    !loading && (
      <>
        <div className="px-12 py-8 flex border-b-[0.5px] border-base-300">
          <div className="flex w-full items-center">
            <SearchToolBar />
          </div>
        </div>
        <div className="prose max-w-none h-fit w-fit px-12 py-8 flex flex-col">
          <div className="flex justify-between items-baseline">
            <div>
              <h2 className="mt-0 w-fit">Content Management</h2>
            </div>
            <div className="flex items-center gap-4">
              <span className="min-w-max">
                <span className="w-6 inline-block">
                  {page * pageSize - (pageSize - 1)}
                </span>
                <span className="w-6 inline-block">-</span>
                <span className="w-6 inline-block">
                  {(totals >= page * pageSize && page * pageSize) ||
                    (totals < page * pageSize &&
                      (page - 1) * pageSize + (totals - pageSize))}
                </span>
                <span className="w-6 inline-block"> of </span>
                <span className="w-6 inline-block">{totals}</span>
              </span>
              <div>
                <button
                  className={
                    activateLeft
                      ? "h-fit btn btn-ghost hover:bg-base-200 shadow-none rounded-full p-3 w-fit"
                      : "btn btn-ghost hover:bg-transparent cursor-default rounded-full opacity-50 !p-3 w-fit h-fit"
                  }
                  onClick={handleLeft}
                >
                  <ChevronLeft />
                </button>
                <button
                  className={
                    activateRight
                      ? "h-fit btn btn-ghost hover:bg-base-200 shadow-none rounded-full p-3 w-fit"
                      : "btn btn-ghost hover:bg-transparent cursor-default rounded-full opacity-50 !p-3 w-fit h-fit"
                  }
                  onClick={handleRight}
                >
                  <ChevronRight className="h-2 w-2" />
                </button>
              </div>
            </div>
          </div>
          <div className="w-fit flex flex-col">
            <div className="pr-4 bg-base-300 pl-0 inline-block h-fit w-fit rounded-none font-bold">
              <div className="flex flex-row w-fit rounded-sm pl-0 justify-between items-center gap-36">
                <div className="flex items-center gap-4">
                  <div className="w-fit">
                    <Checkbox />
                  </div>
                  <div className="w-96 text-left">Title</div>
                </div>
                <div className="w-40 text-center">Date</div>
                <div className="w-20 text-right">Status</div>
              </div>
            </div>
            <div className="max-w-min">
              {contents.map((content) => {
                return (
                  <ContentManagementTC key={content?.Id} content={content} />
                );
              })}
            </div>
          </div>
        </div>
      </>
    )
  );
}
