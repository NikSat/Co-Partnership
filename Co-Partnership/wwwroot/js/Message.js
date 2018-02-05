$(document).ready(function () {


    // AJAX CALLS HERE
    // Get all the messages
    let GetAll = () => {
        $.ajax({
            url: `/api/MessageBoard`,
            contentType: "application/json",
            method: "GET",
            success: (data) => {
                if (data.length === 0) {
                    $(".mailList").append(
                        "<h3>No new messages</h3>"
                    );

                }
                else {
                    $(".mailList").append(
                        `<ul id="messList"></ul>`
                    );

                    data.forEach(x => appendMessages(x));
                }
            }


        });

    };

    // Get one specific message using post
    let GetOne = (id) => {
        $.ajax({
            url: `/api/MessageBoard/Detail`,
            contentType: "application/json",
            method: "POST",
            data: JSON.stringify({
                Id: id
            }),
            success: (data) => {
                BuildResponce(data);
            }
        });
    };


    // Mark a message as read
    let MarkRead = (id) => {
        $.ajax({
            url: `/api/MessageBoard/Read`,
            contentType: "application/json",
            method: "POST",
            data: JSON.stringify({
                Id: id
            }),
            success: () => {
                AlterReadState(id);
            }
        });
    };



    /*
    // Get messages from specific date
    let GetAllByDate = (start,end) => {
        $.ajax({
            url: `/api/MessageBoard/Date/`,
            contentType: "application/json",
            method: "GET",
            success: (data) => {
                // To manipulate the data

            }


        });

    };

    */







    // CONSTRUCTION FUNCTIONS HERE
    // This function creates the message table with all the messages
    let MakeMessageListBody = () => {
        $(".messageBody").append(
            
            `<div class="EmailHead">
                    <h2>Inbox</h2>
            </div>
            <div class="subBody">
                <div class="bodyHead">
                    <div class="checkAll btn-small">
                            <input type="checkbox" name="SelectAll" value="All"><a>All</a>   
                    </div>
                        <a id="markRead" class="btn-small">Mark Read</a>
                        <a id="delete" class="btn-small">Delete</a>
                </div>
                <div class="mailList"></div>
            </div>`
        );
        
    };

    // This function adds a row in the table
    let appendMessages = (mess) => {
        let date;
        // Check the date
        if (mess.dateSent === null) {
            date = "unknown";
        }
        else {
            date = mess.dateSent;
        }

        $("#messList").append(`
                  <li id=${mess.id}>
                            <input type="checkbox">
                            <span>${mess.senderName}</span>
                            <span>${mess.title}</span>
                            <span>${date}</span>
                        </li>
           `
        );
        if (mess.status === false) {
            $(`#${mess.id} span`).addClass("thick");
        }
        document.getElementById(`${mess.id}`).addEventListener("click", function (event) {
            OpenMail(mess.id);
            event.preventDefault();
        });
            
           
  

    };


    // This function deletes the  main body to open a message
    let OpenMail = (id) => {
        $(".messageBody").empty();
        GetOne(id);
       

    };

    let BuildResponce = (data) => {
        let date;
        // Check the date
        if (data.dateSent === null) {
            date = "unknown";
        }
        else {
            date = data.dateSent;
        }


        $(".messageBody").append(
            `
            <div class="EmailHead">
                <h2>View Message</h2>
            </div>
            <div class="subBody">
                <div class="bodyHead">
                    <a id="return" class="btn-small">Return</a>
                    <a id="reply" class="btn-small">Reply</a>
                    <a id="delete" class="btn-small">Delete</a>
                </div>
                <div class="singleMail">
                    <span id="title">${data.title}</span><hr />
                    <span id="sender">${data.senderName}</span><span>${date}</span><hr />
                    <span id="messageText">${data.text}</span>
                </div>
            </div>
            `
        );
    };





    let MakeMessageList = () => {
        GetAll();
    };

    let ApplyEvents = () => {



    };




    ApplyEvents();
    MakeMessageListBody();
    MakeMessageList();

});