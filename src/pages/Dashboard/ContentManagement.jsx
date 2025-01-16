import ContentManagementTC from "../../components/ContentManagementTC";
import Checkbox from "@mui/material/Checkbox";

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
      title: "PolySoundex is a multi-language Soundex library for .NET that enables phonetic searching and indexing across various languages. Easily configure Soundex mappings for different scripts, including English and Nepali, to enhance search functionality. Perfect for applications requiring robust name matching in multilingual environments.",
      createdAt: new Date() - 100000000000,
      status: "Pending",
    },
  ];
  return (
    <div className="prose max-w-none h-full w-fit px-12 py-8 flex flex-col">
      <h2>Content Management</h2>
      <div className="w-12/12">
        <div className="px-4 pl-0 inline-block h-fit w-full rounded-lg font-bold">
          <div className="flex flex-row w-fit pr-4 rounded-sm pl-0 py-4 items-center gap-2">
            <div className="w-fit">
              <Checkbox />
            </div>
            <div className="w-96 text-left">Title</div>
            <div className="w-40 text-center">Date</div>
            <div className="w-20 text-right">Status</div>
          </div>
        </div>
        <div className="">
          <ContentManagementTC content={contents[0]} />
          <ContentManagementTC content={contents[1]} />
          <ContentManagementTC content={contents[2]} />
          <ContentManagementTC content={contents[3]} />
        </div>
      </div>
    </div>
  );
}
