import PropTypes from "prop-types";
import { ApiContext } from "../contexts/ApiContext";

import axios from "axios";

export const ApiProvider = ({ children }) => {
  async function getAllContentMeta(options = {}) {
    const {
      id = "",
      userId = "",
      isDescending = true,
      shortBy = ["createdAt"],
      page = 1,
      pageSize = 8,
    } = {
      id: "",
      userId: "",
      isDescending: true,
      shortBy: ["createdAt"],
      page: 1,
      pageSize: 8,
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

      shortBy.forEach((shortName) => {
        resource += `ShortBy=${shortName}&`;
      });
      resource += `Page=${page}&PageSize=${pageSize}&`;
      console.log(resource)

      const response = await axios.get(resource);
      return response.data;
    } catch (err) {
      console.log(err);
      throw err;
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
  getContentById("0a0fdf1c-944f-4053-8689-565d3b40d572");
  getAllContentMeta();

  const value = {
    getAllContentMeta,
    getContentById,
  };
  return <ApiContext.Provider value={value}>{children}</ApiContext.Provider>;
};

ApiProvider.propTypes = { children: PropTypes.element };
