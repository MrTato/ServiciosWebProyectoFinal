var sidebarOpen = false;
var sidebar = document.getElementById("sidebar");
var sidabarCloseIcon = document.getElementById("sidebar-icon");

function toggleSidebar() {
    if (!sidebarOpen) {
        sidebar.classList.add("sidebar-responsive");
        sidebarOpen = true;
    }
}

function closeSidebar() {
    if (sidebarOpen) {
        sidebar.classList.remove("sidebar-responsive");
        sidebarOpen = false;
    }
}