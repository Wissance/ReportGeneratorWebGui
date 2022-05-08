/// <binding BeforeBuild='min' Clean='clean' />
/* This is a settings of build tool - gulp, previous version was 3.x, recently (2022) we switched tp gulp 4
 * https://gulpjs.com/docs/en/getting-started/javascript-and-gulpfiles/
 * https://webdesign-master.ru/blog/tools/gulp-4-lesson.html
 * to build using gulp install gulp && gulp-cli globally
 * 1. npm install gulp -g && npm install gulp-cli -g (do this only ONCE)
 * 2. npm install
 * 3. gulp build
 * As a result we got 2 files: wwwroot/css/app,min.css and wwwroot/js/app.min.js
 */

//"use strict";

const gulp = require("gulp");
const rimraf = require("rimraf");
const concat = require("gulp-concat");
const cssmin = require("gulp-cssmin");
const uglify = require("gulp-uglify");

const { parallel, series, watch } = gulp;


var paths = {
    webroot: "./wwwroot/",
    nodeModules: "./node_modules/"
};

var nodeCss = [
    // bootstrap
    paths.nodeModules + "bootstrap/dist/css/bootstrap.css",
    // font awesome
    paths.nodeModules + "font-awesome-5-css/css/all.css",
    // report manager stylesheets
    paths.webroot +  "css/reports_manager.css"
];

var nodeJs = [
    // jquery (MUST BE BEFORE bootstrap!)
    paths.nodeModules + "/jquery/dist/jquery.js",
    // bootstrap
    paths.nodeModules + "/bootstrap/dist/js/bootstrap.js",
    // report manager js
    paths.webroot + "/js/reportManager.js"
];

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/app.min.js";
paths.concatCssDest = paths.webroot + "css/app.min.css";

function cleanJs (cb) {
    rimraf(paths.concatJsDest, cb);
};

function cleanCss (cb) {
    rimraf(paths.concatCssDest, cb);
};

function clean(cb) {
    cleanJs(cb);
    cleanCss(cb);
    cb();
};

function buildMinJs(cb) {
    return gulp.src(nodeJs.concat([paths.js, "!" + paths.minJs]), { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
}

function buildMinCss(cb) {
    return gulp.src(nodeCss.concat([paths.css, "!" + paths.minCss]))
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
}

function build(cb) {
    buildMinJs(cb);
    buildMinCss(cb);
    cb();
}

exports.build = series(clean, build);