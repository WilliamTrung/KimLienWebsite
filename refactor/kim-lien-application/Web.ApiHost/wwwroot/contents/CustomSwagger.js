function autoValue() {
    let allEndpoint = $('li.endpoint');
    for (var i = 0; i < allEndpoint.length; i++) {
        let crrLi = $(allEndpoint[i]);
        let tempValue = crrLi.find('div.markdown').find('code').html();
        let txtValueInfo = crrLi.find('textarea[name="valueInfo"]');
        $(txtValueInfo).width('400px');
        if (tempValue) {
            $(txtValueInfo).val(tempValue.trim());
        } else {
            $(txtValueInfo).val('{}');
        }

    }
}

function getCookie(ckName) {
    var re = '';
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(ckName + "=");
        if (c_start != -1) {
            c_start = c_start + ckName.length + 1;
            c_end = document.cookie.indexOf(";", c_start);
            if (c_end == -1) {
                c_end = document.cookie.length;
            }
            re = unescape(document.cookie.substring(c_start, c_end));
        }
    }
    return re;
}

function addTimKiem() {
    let elAppInfo = $('body')//$('#api_info');
    let html = [];

    //html.push('<div style="padding-bottom: 5px"><span style="font-weight:bold">User: </span>');
    //let ck = getCookie("CrrUser");
    //if (ck) {
    //    html.push('<i>' + ck + '</i>');
    //} else {
    //    html.push('<i>N/A</i>');
    //}

    //html.push('<span style="margin-left: 5px; font-weight:bold">Site: </span>');
    //ck = getCookie("SiteUrl");
    //if (ck) {
    //    html.push('<i>' + ck + '</i>');
    //} else {
    //    html.push('<i>N/A</i>');
    //}
    //html.push('</div>')

    html.push(`<div><div style="position: fixed;
    z-index:1000;
    width: 100%;
    background: #e0ddad;
    padding: 20px; "><div class="col-md-8"><input id="txtTimAPI" type="text" placeholder="Tìm api (enter)" class="form-control"/></div>`);
    html.push('<div class="col-4"><input type="button" value="Tìm" style="margin-left: 10px" class="btn btn-success" onclick="fnTimAPI()"/>');
    html.push('<input type="button" value="Hiện hết" style="margin-left: 10px" class="btn btn-success" onclick="openAll()"/>');
    html.push('<input type="button" value="Ẩn hết" style="margin-left: 10px;" class="btn btn-success" onclick="closeAll()"/>');
    html.push('<input type="checkbox" style="margin-left: 10px;" onchange="showModel(this)"/> Hiện model');
    html.push('</div></div></div>');
    $(elAppInfo).prepend(html.join(''));
}

function fnTimAPI() {
    let key = $('#txtTimAPI').val();
    if (key) {
        key = key.toLowerCase();
        $.each($('div.opblock-tag-section'), function (index, value) {
            let methods = $(value).find('div.operation-tag-content span');
            let found = false;
            $.each(methods, function (methodIndex, method) {
                let methodName = $(method).text().toLowerCase();
                if (methodName.indexOf(key) != -1) {
                    $(method).show();
                    found = true;
                } else {
                    $(method).hide();
                }
                if (methodIndex == methods.length - 1) {
                    if (found)
                        $(value).show();
                    else if ($(value).text().toLowerCase().indexOf(key) != -1) {
                        $(value).show(); methods.show();
                    }
                    else
                        $(value).hide();
                }
            });
        });
    } else {
        $('div.opblock-tag-section').show();
        $('div.operation-tag-content span').show();
        openAll();
    }
}

function openAll() {
    $('div.opblock-tag-section').show();
    $('div.operation-tag-content span').show();
}

function closeAll() {
    $('div.opblock-tag-section').show();
    $('div.operation-tag-content span').hide();
}

function fileupload() {
    $('#File_File_Upload_content input[type="file"]').attr('multiple', 'multiple');
}

function showModel($this) {
    if ($this.checked)
        $('section.models ').show();
    else 
        $('section.models ').hide();
}

$(document).ready(function () {
    addTimKiem();
    //autoValue();
    //fileupload();
    $('input.submit').val('Gửi');
    $('#txtTimAPI').on('keypress', function (e) {
        if (e.keyCode == '13') {
            e.preventDefault();
            fnTimAPI();
        }
    });
    setTimeout(function () {
        $('section.models').hide();
    }, 1000);
});
