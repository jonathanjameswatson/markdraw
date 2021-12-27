const path = require('path');
const TerserPlugin = require('terser-webpack-plugin');

module.exports = {
  entry: path.resolve(__dirname, './src/MarkdrawBrowser/JavaScript/index.js'),
  module: {
    rules: [
      {
        test: /\.(js)$/,
        include: path.resolve(__dirname, 'src/MarkdrawBrowser/JavaScript'),
        use: {
          loader: 'babel-loader',
          options: {
            presets: ["@babel/preset-env"],
            cacheCompression: false,
            cacheDirectory: true,
            plugins: [
              [
                "prismjs",
                {
                  "languages": ["html"],
                  "plugins": [
                    "autoloader",
                    "custom-class",
                  ],
                  theme: false,
                }
              ]
            ]
          },
        },
      },
    ],
  },
  resolve: {
    extensions: ['.js'],
  },
  output: {
    path: path.resolve(__dirname, './src/MarkdrawBrowser/wwwroot'),
    filename: 'markdrawBrowser.bundle.js',
  },
  mode: process.env.NODE_ENV ?? 'development',
  devtool: process.env.NODE_ENV === 'production' ? false : 'inline-source-map',
  optimization: {
    minimizer: [
      new TerserPlugin({
        extractComments: false,
      })
    ],
  },
};
