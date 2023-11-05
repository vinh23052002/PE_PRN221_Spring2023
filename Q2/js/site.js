$(() => {
    LoadEmployee();

    var connection = new signalR.HubConnectionBuilder().withUrl("/employeeHub").build();
    connection.start();

    connection.on("LoadEmployee", function () {
        LoadEmployee();
    })

    function LoadEmployee() {
        var tr = '';
        $.ajax({
            url: '/Employee/LoadUser',
            method: 'GET',
            success: (result) => {
                $.each(result, (k, v) => {
                    tr += `<tr>
                    <td>${v.fullName}</td>
                    <td>${v.address}</td>
                    <td>${v.email}</td>
                    <td>${v.password}</td>
                    <td>
                        <a href= '../AppUsers/Edit?id=${v.userId}'>Edit</a> |
                        <a href= '../AppUsers/Details?id=${v.userId}'>Details</a> |
                        <a href= '../AppUsers/Delete?id=${v.userId}'>Delete</a>
                    </td>
                    </tr>`

                })
                console.log(result);

                $("#tableBody").html(tr);
            },
            erorr: (error) => {
                console.log(error);
            }
        });
    }
})