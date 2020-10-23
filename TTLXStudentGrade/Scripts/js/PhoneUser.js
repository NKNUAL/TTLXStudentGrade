function loadSchool() {
    $.ajax({
        type: 'get',
        url: '/PhoneAccount/GetSchools',
        success: function (ret) {
            if (ret.success == true) {
                $('#school').combobox('loadData', ret.data);
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

function selectSchool(record) {
    $.ajax({
        type: 'get',
        url: '/PhoneAccount/GetSpecialtyLimit?schoolNo=' + record.SchoolNo,
        success: function (ret) {
            if (ret.success == true) {
                $('#pg').datagrid("loadData", ret.data);
            }
            else {
                alert('获取数据失败！')
            }
        },
        error: function () {
            alert('获取数据错误！')
        },
    });
}
function formatter_update() {
    return '<a class="fa fa-edit" style="color:green;"></a>';
}
function formatter_save() {
    return '<a class="fa fa-save" style="color:green;"></a>';
}

var editIndex = undefined;
function endEdit() {

    if (editIndex == undefined) { return true; }//如果为undefined的话，为真，说明可以编辑

    if ($('#pg').datagrid('validateRow', editIndex)) {
        $('#pg').datagrid('endEdit', editIndex);//当前行编辑事件取消
        editIndex = undefined;
        return true;//重置编辑行索引对象，返回真，允许编辑
    }
    else {
        return false;
    }//否则，为假，返回假，不允许编辑
}
function edits(index) {
    if (endEdit()) {
        editIndex = index;//给editIndex对象赋值，index为当前行的索引
        var selectRow = $('#pg').datagrid('selectRow', editIndex);
        selectRow.datagrid('beginEdit', editIndex);
    }
}

function save(row, index) {
    if (editIndex != index)
        return;
    $('#pg').datagrid('endEdit', index);
    //alert(row.SpecialtyId + ":" + $('#school').combobox('getValue'));
    $.ajax({
        type: 'post',
        url: '/PhoneAccount/UpdateSchoolLimit',
        data: { 'schoolNo': $('#school').combobox('getValue'), 'specialtyId': row.SpecialtyId, 'limitCount': row.LimitCount },
        success: function (ret) {
            if (ret.success == true) {
            }
            else {
                alert('修改失败：' + ret.message)
            }
        },
        error: function () {
            alert('修改错误！')
        },
    });
}


//单元格单击事件
function clickCell(index, field) {
    if (index >= 0) {
        var rows = $('#pg').datagrid('getRows');
        var row = rows[index];
        if (field == "Edit") {
            edits(index);
        } else if (field == "Save") {
            save(row, index);
        }
    }
}
