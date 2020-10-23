function GetPapers() {
    var specialtyId = $('#curr_specialty_id').val();
    $.ajax({
        type: 'get',
        url: '/Grade/GetPapers?specialtyId=' + specialtyId,
        success: function (rst) {
            if (rst.success) {
                $('#paper').combobox('loadData', rst.data);
                var array = $('#paper').combobox("getData");
                for (var item in array[0]) {
                    if (item == "paperId") {
                        $('#paper').combobox('select', array[0][item]);
                    }
                }
                GetClasses();
            }
            else {
                alert('获取试卷失败！')
            }
        },
        error: function () {
            alert("服务器错误！");
        },
    });
}

function GetClasses() {
    var specialtyId = $('#curr_specialty_id').val();
    $.ajax({
        type: 'get',
        url: '/Grade/GetClassBySpecialty?specialtyId=' + specialtyId,
        success: function (rst) {
            if (rst.success) {
                $('#class').combobox('loadData', rst.data);
                var array = $('#class').combobox("getData");
                for (var item in array[0]) {
                    if (item == "classCode") {
                        $('#class').combobox('select', array[0][item]);
                    }
                }
                GetStudentGrade();
            }
            else {
                alert('获取班级失败！')
            }
        },
        error: function () {
            alert("服务器错误！");
        },
    });
}


function GetStudentGrade() {
    $.ajax({
        url: '/Grade/GetGrade',
        type: 'Post',
        cache: false,
        data:
        {
            "SpecialtyId": $('#curr_specialty_id').val(),
            "ClassCode": $('#class').combobox('getValue'),
            "PaperId": $('#paper').combobox('getValue')
        },
        dataType: 'json',
        success: function (ret) {
            if (ret.success) {
                $('#dg').datagrid("loadData", ret.data.rows.slice(0, 30));
                initPagination($('#dg'), ret.data, 30);
            }
            else {
                alert('获取成绩失败！');
            }
        },
        error: function () {
            alert('服务器错误！');
        }
    });


}

//窗口的table分页栏
function initPagination(dg, data, pageSize) {
    var page = dg.datagrid("getPager");
    page.pagination({
        total: data.total,
        pageSize: pageSize,
        loading: true,
        showRefresh: false,
        pageList: [10, 30, 50, 100],
        beforePageText: '第',
        afterPageText: '页    共 {pages} 页',
        displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
        onSelectPage: $.proxy(function (pageNo, pageSize) {
            var start = (pageNo - 1) * pageSize;
            var end = start + pageSize;

            dg.datagrid("loadData", data.rows.slice(start, end));
            page.pagination('refresh', {
                total: data.total,
                pageNumber: pageNo
            });
        }, this)
    });
}

function initPage() {
    GetPapers();
    //GetClasses();
    //GetStudentGrade();
}

function searchInfo() {
    GetStudentGrade();
}

function OutputGrade() {

    var body = $("body");
    //创建表单
    var form = $("<form></form>");
    //将表单放入body中
    body.append(form);
    //设置表单各项属性
    form.attr("action", "/Grade/SaveExcel");
    form.attr("method", "post");
    //创建input对象并放入表单中
    var input1 = $("<input name='SpecialtyId' value='" + $('#curr_specialty_id').val() + "' />");
    var input2 = $("<input name='ClassCode' value='" + $('#class').combobox('getValue') + "' />");
    var input3 = $("<input name='PaperId' value='" + $('#paper').combobox('getValue') + "' />");
    form.append(input1);
    form.append(input2);
    form.append(input3);
    //提交表单
    form.submit();
    form.remove();

}

//单元格单击事件
function clickCell(index, field) {
    if (index >= 0) {
        var rows = $('#dg').datagrid('getRows');
        var row = rows[index];
        if (field == "WorkCount") {
            showGradeDetail(row)
        }
    }
}

function showGradeDetail(row) {
    var paperId = $('#paper').combobox('getValue');
    var lexueid = row["Lexueid"];
    $.ajax({
        type: 'get',
        url: '/Grade/GetStudentGradeDetails?paperId=' + paperId + '&lexueid=' + lexueid,
        success: function (ret) {
            if (ret.success) {
                $('#dlg-detail').dialog('open');
                $('#dg-detail').datagrid("loadData", ret.data.rows.slice(0, 10));
                initPagination($('#dg-detail'), ret.data, 10);
            }
            else {
                alert('查看失败！')
            }
        },
        error: function () {
            alert("服务器错误！");
        },
    });
}

function closeDlg() {
    $('#dlg-detail').dialog('close');
    $("#dg-detail").datagrid("loadData", { total: 0, rows: [] });
}

function formatA(value) {
    return '<span style=color:blue; ><b><u>' + value + '</u></b></span>';
}