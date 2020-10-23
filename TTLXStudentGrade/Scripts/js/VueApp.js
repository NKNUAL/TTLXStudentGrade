//组件
Vue.component('view-text', {
    name: 'view-text',
    props: ['name','mimg'],
    template: "<a href=\"javascript:void(0)\" style=\"box-sizing: content-box;\"><img v-bind:src=\"mimg\" alt=\"\"></img><small style=\"margin-top: 10px;\">{{name}}</small></a>"
})

Vue.component('view-pointer', {
    name: 'view-pointer',
    props: ['url', 'name','mimg'],
    template: '<a v-on:click="hrefView" style=\"box-sizing: content-box;\"><img v-bind:src=\"mimg\" alt=\"\"></img><strong>{{name}}</strong></a>',
    methods: {
        hrefView: function () {
            $that = this;
            if($.isEmptyObject($that.url)){
                console.log('请求路径未配置。');
            }
            else {
                $that.addEasyUITab($that.name,$that.url);
            }
        },
        addEasyUITab: function (title, url) {
            if ($('#view-child').tabs('exists', title)) {
                $('#view-child').tabs('select', title);
            } else {
                var content = '<iframe id="' + title + '" class="frame-height" scrolling="auto" frameborder="0"  src="' + url + '"></iframe>';
                $('#view-child').tabs('add', {
                    title: title,
                    content: content,
                    closable: true,
                    fit: true
                });
            }
        }
    }
})

Vue.component('view-text-normal', {
    name: 'view-text',
    props: ['name', 'mimg'],
    template: "<div><img class='nav-img' v-bind:src=\"mimg\" alt=\"\"></img><span>{{name}}</span><i class=\"fa fa-caret-right right-ico\" style=\"padding-right:0px;\"></i></div>"
})

Vue.component('view-pointer-normal', {
    name: 'view-pointer',
    props: ['url', 'name', 'mimg'],
    template: '<a v-on:click="hrefView"><img v-bind:src=\"mimg\" alt=\"\" style=\"color:green;\"></img>&nbsp;&nbsp;{{name}}</a>',
    methods: {
        hrefView: function () {
            $that = this;
            if ($.isEmptyObject($that.url)) {
                console.log('请求路径未配置。');
            }
            else {
                $that.addEasyUITab($that.name, $that.url);
            }
        },
        addEasyUITab: function (title, url) {
            if ($('#view-child').tabs('exists', title)) {
                $('#view-child').tabs('select', title);
            } else {
                var content = '<iframe id="' + title + '" class="frame-height" scrolling="auto" frameborder="0"  src="' + url + '"></iframe>';
                if (url.substr(0,4) == 'http') {
                    window.open(url);
                    return;
                }
                $('#view-child').tabs('add', {
                    title: title,
                    content: content,
                    closable: true,
                    fit: true
                });
            }
        }
    }
})

//实例
var vm = new Vue({
    el: '#app',
    data: {
        items: [],
        show: false,
        changed: true
    },
    mounted: function () {
        this.getJsonData();
    },
    methods: {
        getJsonData: function () {
            that = this;
            //ajax异步请求功能模块数据
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: '/Home/GetModules',
                success: function (data) {
                    if(!!data){
                        that.items = data;
                        that.show = true;
                    }
                },
                error: function () {
                    console.log('请求权限菜单失败！')
                }
            })
        },
        collapseItem: function (event) {
            var $el = $(event.currentTarget);
            var $parent = $el.parent();
            var $next = $el.next();
            var home = $el[0].innerText.trim();

            if (home === '联考通知') {
                window.open('http://www.jngk.net.cn/New/new_detail.html?id=250');
            } else {
                $('.accordion .link.active').removeClass('active');
            }
            $next.slideToggle();
            $parent.toggleClass('open');
            $parent.parent().find('.submenu').not($next).slideUp().parent().removeClass('open');
        },
        removeCurrentClass: function (event) {
            var $el = event.target;
            $('#accordion').find('li').removeClass('current');
            $($el).parent().addClass('current');
        }
    }
})