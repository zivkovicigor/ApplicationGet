// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

if (document.getElementById("register")) {
    document.getElementById("register").style.display = 'none';
}

if (document.getElementById("manage")) {
    document.getElementById("manage").style.display = 'none';
}

//const connection = new signalR.HubConnectionBuilder()
//    .withUrl("/reservationData")
//    .configureLogging(signalR.LogLevel.Information)
//    .build()

//const createReservationBtn = document.getElementById("createReservationBtn");

//console.log("btnClick", createReservationBtn);

//if (createReservationBtn) {
//    createReservationBtn.addEventListener('click', (event) => {
//        event.preventDefault();
//        console.log('Btn clicked')
//        const btn = document.getElementById("btnCreate");
//        const flightId = document.getElementById("flightId").getAttribute("data-id");
//        const date = document.getElementById("date").getAttribute("data-date");
//        const numberOfSeats = document.getElementById("numberOfSeats").getAttribute("data-numberOfSeats");
//        const numberOfLayovers = document.getElementById("numberOfLayovers").getAttribute("data-numberOfLayovers");
//        const fromCityName = document.getElementById("fromCityName").getAttribute("data-fromCityName");
//        const toCityName = document.getElementById("toCityName").getAttribute("data-toCityName");
//        const numberOfSeatsRes = document.getElementById("numberOfSeatsRes").value;

//        var formData = new FormData();
//        formData.append("id", 1);
//        formData.append("numberOfSeats", 5);

//        $.ajax({
//            type: 'GET',
//            url: "http://localhost:13123/Reservation/CreateRes?id=" + flightId + "&numberOfSeats=" + numberOfSeatsRes,
//            headers: {
//                'Accept': 'application/json',
//                'Content-Type': 'application/json'
//            },
//            crossDomain: true,
//            xhrFields: {
//                withCredentials: true
//            }  
//        }).done(function (data) {
//            sendMessageSignalR();
//            console.log('poslato')
//            window.location.href ="http://localhost:13123/Reservation/IndexUser"
//        });

//    })
//}


//connection.start().catch(error => console.log(error));


////send message
//function sendMessageSignalR() {
//    connection.invoke("UpdateReservations").catch(error => console.log(error));
//    console.log('Message sent')
//    event.preventDefault();
//}


////recive message

//connection.on("UpdatedData", (r) => {
//    console.log('Got into recieve message');
//    console.log(r);
//});

