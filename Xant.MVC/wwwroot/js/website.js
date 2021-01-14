﻿var Website = (function () {

    //===================================
    //          Json Result Handler
    //===================================
    var jsonResultHandler = function jsonHandler(jsonResult) {
        if (jsonResult !== undefined && jsonResult !== null) {

            //Not found
            if (jsonResult.statusCode === 404) {
                window.location = "/Error/404";
            }
            //Success
            else if (jsonResult.statusCode === 200) {
                $.alert({
                    title: '',
                    content: `<p>${jsonResult.message}</p>`,
                    type: 'green',
                    typeAnimated: true,
                    buttons: {
                        Ok: {
                            text: "بستن"
                        }
                    }
                });
            }
            //Error range 400 to 600
            else if ((jsonResult.statusCode >= 400 && jsonResult.statusCode < 600) && jsonResult.url === null) {
                $.alert({
                    title: '',
                    content: `<p>${jsonResult.message}</p>`,
                    type: 'red',
                    typeAnimated: true,
                    buttons: {
                        Ok: {
                            text: "بستن"
                        }
                    }
                });
            }
            //Redirect
            else if (jsonResult.statusCode === 301 || jsonResult.statusCode === 302)
                window.location = jsonResult.url;
        }
    }



    return {
        JsonResultHandler: jsonResultHandler
    }
})();