"use strict";

/*global require*/
/*global process*/

const gulp = require("gulp");
const sass = require("gulp-sass")(require("node-sass"));
const browserSync = require("browser-sync").create();
const webpack_stream = require("webpack-stream");
const webpack_config = require("./webpack.config.js");

const dirs = {
  scss: {
    src: "MarkdrawBrowser/Styles",
    dest: "MarkdrawBrowser/wwwroot/css",
  },
  js: {
    src: "MarkdrawBrowser/JavaScript",
    dest: "MarkdrawBrowser/wwwroot",
  },
  grammars: {
    src: "node_modules/prismjs/components",
    dest: "MarkdrawBrowser/wwwroot/grammars",
  },
};

const production = process.env.NODE_ENV === "production";

const stylesTask = function stylesTask(done) {
  gulp
    .src(`${dirs.scss.src}/*.scss`)
    .pipe(sass().on("error", sass.logError))
    .pipe(gulp.dest(dirs.scss.dest));

  if (!production) {
    browserSync.reload();
    done();
  }
};

const scriptsTask = function scriptsTask(done) {
  webpack_stream(webpack_config).pipe(gulp.dest(dirs.js.dest));

  if (!production) {
    browserSync.reload();
    done();
  }
};

const grammarsTask = function grammarsTask(done) {
  gulp.src([`${dirs.grammars.src}/**`]).pipe(gulp.dest(dirs.grammars.dest));
};

const watchTask = function watchTask() {
  stylesTask(() => null);
  scriptsTask(() => null);
  grammarsTask(() => null);

  browserSync.init({
    proxy: "http://localhost:5000",
    open: false,
    notify: false,
    minify: false,
    ghostMode: false,
    online: false,
    ui: false,
  });

  gulp.watch(`${dirs.scss.src}/**/*.scss`, gulp.series(stylesTask));
  gulp.watch(`${dirs.js.src}/**/*.js`, gulp.series(scriptsTask));
};

const buildTask = function buildTask() {
  return new Promise(function (resolve) {
    gulp.task("styles")();
    gulp.task("scripts")();
    gulp.task("grammars")();
    resolve();
  });
};

gulp.task("styles", stylesTask);
gulp.task("scripts", scriptsTask);
gulp.task("grammars", grammarsTask);
gulp.task("watch", watchTask);
gulp.task("build", buildTask);
