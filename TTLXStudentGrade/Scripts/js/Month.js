
function initPage() {
    getMonthSpecailty();
}

//获取当前用户的考试专业
function getMonthSpecailty() {
    $.ajax({
        url: '/Month/GetMonthSpecialty',
        type: 'get',
        dataType: 'json',
        success: function (ret) {
            if (ret.success) {
                $('#specialty').combobox('loadData', ret.data);
                commboxSelectFirst($('#specialty'), "SpecialtyType");
            }
            else {
                alert('获取专业失败！');
            }
        },
        error: function () {
            alert('获取专业错误！');
        }
    });
}


function selectSpecialty(record) {
    //加载计划
    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/Month/GetMonthPlan?specialtyType=' + record.SpecialtyType,
        success: function (ret) {
            if (ret.success == true) {
                $('#plan').combobox('loadData', ret.data);
                commboxSelectFirst($('#plan'), "PlanId");
            }
            else {
                alert('获取考试计划出错！')
            }
        },
        error: function () {
            alert("获取考试计划错误！");
        },
    });
}

function selectPlan(record) {

    $('#tt').tree({
        url: '/Month/GetSchoolTree?planId=' + record.PlanId,
        onDblClick: function (node) {
            $(this).tree(node.state === 'closed' ? 'expand' : 'collapse', node.target);
            node.state = node.state === 'closed' ? 'open' : 'closed';
        },
    });

    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/Month/GetJoinSchool?planId=' + record.PlanId,
        success: function (ret) {
            if (ret.success == true) {
                $('#school').combobox('loadData', ret.data);
                commboxSelectFirst($('#school'), "SchoolCode");
            }
            else {
                alert('获取学校失败！')
            }
        },
        error: function () {
            alert("获取学校错误！");
        },
    });

    //$.ajax({
    //    type: 'get',
    //    dataType: 'json',
    //    url: '/Month/GetProvinceScore?planId=' + record.PlanId,
    //    success: function (ret) {

    //        if (ret.rows.length > 0) {
    //            var specialty = ret.rows[0].SpecialtyName == '计算机类';
    //            var columns = getColumns(specialty);

    //            $('#dg-province').datagrid({
    //                title: '成绩列表',
    //                columns: [columns]
    //            });
    //            $('#dg-province').datagrid("loadData", ret.rows.slice(0, 10));
    //            initPagination(ret, 'dg-province', 10);
    //        }

    //    },
    //    error: function (ret) {
    //        alert("加载省份数据错误！");
    //    },
    //});

    loadProvinceScore(record.PlanId);

    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/Month/GetQuestionDetail?planId=' + record.PlanId,
        success: function (ret) {

            $('#dg-question').datagrid("loadData", ret.rows.slice(0, 10));
            initPagination(ret, 'dg-question', 10);

        },
        error: function () {
            alert("加载试题数据错误！");
        },
    });

    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/Month/GetProvinceBaseScore?planId=' + record.PlanId,
        success: function (ret) {

            $('#dg-provinceTotal').datagrid("loadData", ret.rows.slice(0, 10));
            initPagination(ret, 'dg-provinceTotal', 10);

        },
        error: function () {
            alert("加载全省数据错误！");
        },
    });

}

function loadProvinceScore(planId) {
    $('#dg-province').datagrid({
        title: '全省成绩列表',
        height: 'auto',
        nowrap: true,
        striped: true,
        border: true,
        collapsible: true,//是否可折叠的
        url: '/Month/GetProvinceScore?planId=' + planId,
        remoteSort: true,
        idField: 'Lexueid',
        pagination: true,//分页控件  
        pageSize: 20,
        rownumbers: true,//行号  
        showFooter: true,
        fitColumns: true,
    });
    initPaginationProvince();
}
//主界面分页栏
function initPaginationProvince() {
    //设置分页控件  
    var p = $('#dg-province').datagrid('getPager');
    $(p).pagination({
        pageSize: 20,//每页显示的记录条数，默认为20
        pageList: [20, 50, 100],//可以设置每页记录条数的列表  
        beforePageText: '第',//页数文本框前显示的汉字  
        afterPageText: '页    共 {pages} 页',
        displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
    });
}

function searchSchool() {
    $("#tt").tree("search", $("#querySchool").val());
}


function selectSchool(record) {
    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/Month/GetSchoolScore?planId=' + $('#plan').combobox('getValue') + '&schoolCode=' + record.SchoolCode,
        beforeSend: function () {
            load();
        },
        complete: function () {
            disLoad();
        },
        success: function (ret) {
            if (ret.success == true) {
                $('#dg-school').datagrid({
                    title: record.SchoolName + '考试概况',
                });
                $('#dg-school').datagrid("loadData", ret.data.school.rows.slice(0, 10));
                initPagination(ret.data.school, 'dg-school', 10);

                var columns = getColumns(ret.data.isComputerSpecialty);
                $('#dg-student').datagrid({
                    title: '学生信息列表',
                    frozenColumns: [
                        [
                            { "field": "SpecialtyName", "title": "专业", 'width': '100', 'halign': 'center', 'align': 'center' },
                            { "field": "UserName", "title": "姓名", 'width': '100', 'halign': 'center', 'align': 'center' }
                        ]
                    ],
                    columns: columns
                });
                $('#dg-student').datagrid("loadData", ret.data.student.rows.slice(0, 10));
                initPagination(ret.data.student, 'dg-student', 10);

                initStudentName(ret.data.student.rows);

                $("#tt-tabs").tabs('select', 0);

            }
            else {
                alert('加载学校数据失败！')
            }
        },
        error: function () {
            alert("加载学校数据错误！");
        },
    });



}

function initStudentName(rows) {
    var data = [];
    for (var i = 0; i < rows.length; i++) {
        var obj = {};
        obj.StudentId = rows[i].Lexueid;
        obj.StudentName = rows[i].UserName;
        data.push(obj);
    }
    $('#student').combobox('loadData', data);
    commboxSelectFirst($('#student'), "StudentId");
}

function selectStudent(record) {
    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/Month/GetStudentQueDetail?planId=' + $('#plan').combobox('getValue') + '&studentId=' + record.StudentId,
        //beforeSend: function () {
        //    load2();
        //},
        //complete: function () {
        //    disLoad2();
        //},
        success: function (ret) {

            $('#dg-studentQue').datagrid({ title: record.StudentName + '考试明细' });
            $('#dg-studentQue').datagrid("loadData", ret.rows.slice(0, 10));
            initPagination(ret, 'dg-studentQue', 10);

        },
        error: function () {
            alert("加载学生题目数据错误！");
        },
    });
}


//反馈详情分页
function initPagination_title(data, id, pageSize, title) {
    var page = $('#' + id).datagrid("getPager");
    page.pagination({
        title: title,
        total: data.total,
        pageSize: pageSize,
        loading: true,
        showRefresh: false,
        pageList: [pageSize, 50, 100],
        beforePageText: '第',
        afterPageText: '页    共 {pages} 页',
        displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
        onSelectPage: $.proxy(function (pageNo, pageSize) {
            var start = (pageNo - 1) * pageSize;
            var end = start + pageSize;

            $('#' + id).datagrid("loadData", data.rows.slice(start, end));
            page.pagination('refresh', {
                total: data.total,
                pageNumber: pageNo
            });
        }, this)
    });
}

function initPagination(data, id, pageSize) {
    var page = $('#' + id).datagrid("getPager");
    page.pagination({
        total: data.total,
        pageSize: pageSize,
        loading: true,
        showRefresh: false,
        pageList: [pageSize, 50, 100],
        beforePageText: '第',
        afterPageText: '页    共 {pages} 页',
        displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',

        onSelectPage: $.proxy(function (pageNo, pageSize) {
            var start = (pageNo - 1) * pageSize;
            var end = start + pageSize;

            $('#' + id).datagrid("loadData", data.rows.slice(start, end));
            page.pagination('refresh', {
                total: data.total,
                pageNumber: pageNo
            });
        }, this)
    });
}

function pagerFilter(data) {
    //这个函数为每次表格加载数据的过滤函数，data中包含原始数据，返回的是真正要显示出来的数据
    //获取Datagrid对象
    var dg = $(this);
    //获取选项对象，此处该对象用于保存分页时的相关信息
    var opts = dg.datagrid('options');
    //获取分页器
    var pager = dg.datagrid('getPager');
    //重写表格所使用分页器的onSelectPage方法，此方法在用户选择新的页面时触发，即改变pageNum或pageSize时触发
    //此处真正的翻页逻辑并不在onSelectPage方法里面，可以看到在该方法中仅仅是将新页面的参数(pageNum, pageSize)保存在opts对象中，
    //这两个参数在后面的代码中被使用
    pager.pagination({
        pageSize: 20,
        loading: true,
        showRefresh: false,
        pageList: [20, 50, 100],
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
    //真正的分页逻辑
    //第一次进入pagerFilter函数时，data中只有rows中保存了数据，并无originalRows属性，实现客户端分页时，需要在一开始将这些数据保存在
    //originalRows中，成为后续分页的基础数据，所以只要originalRows属性存在且不为空，分页时的基础数据便一直是originalRows中保存的数据
    if (!data.originalRows) {
        data.originalRows = (data.rows);
    }
    //取出选择新的页面时，新页面的参数，前面已经保存在opts对象中了
    var start = (opts.pageNumber - 1) * parseInt(opts.pageSize);
    var end = start + parseInt(opts.pageSize);
    //下面对初始数据进行筛选，并将结果放在data.rows中，页面每次加载时只会显示data.rows中的数据
    data.rows = (data.originalRows.slice(start, end));
    //返回data，完成分页
    return data;
}

function commboxSelectFirst(box, id) {
    var array = box.combobox("getData");
    for (var item in array[0]) {
        if (item == id) {
            box.combobox('select', array[0][item]);
        }
    }
}


function schoolCompare() {
    var nodes = $('#tt').tree('getChecked');

    var arr = [];
    for (var i = 0; i < nodes.length; i++) {
        var obj = {};
        obj.Level = nodes[i].attributes.Level;
        if (obj.Level != 2) {
            continue;
        }
        obj.id = nodes[i].id;
        obj.text = nodes[i].text;
        arr.push(obj);
    }

    if (arr.length == 0) {
        alert('请至少选择一个学校！');
        return;
    }

    $.ajax({
        type: 'post',
        dataType: 'json',
        url: '/Month/DataCompare',
        data: {
            "planId": $('#plan').combobox('getValue'),
            "args": arr
        },
        success: function (ret) {
            if (ret.success == true) {
                $('#dg-compare').datagrid("loadData", ret.data.rows.slice(0, ret.data.total));
                $("#tt-tabs").tabs('select', 2);
            }
            else {
                alert('比较失败！')
            }
        },
        error: function () {
            alert('比较错误！')
        },
    });

}




function OutputSchoolExcel() {

    var planId = $('#plan').combobox('getValue');
    var schoolCode = $('#school').combobox('getValue');
    if (planId == null || planId == '') {
        alert("请选择考试！");
        return;
    }
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
    form.attr("action", "/Month/OutputStudentGrade");
    form.attr("method", "post");
    //创建input对象并放入表单中
    var input1 = $("<input name='planId' value='" + planId + "' />");
    var input2 = $("<input name='schoolCode' value='" + schoolCode + "' />");
    form.append(input1);
    form.append(input2);
    //提交表单
    form.submit();
    form.remove();

}

function OutputProvinceExcel() {
    var planId = $('#plan').combobox('getValue');
    if (planId == null || planId == '') {
        alert("请选择考试！");
        return;
    }

    var body = $("body");
    //创建表单
    var form = $("<form></form>");
    //将表单放入body中
    body.append(form);
    //设置表单各项属性
    form.attr("action", "/Month/OutputProvinceStudentGrade");
    form.attr("method", "post");
    //创建input对象并放入表单中
    var input1 = $("<input name='planId' value='" + planId + "' />");
    form.append(input1);
    //提交表单
    form.submit();
    form.remove();
}


function schoolFormat(value) {
    return '<span style="color:red">' + value + '</span>';
}

function provinceFormat(value) {
    return '<span style="color:blue">' + value + '</span>';
}


//弹出加载层
function load() {
    $("<div class=\"datagrid-mask\"></div>").css({ display: "block", width: "100%", height: $(window).height() }).appendTo("body");
    $("<div class=\"datagrid-mask-msg\"></div>").html("数据加载中，请稍候。。。").appendTo("body").css({ display: "block", left: ($(document.body).outerWidth(true) - 190) / 2, top: ($(window).height() - 45) / 2 });
}
//取消加载层  
function disLoad() {
    $(".datagrid-mask").remove();
    $(".datagrid-mask-msg").remove();
}

//弹出加载层
function load2() {
    $("<div class=\"datagrid-mask1\"></div>").css({ display: "block", width: "100%", height: $(window).height() }).appendTo("body");
    $("<div class=\"datagrid-mask-msg1\"></div>").html("数据加载中，请稍候。。。").appendTo("body").css({ display: "block", left: ($(document.body).outerWidth(true) - 190) / 2, top: ($(window).height() - 45) / 2 });
}
//取消加载层  
function disLoad2() {
    $(".datagrid-mask1").remove();
    $(".datagrid-mask-msg1").remove();
}

function OutputCompareExcel() {

    var nodes = $('#tt').tree('getChecked');

    var arr = [];
    for (var i = 0; i < nodes.length; i++) {
        var obj = {};
        obj.Level = nodes[i].attributes.Level;
        if (obj.Level != 2) {
            continue;
        }
        obj.id = nodes[i].id;
        obj.text = nodes[i].text;
        arr.push(obj);
    }


    if (arr.length == 0) {
        alert('请至少选择一个学校！');
        return;
    }



    var planId = $('#plan').combobox('getValue');
    if (planId == null || planId == '') {
        alert("请选择考试！");
        return;
    }

    var body = $("body");
    //创建表单
    var form = $("<form></form>");
    //将表单放入body中
    body.append(form);
    //设置表单各项属性
    form.attr("action", "/Month/OutputDataCompare");
    form.attr("method", "post");
    //创建input对象并放入表单中
    var input1 = $("<input name='planId' value='" + planId + "' />");

    form.append(input1);
    var input2 = $("<input name='argsJson' value='" + JSON.stringify(arr) + "' />");
    form.append(input2);
    //提交表单
    form.submit();
    form.remove();
}

function getColumns(isComputer) {
    var columns;
    if (isComputer) {
        columns = [
            [
                { "field": "A", "title": "技能高考专业课", 'colspan': 10, 'halign': 'center', 'align': 'center' },
                { "field": "B", "title": "技能高考文化课", "colspan": 4, 'halign': 'center', 'align': 'center' },
                {
                    "field": "StudentTotalScore", "title": "专业+文化总分", "rowspan": 2, 'width': '150', 'halign': 'center', 'align': 'center',
                    //'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
            ],
            [
                {
                    "field": "AnswerTime", "title": "答题用时(分)", 'width': '100', 'halign': 'center', 'align': 'center'
                },
                {
                    "field": "SelectScore", "title": "应知题分数", 'width': '100', 'halign': 'center', 'align': 'center',
                    'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
                {
                    "field": "WinScore", "title": "Win得分", 'width': '80', 'halign': 'center', 'align': 'center',
                    'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
                {
                    "field": "NetScore", "title": "网络得分", 'width': '80', 'halign': 'center', 'align': 'center',
                    'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
                {
                    "field": "WordScore", "title": "Word得分", 'width': '80', 'halign': 'center', 'align': 'center',
                    'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
                {
                    "field": "ExcelScore", "title": "Excel得分", 'width': '80', 'halign': 'center', 'align': 'center',
                    'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
                {
                    "field": "PptScore", "title": "Ppt得分", 'width': '80', 'halign': 'center', 'align': 'center',
                    'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
                {
                    "field": "AccessScore", "title": "Access得分", 'width': '100', 'halign': 'center', 'align': 'center',
                    'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
                {
                    "field": "ProgramScore", "title": "C语言得分", 'width': '100', 'halign': 'center', 'align': 'center',
                    'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
                {
                    "field": "StudentScore", "title": "总得分", 'width': '80', 'halign': 'center', 'align': 'center',
                    'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
                {
                    "field": "ChineseScore", "title": "语文", 'width': '80', 'halign': 'center', 'align': 'center',
                    'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
                {
                    "field": "MathScore", "title": "数学", 'width': '80', 'halign': 'center', 'align': 'center',
                    'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
                {
                    "field": "EnglishScore", "title": "英语", 'width': '80', 'halign': 'center', 'align': 'center',
                    'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
                {
                    "field": "CultureStudentScore", "title": "总得分", 'width': '80', 'halign': 'center', 'align': 'center',
                    'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
            ]
        ];
    }
    else {
        columns = [
            [
                { "field": "", "title": "技能高考专业课", 'colspan': 2, 'width': '300', 'halign': 'center', 'align': 'center' },
                { "field": "", "title": "技能高考文化课", "colspan": 4, 'width': '600', 'halign': 'center', 'align': 'center' },
                {
                    "field": "StudentTotalScore", "title": "专业+文化总分", "rowspan": 2, 'width': '100', 'halign': 'center', 'align': 'center',
                    //'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
            ],
            [
                {
                    "field": "AnswerTime", "title": "答题用时(分)", 'width': '150', 'halign': 'center', 'align': 'center'
                },
                {
                    "field": "StudentScore", "title": "总分", 'width': '150', 'halign': 'center', 'align': 'center',
                    'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
                {
                    "field": "ChineseScore", "title": "语文", 'width': '150', 'halign': 'center', 'align': 'center',
                    'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
                {
                    "field": "MathScore", "title": "数学", 'width': '150', 'halign': 'center', 'align': 'center',
                    'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
                {
                    "field": "EnglishScore", "title": "英语", 'width': '150', 'halign': 'center', 'align': 'center',
                    'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
                {
                    "field": "CultureStudentScore", "title": "总分", 'width': '150', 'halign': 'center', 'align': 'center',
                    'sortable': true, 'sorter': function (a, b) { return a > b ? 1 : -1; }
                },
            ]
        ];
    }
    return columns;
}
