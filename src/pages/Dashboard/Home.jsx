import UsersIcon from "../../assets/icons/users.svg?react";
import OnlineUserIcon from "../../assets/icons/online-user.svg?react";
import DiskOccupiedIcon from "../../assets/icons/disk-occupied.svg?react";
import RecentItem from "../../components/RecentItem";

export default function Home() {
  const users = [
    {
      name: "Chirag Bimali",
      createdAt: new Date("2024-12-10T12:00:00Z"),
    },
    {
      name: "Alish Shrestha",
      createdAt: new Date("2024-12-09T11:55:00Z"), // 5 minutes ago
    },
    {
      name: "Krishna Sharma",
      createdAt: new Date("2024-02-10T11:00:00Z"), // 1 hour ago
    },
    {
      name: "Nitin Timinshina",
      createdAt: new Date("2024-12-19T12:00:00Z"), // 1 day ago
    },
    {
      name: "Kishor Mainali",
      createdAt: new Date("2024-12-14T12:00:00Z"), // 6 days ago
    },
  ];

  const posts = [
    {
      name: "Mastering Transform and Translate in Tailwind CSS",
      createdAt: new Date("2024-12-10T12:00:00Z"),
    },
    {
      name: "How to Use Tailwind CSS for Powerful Element Transitions",
      createdAt: new Date("2024-12-09T11:55:00Z"), // 5 minutes ago
    },
    {
      name: "Transform Your Designs with Tailwind CSS: A Complete Guide",
      createdAt: new Date("2024-02-10T11:00:00Z"), // 1 hour ago
    },
    {
      name: "Smooth Animations in Tailwind: Using Translate and Transition Effects",
      createdAt: new Date("2024-12-19T12:00:00Z"), // 1 day ago
    },
    {
      name: "Tailwind CSS Tricks: Moving Elements with Translate-X and Translate-Y",
      createdAt: new Date("2024-12-14T12:00:00Z"), // 6 days ago
    },
  ];

  return (
    <div className="prose max-w-none h-full w-full px-12 py-8 flex flex-col">
      <h2>Dashboard</h2>
      <div className="flex w-full gap-24 md:gap-48 xl:gap-96">
        <div className="flex flex-col items-center justify-crnter tooltip tooltip-right" data-tip="124938">
          <UsersIcon className="h-32 w-32 fill-base-content/80 mb-2" />
          <small className="block">No. of users</small>
          <span className="block text-4xl font-thin mt-3">124K</span>
        </div>
        <div className="flex flex-col items-center justify-center w-fit tooltip tooltip-right" data-tip="97102 MB">
          <DiskOccupiedIcon className="h-32 w-32  fill-base-content/80 mb-2" />
          <small className="block">Disk Usage</small>
          <span className="block text-4xl font-thin mt-3">97GB</span>
        </div>
        <div className="flex flex-col items-center justify-center w-fit tooltip tooltip-right" data-tip="12345">
          <OnlineUserIcon className="h-32 w-32 fill-base-content/80 mb-2" />
          <small className="block">Active Users</small>
          <span className="block text-4xl font-thin mt-3">12k</span>
        </div>
      </div>
      <div className="flex gap-8">
        <div className="w-1/3">
          <h3>Recent Users</h3>
          <div className="flex  flex-col">
            <RecentItem details={users[1]} />
            <RecentItem details={users[2]} />
            <RecentItem details={users[3]} />
            <RecentItem details={users[4]} />
            <RecentItem details={users[0]} />
          </div>
        </div>
        <div className="w-1/3">
          <h3>Recent Posts</h3>
          <div className="flex  flex-col">
            <RecentItem details={posts[0]} />
            <RecentItem details={posts[1]} />
            <RecentItem details={posts[2]} />
            <RecentItem details={posts[3]} />
            <RecentItem details={posts[4]} />
          </div>
        </div>
      </div>
    </div>
  );
}
