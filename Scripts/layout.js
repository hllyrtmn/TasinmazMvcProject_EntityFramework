$(document).ready(funtion(){

    // SideNav Button Initialization
    $(".button-collapse").sideNav({
        breakpoint: 1200
    });
    // SideNav Scrollbar Initialization
    var sideNavScrollbar = document.querySelector('.custom-scrollbar');
    var ps = new PerfectScrollbar(sideNavScrollbar);
})