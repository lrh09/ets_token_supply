function loadTable(page){
    pageNumber = page
    getTotalPages();
    console.log("TotalPages" + totalPages)
    loadPagination()
    var url = 'http://localhost:5000/TokenSupply/Page?pageNumber='.concat(page.toString());
    $.getJSON(url, function(data){
        console.log(pageNumber)
        var table_body = $('#tbody');
        table_body.empty();
        $(data).each(function(i, d){
            table_body.append($("<tr>")
                .append($("<td>").append(d.id))
                .append($("<td>").append(d.symbol))
                .append($("<td>").append(d.name))
                .append($("<td>").append(d.contractAddress))
                .append($("<td>").append(d.totalSupply))
                .append($("<td>").append(d.totalHolders))
                .append($("<td>").append("<button onclick='deleteRecord("+ d.id +")'>Delete</button><button onclick='openForm("+ JSON.stringify(d) +")'>Update</button>")));
        });


        console.log(totalPages)
    })
}

function getTotalPages(){
    $.ajax({
        type: "GET",
        url: "http://localhost:5000/TokenSupply/TotalPages",
        async : false,
        success: function(result) {
            totalPages = parseInt(result["totalPages"]);
        },
        error: function(result) {
            alert('error');
        }
    });
}

function getStatistics(){
    $.ajax({
        type: "GET",
        url: "http://localhost:5000/TokenSupply/Statistics",
        success: function(result) {
            console.log(JSON.stringify(result))
            return JSON.stringify(result);
        },
        error: function(result) {
            alert('error');
        }
    });
}

function deleteRecord(id){
    $.ajax({
        type: "DELETE",
        url: "http://localhost:5000/TokenSupply/Delete?id=" + id,
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        dataType: "json",
        success: function(result) {
            alert('Deleted');
            getTotalPages()
            if(pageNumber <= totalPages){
                loadTable(pageNumber);
            }
            else{
                loadTable(totalPages-1)
            }
            loadPagination()
            plotStatistics()
        },
        error: function(result) {
            alert('error');
        }
    });
}

function loadPagination(){
    var pagination_label = $('#pagination-label');
    pagination_label.empty();
    for (let i = 1; i <= totalPages; i++) {
        var page_url = 'http://localhost:5000/TokenSupply/Page?pageNumber='.concat(i.toString());
        if(pageNumber !== i){
            pagination_label
                // .append("<li><a href='" + page_url + "'>" +i + "</a></li>");
                .append("<li><a href='#' onclick='loadTable("+ i +");'>" +i + "</a></li>");
        }
        else {
            pagination_label
                .append("<li class='active'><a href='#'>"+ i +"</a></li>");
        }
    }
}

function openForm(data) {
    console.log(data)
    document.getElementById("myForm").style.display = "block";

    currentUpdateId = data['id'];
    console.log("Current Update Id")
    console.log(currentUpdateId)

    $('#symbol_input').val(data['symbol']);
    $('#name_input').val(data['name']);
    $('#contract_address_input').val(data['contractAddress']);
    $('#total_supply_input').val(data['totalSupply']);
    $('#total_holders_input').val(data['totalHolders']);


}

function closeForm() {
    document.getElementById("myForm").style.display = "none";
}

function plotStatistics(){
    google.load('visualization', '1', {packages: ['corechart']});
    google.load('visualization', '1', {packages: ['corechart'], callback : drawChart});
}

function drawChart() {
    $.ajax({
        type: "GET",
        url: "http://localhost:5000/TokenSupply/Statistics",
        success: function(result) {
            console.log(result)

            var data = new google.visualization.DataTable();
            data.addColumn('string', 'name');
            data.addColumn('number', 'totalSupply');

            result.forEach(function (row) {
                data.addRow([
                    row.name,
                    row.totalSupply
                ]);
            });

            // Optional; add a title and set the width and height of the chart
            var options = {'title':'Token Supply Statistics', 'width':550, 'height':400};

            // Display the chart inside the <div> element with id="piechart"
            var chart = new google.visualization.PieChart(document.getElementById('piechart'));
            chart.draw(data, options);

        },
        error: function(result) {
            alert('error');
        }
    });
}