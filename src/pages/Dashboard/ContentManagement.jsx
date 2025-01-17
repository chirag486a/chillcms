import ContentManagementTC from "../../components/ContentManagementTC";
import Checkbox from "@mui/material/Checkbox";
import { ChevronLeft } from "@mui/icons-material";
import { ChevronRight } from "@mui/icons-material";

export default function ContentManagement() {
  const contents = [
    {
      title: "Road to hail ",
      createdAt: new Date() - 100000000000,
      status: "Pending",
    },
    {
      title: "All is well!",
      createdAt: Date.now() - 1000000,
      status: "Uploaded",
    },
    {
      title: "Shree antu ko love story",
      createdAt: Date.now() - 123100900000,
      status: "Uploaded",
    },
    {
      title:
        "PolySoundex is a multi-language Soundex library for .NET that enables phonetic searching and indexing across various languages. Easily configure Soundex mappings for different scripts, including English and Nepali, to enhance search functionality. Perfect for applications requiring robust name matching in multilingual environments.",
      createdAt: new Date() - 100000000000,
      status: "Pending",
    },
  ];
  return (
    <div className="prose max-w-none h-full w-fit px-12 py-8 flex flex-col">
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
            <div className="w-fit">
              <Checkbox />
            </div>
            <div className="w-96 text-left">Title</div>
            <div className="w-40 text-center">Date</div>
            <div className="w-20 text-right">Status</div>
          </div>
        </div>
        <div className="w-full">
          <ContentManagementTC content={contents[0]} />
          <ContentManagementTC content={contents[1]} />
          <ContentManagementTC content={contents[2]} />
          <ContentManagementTC content={contents[3]} />
          <ContentManagementTC content={contents[3]} />
          <ContentManagementTC content={contents[3]} />
          <ContentManagementTC content={contents[3]} />
          <ContentManagementTC content={contents[3]} />
          <ContentManagementTC content={contents[3]} />
        </div>
      </div>
    </div>
  );
}
