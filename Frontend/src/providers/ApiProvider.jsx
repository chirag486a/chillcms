import PropTypes from "prop-types";
import { ApiContext } from "../contexts/ApiContext";
import { useContext } from "react";

import axios from "axios";

import { AuthContext } from "../contexts/AuthContext";

export const ApiProvider = ({ children }) => {
  const { currentUser } = useContext(AuthContext);

  async function getAllContentMeta(options = {}) {
    const {
      id = "",
      userId = "",
      isDescending = true,
      shortBy = ["createdAt"],
      page = 1,
      pageSize = 8,
      fields = "",
    } = {
      id: "",
      userId: "",
      isDescending: true,
      shortBy: ["createdAt"],
      page: 1,
      pageSize: 8,
      fields: "",
      ...options,
    };
    try {
      let resource = `http://localhost:5235/api/content?`;
      if (id != "") {
        resource += `Id=${id}&`;
      }
      if (userId != "") {
        resource += `UserId=${userId}&`;
      }
      resource += `IsDescending=${isDescending}&`;
      if (fields != "") {
        resource += `Fields=${fields}&`;
      }
      shortBy.forEach((shortName) => {
        resource += `ShortBy=${shortName}&`;
      });
      resource += `Page=${page}&PageSize=${pageSize}&`;

      const response = await axios.get(resource);
      return response.data;
    } catch (err) {
      err;
      throw err;
    }
  }

  async function getAllUsers(options = {}) {
    const {
      id = "",
      userId = "",
      isDescending = true,
      shortBy = ["createdAt"],
      page = 1,
      pageSize = 8,
      fields = "",
    } = {
      id: "",
      userId: "",
      isDescending: true,
      shortBy: ["createdAt"],
      page: 1,
      pageSize: 8,
      fields: "",
      ...options,
    };
    try {
      let resource = `http://localhost:5235/api/users?`;
      if (id != "") {
        resource += `Id=${id}&`;
      }
      if (userId != "") {
        resource += `UserId=${userId}&`;
      }
      resource += `IsDescending=${isDescending}&`;
      if (fields != "") {
        resource += `Fields=${fields}&`;
      }
      shortBy.forEach((shortName) => {
        resource += `ShortBy=${shortName}&`;
      });
      resource += `Page=${page}&PageSize=${pageSize}&`;

      if (!currentUser?.token) {
        throw new Error("Login to get access");
      }

      const response = await axios.get(resource, {
        headers: {
          Authorization: `Bearer ${currentUser.token}`,
        },
      });
      console.log(response.data.data);
      return response.data;
    } catch (err) {
      throw new Error(err);
    }
  }
  async function getContentById(id) {
    try {
      if (!id) {
        throw new Error("Id is null or undefined");
      }
      const response = await axios.get(
        `http://localhost:5235/api/content?id=${id}`
      );
      const dataArray = response.data.data;
      if (dataArray.length == 0) {
        throw new Error("Content with the id doesn't exist");
      }
      return response.data;
    } catch (err) {
      console.log(err);
      throw err;
    }
  }
  // getAllContentMeta();
  // getContentById("0a0fdf1c-944f-4053-8689-565d3b40d572");
  // getAllContentMeta();

  const value = {
    getAllContentMeta,
    getContentById,
    getAllUsers,
  };
  return <ApiContext.Provider value={value}>{children}</ApiContext.Provider>;
};

ApiProvider.propTypes = { children: PropTypes.element };
