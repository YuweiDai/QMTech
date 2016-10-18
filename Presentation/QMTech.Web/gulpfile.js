/// <binding AfterBuild='appwebLess' ProjectOpened='appwebLess, lessWatch' />
//导入工具包 
var gulp = require('gulp'),
    less = require('gulp-less');

//定义一个less任务
gulp.task('appwebLess', function () {
    gulp.src([ 'src/css/less/web.extend.less'])
        .pipe(less()).pipe(gulp.dest('src/css'));
});


//监听less变化
gulp.task('lessWatch', function () {
    gulp.watch('src/css/less/*.less', ['appwebLess']); //当所有less文件发生改变时，调用testLess任务
});