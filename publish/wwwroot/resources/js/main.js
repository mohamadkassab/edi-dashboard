const fSuggestionsList = $(".f-suggestions-list");
const fSuggestions = $(".f-suggestions");
const fItemNo = $("#f-item-no");
const fSeason = $("#f-season");
const fDateAfter = $("#f-date-created-after");
const fDateBefore = $("#f-date-created-before");
const fItemFamily = $("#f-item-family");
const fItemCategory = $("#f-item-category");
const fIsApproved = $("#f-isApproved");
const iItemNo = $("#i-item-no");
const iItemFamily = $("#i-item-family");
const iItemSeason = $("#i-item-season");
const iDateCreated = $("#i-date-created");
const iItemCategory = $("#i-item-category");
const iItemDivision = $("#i-division");
const iCountryOrigin = $("#i-country-origin");
const iVendor = $("#i-vendor");
const iProductSubGroup = $("#i-product-sub-group");
const iProductUnit = $("#i-product-unit");
const iProductGroup = $("#i-product-group");
const iBarcode = $("#i-barcode");
const iDescription = $("#i-description");
const iProductCategory = $("#i-product-category");
const iCreatedBy = $("#i-created-by");
const iStatus = $("#i-status");
const iSup = $("#i-sup");
const loadingIconMain = $("#loading-icon-main");
const pagesNumber = $("#pages-number");
const fItemStatus = $("#f-item-status");
const fItemLocation = $("#f-item-location");
const noImage = "../resources/images/frontendImages/noImage.png";
const itemsTotal = $("#items-total");
const attributeSetup = $(".attributeSetup");
const scrollUp = $("#scrollUp");
const successToastMain = $("#success-toast-hidden-main");
const failedToastMain = $("#failed-toast-hidden-main");
const disconnectedToastMain = $("#disconnected-toast-hidden-main");
const album = $(".album-image-container");
const albumImage = $("#album-image");
const image = $("#image");
const editedBy = $("label[id^='edited-by']");
const editionDate = $("label[id^='edition-date']");
const approveCheckbox = $("#approveCheckbox");
const approveLabel = $("#approveLabel");
const searchAll = $("#search-all");
const showAlbum = $("#show-album");
const searchPrevious = $("#search-previous");
const searchNext = $("#search-next");
const hideAlbum = $("#hide-album");
const save = $("#save");
const clearAll = $("#clear-all");

let isSearchAll = false;
let isSearchNext = false;
let isSearchPrevious = false;
let isSearchItem = false;
let isSave = false;
let isApproveItem = false;
let isDataEdited = false;
$(document).ready(function () {

    try {

        // hide suggenstions list
        fSuggestionsList.hide();

        // make sure everything clear
        ClearAll();

        //on click span scroll to the appropriate section
        attributeSetup.on("mousedown", function (event) {
            AttributeScroll(event.target);
        });

        // on click scroll to the top 
        scrollUp.on("mousedown", function () {
            ScrollTop();
        });

        //on click take the value of the li and put it inside the input
        fSuggestionsList.on("mousedown", "li", function (event) {
            SuggestionOnClick(event.target);
        });

        //on blur hide the list
        $(fSuggestions).on("blur", function (event) {
            SuggestionOnBlur(event.target);
        });

        //on input gather the suggestions
        $(fSuggestions).on("input", function (event) {
            SuggestionOnInput(event.target);
        });

        // on checkbox approve
        $(approveCheckbox).on("change", function () {
            ApproveItem();   
        });

        $(fItemNo).on("keydown", function (event) {
            if (event.key === "Enter" || event.keyCode === 13)
            {
                SearchAll();
            }
        });

        // on search all 
        $(searchAll).on("click", function (){
            SearchAll();
        });

        // album container
        $(showAlbum).on("click", function () {
            ShowAlbum();
        });

        $(searchPrevious).on("click", function () {
            SearchPrevious();
        });

        $(searchNext).on("click", function () {
            SearchNext();
        });

        $(pagesNumber).on("keydown", function (event) {
            if (event.key === 'Enter' || event.keyCode === 13) {
                SearchItem($(this).val());
            }
        });

        $(hideAlbum).on("click", function () {
            HideAlbum();
        });

        $(save).on("click", function () {
            Save();
        });

        $(clearAll).on("click", function () {
            ClearAll();
        });


        //right side bar display and hide
        const headerLiDisplay = $('[class^="header-li"]');
        const headerLiDisplayAccount = $(".header-right-side-display-account");
        const headerLiDisplaySections = $(".header-right-side-display-sections");
        const headerLiDisplaySetup = $(".header-right-side-display-setup");
        const headerRightSide = $(".header-right-side");
        const hAccount = $(".h-account");
        const hSections = $(".h-sections");
        const hSetup = $(".h-setup");
        const headerLiHide = $(".header-right-side-hide");
        let isMouseInsideDiv1 = false;
        let isMouseInsideDiv2 = false;

        headerLiDisplayAccount.on('mouseenter', function () {
            try {

                headerRightSide.fadeIn(200);
                hSections.css("display", "none");
                hSetup.css("display", "none");
                hAccount.css("display", "block");
                headerRightSide.css("display", "block");
                isMouseInsideDiv1 = true;
                isMouseInsideDiv2 = true;

            } catch (error) {
                ErrorLogs("main.js", "headerLiDisplayAccount.on.mouseenter", error, error.stack);
            }
        });

        headerLiDisplaySetup.on('mouseenter', function () {
            try {
                headerRightSide.fadeIn(200);
                hSections.css("display", "none");
                hAccount.css("display", "none");
                hSetup.css("display", "block");
                headerRightSide.css("display", "block");
                isMouseInsideDiv1 = true;
                isMouseInsideDiv2 = true;
            }
            catch (error) {
                ErrorLogs("main.js", "headerLiDisplaySetup.on.mouseenter", error, error.stack);
            }
        });

        headerLiDisplaySections.on('mouseenter', function () {
            try {
                headerRightSide.fadeIn(200);
                hSections.css("display", "block");
                hAccount.css("display", "none");
                hSetup.css("display", "none");
                headerRightSide.css("display", "block");
                isMouseInsideDiv1 = true;
                isMouseInsideDiv2 = true;
            }
            catch (error) {
                ErrorLogs("main.js", "headerLiDisplaySections.on.mouseenter", error, error.stack);
            }
        });
        headerLiHide.on('mouseenter', function () {
            try {
                headerRightSide.fadeOut(200);
                headerRightSide.css("display", "none");
                isMouseInsideDiv1 = false;
                isMouseInsideDiv2 = false;
            }
            catch (error) {
                ErrorLogs("main.js", "headerLiHide.on.mouseenter", error, error.stack);
            }     
        });

        headerRightSide.on('mouseleave', function () {
            try {
                isMouseInsideDiv1 = false;
                isMouseInsideDiv2 = false;
                headerRightSide.fadeOut(200);
                headerRightSide.css("display", "none");
            }
            catch (error) {
                ErrorLogs("main.js", "headerRightSide.on.mouseenter", error, error.stack);
            }
        });
    }
    catch (error) {
        console.error("Error:", error, error.stack);   
        $(loadingIconMain).hide();
        ErrorLogs("main.js", "document.ready", error, error.stack);   
    } 
});

function ErrorLogs(className, functionName, errorMessage, stackTrace) {
    try {
        const baseurl = "/error/Insert";
        const data = {
            className: className,
            functionName: functionName,
            errorMessage: errorMessage,
            stackTrace: stackTrace
        };

        $.ajax({
            type: "POST",
            dataType: "json",
            url: baseurl,
            data: data,
            success: null,
            error: null
        });
    }
    catch (error) {
        console.error("Error:", error, error.stack);
    }
}

function AttributeScroll(clickedItem) {
    try {
        const section = $(clickedItem).text();
        const sectionId = `#${section}`;
        const sectionIdValid = sectionId.replace(/\s/g, '');
        $("html, body").animate({
            scrollTop: $(sectionIdValid).offset().top
        }, 500);
    }
    catch (error) {
        ErrorLogs("main.js", "attributeSetup.on.mousedown", error, error.stack);
    }
}

function ScrollTop() {
    try {
        if (window.scrollY !== 0) {
            window.scrollTo(0, 0);
            requestAnimationFrame(scroll);
        }
    }
    catch (error) {
        ErrorLogs("main.js", "scrollUp.onMousedown", error, error.stack);
    }
}

function SuggestionOnClick(clickedItem) {
    try {
        const parent = $(clickedItem).parent().attr("id");
        const $parent = $("#" + parent.replace("-suggestions", ""));
        const text = $(clickedItem).text();
        const code = $(clickedItem).attr("code");
        $parent.attr("code", code);
        $parent.val(text);
    }
    catch (error) {
        ErrorLogs("main.js", "fSuggestionsList.on.mousedown", error, error.stack);
    }
}

function SuggestionOnBlur(clickedItem) {
    try {
        const id = $(clickedItem).attr("id");
        const $Input = $("#" + id);
        const $id = $("#" + id + "-suggestions");
        setTimeout(function () {
            $id.hide();
            $Input.css({
                "border-radius": "var( --Input-radius)"
            });
        }, 90);
    }
    catch (error) {
        ErrorLogs("main.js", "fSuggestions.on.blur", error, error.stack);
    }
}

function SuggestionOnInput(clickedItem) {
    try {
        const query = $(clickedItem).val();
        const id = $(clickedItem).attr("id");
        if (query != "") {
            const searchParams = new URLSearchParams([
                ["field", id],
                ["inputText", query]
            ]);
            const baseurl = "/itemdata/GetSuggestions?" + searchParams.toString();
            $.ajax({
                type: "GET",
                dataType: "json",
                url: baseurl,
                success: function (data) {
                    if (data !== null) {
                        displaySuggestions(data, id);
                    }
                },
                error: function (err) {
                    fSuggestionsList.empty();
                    fSuggestionsList.hide();
                }
            });
        }
        else {
            fSuggestionsList.empty();
            fSuggestionsList.hide();
            return;
        }
    }
    catch (error) {
        ErrorLogs("main.js", "fSuggestions.on.input", error, error.stack);
    }
}

function CommonClear() {
    $("#item-info-row div input").val("");
    $("#item-info-row div select").empty();
    $(image).attr("src", noImage);
    $(editedBy).text("Edited by:");
    $(editionDate).text("Date:");

    $("iframe").each(function () {
        let tinymceElement = $(this).contents().find("#tinymce");
        tinymceElement.empty();
    });
}
function ClearApproveItem() {
    try {
        $(approveCheckbox).off('change');
        $(approveCheckbox).prop('checked', false);
        toggleApproveCheckBox();
        $(approveCheckbox).on('change', function () {
            ApproveItem();
        });
        $(approveLabel).text("");
    }
    catch (error) {
        console.error("Error:", error, error.stack);
        ErrorLogs("main.js", "ClickApproveItem", error, error.stack);
    }
}

function ClearAfterSearchOne() {
    try {

        CommonClear();
        ClearApproveItem();
    }
    catch (error) {
        console.error("Error:", error, error.stack);
        ErrorLogs("main.js", "ClearAfterSearchOne", error, error.stack);
    }
}

function ClearAfterSearchAll() {
    try {
        CommonClear();
        ClearApproveItem();
        $(pagesNumber).val("");
        $(itemsTotal.text(""));
    }
    catch (error) {
        console.error("Error:", error, error.stack);
        ErrorLogs("main.js", "ClearAll", error, error.stack);
    }
}

function ClearAll() {
    try {
        CommonClear();
        ClearApproveItem();
        $("#item-filter-row div input").val("");
        $(pagesNumber).val("");
        $(itemsTotal.text(""));
        $(fDateAfter).val("");
        $(fDateBefore).val("");
    }
    catch (error) {
        console.error("Error:", error, error.stack);
        ErrorLogs("main.js", "ClearAll", error, error.stack);
    }
}
    function ShowAlbum() {
        try {
            const src = image.attr("src");
            album.css({ "display": "flex" });
            albumImage.attr("src", src);

        }
        catch (error) {
            console.error("Error:", error, error.stack);
            $(loadingIconMain).hide();
            ErrorLogs("main.js", "ShowAlbum", error, error.stack);
        }
    }

    function HideAlbum() {
        try {
            album.css({ "display": "none" });

        }
        catch (error) {
            console.error("Error:", error, error.stack);
            $(loadingIconMain).hide();
            ErrorLogs("main.js", "HideAlbum", error, error.stack);
        }
    }

    function toggleApproveCheckBox() {
        try {
            const isChecked = $(approveCheckbox).is(':checked');

            $('.toggle-track .toggle-indicator').css({
                'background': isChecked ? 'var(--Active-color)' : 'var(--secondary-color)',
                'transform': isChecked ? 'translateX(30px)' : 'translateX(0)'
            });

            $('.toggle-track .toggle-indicator .checkMark').css({
                'opacity': isChecked ? 1 : 0
            });

            $('.toggle-track').css({
                'box-shadow': isChecked ? '0px 0px 0px 0.2rem var(--Active-color)' : '0px 0px 0px 0.2rem var(--secondary-color)'
            });

        }
        catch
        {
            console.error("Error:", error, error.stack);
            ErrorLogs("main.js", "toggleApproveCheckBox", error, error.stack);
        }
    }

    function showSuccessToastMain() {
        try {
            setTimeout(function () {
                $(successToastMain).show();
            }, 300);
            setTimeout(function () {
                $(successToastMain).hide();
            }, 1000);
        }
        catch
        {
            console.error("Error:", error, error.stack);
            ErrorLogs("main.js", "showSuccessToastMain", error, error.stack);
        }
    }

    function showFailedToastMain() {
        try {
            setTimeout(function () {
                $(failedToastMain).show();
            }, 300);
            setTimeout(function () {
                $(failedToastMain).hide();
            }, 1000);
        }
        catch
        {
            console.error("Error:", error, error.stack);
            ErrorLogs("main.js", "showFailedToastMain", error, error.stack);
        }
    }

    function showDisconnectedToastMain() {
        try {
            setTimeout(function () {
                $(disconnectedToastMain).show();
            }, 300);
            setTimeout(function () {
                $(disconnectedToastMain).hide();
            }, 3000);
        }
        catch
        {
            console.error("Error:", error, error.stack);
            ErrorLogs("main.js", "showFailedToastMain", error, error.stack);
        }
    }

    function displaySuggestions(data, id) {
        try {
            const $List = $("#" + id + "-suggestions");
            const $Input = $("#" + id);

            $List.empty();
            if (data.suggestions === null || $.isEmptyObject(data.suggestions)) {
                $List.hide();
                $Input.css({
                    "border-radius": "var( --Input_radius);"
                });
                return;
            }
            else {
                $List.show();
                $Input.css({
                    "border-bottom-right-radius": "0px",
                    "border-bottom-left-radius": "0px"
                });
                for (let i = 0; i < data.suggestions.length; i++) {
                    $List.append($("<li>", {
                        text: data.suggestions[i].description,
                        "code": data.suggestions[i].code
                    }));
                }
            }
        }
        catch (error) {
            console.error("Error:", error, error.stack);   
            $(loadingIconMain).hide();
            ErrorLogs("main.js", "displaySuggestions", error, error.stack);  
        }
    }

function FillData(data) {
    try {
        if (data.itemInfo !== null && data.itemInfo !== "") {
            const item = data.itemInfo[0]
            $(iItemNo).val(item.itemNo);
            $(iItemFamily).val(item.itemFamily);
            $(iItemSeason).val(item.itemSeason);
            $(iItemCategory).val(item.itemCategory);
            $(iItemDivision).val(item.itemDivision);
            $(iCountryOrigin).val(item.countryOrigin);
            $(iVendor).val(item.vendor);
            $(iProductSubGroup).val(item.productSubGroup);
            $(iProductCategory).val(item.productCategory);
            $(iProductUnit).val(item.productUnit);
            $(iProductGroup).val(item.productGroup);
            $(iBarcode).val(item.barcode);
            $(iDescription).val(item.description);
            $(iCreatedBy).val(item.createdBy);
            $(itemsTotal).val(item.SearchedLength);
            $(approveLabel).text(`${item.approvedBy}`);
            $(iStatus).append($("<option>", {
                text: `GL: ${item.statusGl}`
            }));

            $(iStatus).append($("<option>", {
                text: `HDM: ${item.statusHdm}`
            }));
        }

        if (data.attributeValueList !== null) {
            if (data.attributeValueList[0] !== null) {
                for (let i = 0; i < data.attributeValueList[0].length; i++) {
                    try {
                        let iframeDocument = $(`#en-${data.attributeValueList[0][i].attributeId}_ifr`).contents();
                        let tinymce = iframeDocument.find("#tinymce");
                        let editedBy = $(`#edited-by-${data.attributeValueList[0][i].attributeId}`)
                        let editionDate = $(`#edition-date-${data.attributeValueList[0][i].attributeId}`)
                        editedByData = data.attributeValueList[0][i].userId.split('@')[0].split('.');
                        editionDateData = data.attributeValueList[0][i].timeStamp.split('T');
                        editionDate.text(`Date: ${editionDateData[0]} - ${editionDateData[1]}`);
                        editedBy.text(`Edited by: ${editedByData[0]} ${editedByData[1]}`)
                        tinymce.append(data.attributeValueList[0][i].attributeDesc);
                    }
                    catch (err) {
                        console.error("Error:", error, error.stack);
                        ErrorLogs("main.js", "FillData", error, error.stack);
                    }
                }
            }
            if (data.attributeValueList[1] !== null) {
                for (let i = 0; i < data.attributeValueList[1].length; i++) {
                    try {
                        let iframeDocument = $(`#ar-${data.attributeValueList[1][i].attributeId}_ifr`).contents();
                        let tinymce = iframeDocument.find("#tinymce");
                        let editedBy = $(`#edited-by-${data.attributeValueList[1][i].attributeId}`)
                        let editionDate = $(`#edition-date-${data.attributeValueList[1][i].attributeId}`)
                        editedByData = data.attributeValueList[1][i].userId.split('@')[0].split('.');
                        editionDateData = data.attributeValueList[1][i].timeStamp.split('T');
                        editionDate.text(`Date: ${editionDateData[0]} - ${editionDateData[1]}`);
                        editedBy.text(`Edited by: ${editedByData[0]} ${editedByData[1]}`)
                        tinymce.append(data.attributeValueList[1][i].attributeDesc);
                    }
                    catch (error) {
                        console.error("Error:", error, error.stack);
                        ErrorLogs("main.js", "FillData", error, error.stack);
                    }
                }
            }
        }

        if (data.ItemSup !== null) {
            for (let i = 0; i < data.itemSup.length; i++) {
                try {
                    const value = $(data.itemSup[i].attributeValue).text();
                    iSup.append($("<option>", {
                        text: `${data.itemSup[i].attributeName}: ${value}`
                    }));
                }
                catch (error) {
                    console.error("Error:", error, error.stack);
                    ErrorLogs("main.js", "FillData", error, error.stack);
                }
            }
        }

        if (data.itemInfo[0].dateCreated !== null && data.itemInfo[0].dateCreated !== "") {
            $(iDateCreated).val(data.itemInfo[0].dateCreated.split("T")[0]);
        }

        if (data.itemInfo[0].isActive == true) {
            $(approveCheckbox).off('change');
            if (!$(approveCheckbox).prop('disabled')) {

                $(approveCheckbox).click();
            }
            else {
                $(approveCheckbox).removeAttr("disabled").click().attr("disabled", true);
            }

            toggleApproveCheckBox();
            $(approveCheckbox).on('change', function () {
                ApproveItem();
            });
        }
        if (data.imagePath !== null && data.imagePath !== "") {
            image.attr("src", data.imagePath);
        }
        else {
            image.attr("src", noImage);
        }
        isDataEdited = false;
    }
    catch (error) {
        console.error("Error:", error, error.stack);
        ErrorLogs("main.js", "FillData", error, error.stack);
    }
}

    //on search button, it will gather rows of item nbrs
    function SearchAll() {
        try {

            if (isSearchAll) {
                return; 
            }
            if (!isDataEdited || confirm("You have unsaved changes. Are you sure you want to continue?")) {
                isDataEdited = false;
                isSearchAll = true;

                let itemNo = "%";
                let season = "%";
                let dateAfter = "";
                let dateBefore = "";
                let itemFamily = "%";
                let itemCategory = "%";
                let isApproved = $(fIsApproved).val();

                if ($(fItemNo).val() != "") {
                    itemNo = $(fItemNo).val();
                }
                if ($(fSeason).val() != "") {
                    season = $(fSeason).attr("code");
                }
                if ($(fDateAfter).val() != "") {
                    dateAfter = $(fDateAfter).val();
                }
                if ($(fDateBefore).val() != "") {
                    dateBefore = $(fDateBefore).val();
                }
                if ($(fItemFamily).val() != "") {
                    itemFamily = $(fItemFamily).attr("code");
                }
                if ($(fItemCategory).val() != "") {
                    itemCategory = $(fItemCategory).attr("code");
                }

                const searchParams = new URLSearchParams([
                    ["itemNo", itemNo],
                    ["season", season],
                    ["dateAfter", dateAfter],
                    ["dateBefore", dateBefore],
                    ["itemFamily", itemFamily],
                    ["itemCategory", itemCategory],
                    ["isApproved", isApproved]
                ]);

                const baseurl = "/itemdata/searchall?" + searchParams.toString();
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    url: baseurl,
                    timeout: 20000,
                    beforeSend: function () {
                        $(loadingIconMain).show();
                        ClearAfterSearchAll();
                    },
                    success: function (data) {
                        if (data !== null) {
                            if (data.searchedLength !== null && data.searchedLength !== "") {
                                if (data.searchedLength > 0) {
                                    $(itemsTotal.text(data.searchedLength));
                                    $(pagesNumber).val("1");
                                    FillData(data);
                                }
                            }
                        }
                        $(loadingIconMain).hide();
                    },
                    error: function (err) {
                        $(loadingIconMain).hide();
                        showDisconnectedToastMain();

                    },
                    complete: function () {
                        isSearchAll = false;
                    }
                });
            }

            else
            {
                return;
            }

           
        }
        catch (error) {
            console.error("Error:", error, error.stack); 
            ClearAfterSearchAll();
            $(loadingIconMain).hide();
            showDisconnectedToastMain();
            isSearchAll = false;
            ErrorLogs("main.js", "SearchAll", error, error.stack);
        }
    }

    //on next button, it will gather one item 
    function SearchNext() {
        try {

            if (isSearchNext) {
                return; 
            }
            isSearchNext = true;

            const nextIndex = parseInt($(pagesNumber).val()) + 1;
            const params = new URLSearchParams([
                ["itemNo", nextIndex],
            ]);
            const baseurl = "/itemdata/searchone?" + params.toString();
            $.ajax({
                type: "GET",
                dataType: "json",
                url: baseurl,
                timeout: 20000,
                beforeSend: function () {
                    $(loadingIconMain).show();
                    ClearAfterSearchOne();
                },
                success: function (data) {
                    if (data !== null) {
                      
                        $(pagesNumber.val(nextIndex));
                        FillData(data);
                    }
                    $(loadingIconMain).hide();
                },
                error: function (err) {
                    $(loadingIconMain).hide();
                    showDisconnectedToastMain();
                },
                complete: function () {
                    isSearchNext = false;
                }
            });
        }
        catch (error) {
            console.error("Error:", error, error.stack); 
            $(loadingIconMain).hide();
            showDisconnectedToastMain();
            isSearchNext = false;
            ErrorLogs("main.js", "SearchNext", error, error.stack);
        }
    }

    //on previous button, it will gather one item 
    function SearchPrevious() {
        try {
            if (isSearchPrevious) {
                return;
            }
            isSearchPrevious = true;

            const previousIndex = parseInt($(pagesNumber).val()) - 1;
            const params = new URLSearchParams([
                ["itemNo", previousIndex],
            ]);
            const baseurl = "/itemdata/searchone?" + params.toString();

            $.ajax({
                type: "GET",
                dataType: "json",
                url: baseurl,
                timeout: 20000,
                beforeSend: function () {
                    $(loadingIconMain).show();
                    ClearAfterSearchOne();
                },
                success: function (data) {
                    if (data !== null) {
                        $(pagesNumber.val(previousIndex));
                        FillData(data);
                    }
                    $(loadingIconMain).hide();
                },
                error: function (err) {
                    $(loadingIconMain).hide();
                    showDisconnectedToastMain();
                },
                complete: function () {
                    isSearchPrevious = false;
                }
            });
        }
        catch (error) {
            console.error("Error:", error, error.stack);
            $(loadingIconMain).hide();
            showDisconnectedToastMain();
            isSearchPrevious = false;
            ErrorLogs("main.js", "SearchPrevious", error, error.stack);

        }
    }

    //on select an item from the list of items, it will gather one item 
    function SearchItem(clickedItem) {
        try {
            if (isSearchItem) {
                return;
            }
            isSearchItem = true;
            const index = clickedItem;
            const params = new URLSearchParams([
                ["itemNo", index],
            ]);
            const baseurl = "/itemdata/searchone?" + params.toString();
            ClearAfterSearchOne();
            $.ajax({
                type: "GET",
                dataType: "json",
                url: baseurl,
                timeout: 20000,
                beforeSend: function () {
                    $(loadingIconMain).show();
                    ClearAfterSearchOne();
                },
                success: function (data) {
                    if (data !== null) {
                      
                        FillData(data);
                    }
                    $(loadingIconMain).hide();
                },
                error: function (err) {
                    $(loadingIconMain).hide();
                    showDisconnectedToastMain();
                },
                complete: function () {
                    isSearchItem = false;
                }
            });
        }
        catch (error) {
            console.error("Error:", error, error.stack);
            $(loadingIconMain).hide();
            showDisconnectedToastMain();
            isSearchItem = false;
            ErrorLogs("main.js", "SearchItem", error, error.stack);
        }
    }
   
    function Save() {
        try {
            if (isSave) {
                return;
            }
            isSave = true;
            const itemNo = iItemNo.val();
            const attributeList = [];
            const baseurl = "/itemdata/Save";
            $("iframe").each(function () {
                const tinymceElement = $(this).contents().find("#tinymce");
                const attributeId = $(this).attr("id");
                const attributeValue = tinymceElement.html();
                const attribute = {
                    AttributeId: attributeId,
                    AttributeValue: attributeValue
                };
                attributeList.push(attribute);
            });

            const data = {
                itemNo: itemNo,
                attributeList: attributeList
            };

            $.ajax({
                type: "POST",
                dataType: "json",
                url: baseurl,
                data: data,
                timeout: 20000,

                beforeSend: function () {
                    $(loadingIconMain).show();
                    ClearApproveItem();
                },

                success: function (data) {
                    isDataEdited = false;
                    if (data != null) {
                        if (data.approvedBy != null) {
                            $(approveLabel).text(`${data.approvedBy}`);
                        }
                        else {
                            $(approveLabel).text(``);
                        }

                        $(loadingIconMain).hide();
                        showSuccessToastMain();
                    }
                    else {
                        $(loadingIconMain).hide();
                        showFailedToastMain();
                    }          
                },
                error: function (err) {
                    $(loadingIconMain).hide();
                    showDisconnectedToastMain();
                }, complete: function () {
                    isSave = false;
                }
            });
        }
        catch (error) {
            console.error("Error:", error, error.stack);
            $(loadingIconMain).hide();
            showDisconnectedToastMain();
            isSave = false;
            ErrorLogs("main.js", "Save", error, error.stack);
        }
    }

    function ApproveItem()
    {
        try {
            if (isApproveItem) {
                return;
            }
            isApproveItem = true;
            const baseurl = "/itemdata/Approve";
            const isChecked = $(approveCheckbox).is(':checked');
            const itemNo = iItemNo.val();
            const data = {
                itemNo: itemNo,
                status: isChecked
            };
            $.ajax({
                type: "POST",
                dataType: "json",
                url: baseurl,
                data: data,
                timeout:20000,
                beforeSend: function () {
                    $(loadingIconMain).show();
                },
                success: function (data) {
                    $(loadingIconMain).hide();
                    if (data != null) {
                        if (data.approvedBy != null) {
                            $(approveLabel).text(`${data.approvedBy}`);
                        }
                        else
                        {
                            $(approveLabel).text(``);
                        }
                        toggleApproveCheckBox();
                        showSuccessToastMain();
                    }
                    else {
                        $(loadingIconMain).hide();
                        showFailedToastMain();
                    }
                },
                error: function (err) {
                    $(loadingIconMain).hide();
                    showDisconnectedToastMain();
                },
                complete: function () {
                    isApproveItem = false;
                }
            });
        }
        catch (error) {
            console.error("Error:", error, error.stack);
            $(loadingIconMain).hide();
            showDisconnectedToastMain();
            isApproveItem = false;
            ErrorLogs("main.js", "Save", error, error.stack);
        }
    }


