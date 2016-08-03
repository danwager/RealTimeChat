var utils = (function () {
    return {
        htmlEncode: function (value) {
            return $('<div />').text(value).html();
        },

        getUserId: function () {
            var id = "";

            var charset = "abcdefghijklmnopqrstuvwxyz0123456789";

            for (var i = 0; i < 7; i++)
                id += charset.charAt(Math.floor(Math.random() * charset.length));

            return id;
        }
    }
})();
