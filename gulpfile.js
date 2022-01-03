'use strict';

/*global require*/
/*global process*/

const path = require('path');
const gulp = require('gulp');
const changed = require('gulp-changed');
const sass = require('gulp-sass')(require('node-sass'));
const webpackStream = require('webpack-stream');
const webpackConfig = require('./webpack.config.js');
const commonMarkSpec = require('commonMark-spec');
const Vinyl = require('vinyl')
const stream = require('stream')

const dirs = {
  scss: {
    src: './src/MarkdrawBrowser/Styles',
    dest: './src/MarkdrawBrowser/wwwroot/css',
  },
  js: {
    src: './src/MarkdrawBrowser/JavaScript',
    dest: './src/MarkdrawBrowser/wwwroot',
  },
  grammars: {
    src: './node_modules/prismjs/components',
    dest: './src/MarkdrawBrowser/wwwroot/grammars',
  },
  commonMarkJson: {
    filename: 'commonmark.json',
    dest: './src/MarkdrawBrowser/wwwroot',
  }
};


const stringSrc = (filename, string) => {
  var src = stream.Readable({objectMode: true})
  src._read = function () {
    this.push(new Vinyl({
      cwd: "",
      base: null,
      path: filename,
      contents: Buffer.from(string, 'utf-8')
    }))
    this.push(null)
  }
  return src
}


const buildStyles = () => gulp
  .src(`${dirs.scss.src}/*.scss`)
  .pipe(changed(dirs.scss.dest))
  .pipe(sass().on('error', sass.logError))
  .pipe(gulp.dest(dirs.scss.dest));

buildStyles.displayName = 'build-styles';
exports.buildStyles = buildStyles;


const buildScripts = () => webpackStream(webpackConfig).pipe(gulp.dest(dirs.js.dest));

buildScripts.displayName = 'build-scripts';
exports.buildScripts = buildScripts;


const buildGrammars = () => gulp
  .src([`${dirs.grammars.src}/**`])
  .pipe(changed(dirs.grammars.dest))
  .pipe(gulp.dest(dirs.grammars.dest));

buildGrammars.displayName = 'build-grammars';
exports.buildGrammars = buildGrammars;


const newCommonMarkSpec = commonMarkSpec.tests.map((test) => test.markdown);

const buildCommonMarkJson = () =>
  stringSrc(dirs.commonMarkJson.filename, JSON.stringify(newCommonMarkSpec))
    .pipe(changed(dirs.commonMarkJson.dest))
    .pipe(gulp.dest(dirs.commonMarkJson.dest));

buildCommonMarkJson.displayName = 'build-commonmark-json';
exports.buildCommonMarkJson = buildCommonMarkJson;


const build = gulp.parallel(buildStyles, buildScripts, buildGrammars, buildCommonMarkJson);

exports.build = build;


const watchInner = () => {
  gulp.watch(`${dirs.scss.src}/**/*.scss`, {ignoreInitial: false}, buildStyles);
  gulp.watch(`${dirs.js.src}/**/*.js`, {ignoreInitial: false}, buildScripts);
}

watchInner.displayName = 'watch-inner'

const watch = gulp.parallel(buildGrammars, buildCommonMarkJson, watchInner);

exports.watch = watch;


