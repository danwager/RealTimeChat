var chat = function() {
    var self = this;
    var chat = $.connection.chatHub;
    var currentUser = {};
    var connectionId;

    var init = function () {
        $.connection.hub.start().done(function () {
            connectionId = $.connection.hub.id;
        });

        chat.client.addUser = function(user) {
            addUser(user);
        };

        chat.client.signalConnectionId = function (contractConnectionId) {
            connectionId = contractConnectionId;
        };

        chat.client.newMessage = function (user, message) {
            addMessage(user, message);
        };

        chat.client.userIsTyping = function (user, message) {
            setTypingSpan(getUserElement(user), message);
        };

        chat.client.userLoggedOut = function (user) {
            $(getUserElement(user)).remove();
        };

        chat.client.addAllUsers = function (user) {
            if (user) {
                $.each(user, function (index, value) {
                    addUser(value);
                });
            }
        };

        chat.client.loadPreviousMessages = function (messages) {
           if (messages) {
               $.each(messages, function (index, message) {
                   addMessage(message.User, message.MessageText);
               });
            }
        };

        chat.client.tooManyUsers = function () {
            window.location = "/home/toomanyusers";
        };

    };

    var sendMessage = function() {
        chat.server.sendMessage(currentUser, $("#chatMessage").val());
        $("#chatMessage").val("").focus();
    };

    var handleKeyPress = function(e) {
        var key = e.keyCode || e.which;

        if (key == 13) {
            sendMessage();
        } else {
            chat.server.userIsTyping(currentUser, $('#chatMessage').val());
        }

        return false;
    };

    var newUser = function() {
        currentUser = {
            Id: utils.getUserId(),
            Name: $("#username").val()
        }

        $("#user").text(currentUser.Name);
        $("#chatMessage").val("").focus();

        chat.server.addNewUser(connectionId, currentUser);
    };

    var logout = function () {
        chat.server.logoutUser(currentUser);
        currentUser = {};
        window.location = "/home/thankyou";
    };

    function addMessage(user, messageText) {
        var div = '<div class="message">' + utils.htmlEncode(user.Name) + ": "
            + utils.htmlEncode(messageText) + "</div>";

        setTypingSpan(getUserElement(user), '');

        var chatMessages = $("#chatMessages");

        chatMessages.append(div);

        updateScroll(chatMessages);
    }
    
    function getUserElement(user) {
        return '#' + user.Id;
    };

    function setTypingSpan(userElement, message) {
        $(userElement).find('.typing-message').text(message);
    };

    function addUser(user) {
        var userElement = getUserElement(user);

        if ($('#users').find(userElement).length === 0) {
            $("#users").append('<div class="user" ' + 'id="' + user.Id + '">'
                + utils.htmlEncode(user.Name) + " <span class='typing-message'></span></div>");
        }
    };

    function updateScroll(element) {
        element.animate({ scrollTop: element.prop("scrollHeight") - element.height() }, 250);
    };

    return {
        init: init,
        sendMessage: sendMessage,
        handleKeyPress: handleKeyPress,
        newUser: newUser,
        logout: logout
    }
}();

chat.init();