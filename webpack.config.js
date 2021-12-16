const path = require('path');

module.exports = {
  entry: path.resolve(__dirname, './src/MarkdrawBrowser/JavaScript/index.js'),
  module: {
    rules: [
      {
        test: /\.(js)$/,
        exclude: /node_modules/,
        use: ['babel-loader']
      },
    ],
  },
  resolve: {
    extensions: ['*', '.js'],
  },
  output: {
    path: path.resolve(__dirname, './src/MarkdrawBrowser/wwwroot'),
    filename: 'markdrawBrowser.bundle.js',
  },
  mode: process.env.NODE_ENV ?? 'development',
  devtool: process.env.NODE_ENV === 'production' ? 'none' : 'inline-source-map',
};
