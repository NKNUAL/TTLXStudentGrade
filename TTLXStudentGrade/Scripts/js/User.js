var com_schools = [];
var com_specialty = [];


function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].trim();
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}


function loadStudent() {
    //datagrid初始化  
    $('#dg-student2').datagrid({
        title: '学生信息列表',
        height: 'auto',
        nowrap: true,
        striped: true,
        border: true,
        collapsible: false,//是否可折叠的
        url: '/Month/GetStudentList',
        remoteSort: true,
        idField: 'IDCard',
        pagination: true,//分页控件  
        pageSize: 20,
        rownumbers: true,//行号  
        showFooter: true,
        fitColumns: true,
        queryParams: {
            SchoolNo: $('#com-school').combobox('getValue'),
            SpecialtyId: $('#com-specialty').combobox('getValue'),
            StudentName: $('#stu_name').textbox('getValue')
        },
    });
    initPagination();
}
function initPagination() {
    //设置分页控件  
    var p = $('#dg-student2').datagrid('getPager');
    $(p).pagination({
        pageSize: 20,//每页显示的记录条数，默认为20
        pageList: [20, 50, 100],//可以设置每页记录条数的列表  
        beforePageText: '第',//页数文本框前显示的汉字  
        afterPageText: '页    共 {pages} 页',
        displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
    });
}

function OutputStudentExcel() {

    var schoolCode = $('#com-school').combobox('getValue');
    if (schoolCode == null || schoolCode == '') {
        alert("请选择学校！");
        return;
    }


    var body = $("body");
    //创建表单
    var form = $("<form></form>");
    //将表单放入body中
    body.append(form);
    //设置表单各项属性
    form.attr("action", "/Month/DownloadStudent");
    form.attr("method", "post");
    //创建input对象并放入表单中
    var input1 = $("<input name='SpecialtyId' value='" + $('#com-specialty').combobox('getValue') + "' />");
    var input2 = $("<input name='SchoolNo' value='" + schoolCode + "' />");

    form.append(input1);
    form.append(input2);
    //提交表单
    form.submit();
    form.remove();
}

function commboxSelectFirst(box, id) {
    var array = box.combobox("getData");
    for (var item in array[0]) {
        if (item == id) {
            box.combobox('select', array[0][item]);
        }
    }
}

function initPage() {
    if (com_schools == null || com_schools.length == 0) {
        $.ajax({
            type: 'get',
            url: '/api/data/schools',
            headers: {
                'x-ttlx-token': getCookie('x-ttlx-token')
            },
            success: function (ret) {
                if (ret.success == true) {
                    com_schools = ret.data;
                    $('#com-school').combobox('loadData', com_schools);
                    commboxSelectFirst($('#com-school'), "SchoolNo");
                }
                else {
                    alert('学校获取失败！')
                }
            },
            error: function () {
                alert('学校获取错误！')
            },
        });
    }

    if (com_specialty == null || com_specialty.length == 0) {
        $.ajax({
            type: 'get',
            url: '/api/data/specialty',
            headers: {
                'x-ttlx-token': getCookie('x-ttlx-token')
            },
            success: function (ret) {
                if (ret.success == true) {
                    com_specialty = ret.data;
                    $('#com-specialty').combobox('loadData', com_specialty);
                    commboxSelectFirst($('#com-specialty'), "SpecialtyId");
                }
                else {
                    alert('专业获取失败！')
                }
            },
            error: function () {
                alert('专业获取错误！')
            },
        });
    }
}