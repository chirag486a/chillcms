/** @type {import('tailwindcss').Config} */

const daisyui = require("daisyui");
const typography = require("@tailwindcss/typography");

module.exports = {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      fontFamily: {
        sans: ["Roboto", "Arial", "sans-serif"], // Replace default sans-serif with Roboto
      },
    },
  },
  plugins: [typography, daisyui],
  daisyui: {
    themes: ["light", "dark", "cupcake"],
    darkTheme: "dark",
  },
};
