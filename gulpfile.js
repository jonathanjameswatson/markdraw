'use strict';

/*global require*/
/*global process*/

const gulp = require('gulp');
const sass = require('gulp-sass');
const browserSync = require('browser-sync').create();

const dirs = {
    scss: {
        src: 'MarkdrawBrowser/Styles',
        dest: 'MarkdrawBrowser/wwwroot/css'
    }
};

const production = process.env.NODE_ENV === 'production';

const stylesTask = function stylesTask(done) {
    gulp.src(`${dirs.scss.src}/*.scss`)
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest(dirs.scss.dest));

    if (!production) {
        browserSync.reload();
        done();
    }
};

const watchTask = function watchTask() {
    stylesTask(() => null);

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
};

const buildTask = function buildTask() {
    return new Promise(function (resolve) {
        gulp.task('styles')();
        resolve();
    });
};

gulp.task('styles', stylesTask);
gulp.task('watch', watchTask);
gulp.task('build', buildTask);