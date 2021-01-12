const path = require('path');
const webpack = require('webpack');

module.exports = {
  resolve: {
    extensions: ['.js']
  },
  mode: process.env.NODE_ENV,
  devtool: process.env.NODE_ENV === 'production' ? 'none' : 'inline-source-map',
  module: {
    rules: [
      {
        test: /\.js?$/,
        loader: {
          loader: 'babel-loader',
          options: {
            presets: ['@babel/preset-env']
          }
        }
      },
    ]
  },
  entry: './MarkdrawBrowser/JavaScript/index.js',
  output: {
    path: path.join(__dirname, '/MarkdrawBrowser/wwwroot'),
    filename: 'markdrawBrowser.bundle.js'
  }
};