
var url = window.location.protocol + '//' + window.location.host;


function login() {
    $('#s-msg').html('');
    var userId = document.getElementById("userId").value;
    var pwd = document.getElementById("pwd").value;
    if (!checkValue(userId)) {
        $('#s-msg').html('请输入用户名');
        return;
    }
    if (!checkValue(pwd)) {
        $('#s-msg').html('请输入密码');
        return;
    }

    $.ajax({
        url: url + '/api/user/login',
        type: 'Post',
        cache: false,
        data:
        {
            "UserId": userId,
            "pwd": pwd
        },
        dataType: 'json',
        success: function (ret) {
            if (ret.success) {
                if (ret.data.userData) {
                    var strYear = JSON.stringify(ret.data.userData.years);
                    localStorage.setItem('years', strYear);
                    localStorage.setItem('userName', ret.data.userData.userName);
                    localStorage.setItem('userSpecialtyName', ret.data.userData.userSpecialtyName);
                }
                window.location.href = ret.data.url;
            }
            else {
                $('#s-msg').html(ret.message);
            }
        },
        error: function (ret) {
            alert('服务器错误！');
        }
    });
}

function checkValue(input) {
    if (input == '' || input == null)
        return false;
    return true;
}
//判断身份证号
function isIDCardAvailable(code) {
    var city = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江 ", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北 ", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏 ", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外 " };
    var tip = "";
    var pass = true;
    if (!code || !/^\d{6}(18|19|20)?\d{2}(0[1-9]|1[012])(0[1-9]|[12]\d|3[01])\d{3}(\d|X)$/i.test(code)) {
        tip = "身份证号格式错误";
        pass = false;
    }
    else if (!city[code.substr(0, 2)]) {
        tip = "地址编码错误";
        pass = false;
    }
    else {
        //18位身份证需要验证最后一位校验位
        if (code.length == 18) {
            code = code.split('');
            //∑(ai×Wi)(mod 11)
            //加权因子
            var factor = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2];
            //校验位
            var parity = [1, 0, 'X', 9, 8, 7, 6, 5, 4, 3, 2];
            var sum = 0;
            var ai = 0;
            var wi = 0;
            for (var i = 0; i < 17; i++) {
                ai = code[i];
                wi = factor[i];
                sum += ai * wi;
            }
            var last = parity[sum % 11];
            if (parity[sum % 11] != code[17]) {
                tip = "校验位错误";
                pass = false;
            }
        }
    }
    return pass;
}

//判断人名
function isNameAvailable(input) {
    var myreg = /^[\u4e00-\u9fa5]{2,4}$/;
    if (!myreg.test(input)) {
        return false;
    } else {
        return true;
    }
}

function queryPwd() {
    $('#s-msg').html('');
    var card = document.getElementById("card").value;
    var name = document.getElementById("name").value;
    if (!checkValue(card)) {
        $('#s-msg').html('请输入身份证号');
        return;
    }
    else {
        if (!isIDCardAvailable(card)) {
            $('#s-msg').html('请输入有效的身份证号');
            return;
        }
    }

    if (!checkValue(name)) {
        $('#s-msg').html('请输入姓名');
        return;
    }
    else {
        if (!isNameAvailable(name)) {
            $('#s-msg').html('请输入有效的姓名');
            return;
        }
    }

    $.ajax({
        url: url + '/api/user/pwd',
        type: 'Post',
        cache: false,
        data:
        {
            "IDCard": card,
            "UserName": name
        },
        dataType: 'json',
        success: function (ret) {
            if (ret.success) {
                window.location.href = url + "/Static/view/register/registerSuccess.html?message=" + ret.message + '&school=' + ret.data.SchoolName + '&specialty=' + ret.data.SpecialtyName
                    + '&name=' + ret.data.UserName + '&kaohao=' + ret.data.Kaohao + '&idcard=' + ret.data.IDCard + '&pwd=' + ret.data.Pwd
            }
            else {
                $('#s-msg').html(ret.message);
            }
        },
        error: function () {
            alert('服务器错误！');
        }
    });
}