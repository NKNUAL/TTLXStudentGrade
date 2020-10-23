function locaBaseData() {
    check_data = [
        { "CheckStatu": -1, "CheckStatuName": "全部", "selected": true },
        { "CheckStatu": 0, "CheckStatuName": "待审核" },
        { "CheckStatu": 1, "CheckStatuName": "通过" },
        { "CheckStatu": 2, "CheckStatuName": "拒绝" },
    ];
    $('#checkStatu').combobox('loadData', check_data);

    paper_data = [
        { "PaperStatu": -1, "PaperStatuName": "全部", "selected": true },
        { "PaperStatu": 0, "PaperStatuName": "下架" },
        { "PaperStatu": 1, "PaperStatuName": "上架" }
    ];
    $('#paperStatu').combobox('loadData', paper_data);

    loadSpecialty();
}

function selectSpecialty(record) {
    $.ajax({
        url: '/Share/GetUsers?specialtyId=' + record.SpecialtyId,
        type: 'get',
        dataType: 'json',
        success: function (ret) {
            if (ret.success) {
                $('#teachers').combobox('loadData', ret.data);
            }
            else {
                alert(ret.message);
            }
        },
        error: function () {
            alert('获取专业错误！');
        }
    });
}


//获取当前用户的考试专业
function loadSpecialty() {
    $.ajax({
        url: '/Share/GetSpecialty',
        type: 'get',
        dataType: 'json',
        success: function (ret) {

            $('#specialties').combobox('loadData', ret);
            commboxSelectFirst($('#specialties'), "SpecialtyId");
        },
        error: function () {
            alert('获取专业错误！');
        }
    });
}
function commboxSelectFirst(box, id) {
    var array = box.combobox("getData");
    for (var item in array[0]) {
        if (item == id) {
            box.combobox('select', array[0][item]);
        }
    }
}
function searchInfo() {

    $('#dg').datagrid({
        title: '共享试卷列表',
        height: 'auto',
        nowrap: false,
        striped: true,
        border: true,
        collapsible: true,//是否可折叠的
        url: '/Share/GetPapers',
        remoteSort: true,
        idField: 'PaperID',
        pagination: true,//分页控件  
        pageSize: 20,
        rownumbers: true,//行号  
        showFooter: true,
        fitColumns: true,
        queryParams: {
            SpecialtyId: $('#specialties').combobox('getValue'),
            UseToken: $('#teachers').combobox('getValue'),
            PaperStatu: $('#paperStatu').combobox('getValue'),
            CheckStatu: $('#checkStatu').combobox('getValue'),
        },
    });
    initPagination();

}
//主界面分页栏
function initPagination() {
    //设置分页控件  
    var p = $('#dg').datagrid('getPager');
    $(p).pagination({
        pageSize: 20,//每页显示的记录条数，默认为20
        pageList: [20, 50, 100],//可以设置每页记录条数的列表  
        beforePageText: '第',//页数文本框前显示的汉字  
        afterPageText: '页    共 {pages} 页',
        displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
    });
}

function formatCheckStatu(value, row) {
    if (value == 0) {
        return '<span style="color:blue">待审核</span>';
    } else if (value == 1) {
        return '<span style="color:green">通过</span>';
    } else {
        return '<span style="color:red">拒绝</span>';
    }
}

function formatPaperStatu(value, row) {
    if (value == 0) {
        return '<span style="color:red">下架</span>';
    } else if (value == 1) {
        return '<span style="color:green">上架</span>';
    }
}


function openAllow(paperId) {
    $('#dlg-allow').dialog('open');

    //$("#a_allow_id").click(function () {
    //    allowReview(paperId);
    //});
    $("#a_allow_id").attr("onclick", "allowReview('" + paperId + "')");
}

function openRefuse(paperId) {
    $('#dlg-refuse').dialog('open');
    //$("#a_refuse_id").attr(function () {
    //    refuseReview(paperId);
    //});
    $("#a_refuse_id").attr("onclick", "refuseReview('" + paperId + "')");
}

function openPutoff(paperId) {
    $('#dlg-putoff').dialog('open');

    //$("#a_allow_id").click(function () {
    //    allowReview(paperId);
    //});
    $("#a_putoff_id").attr("onclick", "putoff('" + paperId + "')");
}

function formatOperate(value, row) {

    if (row.CheckStatu == 0) {
        return '<a href="#"  onclick="getReviewQuestion(\'' + row.PaperID + '\')">审核试卷</a>'
            + '&nbsp;&nbsp;<a href="#"  onclick="openAllow(\'' + row.PaperID + '\')">通过</a>'
            + '&nbsp;&nbsp;<a href="#"  onclick="openRefuse(\'' + row.PaperID + '\')">拒绝</a>';
    }
    if (row.PaperStatu == 1)
        return '<a href="#"  onclick="openPutoff(\'' + row.PaperID + '\')">强制下架</a>';

    return '';
}


function putoff(paperId) {

    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/Share/PutOff?paperId=' + paperId,
        success: function (ret) {
            if (ret.success) {

                alert('下架成功！');
                $('#dlg-putoff').dialog('close');
                searchInfo();
            }
            else {
                alert(ret.message);
            }
        },
        error: function () {
            alert("下架出错，请联系管理员！");
        },
    });
}



function allowReview(paperId) {
    var commentLevel = $('#commentLevel').val();
    if (commentLevel == 0) {
        alert('请您评分！');
        return;
    }
    var commentDesc = $('#commentDesc').val();
    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/Share/CheckPass?commentLevel=' + commentLevel + '&commentDesc=' + commentDesc + '&paperId=' + paperId,
        success: function (ret) {
            if (ret.success) {

                alert('审核成功，试题已通过！');
                $('#dlg-allow').dialog('close');
                searchInfo();
            }
            else {
                alert(ret.message);
            }
        },
        error: function () {
            alert("审核出错，请联系管理员！");
        },
    });
}
function refuseReview(paperId) {
    var refuseReason = $('#refuseReason').val();
    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/Share/CheckRefuse?reason=' + refuseReason + '&paperId=' + paperId,
        success: function (ret) {
            if (ret.success) {

                alert('审核成功，试题已拒绝！');
                $('#dlg-refuse').dialog('close');
                searchInfo();
            }
            else {
                alert(ret.message);
            }
        },
        error: function () {
            alert("审核出错，请联系管理员！");
        },
    });
}


function getReviewQuestion(paperId) {
    var title = '共享试卷题目';
    if (!$('#tt-tabs').tabs('exists', title)) {
        $('#tt-tabs').tabs('add', {
            title: title,
            content: createTabsTable(),
            closable: true,
        });
    } else {
        $('#tt-tabs').tabs('select', title);
    }

    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/Share/GetQuestions?paperId=' + paperId,
        success: function (ret) {
            if (ret.success) {

                $('#dg-Questions').datagrid({
                    pagination: true,//分页控件  
                    idField: 'QueNo',
                    singleSelect: true,
                    striped: true,
                    fitColumns: true,
                    nowrap: false,
                    rownumbers: true,
                });

                $('#dg-Questions').datagrid("loadData", ret.data.slice(0, 10));

                initPaginationClient(ret.data, 'dg-Questions', 10);

            }
            else {
                alert(ret.message);
            }
        },
        error: function () {
            alert("获取题目出错，请联系管理员！");
        },
    });

}

function initPaginationClient(data, id, pageSize) {
    var page = $('#' + id).datagrid("getPager");
    page.pagination({
        total: data.length,
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

            $('#' + id).datagrid("loadData", data.slice(start, end));
            page.pagination('refresh', {
                total: data.length,
                pageNumber: pageNo
            });
        }, this)
    });
}


function createTabsTable() {
    var content = '<table id="dg-Questions" cellspacing="0" class="easyui-datagrid" cellpadding="0" style="width: 100%;" >';
    content += '<thead>';
    content += '<tr>';
    content += '<th data-options="field:\'QueNo\',width:\'3%\',halign:\'center\',align:\'center\',hidden:true">No</th>';
    content += '<th data-options="field:\'QueType\',width:\'3%\',halign:\'center\',align:\'center\'">题型</th>';
    content += '<th data-options="field:\'QueContent\',width:\'13%\',halign:\'center\',align:\'center\'">题干</th>';
    content += '<th data-options="field:\'ContentImg\',width:\'6%\',halign:\'center\',align:\'center\',formatter:formatImg">题干图片</th>';
    content += '<th data-options="field:\'Option0\',width:\'8%\',halign:\'center\',align:\'center\'">选项A</th>';
    content += '<th data-options="field:\'Option0Img\',width:\'6%\',halign:\'center\',align:\'center\',formatter:formatImg">A图片</th>';
    content += '<th data-options="field:\'Option1\',width:\'8%\',halign:\'center\',align:\'center\'">选项B</th>';
    content += '<th data-options="field:\'Option1Img\',width:\'6%\',halign:\'center\',align:\'center\',formatter:formatImg">B图片</th>';
    content += '<th data-options="field:\'Option2\',width:\'8%\',halign:\'center\',align:\'center\'">选项C</th>';
    content += '<th data-options="field:\'Option2Img\',width:\'6%\',halign:\'center\',align:\'center\',formatter:formatImg">C图片</th>';
    content += '<th data-options="field:\'Option3\',width:\'8%\',halign:\'center\',align:\'center\'">选项D</th>';
    content += '<th data-options="field:\'Option3Img\',width:\'6%\',halign:\'center\',align:\'center\',formatter:formatImg">D图片</th>';
    content += '<th data-options="field:\'StandardAnwser\',width:\'5%\',halign:\'center\',align:\'center\'">答案</th>';
    content += '<th data-options="field:\'ResolutionTips\',width:\'12%\',halign:\'center\',align:\'center\'">解析</th>';
    content += '<th data-options="field:\'Similarity\',width:\'6%\',halign:\'center\',align:\'center\',formatter:formatSimilarity">相似度</th>';
    content += '</tr>';
    content += '</thead>';
    content += '</table>';
    return content;
}

function createTabsTable_similar() {
    var content = '<table id="dg-Similar" cellspacing="0" class="easyui-datagrid" cellpadding="0" style="width: 100%;" >';
    content += '<thead>';
    content += '<tr>';
    content += '<th data-options="field:\'QueContent\',width:\'30%\',halign:\'center\',align:\'center\'">题干</th>';
    content += '<th data-options="field:\'Option0\',width:\'15%\',halign:\'center\',align:\'center\'">选项A</th>';
    content += '<th data-options="field:\'Option1\',width:\'15%\',halign:\'center\',align:\'center\'">选项B</th>';
    content += '<th data-options="field:\'Option2\',width:\'15%\',halign:\'center\',align:\'center\'">选项C</th>';
    content += '<th data-options="field:\'Option3\',width:\'15%\',halign:\'center\',align:\'center\'">选项D</th>';
    content += '<th data-options="field:\'Similarity\',width:\'10%\',halign:\'center\',align:\'center\',formatter:formatSimilarity2">相似度</th>';
    content += '</tr>';
    content += '</thead>';
    content += '</table>';
    return content;
}


function formatImg(value) {
    return '<img src="' + value + '"></img>';
}

function formatSimilarity(value, row) {
    if (value >= 0.6) {
        var str = Number(value * 100).toFixed(2);
        str += "%";
        return '<a style="color:red" onclick="getSimilarQuestions(\'' + row.QueNo + '\')">' + str + '</a>';
    } else {
        var str = Number(value * 100).toFixed(2);
        str += "%";
        return '<a style="color:blue">' + str + '</a>';
    }
}
function formatSimilarity2(value, row) {
    if (value >= 0.6) {
        var str = Number(value * 100).toFixed(2);
        str += "%";
        return '<span style="color:red">' + str + '</span>';
    } else {
        var str = Number(value * 100).toFixed(2);
        str += "%";
        return '<span style="color:blue">' + str + '</span>';
    }
}

function getSimilarQuestions(queNo) {
    var title = '相似题目';
    if (!$('#tt-tabs').tabs('exists', title)) {
        $('#tt-tabs').tabs('add', {
            title: title,
            content: createTabsTable_similar(),
            closable: true,
        });
    } else {
        $('#tt-tabs').tabs('select', title);
    }

    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/Share/GetSimilarQuestions?queNo=' + queNo,
        success: function (ret) {
            if (ret.success) {

                $('#dg-Similar').datagrid({
                    pagination: true,//分页控件  
                    idField: 'QueContent',
                    singleSelect: true,
                    striped: true,
                    fitColumns: true,
                    nowrap: false,
                    rownumbers: true,
                });

                $('#dg-Similar').datagrid("loadData", ret.data.slice(0, 10));

                initPaginationClient(ret.data, 'dg-Similar', 10);

            }
            else {
                alert(ret.message);
            }
        },
        error: function () {
            alert("获取题目出错，请联系管理员！");
        },
    });
}

function closeAllow() {
    $('#commentDesc').textbox('setValue', '');
    var s = document.getElementById("pingStar");
    var n = s.getElementsByTagName("li");
    for (var i = 0; i < n.length; i++) {
        n[i].className = '';
    }
    $('#commentLevel').val(0);
    document.getElementById('dir').innerHTML = '';
}

function closeRefuse() {
    $('#refuseReason').textbox('setValue', '');


}