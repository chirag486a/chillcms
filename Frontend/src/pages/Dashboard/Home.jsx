import UsersIcon from "../../assets/icons/users.svg?react";
import DiskOccupiedIcon from "../../assets/icons/disk-occupied.svg?react";
import OnlineUserIcon from "../../assets/icons/online-user.svg?react";
import RecentPost from "./Components/Home/RecentPost";
import RecentUser from "./Components/Home/RecentUser";
import { ApiContext } from "../../contexts/ApiContext";
import { useContext, useEffect, useState } from "react";
import { AuthContext } from "../../contexts/AuthContext";
import { useNavigate } from "react-router-dom";
import { useToast } from "../../contexts/ToastContext";

export default function Home() {
  const [loadingPosts, setLoadingPosts] = useState(true);
  const [loadingUsers, setLoadingUsers] = useState(true);
  // const users = [
  //   {
  //     name: "Chirag Bimali",
  //     createdAt: new Date("2024-12-10T12:00:00Z"),
  //   },
  //   {
  //     name: "Alish Shrestha",
  //     createdAt: new Date("2024-12-09T11:55:00Z"), // 5 minutes ago
  //   },
  //   {
  //     name: "Krishna Sharma",
  //     createdAt: new Date("2024-02-10T11:00:00Z"), // 1 hour ago
  //   },
  //   {
  //     name: "Nitin Timinshina",
  //     createdAt: new Date("2024-12-19T12:00:00Z"), // 1 day ago
  //   },
  //   {
  //     name: "Kishor Mainali",
  //     createdAt: new Date("2024-12-14T12:00:00Z"), // 6 days ago
  //   },
  // ];

  const { currentUser } = useContext(AuthContext);
  const navigate = useNavigate();
  const { addToast } = useToast();

  const [posts, setPosts] = useState(null);
  const [users, setUsers] = useState(null);
  const { getAllContentMeta, getAllUsers } = useContext(ApiContext);
  useEffect(() => {
    const fetchPostsData = async () => {
      try {
        const data = await getAllContentMeta({
          isDescending: true,
          shortBy: ["CreatedAt"],
          page: 1,
          pageSize: 3,
          fields: "CreatedAt,ContentTitle,Id",
        });
        if (!data.data) {
          throw new Error("Data is undefined");
        }
        setPosts([...data.data]);
        setLoadingPosts(false);
      } catch (err) {
        console.log(err);
      }
    };

    const fetchUsersData = async () => {
      try {
        const data = await getAllUsers({
          isDescending: true,
          shortBy: ["CreatedAt"],
          page: 1,
          pageSize: 3,
          fields: "CreatedAt,Name,Id",
        });
        if (!data.data) {
          throw new Error("Data is undefined");
        }
        setPosts([...data.data]);
        setLoadingUsers(false);
      } catch (err) {
        if (!currentUser) {
          navigate("/login");
          addToast("Login to get access");
        }
        console.error(err);
      }
    };

    fetchPostsData();
    fetchUsersData();
  }, [getAllContentMeta, getAllUsers, addToast, currentUser, navigate]);

  // const posts = [
  //   {
  //     name: "Mastering Transform and Translate in Tailwind CSS",
  //     createdAt: new Date("2024-12-10T12:00:00Z"),
  //   },
  //   {
  //     name: "How to Use Tailwind CSS for Powerful Element Transitions",
  //     createdAt: new Date("2024-12-09T11:55:00Z"), // 5 minutes ago
  //   },
  //   {
  //     name: "Transform Your Designs with Tailwind CSS: A Complete Guide",
  //     createdAt: new Date("2024-02-10T11:00:00Z"), // 1 hour ago
  //   },
  //   {
  //     name: "Smooth Animations in Tailwind: Using Translate and Transition Effects",
  //     createdAt: new Date("2024-12-19T12:00:00Z"), // 1 day ago
  //   },
  //   {
  //     name: "Tailwind CSS Tricks: Moving Elements with Translate-X and Translate-Y",
  //     createdAt: new Date("2024-12-14T12:00:00Z"), // 6 days ago
  //   },
  // ];

  return (
    <div>
      <div>
        <div className="prose max-w-none h-full w-full px-12 py-8 flex flex-col">
          <h2>Dashboard</h2>
          <div className="flex w-full gap-24 md:gap-26 xl:gap-96 mb-12">
            <div
              className="flex flex-col items-center justify-crnter tooltip tooltip-right"
              data-tip="124938"
            >
              <UsersIcon className="sm:h-24 sm:w-24 w-20 h-20 lg:h-32 lg:w-32 fill-base-content/80 mb-2" />
              <small className="block">No. of users</small>
              <span className="block text-4xl font-thin mt-3">124K</span>
            </div>
            <div
              className="flex flex-col items-center justify-center w-fit tooltip tooltip-right"
              data-tip="97102 MB"
            >
              <DiskOccupiedIcon className="sm:h-24 sm:w-24 w-20 h-20 lg:h-32 lg:w-32 fill-base-content/80 mb-2" />
              <small className="block">Disk Usage</small>
              <span className="block text-4xl font-thin mt-3">97GB</span>
            </div>
            <div
              className="flex flex-col items-center justify-center w-fit tooltip tooltip-right"
              data-tip="12345"
            >
              <OnlineUserIcon className="sm:h-24 sm:w-24 w-20 h-20 lg:h-32 lg:w-32 fill-base-content/80 mb-2" />
              <small className="block">Active Users</small>
              <span className="block text-4xl font-thin mt-3">12k</span>
            </div>
          </div>
          <div className="flex gap-8 w-full">
            <div className="lg:w-1/3 w-1/2 md:w-1/2 xl:w-1/3">
              <h3>Recent Users</h3>
              <div className="flex  flex-col">
                {/* <RecentUser details={users[1]} />
                <RecentUser details={users[2]} />
                <RecentUser details={users[3]} /> */}

                {!loadingPosts &&
                  posts.map((post) => {
                    return <RecentUser key={post.Id} details={post} />;
                  })}
              </div>
            </div>
            <div className="lg:w-1/3 w-1/2 md:w-1/2 xl:w-1/3">
              <h3>Recent Posts</h3>
              <div className="flex  flex-col w-fit">
                {!loadingPosts &&
                  posts.map((post) => {
                    return <RecentPost key={post.Id} details={post} />;
                  })}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
