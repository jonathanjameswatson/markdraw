const path = require("path");

module.exports = {
  resolve: {
    extensions: [".js"],
  },
  mode: process.env.NODE_ENV ?? "development",
  devtool: process.env.NODE_ENV === "production" ? "none" : "inline-source-map",
  module: {
    rules: [
      {
        test: /\.js?$/,
        include: path.resolve(__dirname, 'src/MarkdrawBrowser/JavaScript'),
        loader: {
          loader: "babel-loader",
          options: {
            presets: ["@babel/preset-env"],
            cacheCompression: false,
            cacheDirectory: true,
          },
        },
      },
    ],
  },
  entry: "./src/MarkdrawBrowser/JavaScript/index.js",
  output: {
    path: path.resolve(__dirname, "src/MarkdrawBrowser/wwwroot"),
    filename: "markdrawBrowser.bundle.js",
  },
};
