"use strict";

/*global require*/
/*global process*/

const gulp = require("gulp");
const changed = require('gulp-changed');
const sass = require("gulp-sass")(require("node-sass"));
const webpack_stream = require("webpack-stream");
const webpack_config = require("./webpack.config.js");

const dirs = {
  scss: {
    src: "src/MarkdrawBrowser/Styles",
    dest: "src/MarkdrawBrowser/wwwroot/css",
  },
  js: {
    src: "src/MarkdrawBrowser/JavaScript",
    dest: "src/MarkdrawBrowser/wwwroot",
  },
  grammars: {
    src: "node_modules/prismjs/components",
    dest: "src/MarkdrawBrowser/wwwroot/grammars",
  },
};

const buildStyles = () => gulp
    .src(`${dirs.scss.src}/*.scss`)
    .pipe(changed(dirs.scss.dest))
    .pipe(sass().on("error", sass.logError))
    .pipe(gulp.dest(dirs.scss.dest));
buildStyles.displayName = 'build-styles';

const buildScripts = () => webpack_stream(webpack_config).pipe(gulp.dest(dirs.js.dest));
buildScripts.displayName = 'build-scripts';

const buildGrammars = () => gulp
  .src([`${dirs.grammars.src}/**`])
  .pipe(changed(dirs.grammars.dest))
  .pipe(gulp.dest(dirs.grammars.dest));
buildGrammars.displayName = 'build-grammars';

const build = gulp.parallel(buildStyles, buildScripts, buildGrammars);

const watch = () => {
  gulp.watch(`${dirs.scss.src}/**/*.scss`, { ignoreInitial: false }, buildStyles);
  gulp.watch(`${dirs.js.src}/**/*.js`, { ignoreInitial: false }, buildScripts);
};

exports.watch = watch;
exports.build = build;
exports.buildGrammars = buildGrammars;
