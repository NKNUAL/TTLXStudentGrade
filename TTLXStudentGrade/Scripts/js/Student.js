function SaveEdit() {
    $('#edit-form').form('submit', {
        url: '/User/EditStudent',
        success: function (ret) {
            if (ret) {
                $('#dlg-edit').dialog('close');
                initPage();
            }
            else {
                alert('不存在此学生！');
            }
        },
        onLoadError: function (ret) {
            alert('添加失败！');
        }
    });
}


function EditStudent(index) {

    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/User/GetSpecialtyList',
        success: function (rst) {
            if (rst.success) {
                $('#FK_Specialty').combobox('loadData', rst.data);
            }
            else {
                alert('获取专业出错！')
            }
        },
        error: function () {
            alert("获取专业出错！");
        },
    });
    $('#dlg-addUser').dialog('open');


    $('#KaoHao').textbox('textbox').attr('disabled', 'disabled');
    $('#lexueid').textbox('textbox').attr('disabled', 'disabled')
    var rows = $('#dg').datagrid('getRows');
    var row = rows[index]
    $('#edit-form').form('load', {
        "lexueid": row.lexueid,
        "KaoHao": row.KaoHao,
        "UserName": row.UserName,
        "UserClass": row.UserClass,
        "UserClassCode": row.UserClassCode,
        "UserPassword": row.UserPassword,
        "UserDesc": row.UserDesc
    });
    $('#FK_Specialty').combobox('setValue', row.FK_SpecialtyName);
    $('#dlg-edit').dialog('open');
}

function DeleteStudent(id, index) {
    $.ajax({
        type: 'POST',
        url: '/User/DeleteUser',
        data: { "lexueid": id },
        success: function (rst) {
            if (rst) {
                $('#dg').datagrid('deleteRow', index);
            }
            else {
                alert('删除失败');
            }
        },
        error: function (ret) {
            alert('删除失败');
        }
    });
}


function BatchDelStudent() {
    $.messager.confirm({
        title: "提示",
        msg: "确定删除 ?",
        ok: "确定",
        cancel: "取消",
        fn: function (r) {
            if (r) {
                var checkedItems = $('#dg').datagrid('getChecked');
                if (checkedItems != []) {
                    var ids = [];
                    var indexs = [];
                    $.each(checkedItems, function (index, item) {
                        ids.push(item.lexueid);
                        indexs.push(index);
                    });

                    $.ajax({
                        type: 'POST',
                        url: '/User/BatchDelUser',
                        data: { "lexueids": ids },
                        success: function (rst) {
                            if (rst == "True") {
                                $('#dg').datagrid("reload");
                            }
                            else {
                                alert('删除失败');
                            }
                        },
                        error: function (ret) {
                            alert('删除失败');
                        }
                    });
                }
                else {
                    alert('请勾选需要删除的学生！');
                }
            }
        }
    });
}

//重置
function reload() {
    $('#schoolName').textbox('setValue', '');
    $('#specialtyName').textbox('setValue', '');
    $('#studentName').textbox('setValue', '');
}

//保存学生信息
function SaveStudent() {
    var files = $("input[type='file']")[0].files
    if (files == null || files.length == 0) {
        alert('请先选择要上传的Excel文件！');
        return;
    }

    var formData = new FormData();
    formData.append("stufile", files[0]);

    cssChangeStart(2);

    $.ajax({
        url: '/User/AddStudent',
        type: 'Post',
        cache: false,
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.success) {
                alert('成功插入' + data.count + '条');
                closeDlg();
                initPage();
            }
            else {
                alert('添加出错！');
            }
        },
        error: function () {
            alert('添加出错！');
        }
    });
}
function AddStudent() {
    $('#dlg-add').dialog('open');
}
//预览
function Preview() {
    var files = $("input[type='file']")[0].files
    if (files == null || files.length == 0) {
        alert('请先选择要上传的Excel文件！');
        return;
    }

    cssChangeStart(1);

    var formData = new FormData();
    formData.append("stufile", files[0]);

    $.ajax({
        url: '/User/Preview',
        type: 'Post',
        cache: false,
        data: formData,
        contentType: false,
        processData: false,
        success: function (ret) {
            if (ret.success) {
                $('#dg-add').datagrid("loadData", ret.data.rows.slice(0, 10));
                initPaginationPreview(ret.data);
                cssChangeEnd(1);
            }
            else {
                cssChangeEnd(2);
            }
        },
        error: function () {
            alert('预览出错！');
        }
    });
}
//搜索
function searchInfo() {
    initPage();
}
//单元格单击事件
function clickCell(index, field) {
    if (index >= 0) {
        var rows = $('#dg').datagrid('getRows');
        var row = rows[index];
        if (field == "EDIT") {
            EditStudent(index)
        } else if (field == "DEL") {
            $.messager.confirm({
                title: "提示",
                msg: "确定删除此学生 ?",
                ok: "确定",
                cancel: "取消",
                fn: function (r) {
                    if (r) {
                        DeleteStudent(row.lexueid, index);
                    }
                }
            });
        }
    }
}
//编辑
function formatter_update() {
    return '<a class="fa fa-edit" style="color:green;"></a>';
}
//删除
function formatter_del() {
    return '<a class="fa fa-close" style="color:red;"></a>';
}
//主界面加载数据
function initPage() {

    var schoolName = "";
    if ($("#schoolName").length > 0) {
        var schoolName = $('#schoolName').textbox('getValue');
    }

    //datagrid初始化  
    $('#dg').datagrid({
        title: '学生信息列表',
        height: 'auto',
        nowrap: true,
        striped: true,
        border: true,
        collapsible: false,//是否可折叠的
        url: '/User/GetStudentList',
        remoteSort: true,
        idField: 'lexueid',
        pagination: true,//分页控件  
        pageSize: 20,
        rownumbers: true,//行号  
        showFooter: true,
        fitColumns: true,
        queryParams: {
            schoolName: schoolName,
            specialtyName: $('#specialtyName').textbox('getValue'),
            studentName: $('#studentName').textbox('getValue')
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
//窗口的table分页栏
function initPaginationPreview(data) {
    var page = $('#dg-add').datagrid("getPager");
    page.pagination({
        total: data.total,
        pageSize: 10,
        loading: true,
        showRefresh: false,
        pageList: [10, 20, 30, 50, 100],
        beforePageText: '第',
        afterPageText: '页    共 {pages} 页',
        displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',

        onSelectPage: $.proxy(function (pageNo, pageSize) {
            var start = (pageNo - 1) * pageSize;
            var end = start + pageSize;

            $('#dg-add').datagrid("loadData", data.rows.slice(start, end));
            page.pagination('refresh', {
                total: data.total,
                pageNumber: pageNo
            });
        }, this)
    });
}
//关闭窗口时
function closeDlg() {
    $('#dlg-add').dialog('close');
    $("#wait").text('');
    $('#pre-easyui-linkbutton').linkbutton('enable')
    $('#save-easyui-linkbutton').linkbutton('enable')
    $("#dg-add").datagrid("loadData", { total: 0, rows: [] });
}

//关闭窗口时
function closeDlgEdit() {
    $('#dlg-edit').dialog('close');
}

//关闭窗口时
function closeOutput() {
    $('#dlg-outputUser').dialog('close');
}

//点击预览或者保存时
function cssChangeStart(type) {
    if (type == 1) {
        $("#wait").text('正在加载数据...');
        $("#wait").css('color', 'red');
        $('#pre-easyui-linkbutton').linkbutton('disable')
    } else if (type == 2) {
        $("#wait").text('正在保存数据...');
        $("#wait").css('color', 'red');
        $('#save-easyui-linkbutton').linkbutton('disable')
    }
}
//预览数据加载成功后
function cssChangeEnd(type) {
    if (type == 1) {
        $("#wait").text('加载完成！');
        $("#wait").css('color', 'green');
        $('#pre-easyui-linkbutton').linkbutton('enable')
    }
    else if (type == 2) {
        $("#wait").text('加载失败！');
        $("#wait").css('color', 'red');
        $('#pre-easyui-linkbutton').linkbutton('enable')
    }
}


function OpenOutputUser() {
    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/User/GetSpecialtyList',
        success: function (rst) {
            if (rst.success) {
                $('#FK_SpecialtyName1').combobox('loadData', rst.data);
            }
            else {
                alert('获取专业出错！')
            }
        },
        error: function () {
            alert("获取专业出错！");
        },
    });
    $('#dlg-outputUser').dialog('open');
}


function AddSchoolAdmin() {
    var value = $('#FK_SpecialtyName1').val();
    if (value == '' || value == null) {
        alert('请选择专业！');
        return;
    }
    $('#output-form').form('submit', {
        url: '/User/SaveExcel',
        success: function (ret) {
            alert(ret);
        },
        onLoadError: function (ret) {
            alert('添加失败！');
        }
    });
}