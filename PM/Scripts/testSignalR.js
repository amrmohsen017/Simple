



//1. connection is created  implicitly via the dynamic proxy :) 
$.connection.hub.logging = true;
var testProxy = $.connection.test;



// senario#1 :: client sends data to server to process and then respond back
console.log(testProxy.client)


testProxy.client.clientMethod = function (msg) {
    console.log('Server sent: ' + msg);
};


testProxy.client.sessionMethod  = function (date) {
        console.log('User sessions Count: ' + date);
};


// scenario#3 :: getting data from hubContext inside an mvc controller :)
testProxy.client.controllerMethod = function (date) {
    //alert(date)
    console.log( date);
};
// senario#3 END 



$.connection.hub.start(


    function () {


        console.log("connection started!");
        // scenario#2 :: invoke a method on server and get its output immediately "useful when getting data from the hubcontext :)       
        testProxy.server.serverMethod2().then(data => console.log(data))
        // senario#2 END 

    }

).done(function () {
  
  
    //testProxy.server.serverMethod("client value");
    console.log(testProxy.server)
    // methods signatures should match friend :) 
    testProxy.server.serverMethod();
    
});


// senario#1 END 



// GROUPS 


let lbl_houseJoined = document.getElementById("lbl_houseJoined");


let btn_un_gryffindor = document.getElementById("btn_un_gryffindor");
let btn_un_slytherin = document.getElementById("btn_un_slytherin");
let btn_un_hufflepuff = document.getElementById("btn_un_hufflepuff");
let btn_un_ravenclaw = document.getElementById("btn_un_ravenclaw");
let btn_gryffindor = document.getElementById("btn_gryffindor");
let btn_slytherin = document.getElementById("btn_slytherin");
let btn_hufflepuff = document.getElementById("btn_hufflepuff");
let btn_ravenclaw = document.getElementById("btn_ravenclaw");

let trigger_gryffindor = document.getElementById("trigger_gryffindor");
let trigger_slytherin = document.getElementById("trigger_slytherin");
let trigger_hufflepuff = document.getElementById("trigger_hufflepuff");
let trigger_ravenclaw = document.getElementById("trigger_ravenclaw");




btn_gryffindor.addEventListener("click", function (event) {
    testProxy.server.joinHouse("Gryffindor");
    event.preventDefault();
});
btn_hufflepuff.addEventListener("click", function (event) {
    testProxy.server.joinHouse("Hufflepuff");
    event.preventDefault();
});
btn_ravenclaw.addEventListener("click", function (event) {
    testProxy.server.joinHouse("Ravenclaw");
    event.preventDefault();
});
btn_slytherin.addEventListener("click", function (event) {
    testProxy.server.joinHouse( "Slytherin");
    event.preventDefault();
});


trigger_gryffindor.addEventListener("click", function (event) {
    testProxy.server.triggerHouseNotify( "Gryffindor");
    event.preventDefault();
});
trigger_hufflepuff.addEventListener("click", function (event) {
    testProxy.server.triggerHouseNotify( "Hufflepuff");
    event.preventDefault();
});
trigger_ravenclaw.addEventListener("click", function (event) {
    testProxy.server.triggerHouseNotify( "Ravenclaw");
    event.preventDefault();
});
trigger_slytherin.addEventListener("click", function (event) {
    testProxy.server.triggerHouseNotify( "Slytherin");
    event.preventDefault();
});



btn_un_gryffindor.addEventListener("click", function (event) {
    testProxy.server.leaveHouse( "Gryffindor");
    event.preventDefault();
});
btn_un_hufflepuff.addEventListener("click", function (event) {
    testProxy.server.leaveHouse( "Hufflepuff");
    event.preventDefault();
});
btn_un_ravenclaw.addEventListener("click", function (event) {
    testProxy.server.leaveHouse( "Ravenclaw");
    event.preventDefault();
});
btn_un_slytherin.addEventListener("click", function (event) {
    testProxy.server.leaveHouse( "Slytherin");
    event.preventDefault();
});



testProxy.client.triggerHouseNotification =  (houseName) => {
    toastr.error(`A new notification for ${houseName} has been launched.`);
}

testProxy.client.newMemberAddedToHouse =  (houseName) => {
    toastr.success(`Member has subscribed to ${houseName}`);
}

testProxy.client.newMemberRemovedFromHouse =  (houseName) => {
    toastr.warning(`Member has unsubscribed from ${houseName}`);
}

testProxy.client.subscriptionStatus =  (strGroupsJoined, houseName, hasSubscribed) => {
    lbl_houseJoined.innerText = strGroupsJoined;

    if (hasSubscribed) {
        //subscribe to

        switch (houseName) {
            case 'slytherin':
                btn_slytherin.style.display = "none";
                btn_un_slytherin.style.display = "";
                break;
            case 'gryffindor':
                btn_gryffindor.style.display = "none";
                btn_un_gryffindor.style.display = "";
                break;
            case 'hufflepuff':
                btn_hufflepuff.style.display = "none";
                btn_un_hufflepuff.style.display = "";
                break;
            case 'ravenclaw':
                btn_ravenclaw.style.display = "none";
                btn_un_ravenclaw.style.display = "";
                break;
            default:
                break;
        }

        toastr.success(`You have Subscribed Successfully. ${houseName}`);
    }
    else {
        //unsubscribe
        switch (houseName) {
            case 'slytherin':
                btn_slytherin.style.display = "";
                btn_un_slytherin.style.display = "none";
                break;
            case 'gryffindor':
                btn_gryffindor.style.display = "";
                btn_un_gryffindor.style.display = "none";
                break;
            case 'hufflepuff':
                btn_hufflepuff.style.display = "";
                btn_un_hufflepuff.style.display = "none";
                break;
            case 'ravenclaw':
                btn_ravenclaw.style.display = "";
                btn_un_ravenclaw.style.display = "none";
                break;
            default:
                break;
        }

        toastr.success(`You have Unsubscribed Successfully. ${houseName}`);
    }

}


