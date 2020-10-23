var areas = null;
var schools = null;
var specialties = null;

var url = window.location.protocol + '//' + window.location.host;

!(function (window, document) {
    var size = 4;//设置验证码长度
    function GVerify(options) { //创建一个图形验证码对象，接收options对象为参数
        this.options = { //默认options参数值
            id: "", //容器Id
            canvasId: "verifyCanvas", //canvas的ID
            width: "80", //默认canvas宽度
            height: "30", //默认canvas高度
            type: "blend", //图形验证码默认类型blend:数字字母混合类型、number:纯数字、letter:纯字母
            code: "",
        }
        if (Object.prototype.toString.call(options) == "[object Object]") {//判断传入参数类型
            for (var i in options) { //根据传入的参数，修改默认参数值
                this.options[i] = options[i];
            }
        } else {
            this.options.id = options;
        }

        this.options.numArr = "0,1,2,3,4,5,6,7,8,9".split(",");
        this.options.letterArr = getAllLetter();

        this._init();
        this.refresh();
    }

    GVerify.prototype = {
        /**版本号**/
        version: '1.0.0',

        /**初始化方法**/
        _init: function () {
            var con = document.getElementById(this.options.id);
            var canvas = document.createElement("canvas");
            this.options.width = con.offsetWidth > 0 ? con.offsetWidth : "100";
            this.options.height = con.offsetHeight > 0 ? con.offsetHeight : "30";
            canvas.id = this.options.canvasId;
            canvas.width = this.options.width;
            canvas.height = this.options.height;
            canvas.style.cursor = "pointer";
            canvas.innerHTML = "您的浏览器版本不支持canvas";
            con.appendChild(canvas);
            var parent = this;
            canvas.onclick = function () {
                parent.refresh();
            }
        },

        /**生成验证码**/
        refresh: function () {
            this.options.code = "";
            var canvas = document.getElementById(this.options.canvasId);
            if (canvas.getContext) {
                var ctx = canvas.getContext('2d');
            } else {
                return;
            }

            ctx.textBaseline = "middle";

            ctx.fillStyle = randomColor(180, 240);
            ctx.fillRect(0, 0, this.options.width, this.options.height);

            if (this.options.type == "blend") { //判断验证码类型
                var txtArr = this.options.numArr.concat(this.options.letterArr);
            } else if (this.options.type == "number") {
                var txtArr = this.options.numArr;
            } else {
                var txtArr = this.options.letterArr;
            }

            for (var i = 1; i <= size; i++) {
                var txt = txtArr[randomNum(0, txtArr.length)];
                this.options.code += txt;
                ctx.font = randomNum(this.options.height / 2, this.options.height) + 'px SimHei'; //随机生成字体大小
                ctx.fillStyle = randomColor(50, 160); //随机生成字体颜色        
                ctx.shadowOffsetX = randomNum(-3, 3);
                ctx.shadowOffsetY = randomNum(-3, 3);
                ctx.shadowBlur = randomNum(-3, 3);
                ctx.shadowColor = "rgba(0, 0, 0, 0.3)";
                var x = this.options.width / (size + 1) * i;
                var y = this.options.height / 2;
                var deg = randomNum(-30, 30);
                /**设置旋转角度和坐标原点**/
                ctx.translate(x, y);
                ctx.rotate(deg * Math.PI / 180);
                ctx.fillText(txt, 0, 0);
                /**恢复旋转角度和坐标原点**/
                ctx.rotate(-deg * Math.PI / 180);
                ctx.translate(-x, -y);
            }
            /**绘制干扰线**/
            //for (var i = 0; i < 4; i++) {
            //    ctx.strokeStyle = randomColor(40, 180);
            //    ctx.beginPath();
            //    ctx.moveTo(randomNum(0, this.options.width), randomNum(0, this.options.height));
            //    ctx.lineTo(randomNum(0, this.options.width), randomNum(0, this.options.height));
            //    ctx.stroke();
            //}
            /**绘制干扰点**/
            //for (var i = 0; i < this.options.width / 4; i++) {
            //    ctx.fillStyle = randomColor(0, 255);
            //    ctx.beginPath();
            //    ctx.arc(randomNum(0, this.options.width), randomNum(0, this.options.height), 1, 0, 2 * Math.PI);
            //    ctx.fill();
            //}
        },

        /**验证验证码**/
        validate: function (code) {
            var code = code.toLowerCase();
            var v_code = this.options.code.toLowerCase();
            if (code == v_code) {
                return true;
            } else {
                this.refresh();
                return false;
            }
        }
    }
    /**生成字母数组**/
    function getAllLetter() {
        var letterStr = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        return letterStr.split(",");
    }
    /**生成一个随机数**/
    function randomNum(min, max) {
        return Math.floor(Math.random() * (max - min) + min);
    }
    /**生成一个随机色**/
    function randomColor(min, max) {
        var r = randomNum(min, max);
        var g = randomNum(min, max);
        var b = randomNum(min, max);
        return "rgb(" + r + "," + g + "," + b + ")";
    }
    window.GVerify = GVerify;
})(window, document);

var verifyCode = new GVerify("v_container");

//判断手机号
function isPhoneAvailable(input) {
    var myreg = /^[1][3,4,5,7,8][0-9]{9}$/;
    if (!myreg.test(input)) {
        return false;
    } else {
        return true;
    }
}

//判断身份证号
function isIDCardAvailable(code) {
    var city = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江 ", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北 ", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏 ", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外 " };
    var tip = "";
    var pass = true;
    ///^\d{6}(18|19|20)?\d{2}(0[1-9]|1[12])(0[1-9]|[12]\d|3[01])\d{3}(\d|X)$/
    var reg = /^\d{6}(18|19|20)?\d{2}(0[1-9]|1[012])(0[1-9]|[12]\d|3[01])\d{3}(\d|X)$/;
    if (!code || !reg.test(code)) {
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

//注册
function register() {

    $("#s-msg").html('');


    var areaValue = $('#Area').combobox('getValue');
    if (!checkValue(areaValue)) {
        $("#s-msg").html('请选择区域！');
        return;
    }

    var schoolValue = $('#School').combobox('getValue');
    if (!checkValue(schoolValue)) {
        $("#s-msg").html('请选择学校！');
        return;
    }

    var specialtyValue = $('#Specialty').combobox('getValue');
    if (!checkValue(specialtyValue)) {
        $("#s-msg").html('请选择专业！');
        return;
    }

    var nameValue = $('#Name').textbox('getValue');
    if (!checkValue(nameValue)) {
        $("#s-msg").html('请输入您的姓名！')
        return;
    }
    else {
        if (!isNameAvailable(nameValue)) {
            $("#s-msg").html('请输入您的真实姓名！');
            return;
        }
    }

    var cardValue = $('#IDCard').textbox('getValue');
    if (!checkValue(cardValue)) {
        $("#s-msg").html('请输入身份证号！')
        return;
    }
    else {
        if (!isIDCardAvailable(cardValue)) {
            $("#s-msg").html('请输入真实的身份证号！');
            return
        }
    }

    // var phoneValue = $('#PhoneNumber').textbox('getValue');
    // if (!checkValue(phoneValue)) {
    //     $("#s-msg").html('请输入手机号方便我们与您联系！')
    //     return;
    // }
    // else {
    //     if (!isPhoneAvailable(phoneValue)) {
    //         $("#s-msg").html('请输入有效的手机号！')
    //         return;
    //     }
    // }

    var pwdValue = $('#Pwd').textbox('getValue');
    var pwd2Value = $('#Pwd2').textbox('getValue');
    if (!checkValue(pwdValue)) {
        $("#s-msg").html('请输入密码！')
        return;
    }
    else {
        if (pwdValue.length < 6) {
            $("#s-msg").html('密码长度至少为6位！')
            return;
        }
        else {
            if (!checkValue(pwd2Value)) {
                $("#s-msg").html('请再次输入您的密码！');
                return;
            }
            else {
                if (pwdValue != pwd2Value) {
                    $("#s-msg").html('两次输入的密码不一致！');
                    return;
                }
            }
        }
    }

    var codeValue = $('#code_input').textbox('getValue');
    if (!checkValue(codeValue)) {
        $("#s-msg").html('请输入下方验证码！')
        return;
    }
    else {
        var res = verifyCode.validate(codeValue);
        if (!res) {
            $("#s-msg").html('验证码错误！');
            return;
        }
    }


    $.ajax({
        url: url + '/api/user/reg',
        type: 'Post',
        cache: false,
        data:
        {
            "SchoolNo": schoolValue,
            "SpecialtyId": specialtyValue,
            "UserName": nameValue,
            "IDCard": cardValue,
            // "PhoneNumber": phoneValue,
            "Password": pwdValue,
            // "QQ": $('#QQ').textbox('getValue')
        },
        dataType: 'json',
        success: function (ret) {
            if (ret.success) {
                window.location.href = url + "/Static/view/register/registerSuccess.html?message=" + ret.message + '&school=' + ret.data.SchoolName + '&specialty=' + ret.data.SpecialtyName
                    + '&name=' + ret.data.UserName + '&kaohao=' + ret.data.Kaohao + '&idcard=' + ret.data.IDCard + '&pwd=' + ret.data.Pwd
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

//判断值是否为空
function checkValue(input) {
    if (input == '' || input == null)
        return false;
    return true;
}

function loadArea() {

    if (areas == null || schools == null || specialties == null) {
        $.ajax({
            url: url + '/api/data/basereg',
            type: 'get',
            async: false,
            success: function (ret) {
                if (ret.success) {
                    areas = ret.data.areas;
                    schools = ret.data.schools;
                    specialties = ret.data.specialties;
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



    $('#Area').combobox('loadData', areas);
    $('#Specialty').combobox('loadData', specialties);
}

function selectArea(record) {
    if (record.AreaNo != '-1') {
        $('#School').combobox('clear');
        for (var i = 0; i < schools.length; i++) {
            if (schools[i].AreaNo == record.AreaNo) {
                $('#School').combobox('loadData', schools[i].Schools);
            }
        }
    }
}


function bindMessage() {
    alert(window.token);
    $("#s-msg").html('');
    var kaohaoValue = $('#KaoHao').textbox('getValue');
    var nameValue = $('#UserName').textbox('getValue');
    var pwdValue = $('#Pwd').textbox('getValue');
    var cardValue = $('#IDCard').textbox('getValue');
    // var phoneValue = $('#PhoneNumber').textbox('getValue');
    // var qqValue = $('#QQ').textbox('getValue');
    var codeValue = $('#code_input').textbox('getValue');

    if (!checkValue(kaohaoValue)) {
        $("#s-msg").html('请输入考号');
        return;
    }

    if (!checkValue(nameValue)) {
        $("#s-msg").html('请输入姓名');
        return;
    }

    if (!checkValue(pwdValue)) {
        $("#s-msg").html('请输入密码');
        return;
    }

    if (!checkValue(cardValue)) {
        $("#s-msg").html('请输入真实身份证号');
        return;
    }
    else {
        if (!isIDCardAvailable(cardValue)) {
            $("#s-msg").html('请输入有效的真实身份证号');
            return;
        }
    }

    // if (!checkValue(phoneValue)) {
    //     $("#s-msg").html('请输入手机号');
    //     return;
    // }
    // else {
    //     if (!isPhoneAvailable(phoneValue)) {
    //         $("#s-msg").html('请输入有效的手机号');
    //         return;
    //     }
    // }


    if (!checkValue(codeValue)) {
        $("#s-msg").html('请输入下方验证码！')
        return;
    }
    else {
        var res = verifyCode.validate(codeValue);
        if (!res) {
            $("#s-msg").html('验证码错误！');
            return;
        }
    }

    $.ajax({
        url: url + '/api/user/bind',
        type: 'Post',
        cache: false,
        data:
        {
            "Kaohao": kaohaoValue,
            "UserName": nameValue,
            "Pwd": pwdValue,
            "IDCard": cardValue,
            // "PhoneNumber": phoneValue,
            // "QQ": qqValue
        },
        dataType: 'json',
        success: function (ret) {
            if (ret.success) {
                alert('绑定成功');
                window.location.href = url + "/Static/view/login/login.html";
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

