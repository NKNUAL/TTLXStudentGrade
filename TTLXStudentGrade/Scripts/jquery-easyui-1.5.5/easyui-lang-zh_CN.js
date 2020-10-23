if ($.fn.pagination) {
	$.fn.pagination.defaults.beforePageText = '第';
	$.fn.pagination.defaults.afterPageText = '共{pages}页';
	$.fn.pagination.defaults.displayMsg = '显示{from}到{to},共{total}记录';
}
if ($.fn.datagrid) {
	$.fn.datagrid.defaults.loadMsg = '正在处理...';
}
if ($.fn.treegrid && $.fn.datagrid) {
	$.fn.treegrid.defaults.loadMsg = $.fn.datagrid.defaults.loadMsg;
}
if ($.messager) {
	$.messager.defaults.ok = '确定';
	$.messager.defaults.cancel = '取消';
}
$.map(['validatebox', 'textbox', 'passwordbox', 'filebox', 'searchbox',
	'combo', 'combobox', 'combogrid', 'combotree',
	'datebox', 'datetimebox', 'numberbox',
	'spinner', 'numberspinner', 'timespinner', 'datetimespinner'
], function (plugin) {
	if ($.fn[plugin]) {
		$.fn[plugin].defaults.missingMessage = '该输入项为必输项';
	}
});
if ($.fn.validatebox) {
	$.fn.validatebox.defaults.rules.email.message = '请输入有效的电子邮件地址';
	$.fn.validatebox.defaults.rules.url.message = '请输入有效的URL地址';
	$.fn.validatebox.defaults.rules.length.message = '输入内容长度必须介于{0}和{1}之间';
	$.fn.validatebox.defaults.rules.remote.message = '请修正该字段';
}
if ($.fn.calendar) {
	$.fn.calendar.defaults.weeks = ['日', '一', '二', '三', '四', '五', '六'];
	$.fn.calendar.defaults.months = ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'];
}
if ($.fn.datebox) {
	$.fn.datebox.defaults.currentText = '今天';
	$.fn.datebox.defaults.closeText = '关闭';
	$.fn.datebox.defaults.okText = '确定';
	$.fn.datebox.defaults.formatter = function (date) {
		var y = date.getFullYear();
		var m = date.getMonth() + 1;
		var d = date.getDate();
		return y + '-' + (m < 10 ? ('0' + m) : m) + '-' + (d < 10 ? ('0' + d) : d);
	};
	$.fn.datebox.defaults.parser = function (s) {
		if (!s) return new Date();
		var ss = s.split('-');
		var y = parseInt(ss[0], 10);
		var m = parseInt(ss[1], 10);
		var d = parseInt(ss[2], 10);
		if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
			return new Date(y, m - 1, d);
		} else {
			return new Date();
		}
	};
}
if ($.fn.datetimebox && $.fn.datebox) {
	$.extend($.fn.datetimebox.defaults, {
		currentText: $.fn.datebox.defaults.currentText,
		closeText: $.fn.datebox.defaults.closeText,
		okText: $.fn.datebox.defaults.okText
	});
}
if ($.fn.datetimespinner) {
	$.fn.datetimespinner.defaults.selections = [
		[0, 4],
		[5, 7],
		[8, 10],
		[11, 13],
		[14, 16],
		[17, 19]
	]
}


/*====================================================================
							扩展combobox方法
====================================================================*/

/**
 * 获取combobox选中数据数组
 * @param  {jQuery} jq jQuery对象
 * @return {Array}    选中行数据数组
 */
$.fn.combobox.methods.getSelections = function (jq) {
	var _a67 = jq[0];
	var selections = [];
	var opts = $.data(_a67, "combobox").options;
	jq.combo("panel").find(".combobox-item-selected").each(function () {
		var row = opts.finder.getRow(_a67, $(this));
		selections.push(row);
	});

	return selections;
}

/**
 * 获取combobox第一行选中数据
 * @param  {jQuery} jq jQuery对象
 * @return {Array}    选中第一行数据
 */
$.fn.combobox.methods.getSelected = function (jq) {
	var _a67 = jq[0];
	var selected;
	var opts = $.data(_a67, "combobox").options;
	jq.combo("panel").find(".combobox-item-selected:first").each(function () {
		selected = opts.finder.getRow(_a67, $(this));
	});

	return selected;
}

/*====================================================================
							扩展filebox方法
====================================================================*/

/**
 * 获取filebox选中文件
 * @param  {jQuery} jq jQuery对象
 * @return {Array}    选中文件信息
 */
$.fn.filebox.methods.getFiles = function (jq) {
	var domData = jq.data('filebox');
	var fileDom = domData.filebox.find(".textbox-value");

	return fileDom[0].files;
}



/*====================================================================
							扩展验证方法
====================================================================*/

$.extend($.fn.validatebox.defaults.rules, {
	// filebox验证文件大小的规则函数
	// 如：validType : ['fileSize[1,"MB"]']
	fileSize: {
		validator: function (value, array) {
			var size = array[0];
			var unit = array[1];
			if (!size || isNaN(size) || size == 0) {
				$.error('验证文件大小的值不能为 "' + size + '"');
			} else if (!unit) {
				$.error('请指定验证文件大小的单位');
			}
			var index = -1;
			var unitArr = new Array("bytes", "kb", "mb", "gb", "tb", "pb", "eb", "zb", "yb");
			for (var i = 0; i < unitArr.length; i++) {
				if (unitArr[i] == unit.toLowerCase()) {
					index = i;
					break;
				}
			}
			if (index == -1) {
				$.error('请指定正确的验证文件大小的单位：["bytes", "kb", "mb", "gb", "tb", "pb", "eb", "zb", "yb"]');
			}
			// 转换为bytes公式
			var formula = 1;
			while (index > 0) {
				formula = formula * 1024;
				index--;
			}
			// this为页面上能看到文件名称的文本框，而非真实的file
			// $(this).next()是file元素
			return $(this).next().get(0).files[0].size < parseFloat(size) * formula;
		},
		message: '文件大小必须小于 {0}{1}'
	}
});


/**
 * easyui控件是否已初始化过，主要是combobox
 */

$.fn.isInit = function (className) {
	className = className || 'easyui-fluid';
	// 是否已被easyui修改过--这里不能判断手动添加easyui样式的组件调用方式
	if (this.hasClass('textbox-f')) {
		return true;
	} else if (this.next('.' + className).length > 1) {
		return true;
	} else {
		// 这里可以之后扩充
		return false;
	}
}