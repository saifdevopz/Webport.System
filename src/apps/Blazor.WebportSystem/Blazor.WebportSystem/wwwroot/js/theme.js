///*=============== SHOW MENU ===============*/
window.showMenu = (toggleId, navId) => {
    console.log("JavaScript Loaded (main.js)");
    const toggle = document.getElementById(toggleId),
        nav = document.getElementById(navId);

    toggle.addEventListener('click', () => {
        nav.classList.toggle('show-menu');
        toggle.classList.toggle('show-icon');
    });
};

///*=============== SIDEBAR ===============*/

//let arrow = document.querySelectorAll(".arrow");
//for (var i = 0; i < arrow.length; i++) {
//    arrow[i].addEventListener("click", (e) => {
//        let arrowParent = e.target.parentElement.parentElement;//selecting main parent of arrow
//        arrowParent.classList.toggle("showMenu");
//    });
//}

//document.addEventListener("DOMContentLoaded", () => {
//    let sidebar = document.querySelector(".sidebar");
//    let sidebarBtn = document.querySelector(".bx-menu");
//    if (sidebarBtn) {
//        sidebarBtn.addEventListener("click", () => {
//            sidebar.classList.toggle("close");
//            console.log("Sidebar button clicked");
//        });
//    }
//});
