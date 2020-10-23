var areas = [
    { AreaNo: '1', AreaName: '十堰市' },
    { AreaNo: '2', AreaName: '襄阳市' },
    { AreaNo: '3', AreaName: '宜昌市' },
    { AreaNo: '4', AreaName: '恩施市' },
    { AreaNo: '5', AreaName: '荆州市' },
    { AreaNo: '6', AreaName: '荆门市' },
    { AreaNo: '7', AreaName: '随州市' },
    { AreaNo: '8', AreaName: '潜江市' },
    { AreaNo: '9', AreaName: '仙桃市' },
    { AreaNo: '10', AreaName: '天门市' },
    { AreaNo: '11', AreaName: '孝感市' },
    { AreaNo: '12', AreaName: '武汉市' },
    { AreaNo: '13', AreaName: '咸宁市' },
    { AreaNo: '14', AreaName: '黄石市' },
    { AreaNo: '15', AreaName: '鄂州市' },
    { AreaNo: '16', AreaName: '黄冈市' },
    { AreaNo: '17', AreaName: '其他' }
];
var schools = [
    {
        AreaNo: '16',
        Schools:
            [{ SchoolNo: '240', SchoolName: '黄冈职业技术学院' }, { SchoolNo: '233', SchoolName: '黄冈市第二技校' }, { SchoolNo: '232', SchoolName: '黄冈电子信息中等专业学校' }, { SchoolNo: '169', SchoolName: '武穴市职业技术教育中心' }, { SchoolNo: '163', SchoolName: '蕲春李时珍职校' }, { SchoolNo: '142', SchoolName: '罗田理工中专' }, { SchoolNo: '138', SchoolName: '黄冈科技职业学院' }, { SchoolNo: '127', SchoolName: '武穴财贸中等专业学校' }, { SchoolNo: '124', SchoolName: '黄冈市技师学院' }, { SchoolNo: '123', SchoolName: '蕲春理工中专' }, { SchoolNo: '105', SchoolName: '英山理工中专' }, { SchoolNo: '104', SchoolName: '麻城职教集团' }, { SchoolNo: '083', SchoolName: '黄冈市中等职业学校（集团）' }, { SchoolNo: '075', SchoolName: '武穴职业技术学校' }, { SchoolNo: '074', SchoolName: '浠水县理工中专' }, { SchoolNo: '066', SchoolName: '红安县职教中心' }, { SchoolNo: '061', SchoolName: '武穴理工中等专业学校' }, { SchoolNo: '046', SchoolName: '团风县理工中专' }, { SchoolNo: '037', SchoolName: '黄梅县理工学校' }, { SchoolNo: '036', SchoolName: '黄州理工中专学校' }, { SchoolNo: '325', SchoolName: '李时珍职业技术学校' }]
    }, {
        AreaNo: '12',
        Schools:
            [{ SchoolNo: '239', SchoolName: '武汉技师学院' }, { SchoolNo: '238', SchoolName: '武汉助产学校' }, { SchoolNo: '237', SchoolName: '武汉市财政学校' }, { SchoolNo: '235', SchoolName: '长江工程职业技术学院' }, { SchoolNo: '234', SchoolName: '武汉光谷职业学院' }, { SchoolNo: '207', SchoolName: '武钢高级技工学校' }, { SchoolNo: '206', SchoolName: '武汉应用科技学校' }, { SchoolNo: '205', SchoolName: '湖北广播电视学校' }, { SchoolNo: '173', SchoolName: '武汉百大学堂教育学校' }, { SchoolNo: '157', SchoolName: '武汉军需工业技工学校' }, { SchoolNo: '165', SchoolName: '武汉市工业科技学校' }, { SchoolNo: '155', SchoolName: '武汉市新洲高级职业中学' }, { SchoolNo: '152', SchoolName: '湖北生态工程职业技术学院' }, { SchoolNo: '148', SchoolName: '武汉市凡谷电子职业技术学校' }, { SchoolNo: '147', SchoolName: '武汉市第三职业教育中心' }, { SchoolNo: '146', SchoolName: '武汉市机电工程学校' }, { SchoolNo: '141', SchoolName: '武汉市第一商业学校' }, { SchoolNo: '139', SchoolName: '武汉工程职业技术学院' }, { SchoolNo: '131', SchoolName: '武汉市第二轻工业学校' }, { SchoolNo: '130', SchoolName: '武汉市蔡甸区职教中心' }, { SchoolNo: '122', SchoolName: '武汉市东西湖职业技术学校' }, { SchoolNo: '118', SchoolName: '武汉市汉南区职业教育培训中心' }, { SchoolNo: '114', SchoolName: '武汉市第二职教中心' }, { SchoolNo: '101', SchoolName: '武汉市供销商校' }, { SchoolNo: '098', SchoolName: '武汉市石牌岭职高' }, { SchoolNo: '088', SchoolName: '武汉经济开发区职业技术学校' }, { SchoolNo: '087', SchoolName: '武汉市电子信息职业技术学校' }, { SchoolNo: '081', SchoolName: '武汉市财贸学校' }, { SchoolNo: '069', SchoolName: '武汉市黄陂区职业技术学校' }, { SchoolNo: '060', SchoolName: '武汉市第二卫生学校' }, { SchoolNo: '057', SchoolName: '武汉市江夏职业技术学校' }, { SchoolNo: '055', SchoolName: '武汉市第一职业教育中心' }, { SchoolNo: '054', SchoolName: '武汉旅游学校' }, { SchoolNo: '053', SchoolName: '武汉市新洲区高级职业中学' }, { SchoolNo: '052', SchoolName: '武汉交通学校' }, { SchoolNo: '043', SchoolName: '武汉市第二高级技工学校' }, { SchoolNo: '038', SchoolName: '武汉市仪表电子学校' }, { SchoolNo: '241', SchoolName: '湖北邮电学校' }, { SchoolNo: '800', SchoolName: '社会职业技术学校' }, { SchoolNo: '242', SchoolName: '美约科技' }]
    }, {
        AreaNo: '13',
        Schools:
            [{ SchoolNo: '231', SchoolName: '咸宁职业技术学院中专部' }, { SchoolNo: '230', SchoolName: '湖北新产业技师学院' }, { SchoolNo: '164', SchoolName: '咸宁财税会计学校' }, { SchoolNo: '099', SchoolName: '赤壁市机电信息技术学校' }, { SchoolNo: '095', SchoolName: '赤壁市职教集团学校' }, { SchoolNo: '093', SchoolName: '咸宁职业教育（集团）学校二区' }, { SchoolNo: '079', SchoolName: '咸宁市蒲圻师范学校' }, { SchoolNo: '068', SchoolName: '通山县职业教育中心' }, { SchoolNo: '015', SchoolName: '通城县职业教育中心' }, { SchoolNo: '014', SchoolName: '咸宁职业教育(集团)学校' }, { SchoolNo: '012', SchoolName: '崇阳县中等职业技术学校' }]
    }, {
        AreaNo: '11',
        Schools:
            [{ SchoolNo: '229', SchoolName: '湖北职业技术学院应用技术分院' }, { SchoolNo: '228', SchoolName: '孝感市孝南区职业技术学校' }, { SchoolNo: '208', SchoolName: '孝昌中等职业技术学校' }, { SchoolNo: '161', SchoolName: '孝感红人中等职业学校' }, { SchoolNo: '160', SchoolName: '应城市中等职业技术学校' }, { SchoolNo: '153', SchoolName: '孝感师范' }, { SchoolNo: '151', SchoolName: '孝感市职业中等专业学校' }, { SchoolNo: '117', SchoolName: '云梦县中等职业技术学校' }, { SchoolNo: '109', SchoolName: '孝感市高级技工学校' }, { SchoolNo: '080', SchoolName: '安陆市中等职业技术学校' }, { SchoolNo: '070', SchoolName: '孝感市护士学校' }, { SchoolNo: '024', SchoolName: '大悟县中等职业技术学校' }, { SchoolNo: '023', SchoolName: '孝感生物工程学校' }, { SchoolNo: '022', SchoolName: '孝感工业学校' }, { SchoolNo: '021', SchoolName: '汉川市中等职业技术学校' }]
    }, {
        AreaNo: '15',
        Schools:
            [{ SchoolNo: '227', SchoolName: '鄂州电子科技学校' }, { SchoolNo: '226', SchoolName: '鄂州市经贸学校（湖北航空技术学校）' }, { SchoolNo: '119', SchoolName: '湖北宝业建工学校' }, { SchoolNo: '091', SchoolName: '鄂州中专' }, { SchoolNo: '025', SchoolName: '鄂州职业大学中等职业技术学院' }]
    }, {
        AreaNo: '9',
        Schools:
            [{ SchoolNo: '225', SchoolName: '仙桃市理工中等专业学校' }, { SchoolNo: '162', SchoolName: '仙桃市创源计算机科技中等职业学校' }, { SchoolNo: '113', SchoolName: '仙桃职业学院' }, { SchoolNo: '100', SchoolName: '仙桃市电子商贸学校' }]
    }, {
        AreaNo: '8',
        Schools:
            [{ SchoolNo: '224', SchoolName: '潜江卫校' }, { SchoolNo: '120', SchoolName: '江汉油田职业技术学校' }, { SchoolNo: '077', SchoolName: '潜江市职教中心' }]
    }, {
        AreaNo: '5',
        Schools:
            [{ SchoolNo: '222', SchoolName: '监利职业技术教育中心' }, { SchoolNo: '221', SchoolName: '石首市职业高级中学' }, { SchoolNo: '220', SchoolName: '荆州市创业职业中等专业学校' }, { SchoolNo: '167', SchoolName: '荆州市机械电子工业学校' }, { SchoolNo: '159', SchoolName: '荆州技师学院' }, { SchoolNo: '126', SchoolName: '湖北省创业高级技工学校' }, { SchoolNo: '121', SchoolName: '石首市文汇高中' }, { SchoolNo: '116', SchoolName: '洪湖职业技术教育中心' }, { SchoolNo: '112', SchoolName: '石首高级技工学校' }, { SchoolNo: '103', SchoolName: '荆州市太湖港中学' }, { SchoolNo: '089', SchoolName: '荆州市荆南中学' }, { SchoolNo: '058', SchoolName: '洪湖市园林中学' }, { SchoolNo: '039', SchoolName: '松滋市职教中心' }, { SchoolNo: '028', SchoolName: '松滋市言程中学' }, { SchoolNo: '027', SchoolName: '江陵县实验高级中学' }, { SchoolNo: '026', SchoolName: '公安县职教中心' }]
    }, {
        AreaNo: '4',
        Schools:
            [{ SchoolNo: '219', SchoolName: '恩施土家族苗族自治州卫生学校' }, { SchoolNo: '218', SchoolName: '恩施州广播电视大学' }, { SchoolNo: '171', SchoolName: '咸丰县民族技工学校' }, { SchoolNo: '108', SchoolName: '恩施职业技术学院中职部' }, { SchoolNo: '013', SchoolName: '来凤县中等职业技术学校' }, { SchoolNo: '011', SchoolName: '巴东县民族职业高级中学' }, { SchoolNo: '010', SchoolName: '建始县中等职业技术学校' }, { SchoolNo: '009', SchoolName: '宣恩县中等职业技术学校' }, { SchoolNo: '008', SchoolName: '咸丰县中等职业技术学校' }, { SchoolNo: '007', SchoolName: '恩施市中等职业技术学校' }, { SchoolNo: '006', SchoolName: '鹤峰县中等职业技术学校' }, { SchoolNo: '005', SchoolName: '利川市民族中等职业技术学校' }, { SchoolNo: '002', SchoolName: '咸丰县高级技工学校' }, { SchoolNo: '001', SchoolName: '恩施州信息技术学校' }]
    }, {
        AreaNo: '3',
        Schools:
            [{ SchoolNo: '217', SchoolName: '葛洲坝高级技工学校' }, { SchoolNo: '211', SchoolName: '三峡电力职业学院' }, { SchoolNo: '170', SchoolName: '宜昌远安职教中心' }, { SchoolNo: '143', SchoolName: '三峡中等专业学校' }, { SchoolNo: '107', SchoolName: '兴山县职教中心' }, { SchoolNo: '092', SchoolName: '五峰县职教中心' }, { SchoolNo: '064', SchoolName: '当阳市职教中心' }, { SchoolNo: '063', SchoolName: '远安县职业教育中心' }, { SchoolNo: '062', SchoolName: '宜昌市机电工程学校' }, { SchoolNo: '049', SchoolName: '三峡旅游职业技术学院' }, { SchoolNo: '034', SchoolName: '宜昌现代信息中等职业技术学校' }, { SchoolNo: '032', SchoolName: '秭归职业教育中心' }, { SchoolNo: '031', SchoolName: '长阳土家族自治县职业教育中心' }, { SchoolNo: '030', SchoolName: '枝江市职教中心' }, { SchoolNo: '029', SchoolName: '宜都市职业教育中心' }]
    }, {
        AreaNo: '2',
        Schools:
            [{ SchoolNo: '216', SchoolName: '襄阳市襄州区职教中心' }, { SchoolNo: '215', SchoolName: '襄阳技师学院' }, { SchoolNo: '214', SchoolName: '襄阳铁路工业学校(内燃机)' }, { SchoolNo: '213', SchoolName: '枣阳市机电工程学校' }, { SchoolNo: '210', SchoolName: '襄阳市护士学校' }, { SchoolNo: '168', SchoolName: '襄阳职业技术学院' }, { SchoolNo: '150', SchoolName: '枣阳市第二职业高中' }, { SchoolNo: '145', SchoolName: '襄阳汽车职业技术学院' }, { SchoolNo: '129', SchoolName: '樊城区职业技术教育中心' }, { SchoolNo: '082', SchoolName: '老河口市职业技术学校' }, { SchoolNo: '076', SchoolName: '襄阳市第九中学' }, { SchoolNo: '071', SchoolName: '南漳县职业教育中心' }, { SchoolNo: '059', SchoolName: '襄阳市谷城县职教中心' }, { SchoolNo: '056', SchoolName: '襄阳市汇文综合高中' }, { SchoolNo: '020', SchoolName: '襄阳市第八中学' }, { SchoolNo: '019', SchoolName: '枣阳市职业高中' }, { SchoolNo: '018', SchoolName: '襄城区职业高级中学' }, { SchoolNo: '017', SchoolName: '宜城职业高中' }, { SchoolNo: '016', SchoolName: '保康县中等职业技术学校' }, { SchoolNo: '300', SchoolName: '襄阳职业技术学院汽车工程学院' }]
    }, {
        AreaNo: '1',
        Schools:
            [{ SchoolNo: '212', SchoolName: '十堰市医药卫生学校' }, { SchoolNo: '149', SchoolName: '竹山县卫生学校' }, { SchoolNo: '137', SchoolName: '丹江口市第二中学' }, { SchoolNo: '136', SchoolName: '十堰科技学校' }, { SchoolNo: '125', SchoolName: '房县职业技术学校（二区）' }, { SchoolNo: '115', SchoolName: '十堰职业技术集团学校' }, { SchoolNo: '106', SchoolName: '竹山县职业技术集团学校' }, { SchoolNo: '094', SchoolName: '十堰市郧阳科技学校' }, { SchoolNo: '086', SchoolName: '郧西县职业技术学校西校区' }, { SchoolNo: '085', SchoolName: '郧西县职业技术学校' }, { SchoolNo: '073', SchoolName: '汉江科技学校' }, { SchoolNo: '067', SchoolName: '房县职业技术学校' }, { SchoolNo: '065', SchoolName: '十堰高级职业学校' }, { SchoolNo: '004', SchoolName: '竹溪县职业技术学校' }, { SchoolNo: '003', SchoolName: '十堰市高级技工学校' }]
    }, {
        AreaNo: '7',
        Schools:
            [{ SchoolNo: '209', SchoolName: '随县职业技术教育中心' }, { SchoolNo: '154', SchoolName: '广水市文华高级中学' }, { SchoolNo: '134', SchoolName: '随州职业技术学院' }, { SchoolNo: '111', SchoolName: '随州职业技术学院' }, { SchoolNo: '110', SchoolName: '随州市技师学院' }, { SchoolNo: '072', SchoolName: '广水市育才高中' }, { SchoolNo: '047', SchoolName: '随州机电工程学校' }, { SchoolNo: '033', SchoolName: '随州市第一职业中学（湖北现代教育集团）' }]
    }, {
        AreaNo: '14',
        Schools:
            [{ SchoolNo: '172', SchoolName: '黄石市俊贤高级技工学校' }, { SchoolNo: '144', SchoolName: '黄石艺术学校' }, { SchoolNo: '128', SchoolName: '大冶市职业技术学校' }, { SchoolNo: '102', SchoolName: '湖北城市职业学校' }, { SchoolNo: '096', SchoolName: '湖北省机械工业学校' }, { SchoolNo: '078', SchoolName: '阳新县职业教育中心' }, { SchoolNo: '048', SchoolName: '大冶市还地桥高中' }, { SchoolNo: '045', SchoolName: '大冶市第三中学' }]
    }, {
        AreaNo: '6',
        Schools:
            [{ SchoolNo: '166', SchoolName: '京山市中等职业技术学校' }, { SchoolNo: '140', SchoolName: '荆门技师学院' }, { SchoolNo: '135', SchoolName: '荆门市现代学校' }, { SchoolNo: '133', SchoolName: '钟祥市技工学校' }, { SchoolNo: '132', SchoolName: '荆门掇刀职业高级中学' }, { SchoolNo: '090', SchoolName: '沙洋县职教中心' }, { SchoolNo: '084', SchoolName: '京山县职业教育中心' }, { SchoolNo: '051', SchoolName: '荆门市掇刀职业高中' }, { SchoolNo: '044', SchoolName: '钟祥市长寿职业高中' }, { SchoolNo: '042', SchoolName: '钟祥市志强职业中学' }, { SchoolNo: '041', SchoolName: '钟祥市职业高中' }, { SchoolNo: '040', SchoolName: '湖北信息工程学校' }, { SchoolNo: '035', SchoolName: '荆门市东宝职教中心' }]
    }, {
        AreaNo: '10',
        Schools:
            [{ SchoolNo: '097', SchoolName: '天门职业技术学院' }, { SchoolNo: '050', SchoolName: '天门市职教中心' }]
    }, {
        AreaNo: '17',
        Schools:
            [{ SchoolNo: '998', SchoolName: '其他' }]
    }];
var specialties = [
    { SpecialtyId: '7', SpecialtyName: '车工' },
    { SpecialtyId: '11', SpecialtyName: '钳工' },
    { SpecialtyId: '8', SpecialtyName: '电气电子专业' },
    { SpecialtyId: '3', SpecialtyName: '会计专业' },
    { SpecialtyId: '12', SpecialtyName: '电子商务专业' },
    { SpecialtyId: '0', SpecialtyName: '计算机类' },
    { SpecialtyId: '10', SpecialtyName: '酒店服务' },
    { SpecialtyId: '9', SpecialtyName: '导游服务' },
    { SpecialtyId: '6', SpecialtyName: '建筑技术类' },
    { SpecialtyId: '1', SpecialtyName: '学前教育' },
    { SpecialtyId: '5', SpecialtyName: '护理专业' },
    { SpecialtyId: '4', SpecialtyName: '汽车维修类' },
    { SpecialtyId: '2', SpecialtyName: '种植类' },
];






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
function isIDCardAvailable(input) {
    var myreg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
    if (!myreg.test(input)) {
        return false;
    } else {
        return true;
    }
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

var verifyCode = new GVerify("v_container");
function register() {

    $("#s-code").html('');
    $("#s-area").html('');
    $("#s-school").html('');
    $("#s-specialty").html('');
    $("#s-name").html('');
    $("#s-idcard").html('');
    $("#s-phone").html('');
    $("#s-pwd").html('');
    $("#s-pwd2").html('');


    var areaValue = $('#Area').combobox('getValue');
    if (!checkValue(areaValue)) {
        $("#s-area").html('请选择区域！');
        return;
    }

    var schoolValue = $('#School').combobox('getValue');
    if (!checkValue(schoolValue)) {
        $("#s-school").html('请选择学校！');
        return;
    }

    var specialtyValue = $('#Specialty').combobox('getValue');
    if (!checkValue(specialtyValue)) {
        $("#s-specialty").html('请选择专业！');
        return;
    }

    var nameValue = $('#Name').textbox('getValue');
    if (!checkValue(nameValue)) {
        $("#s-name").html('请输入您的姓名！')
        return;
    }
    else {
        if (!isNameAvailable(nameValue)) {
            $("#s-name").html('请输入您的真实姓名！');
            return;
        }
    }

    var cardValue = $('#IDCard').textbox('getValue');
    if (!checkValue(cardValue)) {
        $("#s-idcard").html('请输入身份证号！')
        return;
    }
    else {
        if (!isIDCardAvailable(cardValue)) {
            $("#s-idcard").html('请输入真实的身份证号！');
            return
        }
    }

    var phoneValue = $('#PhoneNumber').textbox('getValue');
    if (!checkValue(phoneValue)) {
        $("#s-phone").html('请输入手机号方便我们与您联系！')
        return;
    }
    else {
        if (!isPhoneAvailable(phoneValue)) {
            $("#s-phone").html('请输入有效的手机号！')
            return;
        }
    }

    var pwdValue = $('#Pwd').textbox('getValue');
    var pwd2Value = $('#Pwd2').textbox('getValue');
    if (!checkValue(pwdValue)) {
        $("#s-pwd").html('请输入密码！')
        return;
    }
    else {
        if (pwdValue.length < 6) {
            $("#s-pwd").html('密码长度至少为6位！')
            return;
        }
        else {
            if (!checkValue(pwd2Value)) {
                $("#s-pwd2").html('请再次输入您的密码！');
                return;
            }
            else {
                if (pwdValue != pwd2Value) {
                    $("#s-pwd2").html('两次输入的密码不一致！');
                    return;
                }
            }
        }
    }

    var codeValue = $('#code_input').textbox('getValue');
    if (!checkValue(codeValue)) {
        $("#s-code").html('请输入下方验证码！')
        return;
    }
    else {
        var res = verifyCode.validate(codeValue);
        if (!res) {
            $("#s-code").html('验证码错误！');
            return;
        }
    }


    var body = $("body");
    //创建表单
    var form = $("<form></form>");
    //将表单放入body中
    body.append(form);
    //设置表单各项属性
    form.attr("action", "/Register/StudentRegister");
    form.attr("method", "post");
    //创建input对象并放入表单中
    var input1 = $("<input name='SchoolNo' value='" + schoolValue + "' />");
    form.append(input1);
    var input2 = $("<input name='SpecialtyId' value='" + specialtyValue + "' />");
    form.append(input2);
    var input3 = $("<input name='UserName' value='" + nameValue + "' />");
    form.append(input3);
    var input4 = $("<input name='IDCard' value='" + cardValue + "' />");
    form.append(input4);
    var input5 = $("<input name='PhoneNumber' value='" + phoneValue + "' />");
    form.append(input5);
    var input6 = $("<input name='Password' value='" + pwdValue + "' />");
    form.append(input6);
    var input7 = $("<input name='QQ' value='" + $('#QQ').textbox('getValue') + "' />");
    form.append(input7);

    //提交表单
    form.submit();
    form.remove();


}


function OutputProvinceExcel() {


}


function checkValue(input) {
    if (input == '' || input == null)
        return false;
    return true;
}


function loadArea() {

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


function findPwd() {


    var body = $("body");
    //创建表单
    var form = $("<form></form>");
    //将表单放入body中
    body.append(form);
    //设置表单各项属性
    form.attr("action", "/Register/FindPwd");
    form.attr("method", "post");
    //创建input对象并放入表单中

    var input3 = $("<input name='UserName' value='" + $('#UserName').textbox('getValue') + "' />");
    form.append(input3);
    var input4 = $("<input name='IDCard' value='" + $('#IDCard').textbox('getValue') + "' />");
    form.append(input4);


    //提交表单
    form.submit();
    form.remove();

}
