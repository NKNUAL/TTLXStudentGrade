$("#select-year").change(function () {
    alert($("#select-year").val())
})


var url = window.location.protocol + '//' + window.location.host;

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].trim();
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}


function loadStudentData(year) {
    if (year == null || year == '') {
        alert('您还未在天天乐学平台参加任何考试！')
        return;
    }
    $.ajax({
        url: url + '/api/data/' + year,
        type: 'get',
        headers: {
            'x-ttlx-token': getCookie('x-ttlx-token')
        },
        success: function (ret) {
            if (ret.success) {
                ret.data.forEach(element => {
                    divBodyAppend(element);
                });

            }
            else {
                alert(ret.message);
            }
        },
        error: function () {
            alert('服务器错误！');
        }
    });
}

function divBodyAppend(data) {
    var app = '<div class="card mb-4 shadow-sm">';
    app += '<div class="card-header">';
    app += '<h4 class="my-0 font-weight-normal">' + data.ExamDate + '</h4>';
    app += '</div>';
    app += '<div class="card-body">';
    app += '<h1 class="card-title pricing-card-title"><small>' + data.PlanName + '</small></h1>';
    app += '<ul class="list-unstyled mt-3 mb-4">';
    app += '<li>你的得分：' + data.StudentScore + '</li>';
    app += '</ul>';
    app += '<button type="button" class="btn btn-lg btn-block btn-outline-primary" onclick="btnClick(' + data.PlanID + ',\'' + data.PlanName + '\')">查看详细</button>';
    app += '</div>';
    app += '</div>';
    $('#div-app').append(app);
}


function tBodyAppend(data, count) {
    var app = '<tr>';
    app += '<th scope="row">' + count + '</th>';
    app += '<td>' + data.QueName + '</td>';
    app += '<td>' + data.StudentAnswer + '</td>';
    app += '<td>' + data.CorrectAnswer + '</td>';
    app += '</tr>';
    $('#t-app').append(app);
}


function btnClick(PlanID, PlanName) {
    $.ajax({
        url: url + '/api/data/detail/' + PlanID,
        type: 'get',
        headers: {
            'x-ttlx-token': getCookie('x-ttlx-token')
        },
        success: function (ret) {
            if (ret.success) {
                localStorage.setItem('userPaperDetail', JSON.stringify(ret.data));
                window.location.href = url + '/Static/view/home/studentDetail.html?PlanName=' + PlanName + '&UserName=' + $('#s-userName').text();
            }
            else {
                alert(ret.message);
            }
        },
        error: function () {
            alert('服务器错误！');
        }
    });
}





function GetQueryString(name) {
    if (window.location.search.length >= 1) {
        // 取得url中?后面的字符
        var query = window.location.search.substring(1);

        // 把参数按&拆分成数组
        var param_arr = decodeURIComponent(query).split("&");
        for (var i = 0; i < param_arr.length; i++) {
            var pair = param_arr[i].split("=");
            if (pair[0] == name) return pair[1]
        }
    }

    return '';
}



function initStudentPage() {
    var years = localStorage.getItem('years');
    var userName = localStorage.getItem('userName');
    var userSpecialtyName = localStorage.getItem('userSpecialtyName');
    if (years == '' || years == null || userName == '' || userName == null || userSpecialtyName == '' || userSpecialtyName == null) {
        alert('请您先登录！');
        window.location.href = url + '/Static/view/login/login.html';
        return;
    }
    $('#s-userName').html(userName);
    $('#p-specialty').html(userSpecialtyName);
    var list_years = JSON.parse(years);
    var option = '';
    list_years.forEach(element => {
        option += '<option value="' + element + '">' + element + '</option>';
    });
    $('#select-year').append(option);
    loadStudentData($("#select-year").val());
}


function initStudentDeatilPage() {
    var userPaperDetail = localStorage.getItem('userPaperDetail');
    var userName = GetQueryString('UserName');
    if (userPaperDetail == '' || userPaperDetail == null || userName == '' || userName == null) {
        alert('请您先登录！');
        window.location.href = url + '/Static/view/login/login.html';
        return;
    }

    var planName = GetQueryString('PlanName');
    if (planName == '' || planName == null) {
        alert('请您先选择考试场次！');
        window.location.href = url + '/Static/view/home/student.html';
        return;
    }

    $('#h-paperName').html(planName);
    $('#s-userName').html(userName);
    var detail = JSON.parse(userPaperDetail);
    $('#s-yourScore').html(detail.StudentSpecialtyScore);
    $('#s-avgScore').html(detail.ProvinceAvgScore);
    $('#s-maxScore').html(detail.ProvinceMaxScore);

    $('#s-totalScore').html(detail.TotalScore);
    $('#s-englishScore').html(detail.EnglishScore);
    $('#s-mathScore').html(detail.MathScore);
    $('#s-chineseScore').html(detail.ChineseScore);

    var count = 1;
    detail.QueDetail.forEach(element => {
        tBodyAppend(element, count++);
    });
}


function loginout() {
    $.ajax({
        url: url + '/api/user/out/',
        type: 'get',
        success: function (ret) {
            if (ret.success) {

                window.location.href = ret.data;
            }
            else {
                alert(ret.message);
            }
        },
        error: function () {
            alert('服务器错误！');
        }
    });
}