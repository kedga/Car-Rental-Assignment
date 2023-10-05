function reinitializePopovers() {
    const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]');
    popoverTriggerList.forEach((popoverTriggerEl) => {
        new bootstrap.Popover(popoverTriggerEl, {
            html: true // Enable HTML content in the popover
        });
    });
}
function DataTablesAdd(table, columnsToDisableOrdering) {
    $(document).ready(function () {
        $(table).DataTable({
            "columnDefs": columnsToDisableOrdering.map(function (colIndex) {
                return {
                    "orderable": false, "targets": colIndex
                };
            }),
            "lengthChange": false,
            "searching": false,
            "paging": false,
            "lengthMenu": [[-1, 10, 25, 50], ["All", 10, 25, 50]]

        });
    });
}