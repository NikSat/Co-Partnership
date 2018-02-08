$(document).ready(function () {


    // AJAX CALLS HERE
    // Get all the messages for this user
    let GetAll = () => {
        $.ajax({
            url: `/api/MessageBoard/GetSum`,
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


    // Get all sent messages
    let GetAllSent = () => {
        $.ajax({
            url: `/api/MessageBoard/Sent`,
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

                    data.forEach(x => appendSentMessages(x));
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


    // Delete a message
    let Delete = (id) => {
        $.ajax({
            url: `/api/MessageBoard/Delete`,
            contentType: "application/json",
            method: "POST",
            data: JSON.stringify({
                Id: id
            }),
            success: () => {
                //RemovefromDom(id);

            }
        });
    };


    // Search user depending on the level of the current user
    let Search = (searchinput) => {
        $.ajax({
            url: `/api/MessageBoard/Search`,
            contentType: "application/json",
            method: "POST",
            data: JSON.stringify({
                Title: searchinput
            }),
            success: (data) => {
                BuildResults(data);

            }
        });

    };

    let GetAdmins = () => {
        $.ajax({
            url: `/api/MessageBoard/GetAdmins`,
            contentType: "application/json",
            method: "POST",
            success: (data) => {
                BuildResults(data);

            }
        });


    };


    let Send = () => {
        $.ajax({
            url: `/api/MessageBoard/Send`,
            contentType: "application/json",
            method: "POST",
            data: JSON.stringify({
                receiverId: $("#recei").val(),
                title: $("#titl").val(),
                message1: $("#messageText").val()
            }),
            success: (data) => {
                SuccessSent();

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

                $(".singleMail").prepend(
                    `
            <div class="alert alert-warning">
                Error, message not sent  
              </div>
                `
                );
                $(".alert").delay(4000).slideUp(200, function () {
                    $(this).alert('close');
                });
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

    // This function checks or unchecks
    let unCheck = (ele) => {
        if (ele.checked) {
            // Find all unchecked and check them
            $("input:checkbox:not(:checked)").prop("checked", true);
        }
        else {
            // Find all checked and uncheck them
            $("input:checkbox:checked").prop("checked", false);
        }
    };

    // This function checks before it sends a message
    let CheckMessage = () => {
        if (!$("#recei").val() || !$("#titl").val() || !$("#messageText").val()) {
            let error = "Message not valid, it lacks:";
            if (!$("#recei").val()) {
                error += " recipient,";
            }
            if (!$("#titl").val()) {
                error += " title,";
            }
            if (!$("#messageText").val()) {
                error += " text";
            }
            $(".singleMail").prepend(
                `
            <div class="alert alert-warning">
                ${error} 
              </div>
                `
            );
            $(".alert").delay(4000).slideUp(200, function () {
                $(this).alert('close');
            });
            return;
        }
        else {
            Send();
        }


    };

    // CONSTRUCTION FUNCTIONS HERE
    // This function creates the message table with all the messages
    let MakeMessageListBody = (typeofbody) => {
        $(".messageBody").append(

            `<div class="EmailHead">
                    <h2>${typeofbody}</h2>
            </div>
            <div class="subBody">
                <div class="bodyHead">
                    <div class="checkAll btn-small">
                            <input type="checkbox" name="SelectAll" value="All"><a>All</a>   
                    </div>
                        <a id="markRead" class="btn-small">Mark Read</a>
                        <a id="deleteMessages" class="btn-small">Delete</a>
                </div>
                <div class="mailList"></div>
            </div>`
        );
        let x = document.querySelector(".checkAll");
        x.querySelector('input[type = checkbox]').addEventListener('change', function (event) {
            unCheck(this);
            event.preventDefault();
        });
        x = document.querySelector("#markRead");
        x.addEventListener('click', function (event) {
            MarkAll();
            event.preventDefault();
        });
        x = document.querySelector("#deleteMessages");
        x.addEventListener('click', function (event) {
            DeleteAll();
            event.preventDefault();
        });


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
                  <li id=mess${mess.id}>
                            <input type="checkbox">
                            <a class="ordered">
                            <span>${mess.senderName}</span>
                            <span>${mess.title}</span>
                            <span>${date}</span>
                            <a>
                        </li>
           `
        );
        if (mess.status === false) {
            $(`#mess${mess.id} span`).addClass("thick");
        }
       // let x = document.getElementById(`${mess.id}`);
        //let y=x.querySelector("a");
        let x = $(`#mess${mess.id} a`);
            x[0].addEventListener("click", function (event) {
            OpenMail(mess.id);
            event.preventDefault();
        });
    };


    // Same but with sent messages
    let appendSentMessages = (mess) => {
        let date;
        // Check the date
        if (mess.dateSent === null) {
            date = "unknown";
        }
        else {
            date = mess.dateSent;
        }

        $("#messList").append(`
                  <li id=mess${mess.id}>
                            <input type="checkbox">
                            <a class="ordered">
                            <span>${mess.senderName}</span>
                            <span>${mess.title}</span>
                            <span>${date}</span>
                            <a>
                        </li>
           `
        );
        let x = document.getElementById(`mess${mess.id}`);
        let y = x.querySelector("a");
        y.addEventListener("click", function (event) {
            OpenSentMail(mess.id);
            event.preventDefault();
        });
    };



    // This function marks checked items as read
    let MarkAll = () => {
        let chec = document.querySelectorAll('input[type=checkbox]:checked');



        if (chec.length !== 0) {
            chec.forEach(function (element) {
                let nId = (element.parentElement.id).replace(/\D/g, '');
                MarkRead(nId);
                $("input:checkbox:checked").prop("checked", false);
            });

        }
          

    };

    // This function deletes selected items (checks first)
    let DeleteAll = () => {
        let chec = document.querySelectorAll('input[type=checkbox]:checked');
        if (chec.length !== 0) {
            if (confirm('Are you sure?')) {
                chec.forEach(function (element) {
                    let nId = (element.parentElement.id).replace(/\D/g, '');
                    Delete(nId);
                    let pa = document.getElementById("messList");
                    pa.removeChild(element.parentElement);
                });
            }
        }
    };

    // This function 
    let SuccessSent = () => {
        $(".subBody").empty();
        $(".subBody").append(
            `
                </br></br>
                <h4>Message sent</h4>
                </br><br>
                <a id="return" class="btn-small">Return</a>
                
            `

        );
        // Apply event listeners
        let x = document.querySelector("#return");
        x.addEventListener('click', function (event) {
            $(".messageBody").empty();
            MakeMessageListBody("Inbox");
            MakeMessageList();
            event.preventDefault();
        });

    };


    // This function deletes the  main body to open a message
    let OpenMail = (id) => {
        $(".messageBody").empty();
        MarkRead(id);
        GetOne(id);
       

    };

    let OpenSentMail = (id) => {
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
                    <a id="deleteSingle" class="btn-small">Delete</a>
                </div>
                <div class="singleMail"  id=${data.id}>
                    <span id="title">Title:  <b>${data.title}</b></span><hr />
                    <span id="sender">Sent by:  <b>${data.senderName}</b></span><span>On: ${date}</span><hr />
                    <span id="message">${data.text}</span>
                </div>
            </div>
            `
        );

        // Apply event listeners
        let x = document.querySelector("#return");
        x.addEventListener('click', function (event) {
            $(".messageBody").empty();
            MakeMessageListBody("Inbox");
            MakeMessageList();
            event.preventDefault();
        });
 

        // Helper function deleteself
        let deleteSelf = () => {
            if (confirm('Are you sure?')) {
                Delete(document.querySelector(".singleMail").id);
                $(".messageBody").empty();
                MakeMessageListBody("Inbox");
                MakeMessageList();

            }
        };


        // Delete Button
        x = document.querySelector("#deleteSingle");
        x.addEventListener('click', function (event) {
            deleteSelf();          
            event.preventDefault();
        });


    };

    // This function alters the state of a list item
    let AlterReadState = (id) => {
        $(`#mess${id} span`).removeClass("thick");
    };


    // This function creates the compose mail body
    MakeCompose = () => {
        $(".messageBody").append(
            `
            <div class="EmailHead">
                        <h2>Compose new message</h2>
                </div>
                <div class="subBody">
                    <div class="bodyHead">
                            <a id="return" class="btn-small">Return</a>                   
                            <a id="send" class="btn-small">Send</a>
                       
                    </div>
                    <div class="singleMail">                        
                            <form action="/api/sessageBoard/send" method="post">
                                <label for="recei">To:</label></br>
                                <input id="recei" type="text" name="receiverId" value="">
                                <a id="search" class="btn-small">Search</a></br>
                                <label for="titl">Title</label></br>
                                <input id="titl" type="text" name="title"></br></br>
                                
                                <input type="textarea" id="messageText" name="message">
                            </form>
                    </div>
                </div>
            `
        );
        // Apply event listeners
        let x = document.querySelector("#return");
        x.addEventListener('click', function (event) {
            $(".messageBody").empty();
            MakeMessageListBody("Inbox");
            MakeMessageList();
            event.preventDefault();
        });

        // The search button
        x = document.querySelector("#search");
        x.addEventListener('click', function (event) {
            SearchUser();
            event.preventDefault();
        });
        // The send button
        x = document.querySelector("#send");
        x.addEventListener('click', function (event) {            
            CheckMessage();
            //Send();
            event.preventDefault();
        });
    };

    // This function builds the a list from the results
    let BuildResults = (data) => {
        // Check if there is no data
        if (data.length === 0) {
            // Show a warning
            $(".singleMail").prepend(
                `
            <div class="alert alert-warning">
                No users found with these search terms! 
              </div>
                `
            );
            $(".alert").delay(4000).slideUp(200, function () {
                $(this).alert('close');
            }); 
            // Clear values from input
            $("#recei").val("");
            return;
        }
        else {
            // Replace the input with a select list
            let ele = document.getElementById('recei');
            let select = document.createElement("select");

            ele.parentNode.replaceChild(select, ele);
            select.id = 'recei';
            // Append the names to the select list
            data.forEach(function (name) {
                let option = document.createElement("option");
                option.id = name.id;
                option.value = name.id;
                option.text = name.senderName;
                select.appendChild(option);
            });



        }

    };



    // This function does the search
    let SearchUser = () => {


        let sval = document.getElementById('recei');
        if (sval.tagName === "SELECT") {
            let select = document.createElement("INPUT");
            select.setAttribute("type", "text");

            sval.parentNode.replaceChild(select,sval);
            select.id = 'recei';
            return;
        }
        // Check the value
        if (!sval.value) {
            GetAdmins();
            return;
        }
        // Use api to send search request
        Search(sval.value);

    };



    let MakeMessageList = () => {
        GetAll();
    };


    let MakeSentMessageList = () => {
        GetAllSent();
    };


    // This function applies events on the sidebar
    let ApplyEvents = () => {
        // The compose button
        let comp = document.getElementById("Compose");
        comp.addEventListener('click', function (event) {
            $(".messageBody").empty();
            MakeCompose();

            event.preventDefault();
        });


        // The index button
        let ind = document.getElementById("actionInbox");
        ind.addEventListener('click', function (event) {
            $(".messageBody").empty();
            MakeMessageListBody("Inbox");
            MakeMessageList();
            event.preventDefault();
        });

        // The sent button
        let sent = document.getElementById("actionSent");
        sent.addEventListener('click', function (event) {
            $(".messageBody").empty();
            MakeMessageListBody("Sent messages");
            MakeSentMessageList();
            event.preventDefault();
        });

    };




    ApplyEvents();
    MakeMessageListBody("Inbox");
    MakeMessageList();

});