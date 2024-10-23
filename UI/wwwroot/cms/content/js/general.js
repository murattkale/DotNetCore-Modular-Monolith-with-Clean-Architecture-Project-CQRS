if (window.addEventListener) {
    window.addEventListener('load', general_Load, false);
}
else {
    window.attachEvent('onload', general_Load);
}

function isFloat(n) {
    return Number(n) === n && n % 1 !== 0;
}

function getQuery(name, url = window.location.href) {
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('#') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}




function general_Load() {
    window.onbeforeunload = setClick;
}



var postArray = [];
function setClick() {
    for (var i = 0; i < postArray.length; i++) {
        postArray[i].abort();
    }
    // cl();
}


var toStr = function (e) {
    return e == undefined || e == "undefined" || e == null || e == "null" ? "" : e.toString();
};

var toInt = function (e) {
    return e == undefined || e == "undefined" || e == null || e == "null" ? 0 : parseInt(e);
};

var toFloat = function (e) {
    return e == "" || e == undefined || e == "undefined" || e == null || e == "null" ? 0 : parseFloat(e);
};

var toFloat1 = function (e) {
    return e == "" || e == undefined || e == "undefined" || e == null || e == "null" ? 1 : parseFloat(e);
};


var isBool = function (variable) {
    if (typeof (variable) === "boolean") {
        return true;
    }
    else {
        return false;
    }
}

var toBool = function (value) {
    return value == true ? "True" : "False";
}

var isNumeric = function (n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

jQuery('.isnumeric').keyup(function () {
    this.value = this.value.replace(/[^0-9\.]/g, '');
});





function GetSpecListType(SpecListType, name) {
    $.get('/Spec/GetSpecListTypeList', { SpecListType: SpecListType, name: name }).fail(function (err, exception) { console.error(err.responseText); })
        .done(function (res) {
            $('#dp_' + res.Joker + '').append($('<option>', { value: "", text: "Select" }));
            var joker = rowProject[res.Joker];
            $.each(res.ResultRow?.SpecAttrs, function (i, item) {
                var str = '<option  value="' + item.Id + '" ' + (joker == item.Id ? 'selected="selected"' : '') + '>' + item.Name + (item.Desc != null ? ' (' + item.Desc + ')' : '') + '</option>';
                $('#dp_' + res.Joker + '').append(str);
            });
        });
}

function GetHazardRatingType(RatingModel, RatingMeasureType, name) {
    $('#dp_' + name + '_label').removeClass('d-none');
    $('#' + name + '_label').removeClass('d-none');

    $.get('/HazardRating/GetHazardRatingType', { RatingModel: RatingModel, RatingMeasureType: RatingMeasureType, name: name }).fail(function (err, exception) { console.error(err.responseText); })
        .done(function (res) {
            $('#dp_' + res.Joker + '').append($('<option>', { value: "", text: "Select" }));
            var joker = rowProject[res.Joker];
            $.each(res.ResultList, function (i, item) {
                var str = '<option datavalue="' + item.RatingMeasureValue + '" value="' + item.Id + '" ' + (joker == item.Id ? 'selected="selected"' : '') + '>' + item.RatingMeasureValue + ' - ' + item.RatingMeasureName + '</option>';
                $('#dp_' + res.Joker + '').append(str);
            });


        });
}

function getLang() {
    $.get('/Lang/GetSelect').fail(function (err, exception) { console.error(err.responseText); })
        .done(function (res) {
            $('#dp_LangId').append($('<option>', { value: "", text: "Select" }));
            var LangId = "";
            try {
                LangId = rowProject.LangId;
            } catch (e) {

            }
            $.each(res, function (i, item) {
                var str = '<option  value="' + item.value + '" ' + (LangId == item.value ? 'selected="selected"' : '') + '>' + item.text + '</option>';
                $('#dp_LangId').append(str);
            });
        });
}

function GetHazardSection(ProjectId) {
    $.get('/ProjectHazardSection/GetSelect?ProjectId=' + ProjectId).fail(function (err, exception) { console.error(err.responseText); })
        .done(function (res) {
            $('#dp_ProjectHazardSectionId').append($('<option>', { value: "", text: "Select" }));

            var joker = "";
            try {
                joker = rowProject.ProjectHazardSectionId;
            } catch (e) {

            }

            $.each(res, function (i, item) {
                var str = '<option  value="' + item.value + '" ' + (joker == item.value ? 'selected="selected"' : '') + '>' + item.text + '</option>';
                $('#dp_ProjectHazardSectionId').append(str);
            });
        });
}


function GetContentPage(ContentTypes, Joker, Joker2) {
    $.get('/ContentPage/GetSelectCustom?ContentTypes=' + ContentTypes + '&Joker=' + Joker + '&Joker2=' + Joker2).fail(function (err, exception) { console.error(err.responseText); })
        .done(function (res) {
            var Joker = res.Joker;
            var Joker2 = res.Joker2;
            $('#' + Joker).append($('<option>', { value: "", text: "Select" }));
            $.each(res.ResultList, function (i, item) {
                var str = '<option  value="' + item.value + '" ' + '>' + item[Joker2] + '</option>';
                $('#' + Joker).append(str);
            });
        });
}



function getObjects(obj, key, val) {
    var objects = [];
    for (var i in obj) {
        if (!obj.hasOwnProperty(i)) continue;
        if (typeof obj[i] == 'object') {
            objects = objects.concat(getObjects(obj[i], key, val));
        } else
            if (i == key && obj[i] == val || i == key && val == '') { //
                objects.push(obj);
            } else if (obj[i] == val && key == '') {
                if (objects.lastIndexOf(obj) == -1) {
                    objects.push(obj);
                }
            }
    }
    return objects;
}



function getEnumList(dataResult, id, selectid, selectText) {
    $(id).addOption(dataResult, "value", "text", null, null, selectid, '', selectText);
}

function getEnumRow(dataResult, value) {
    if (value < 1) {
        row = { value: "", text: "", name: "" };
        return row;
    }
    var row;
    for (var i = 0; i < dataResult.length; i++) {
        var rowItem = dataResult[i];
        if (parseInt(rowItem.value) == parseInt(value)) {
            row = rowItem;
            break;
        }
    }
    return row;
}

function getEnumRowName(dataResult, value) {
    if (value < 1) {
        row = { value: "", text: "", name: "" };
        return row;
    }
    var row;
    for (var i = 0; i < dataResult.length; i++) {
        var rowItem = dataResult[i];
        if (rowItem.name == value) {
            row = rowItem;
            break;
        }
    }
    return row;
}




(function ($) {
    $.fn.ceo = function (ayarlar) {
        var ayar = $.extend({
            'source': "#" + $(this)[0].name,
            'target': "",
        }, ayarlar);

        $(ayar.source).on("keyup", function () {
            str = $(this).val();
            str = replaceSpecialChars(str);
            str = str.toLowerCase();
            str = str.replace(/\s\s+/g, ' ').replace(/[^a-z0-9\s]/gi, '').replace(/[^\w]/ig, "-");
            function replaceSpecialChars(str) {
                var specialChars = [["ş", "s"], ["ğ", "g"], ["ü", "u"], ["ı", "i"], ["_", "-"],
                ["ö", "o"], ["Ş", "S"], ["Ğ", "G"], ["Ç", "C"], ["ç", "c"],
                ["Ü", "U"], ["İ", "I"], ["Ö", "O"], ["ş", "s"]];
                for (var i = 0; i < specialChars.length; i++) {
                    str = str.replace(eval("/" + specialChars[i][0] + "/ig"), specialChars[i][1]);
                }
                return str;
            }
            $(ayar.target).val(str);
        });
    };

})(jQuery);

(function ($) {
    $.fn.dup = function (ayarlar) {
        var ayar = $.extend({
            'source': "#" + $(this)[0].name,
            'target': "",
        }, ayarlar);

        $(ayar.source).on("keyup", function () {
            str = $(this).val();
            $(ayar.target).val(str);
        });
    };

})(jQuery);


function alerts(message, button, call) {
    var but = [];
    if (button == "yesno") {
        but = {
            confirm: {
                label: "Evet",
                className: 'btn-success'
            },
            cancel: {
                label: "Hayır",
                className: 'btn-danger'
            }
        };
    }
    else if (button == "ok") {
        but = {
            confirm: {
                label: "Tamam",
                className: 'btn-success'
            }
        };
    }

    if (call) {
        bootbox.confirm({
            message: message,
            buttons: but,
            callback: call,
        });
    }
    else {
        bootbox.alert(message);
    }

}


(function ($) {
    "use strict";



    var postArray = [];
    $.ajx = function (url, postmodel, successMethod, error) {
        //if (typeof data != "string" && data != null) {
        //    data = JSON.stringify(data);
        //}
        var slash = url.substr(0, 1) == "/" ? "" : "/";

        var post = $.post(slash + url, postmodel, successMethod)
            .fail(function (e, exception) {
                if (error) {
                    var errorResult = $.errorSend(e, exception);
                    console.log(exception);
                }
            });
        postArray.push(post);
    };

    $.ajxUpload = function (url, data, successMethod, error) {
        //if (typeof data != "string" && data != null) {
        //    data = JSON.stringify(data);
        //}
        var slash = url.substr(0, 1) == "/" ? "" : "/";

        $.ajax({
            url: slash + url,
            type: "POST",
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data  
            data: data,
            success: successMethod,
            error: function (e, exception) {
                if (error) {
                    var errorResult = $.errorSend(e, exception);
                    console.log(errorResult);
                }
            }
        });

    };

    $.errorSend = function (e, exception) {
        var error = '';
        if (e.status === 0) {
            error = 'Not connect. Verify Network.';
        } else if (e.status === 404) {
            error = 'Requested page not found. [404]';
        } else if (e.status === 500) {
            error = 'Internal Server Error [500].';
        } else if (exception === 'parsererror') {
            error = 'Requested JSON parse failed.';
        } else if (exception === 'timeout') {
            error = 'Time out error.';
        } else if (exception === 'abort') {
            error = 'Ajax request aborted.';
        } else {
            error = 'Uncaught Error. \n' + error.responseText;
        }
        var data = { error: error };

        return error;
    };

    $.fn.addOption = function (selectValues, value, text, dpChange, dpSuccess, selectValue, selectText, selectDefault, attrName) {

        var thisid = $(this);
        $.each(thisid, function (i, id) {
            $(id).html('');
            if (selectDefault) {
                var optionsAll = $("<option></option>")
                    .attr("value", '')
                    .text(selectDefault);
                $(id).append(optionsAll);
            }

            $.each(selectValues, function (i, item) {

                var splitText = text.split(',');
                var textValue = "";
                for (var ii = 0; ii < splitText.length; ii++)
                    textValue += item[splitText[ii]] + (ii >= 0 ? ' ' : '');

                var ustkategori = "";
                try {
                    ustkategori = item.Parent.text + ' / ';
                } catch (e) {

                }

                var optionsAll = $("<option></option>")
                    .attr("value", item[value])
                    .text(ustkategori + textValue);

                if (attrName)
                    $(optionsAll)
                        .attr(attrName, item[attrName]);


                if (selectValue != null && selectValue != "" && value != '' && value != undefined && selectValue.toString() == item[value].toString()) {
                    $(optionsAll).attr('selected', "selected");
                }
                if (selectText != null && text != '' && text != undefined && selectText == item[text]) {
                    $(optionsAll).attr('selected', "selected");
                }

                var sectionid = toInt($(id).attr('sectionid'));
                if (sectionid > 0 && sectionid == item.value) {
                    $(optionsAll).attr('selected', "selected");
                }

                //var contentid = $(id).attr('contentid');
                //if (contentid > 0 && contentid == item.value) {
                //    $(optionsAll).attr('selected', "selected");
                //}


                $(id).append(optionsAll);

            });

            if (dpChange != null) {
                $(document).on('change', id, function (e) { dpChange(e.target); });
            }
            if (dpSuccess != null) {
                dpSuccess(id);
                //$(document).on('', id, function (e) { dpSuccess(e.target); });
            }
            //try {
            //    $(id).select2({});
            //} catch (e) {
            //    console.log(e);
            //}

        });



    };

    $.fn.addOptionAjax = function (url, param, value, text, dpChange, dpSuccess, selectValue, selectText, selectDefault, attrName) {
        var id = this;
        $(id).LoadingOverlay("show");
        $(id).html('');

        var slash = url.substr(0, 1) == "/" ? "" : "/";

        $.ajx(slash + url, param, function (dataResult) {
            $(id).addOption(dataResult, value, text, dpChange, dpSuccess, selectValue, selectText, selectDefault, attrName);
            $(id).LoadingOverlay("hide");
        }, null);

    };


    $.fn.toForm = function (id) {//serialize data function

        var formArray = $(id).serializeArray();
        var returnArray = {};

        for (var i = 0; i < formArray.length; i++) {
            var finame = (formArray[i]['name']).toString();
            var fivalue = (formArray[i]['value']).toString();
            if (finame.indexOf("Id") != -1 && fivalue == "-1") {
                returnArray[formArray[i]['name']] = null;
            }
            else {
                if (finame) {
                    if (finame.indexOf('file_link_') != -1) {
                        returnArray[finame.replace('file_link_', '')] = fivalue.trim();
                    }
                    else
                        returnArray[formArray[i]['name']] = fivalue.trim();
                }

            }
        }
        $(id + ' select[disabled],' + id + ' select').each(function () {
            var dp = $(this).attr('name');
            try {
                if (dp.indexOf('dp_') != -1) {
                    dp = dp.substr(3, dp.length - 1);
                }
            } catch (e) {
                console.log('select error: ' + e);
            }

            returnArray[dp] = $(this).val();
            if (isFloat(returnArray[dp]))
                returnArray[dp] = parseFloat(returnArray[dp]);

            try {
                if (dp.indexOf('Id') != -1 && ($(this).val() == "-1" || $(this).val() == "0" || $(this).val() == "")) {
                    returnArray[dp] = null;
                }
            } catch (e) {
                console.log(e);
            }


            delete returnArray[$(this).attr('name')];

        });
        $(id + ' input[disabled]').each(function (i, item) {
            if ($(this).attr('name')) {
                if ($(this).attr('name').indexOf('file_link_') != -1) {
                    returnArray[$(this).attr('name').replace('file_link_', '')] = $(this).val().trim();
                }
                else
                    returnArray[$(this).attr('name')] = $(this).val().trim();
            }

        });

        $(id + ' textarea').each(function () {
            try {
                returnArray[$(this).attr('name')] = $(this).val();

            } catch (e) {
                console.error(e);
            }

        });


        $(id + ' input[type="checkbox"]').each(function () {
            returnArray[$(this).attr('name')] = $(this).prop("checked");
        });

        return returnArray;
    };

    $.getAttrs = function (attrName) {
        var data = this;
        var attrValues = [];
        $.each(data, function (i, item) {
            attrValues.push({ id: $(item).attr(attrName) });
        });
        return attrValues;
    };


})(jQuery);