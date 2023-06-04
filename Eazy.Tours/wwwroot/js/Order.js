var dtable;
$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("Pending")) {
        OrderTable("Pending");
    }
    else {
        if (url.includes("Approved")) {
            OrderTable("Approved");
        }
        else {
            if (url.includes("Booked")) {
                OrderTable("Booked");
            }
            else {
                if (url.includes("InProcess")) {
                    OrderTable("InProcess");
                }
                else {
                    OrderTable("All");
                }
            }
        }
    }
});

function OrderTable(status) {
    dtable = $('#myTable').DataTable({
        "ajax": { "url": "/Order/AllOrders?status=" + status },
        "columns": [
            { "data": "Name" },
            { "data": "Phone Number" },
            { "data": "Booking status" },
            { "data": "Booking total" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <a href="/Order/OrderDetails?id=${data}"><i class="bi bi-pencil-square"></i></a>

`   }
            }
        ]
    });
}