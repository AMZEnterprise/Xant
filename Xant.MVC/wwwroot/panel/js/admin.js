var AdminPanel = (function () {

    //Render Table Buttons
    function renderButtons(editUrl, detailsUrl, deleteUrl, id) {

        var result = "";

        if (editUrl !== undefined) {
            var linkEdit = `<a href="${editUrl}/-1" class="btn btn-primary" title="ویرایش"><i class="fa fa-pen"></i></a>`;
            linkEdit = linkEdit.replace("-1", id);

            result += linkEdit;
        }

        if (detailsUrl !== undefined) {
            var linkDetails = `<a href="${detailsUrl}/-1" class="btn btn-warning" title="جزئیات"><i class="fa fa-list"></i></a>`;
            linkDetails = linkDetails.replace("-1", id);

            result += " | " + linkDetails;
        }

        if (deleteUrl !== undefined) {
            var linkDelete = `<a href="${deleteUrl}/-1" class="btn btn-danger" title="حذف"><i class="fa fa-trash"></i></a>`;
            linkDelete = linkDelete.replace("-1", id);

            result += " | " + linkDelete;
        }

        return result;
    }

    function boolToText(val) {
        if (val === true) {
            return "بله";
        }
        return "خیر";
    }

    function reduceTextToSmallerWidth(text, charsCount) {
        return (text.length > charsCount) ? text.substr(0, charsCount - 1) + '&hellip;' : text;
    }

    var loadContacts = function contacts(apiUrl, editUrl) {
        $("#jq-table").DataTable({
            responsive: true,
            "language": {
                "url": "/lib/datatables/js/Persian.json"
            },
            // Design Assets
            stateSave: true,
            autoWidth: true,
            // ServerSide Setups
            processing: true,
            serverSide: true,
            // Paging Setups
            paging: true,
            // Searching Setups
            searching: { regex: true },
            // Ajax Filter
            ajax: {
                url: apiUrl,
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: function (d) {
                    return JSON.stringify(d);
                }
            },
            // Columns Setups
            columns: [
                { data: "userFullName" },
                { data: "subject" },
                { data: "email" },
                {
                    data: "body",
                    render: function (data, type, row) {
                        return reduceTextToSmallerWidth(row.message, 50);
                    }
                },
                { data: "ip" },
                {
                    data: "createDate",
                    render: function (data, type, row) {
                        return AdminPanel.ConvertEnglishDateToPersianDataTable(row.createDate);
                    }
                },
                {
                    mRender: function (data, type, row) {
                        return renderButtons(editUrl, undefined, undefined, row.id);
                    }
                }
            ],
            // Column Definitions
            columnDefs: [
                { targets: "no-sort", orderable: false },
                { targets: "no-search", searchable: false },
                {
                    targets: "trim",
                    render: function (data, type, full, meta) {
                        if (type === "display") {
                            data = strtrunc(data, 10);
                        }

                        return data;
                    }
                },
                { targets: "date-type", type: "date-eu" }
            ]
        });
    }

    var loadSurveyComments = function surveyComments(detailsUrl, deleteUrl) {
        $("#jq-table").DataTable({
            responsive: true,
            "language": {
                "url": "/lib/datatables/js/Persian.json"
            },
            // Design Assets
            stateSave: true,
            autoWidth: true,
            // ServerSide Setups
            processing: true,
            serverSide: true,
            // Paging Setups
            paging: true,
            // Searching Setups
            searching: { regex: true },
            // Ajax Filter
            ajax: {
                url: "/api/DataTable/LoadSurveyCommentsTable",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: function (d) {
                    return JSON.stringify(d);
                }
            },
            // Columns Setups
            columns: [
                { data: "fullName" },
                { data: "email" },
                { data: "subject" },
                {
                    data: "message",
                    render: function (data, type, row) {
                        return reduceTextToSmallerWidth(row.message, 50);
                    }
                },
                { data: "ip" },
                {
                    data: "createDate",
                    render: function (data, type, row) {
                        return AdminPanel.ConvertEnglishDateToPersianDataTable(row.createDate);
                    }
                },
                {
                    mRender: function (data, type, row) {

                        var linkDetails = `<a href="${detailsUrl}/-1" class="btn btn-warning" title="جزئیات"><i class="fa fa-list"></i></a>`;
                        linkDetails = linkDetails.replace("-1", row.id);

                        var linkDelete = `<a href="${deleteUrl}/-1" class="btn btn-danger" title="حذف"><i class="fa fa-trash"></i></a>`;
                        linkDelete = linkDelete.replace("-1", row.id);

                        return linkDetails + " | " + linkDelete;
                    }
                }
            ],
            // Column Definitions
            columnDefs: [
                { targets: "no-sort", orderable: false },
                { targets: "no-search", searchable: false },
                {
                    targets: "trim",
                    render: function (data, type, full, meta) {
                        if (type === "display") {
                            data = strtrunc(data, 10);
                        }

                        return data;
                    }
                },
                { targets: "date-type", type: "date-eu" }
            ]
        });
    }

    var loadUsers = function users(editUrl, detailsUrl, deleteUrl, disableUrl) {
        let table = $("#jq-table").DataTable({
            responsive: true,
            "language": {
                "url": "/lib/datatables/js/Persian.json"
            },
            // Design Assets
            stateSave: true,
            autoWidth: true,
            // ServerSide Setups
            processing: true,
            serverSide: true,
            // Paging Setups
            paging: true,
            // Searching Setups
            searching: { regex: true },
            // Ajax Filter
            ajax: {
                url: "/api/DataTable/LoadUsersTable",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: function (d) {
                    return JSON.stringify(d);
                }
            },
            // Columns Setups
            columns: [
                { data: "firstName" },
                { data: "lastName" },
                { data: "phoneNumber" },
                {
                    data: "isActive",
                    render: function (data, type, row) {
                        return `<input type="checkbox" class="chk-user-active" id="${row.id}" ${row.isActive ? 'checked="true"' : ""}>`;
                    }
                },
                {
                    data: "createDate",
                    render: function (data, type, row) {
                        return AdminPanel.ConvertEnglishDateToPersianDataTable(row.createDate);
                    }
                },
                {
                    mRender: function (data, type, row) {
                        return renderButtons(editUrl, detailsUrl, undefined, row.id);
                    }
                }
            ],
            // Column Definitions
            columnDefs: [
                { targets: "no-sort", orderable: false },
                { targets: "no-search", searchable: false },
                {
                    targets: "trim",
                    render: function (data, type, full, meta) {
                        if (type === "display") {
                            data = strtrunc(data, 10);
                        }

                        return data;
                    }
                },
                { targets: "date-type", type: "date-eu" }
            ]
        });


        return table;
    }

    var getUserImage = function userImage(url, userId) {
        $.ajax({
            url: url,
            dataType: "json",
            data: {
                id: `${userId}`
            },
            success: function (data) {
                let imgSrc;

                if (data != null) {
                    imgSrc = data;
                } else {
                    imgSrc = "/images/no-image.png";
                }

                var output = document.getElementById("files-preview");
                var img = document.createElement("IMG");
                img.setAttribute("class", "img-fluid w-100");
                img.setAttribute("src", imgSrc);
                output.appendChild(img);
            }
        });
    }

    //Blog
    var loadPosts = function posts(apiUrl, editUrl, deleteUrl) {
        $("#jq-table").DataTable({
            responsive: true,
            "language": {
                "url": "/lib/datatables/js/Persian.json"
            },
            // Design Assets
            stateSave: true,
            autoWidth: true,
            // ServerSide Setups
            processing: true,
            serverSide: true,
            // Paging Setups
            paging: true,
            // Searching Setups
            searching: { regex: true },
            // Ajax Filter
            ajax: {
                url: apiUrl,
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: function (d) {
                    return JSON.stringify(d);
                }
            },
            // Columns Setups
            columns: [
                { data: "title" },
                { data: "type" },
                {
                    data: "PostCategoryTitle",
                    render: function (data, type, row) {
                        if (row.PostCategoryTitle === null) {
                            return "بدون دسته بندی";
                        }
                        return row.PostCategoryTitle;
                    }
                },
                {
                    data: "body",
                    render: function (data, type, row) {
                        return reduceTextToSmallerWidth(row.body, 50);
                    }
                },
                { data: "tags" },
                { data: "userFullName" },
                {
                    data: "isCommentsOn",
                    render: function (data, type, row) {
                        return boolToText(row.isCommentsOn);
                    }
                },
                {
                    data: "createDate",
                    render: function (data, type, row) {
                        return AdminPanel.ConvertEnglishDateToPersianDataTable(row.createDate);
                    }
                },
                {
                    data: "lastEditDate",
                    render: function (data, type, row) {
                        return AdminPanel.ConvertEnglishDateToPersianDataTable(row.lastEditDate);
                    }
                },
                {
                    mRender: function (data, type, row) {
                        return renderButtons(editUrl, undefined, deleteUrl, row.id);
                    }
                }
            ],
            // Column Definitions
            columnDefs: [
                { targets: "no-sort", orderable: false },
                { targets: "no-search", searchable: false },
                {
                    targets: "trim",
                    render: function (data, type, full, meta) {
                        if (type === "display") {
                            data = strtrunc(data, 10);
                        }

                        return data;
                    }
                },
                { targets: "date-type", type: "date-eu" }
            ]
        });
    }

    var getPostImage = function postImage(url, postId) {
        $.ajax({
            url: url,
            dataType: "json",
            data: {
                id: `${postId}`
            },
            success: function (data) {
                let imgSrc;

                if (data != null) {
                    imgSrc = data;
                } else {
                    imgSrc = "/images/no-image.png";
                }

                var output = document.getElementById("files-preview");
                var img = document.createElement("IMG");
                img.setAttribute("class", "img-fluid w-100");
                img.setAttribute("src", imgSrc);
                output.appendChild(img);
            }
        });
    }


    var loadPostCategories = function cats(editUrl, detailsUrl, deleteUrl) {
        $("#jq-table").DataTable({
            responsive: true,
            "language": {
                "url": "/lib/datatables/js/Persian.json"
            },
            // Design Assets
            stateSave: true,
            autoWidth: true,
            // ServerSide Setups
            processing: true,
            serverSide: true,
            // Paging Setups
            paging: true,
            // Searching Setups
            searching: { regex: true },
            // Ajax Filter
            ajax: {
                url: "/api/DataTable/LoadPostCategoriesTable",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: function (d) {
                    return JSON.stringify(d);
                }
            },
            // Columns Setups
            columns: [
                { data: "name" },
                {
                    data: "createDate",
                    render: function (data, type, row) {
                        return AdminPanel.ConvertEnglishDateToPersianDataTable(row.createDate);
                    }
                },
                {
                    data: "lastEditDate",
                    render: function (data, type, row) {
                        return AdminPanel.ConvertEnglishDateToPersianDataTable(row.lastEditDate);
                    }
                },
                {
                    mRender: function (data, type, row) {
                        return renderButtons(editUrl, detailsUrl, deleteUrl, row.id);
                    }
                }
            ],
            // Column Definitions
            columnDefs: [
                { targets: "no-sort", orderable: false },
                { targets: "no-search", searchable: false },
                {
                    targets: "trim",
                    render: function (data, type, full, meta) {
                        if (type === "display") {
                            data = strtrunc(data, 10);
                        }

                        return data;
                    }
                },
                { targets: "date-type", type: "date-eu" }
            ]
        });
    }

    var loadPostComments = function postComments(apiUrl, editUrl, detailsUrl, deleteUrl, replyUrl) {
        $("#jq-table").DataTable({
            responsive: true,
            "language": {
                "url": "/lib/datatables/js/Persian.json"
            },
            // Design Assets
            stateSave: true,
            autoWidth: true,
            // ServerSide Setups
            processing: true,
            serverSide: true,
            // Paging Setups
            paging: true,
            // Searching Setups
            searching: { regex: true },
            // Ajax Filter
            ajax: {
                url: apiUrl,
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: function (d) {
                    return JSON.stringify(d);
                }
            },
            // Columns Setups
            columns: [
                { data: "userFullName" },
                { data: "postTitle" },
                {
                    data: "body",
                    render: function (data, type, row) {
                        return reduceTextToSmallerWidth(row.body, 50);
                    }
                },
                { data: "email" },
                {
                    data: "status",
                    render: function (data, type, row) {
                        switch (row.status) {
                            case 1000:
                                return "تایید شده";
                            case 2000:
                                return "نامشخص";
                            case 3000:
                                return "رد شده";
                            default:
                                return "";
                        }
                    }
                },
                { data: "ip" },
                {
                    data: "isEdited",
                    render: function (data, type, row) {
                        return boolToText(row.isEdited);
                    }
                },
                {
                    data: "createDate",
                    render: function (data, type, row) {
                        return AdminPanel.ConvertEnglishDateToPersianDataTable(row.createDate);
                    }
                },
                {
                    data: "lastEditDate",
                    render: function (data, type, row) {
                        return AdminPanel.ConvertEnglishDateToPersianDataTable(row.lastEditDate);
                    }
                },
                {
                    mRender: function (data, type, row) {

                        var linkReply = `<a href="${replyUrl}/-1" class="btn btn-success" title="پاسخ دادن"><i class="fa fa-comment"></i></a>`;
                        linkReply = linkReply.replace("-1", row.id);

                        return linkReply + " | " + renderButtons(editUrl, undefined, undefined, row.id);
                    }
                }
            ],
            // Column Definitions
            columnDefs: [
                { targets: "no-sort", orderable: false },
                { targets: "no-search", searchable: false },
                {
                    targets: "trim",
                    render: function (data, type, full, meta) {
                        if (type === "display") {
                            data = strtrunc(data, 10);
                        }

                        return data;
                    }
                },
                { targets: "date-type", type: "date-eu" }
            ]
        });
    }


    //===================================
    //              Datepicker
    //===================================
    var
        persianNumbers = [/۰/g, /۱/g, /۲/g, /۳/g, /۴/g, /۵/g, /۶/g, /۷/g, /۸/g, /۹/g],
        englishNumbers = [/0/g, /1/g, /2/g, /3/g, /4/g, /5/g, /6/g, /7/g, /8/g, /9/g],
        convertToEnglishNumber = function (str) {
            if (typeof str === 'string') {
                for (var i = 0; i < 10; i++) {
                    str = str.replace(persianNumbers[i], i).replace(englishNumbers[i], i);
                }
            }
            return str;
        };

    var getDatepickerDate = function getDatepickerValue(date) {
        date = convertToEnglishNumber(date);
        console.log(date);
        return moment.from(date, 'fa', 'YYYY/MM/DD HH:mm:ss').format('YYYY/MM/DD HH:mm:ss');
    }
    var datepickerRangeConvertToPersian = function dateConvertToPersian(englishDate) {
        return new persianDate(new Date(englishDate)).format("LLLL");
    }

    var initialDatePicker = function datepicker(elementId, initialValue) {
        var options;

        if (initialValue === "" || initialValue === null || initialValue == undefined) {
            options = {
                autoClose: true,
                format: 'YYYY/MM/DD HH:mm:ss',
                initialValue: false,
                timePicker: {
                    enabled: true,
                    meridian: {
                        enabled: false
                    }
                }
            }
        } else {
            options = {
                autoClose: true,
                format: 'YYYY/MM/DD HH:mm:ss',
                initialValueType: 'gregorian',
                timePicker: {
                    enabled: true,
                    meridian: {
                        enabled: false
                    }
                }
            }
        }

        return $(elementId).persianDatepicker(options);
    }

    //===================================
    //              Editor
    //===================================
    var initialEditor = function bindEditor(elementId) {
        let element;

        if (elementId === undefined) {
            element = $("#txtContent");
        } else {
            element = $(elementId);
        }

        element.ckeditor({
            language: 'fa',
            extraPlugins: 'wordcount'
        });
    }

    var initialEditorReadOnly = function bindEditorReadOnly(elementId) {
        let element;

        if (elementId === undefined) {
            element = $("#txtContent");
        } else {
            element = $(elementId);
        }

        element.ckeditor({
            readOnly: true,
            language: 'fa',
            extraPlugins: 'wordcount'
        });
    }

    //===================================
    //              Files Preview
    //===================================
    var initialFilesPreview = function filePreview() {

        window.onload = function () {
            //Check File API support
            if (window.File && window.FileList && window.FileReader) {
                var filesInput = document.getElementById("input-files-hidden");
                filesInput.addEventListener("change",
                    function (event) {
                        var files = event.target.files; //FileList object
                        var output = document.getElementById("files-preview");
                        output.innerHTML = "";
                        for (var i = 0; i < files.length; i++) {
                            var file = files[i];
                            //Only pics
                            if (!file.type.match("image"))
                                continue;
                            var picReader = new FileReader();
                            picReader.addEventListener("load",
                                function (event) {
                                    var picFile = event.target;
                                    var img = document.createElement("IMG");
                                    img.setAttribute("class", "thumbnail");
                                    img.setAttribute("src", `${picFile.result}`);

                                    output.appendChild(img);
                                });
                            //Read the image
                            picReader.readAsDataURL(file);
                        }
                    });
            } else {
                console.log("Your browser does not support File API");
            }
        }
    }
    var initialSingleFilePreview = function singleFilePreview() {
        window.onload = function () {
            //Check File API support
            if (window.File && window.FileList && window.FileReader) {
                var filesInput = document.getElementById("input-file-hidden");
                filesInput.addEventListener("change",
                    function (event) {
                        var files = event.target.files; //FileList object
                        var output = document.getElementById("files-preview");
                        output.innerHTML = "";
                        for (var i = 0; i < files.length; i++) {
                            var file = files[i];
                            //Only pics
                            if (!file.type.match("image"))
                                continue;
                            var picReader = new FileReader();
                            picReader.addEventListener("load",
                                function (event) {
                                    var picFile = event.target;
                                    var img = document.createElement("IMG");
                                    img.setAttribute("class", "thumbnail");
                                    img.setAttribute("src", `${picFile.result}`);

                                    output.appendChild(img);
                                });
                            //Read the image
                            picReader.readAsDataURL(file);
                        }
                    });
            } else {
                console.log("Your browser does not support File API");
            }
        }
    }

    //===================================
    //              Tags Input
    //===================================

    var initialTagsInput = function tagsInput() {
        tagger(document.getElementById("tags-input"),
            {
                allow_duplicates: false,
                allow_spaces: true,
                link: function () {
                    return false;
                }
            });
    }

    var initialTagsInputReadOnly = function tagsInputReadOnly() {
        tagger(document.getElementById("tags-input"),
            {
                allow_duplicates: false,
                allow_spaces: true,
                link: function () {
                    return false;
                }
            });

        $('.tagger').find('a.close').each(function () {
            $(this).remove();
        });

        $('.tagger').find('li.tagger-new').remove();
    }




    //===================================
    //              Date Convert
    //===================================
    var convertEnglishDateToPersianDataTable = function convertEnToPerDateDTable(persianDate) {
        var date = moment(persianDate).format('YYYY/MM/DD HH:mm:ss');
        return moment(date, 'YYYY/MM/DD HH:mm:ss').locale('fa').format('HH:mm:ss YYYY/MM/DD');
    }




    //Return Values(inner functions)
    return {
        LoadContacts: loadContacts,

        LoadSurveyComments: loadSurveyComments,

        LoadUsers: loadUsers,
        GetUserImage: getUserImage,

        LoadPosts: loadPosts,
        GetPostImage: getPostImage,
        LoadPostCategories: loadPostCategories,
        LoadPostComments: loadPostComments,

        InitialDatePicker: initialDatePicker,
        GetDatepickerDate: getDatepickerDate,
        DatepickerRangeConvertToPersian: datepickerRangeConvertToPersian,

        InitialEditor: initialEditor,
        InitialEditorReadOnly: initialEditorReadOnly,
        InitialFilesPreview: initialFilesPreview,
        InitialSingleFilePreview: initialSingleFilePreview,
        InitialTagsInput: initialTagsInput,
        InitialTagsInputReadOnly: initialTagsInputReadOnly,
        ConvertEnglishDateToPersianDataTable: convertEnglishDateToPersianDataTable
    }

})();