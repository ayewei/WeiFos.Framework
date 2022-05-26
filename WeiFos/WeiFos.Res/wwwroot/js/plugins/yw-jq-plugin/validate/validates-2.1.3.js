
/// <summary>
/// 验证对象
/// @author             叶委  
/// @date               2013-05-23         
/// </summary>
var verifyType = {

    /// <summary>
    /// 没有验证 
    /// @returns boolean
    /// </summary>
    normal: function () {
        return true;
    },
    /// <summary>
    /// 匹配任意字符
    /// @returns boolean
    /// </summary>
    anyCharacter: function (str) {
        return $.trim(str).length >= 1;
    },
    /// <summary>
    /// 编号验证
    /// @param price
    /// @returns boolean
    /// </summary>
    isSerialNumber: function (serialNumber) {
        return /^[a-zA-Z0-9_-]{1,100}$/.test(serialNumber);
    },
    /// <summary>
    /// 英文名称验证
    /// @param englishName
    /// @returns boolean
    /// </summary>
    isEnglishName: function (englishName) {
        return /^[a-zA-Z_-]{1,100}$/.test(englishName);
    },
    /// <summary>
    /// 货币英文简写验证
    /// @param abbreviation
    /// @returns boolean
    /// </summary>
    isEnglishAbbreviation: function (abbreviation) {
        return /^[A-Z]{3}$/.test(abbreviation);
    },
    /// <summary>
    /// 价格格式验证，验证小数点后2位
    /// @param price
    /// @returns boolean
    /// </summary>
    isPrice: function (price) {
        return /(^[1-9]\d*(\.\d{1,4})?$)|(^[-+]?[0]{1}(\.\d{1,4})?$)/.test(price);
    },
    /// <summary>
    /// 价格格式验证，验证小数点后2位
    /// @param price
    /// @returns boolean
    /// </summary>
    isLGZeroPrice: function (price) {
        return /(^[-+]?[1-9]\d*(\.\d{1,4})?$)|(^[-+]?[0]{1}(\.\d{1,4})?$)/.test(price) && price > 0;
    },
    /// <summary>
    /// 金额格式验证，小数点后位不做验证
    /// @param validateDate 
    /// @returns boolean
    /// </summary>
    isAmount: function (amount) {
        return /^-?\d+\.{0,}\d{0,}$/.test(amount);
    },
    /// <summary>
    /// 金额格式 验证大于等于0，验证小数点后2位
    /// @param validateDate 
    /// @returns boolean
    /// </summary>
    isShipments: function (amount) {
        // return /^\d+\.{0,}\d{0,}$/.test(amount);
        return /^\d+\.*(\.\d{1,2})?$/.test(amount);
    },
    /// <summary>
    /// 金额格式 验证大于等于0，验证小数点后4位
    /// @param validateDate 
    /// @returns boolean
    /// </summary>
    isShipments4: function (amount) {
        // return /^\d+\.{0,}\d{0,}$/.test(amount);
        return /^\d+\.*(\.\d{1,4})?$/.test(amount);
    },
    /// <summary>
    /// 金额格式 验证大于0，验证小数点后4位
    /// @param validateDate 
    /// @returns boolean
    /// </summary>
    isLGZeroShipments4: function (amount) {
        // return /^\d+\.{0,}\d{0,}$/.test(amount);
        return /^\d+\.*(\.\d{1,4})?$/.test(amount) && amount > 0;
    },
    /// <summary>
    /// 金额格式 验证大于0，验证小数点后2位
    /// @param validateDate 
    /// @returns boolean
    /// </summary>
    isLGZeroShipments2: function (amount) {
        // return /^\d+\.{0,}\d{0,}$/.test(amount);
        return /^\d+\.*(\.\d{1,2})?$/.test(amount) && amount > 0;
    },
    /// <summary>
    /// 金额格式验证，小数点后位不做验证
    /// @param validateDate 
    /// @returns boolean
    /// </summary>
    isProfitRatio: function (amount) {
        var profit = /(^[-+]?[1-9]\d*(\.\d{1,2})?$)|(^[-+]?[0]{1}(\.\d{1,2})?$)/.test(amount);
        //数字
        if (profit) {
            if (amount < 0) {
                return false;
            }
            if (amount.indexOf('.') > -1) {
                //alert(amount.substring(0, amount.indexOf('.')).length)
                if (amount.substring(0, amount.indexOf('.')).length > 2) {
                    return false;
                }
                if (amount.length > 5) {
                    return false;
                }
            }
            else {
                if (amount.length > 2) {
                    if (amount == 100) {

                    }
                    else {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;
    },
    /// <summary>
    /// 大于0金额格式验证 
    /// @param validateDate 
    /// @returns boolean
    /// </summary>
    isLGZeroAmount: function (amount) {
        return /^-?\d+\.{0,}\d{0,}$/.test(amount) && amount > 0;
    },
    /// <summary>
    /// 邮箱格式验证
    /// @param email
    /// @returns boolean
    /// </summary>
    isEmail: function (email) {
        return /^\s*\w+(?:\.{0,1}[\w-]+)*@[a-zA-Z0-9]+(?:[-.][a-zA-Z0-9]+)*\.[a-zA-Z]+\s*$/i.test(email);
    },
    /// <summary>
    /// QQ号码格式验证
    /// @param email
    /// @returns boolean
    /// </summary>
    isQQNumber: function (qq) {
        return /^[1-9]\d{4,10}$/.test(qq);
    },
    /// <summary>
    /// 登录名格式验证
    /// @param loginName
    /// @returns boolean
    /// </summary>
    isLoginName: function (loginName) {
        return /^[a-zA-Z0-9_-]{6,16}$/.test(loginName);
    },
    /// <summary>
    /// 密码格式验证 验证用户密码(正确格式为：长度在6~16 之间，任意字符)  
    /// @param psw 
    /// @returns boolean
    /// </summary>
    isPassword: function (psw) {
        return /^[a-zA-Z0-9_-]{6,16}$/.test(psw);
    },
    /// <summary>
    /// 手机号码格式验证  
    /// @param mobile 
    /// @returns boolean
    /// </summary>
    isMoblie: function (mobile) {
        return /^1[3456789]\d{9}$/.test(mobile);
    },
    /// <summary>
    /// 电话号码格式验证  
    /// @param phone 
    /// @returns boolean
    /// </summary>
    isPhone: function (phone) {
        return /(^[0-9]{3,4}[\-|\s][0-9]{7,8}$)|(^[0-9]{7,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^0{0,1}1[345678]\d{9}$)/.test(phone);
    },
    /// <summary>
    /// 邮编格式验证  
    /// @param postCode 
    /// @returns boolean
    /// </summary>
    isPostCode: function (postCode) {
        return /^[0-9]{6}$/.test(postCode);
    },
    /// <summary>
    /// 验证码基本格式验证  
    /// @param validateCode 
    /// @returns boolean
    /// </summary>
    validateCode4: function (validateCode) {
        return /^[a-zA-Z0-9]{4,4}$/.test(validateCode);
    },
    /// <summary>
    /// 验证码基本格式验证  
    /// @param validateCode 
    /// @returns boolean
    /// </summary>
    validateCode5: function (validateCode) {
        return /^[a-zA-Z0-9]{5,5}$/.test(validateCode);
    },
    /// <summary>
    /// 验证汉字数字字母 
    /// @param validateUserName 
    /// @returns boolean
    /// </summary>
    isNumberlatterCcter: function (userName) {
        return /^[\u0391-\uFFE5A-Za-z0-9]+$/.test(userName);
    },

    /// <summary>
    /// 验证汉字逗号 
    /// @param validateUserName 
    /// @returns boolean
    /// </summary>
    isHanZiDouHao: function (economyCity) {
        return /^[\u4e00-\u9fff,]+$/.test(economyCity);
    },
    /// <summary>
    /// 日期格式验证 
    /// @param validateDate 
    /// @returns boolean
    /// </summary>
    isDate: function (date) {
        return /((^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(10|12|0?[13578])([-\/\._])(3[01]|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(11|0?[469])([-\/\._])(30|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(0?2)([-\/\._])(2[0-8]|1[0-9]|0?[1-9])$)|(^([2468][048]00)([-\/\._])(0?2)([-\/\._])(29)$)|(^([3579][26]00)([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][0][48])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][0][48])([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][2468][048])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][2468][048])([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][13579][26])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][13579][26])([-\/\._])(0?2)([-\/\._])(29)$))/.test(date);
    },
    /// <summary>
    /// 网络地址验证 
    /// @param url 
    /// @returns boolean
    /// </summary>
    isUrl: function (url) {
        return /^((http|https|ftp):\/\/)?(\w(\:\w)?@)?([0-9a-z_-]+\.)*?([a-z0-9-]+\.[a-z]{2,6}(\.[a-z]{2})?(\:[0-9]{2,6})?)((\/[^?#<>\/\\*":]*)+(\?[^#]*)?(#.*)?)?$/i.test(url);
    },
    /// <summary>
    /// IP地址 
    /// @param ip 
    /// @returns boolean
    /// </summary>
    isIpAddress: function (ip) {
        return /^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/i.test(ip);
    },
    /// <summary>
    /// 正整数格式验证 
    /// @param validateDate 
    /// @returns boolean
    /// </summary>
    isNumber: function (number) {
        return /^\d+$/g.test(number);
    },
    /// <summary>
    /// 浮点数格式验证 
    /// @param validateDate 
    /// @returns boolean
    /// </summary>
    isFloat: function (number) {
        return /^\d+(\.\d+)?$/.test(number);
    },
    /// <summary>
    /// 大于0的正整数格式验证 
    /// @param no
    /// @returns boolean
    /// </summary>
    isLGZeroNumber: function (number) {
        return /^([1-9]\d{0,15})$/g.test(number);
    },
    /// <summary>
    /// 是否是身份证号码  
    /// @param idCard 
    /// @returns boolean
    /// </summary>
    isIdCard: function (idCard) {
        idCard = $.trim(idCard);
        if (idCard.length == 15) {
            return verifyType.isValidityBrithBy15IdCard(idCard);
        } else if (idCard.length == 18) {
            var a_idCard = idCard.split("");// 得到身份证数组   
            if (verifyType.isValidityBrithBy18IdCard(idCard) && verifyType.isTrueValidateCodeBy18IdCard(a_idCard)) {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    },
    /// <summary>
    /// 验证15位数身份证号码中的生日是否是有效生日   
    /// @param idCard 
    /// @returns boolean
    /// </summary>
    isValidityBrithBy15IdCard: function (idCard) {
        var year = idCard15.substring(6, 8);
        var month = idCard15.substring(8, 10);
        var day = idCard15.substring(10, 12);
        var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
        // 对于老身份证中的你年龄则不需考虑千年虫问题而使用getYear()方法   
        if (temp_date.getYear() != parseFloat(year)
            || temp_date.getMonth() != parseFloat(month) - 1
            || temp_date.getDate() != parseFloat(day)) {
            return false;
        } else {
            return true;
        }
    },
    /// <summary>
    /// 验证18位数身份证号码中的生日是否是有效生日   
    /// @param idCard 18位书身份证字符串 
    /// @returns boolean
    /// </summary>
    isValidityBrithBy18IdCard: function (idCard18) {
        var year = idCard18.substring(6, 10);
        var month = idCard18.substring(10, 12);
        var day = idCard18.substring(12, 14);
        var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
        // 这里用getFullYear()获取年份，避免千年虫问题   
        if (temp_date.getFullYear() != parseFloat(year)
            || temp_date.getMonth() != parseFloat(month) - 1
            || temp_date.getDate() != parseFloat(day)) {
            return false;
        } else {
            return true;
        }
    },
    /// <summary>
    /// 判断身份证号码为18位时最后的验证位是否正确   
    /// @param a_idCard 身份证号码数组   
    /// @returns boolean
    /// </summary>
    isTrueValidateCodeBy18IdCard: function (a_idCard) {
        var Wi = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1];
        // 身份证验证位值.10代表X
        var ValideCode = [1, 0, 10, 9, 8, 7, 6, 5, 4, 3, 2];
        // 声明加权求和变量   
        var sum = 0;
        if (a_idCard[17].toLowerCase() == 'x') {
            a_idCard[17] = 10;// 将最后位为x的验证码替换为10方便后续操作   
        }
        for (var i = 0; i < 17; i++) {
            sum += Wi[i] * a_idCard[i];// 加权求和   
        }
        var valCodePosition = sum % 11;// 得到验证码所位置   
        if (a_idCard[17] == ValideCode[valCodePosition]) {
            return true;
        } else {
            return false;
        }
    },
    /// <summary>
    /// 图书ISBN标准号 13位
    /// @param 图书ISBN标准号
    /// @returns boolean
    /// </summary>
    isISBN13: function (code) {
        code = (code + '').replace(/[-\s]/g, '');
        if (!/^\d{12,13}$/.test(code)) return;
        var i = 1, c = 0; // c:checksum
        for (; i < 12; i += 2)
            c += Math.floor(code.charAt(i));

        for (c *= 3, i = 0; i < 12; i += 2)
            c += Math.floor(code.charAt(i));

        c = (220 - c) % 10; // 220:大於(1*6+3*6)，%10==0即可。
        if (code.length == 12) return false;
        //if (code.length == 12) return code + c;
        return c == code.charAt(12);
    },
    /// <summary>
    /// 图书ISBN标准号 10位
    /// @param 图书ISBN标准号
    /// @returns boolean
    /// </summary>
    isISBN10: function (code) {
        code = (code + '').replace(/[-\s]/g, '');
        if (!/^\d{9}[\dxX]?$/.test(code)) return false;
        var i = 0, c = 0; // c:checksum
        for (; i < 9;)
            c += code.charAt(i++) * i;
        c %= 11; if (c == 10) c = 'X';
        //if (code.length == 9) return code + c;
        if (code.length == 9) return false;
        return c == (i = code.charAt(9)) || c == 'X' && i == 'x';
    },
    /// <summary>
    /// 银行卡号验证 
    /// @param a_idCard 身份证号码数组   
    /// @returns boolean
    /// </summary>
    isBankCardNo: function (bankno) {
        if ($.trim(bankno).length == 0) return false;
        var lastNum = bankno.substr(bankno.length - 1, 1);//取出最后一位（与luhm进行比较）

        var first15Num = bankno.substr(0, bankno.length - 1);//前15或18位
        var newArr = new Array();
        for (var i = first15Num.length - 1; i > -1; i--) {    //前15或18位倒序存进数组
            newArr.push(first15Num.substr(i, 1));
        }
        var arrJiShu = new Array();  //奇数位*2的积 <9
        var arrJiShu2 = new Array(); //奇数位*2的积 >9

        var arrOuShu = new Array();  //偶数位数组
        for (var j = 0; j < newArr.length; j++) {
            if ((j + 1) % 2 == 1) {//奇数位
                if (parseInt(newArr[j]) * 2 < 9)
                    arrJiShu.push(parseInt(newArr[j]) * 2);
                else
                    arrJiShu2.push(parseInt(newArr[j]) * 2);
            }
            else //偶数位
                arrOuShu.push(newArr[j]);
        }

        var jishu_child1 = new Array();//奇数位*2 >9 的分割之后的数组个位数
        var jishu_child2 = new Array();//奇数位*2 >9 的分割之后的数组十位数
        for (var h = 0; h < arrJiShu2.length; h++) {
            jishu_child1.push(parseInt(arrJiShu2[h]) % 10);
            jishu_child2.push(parseInt(arrJiShu2[h]) / 10);
        }

        var sumJiShu = 0; //奇数位*2 < 9 的数组之和
        var sumOuShu = 0; //偶数位数组之和
        var sumJiShuChild1 = 0; //奇数位*2 >9 的分割之后的数组个位数之和
        var sumJiShuChild2 = 0; //奇数位*2 >9 的分割之后的数组十位数之和
        var sumTotal = 0;
        for (var m = 0; m < arrJiShu.length; m++) {
            sumJiShu = sumJiShu + parseInt(arrJiShu[m]);
        }

        for (var n = 0; n < arrOuShu.length; n++) {
            sumOuShu = sumOuShu + parseInt(arrOuShu[n]);
        }

        for (var p = 0; p < jishu_child1.length; p++) {
            sumJiShuChild1 = sumJiShuChild1 + parseInt(jishu_child1[p]);
            sumJiShuChild2 = sumJiShuChild2 + parseInt(jishu_child2[p]);
        }
        //计算总和
        sumTotal = parseInt(sumJiShu) + parseInt(sumOuShu) + parseInt(sumJiShuChild1) + parseInt(sumJiShuChild2);

        //计算Luhm值
        var k = parseInt(sumTotal) % 10 == 0 ? 10 : parseInt(sumTotal) % 10;
        var luhm = 10 - k;

        if (lastNum == luhm) {
            //$("#banknoInfo").html("Luhm验证通过");
            return true;
        }
        else {
            //$("#banknoInfo").html("银行卡号必须符合Luhm校验");
            return false;
        }
    },
    /// <summary>
    /// 验证营业执照是否合法 
    /// @param busCode 营业执照号码   
    /// @returns boolean
    /// </summary>
    isBusinessLicense: function (busCode) {
        var ret = false;
        if (busCode.length == 15) {
            var sum = 0;
            var s = [], p = [], a = [], m = 10;
            p[0] = m;
            for (var i = 0; i < busCode.length; i++) {
                a[i] = parseInt(busCode.substring(i, i + 1), m);
                s[i] = (p[i] % (m + 1)) + a[i];
                if (0 == s[i] % m) {
                    p[i + 1] = 10 * 2;
                } else {
                    p[i + 1] = (s[i] % m) * 2;
                }
            }
            if (1 == (s[14] % m)) {
                ret = true;
            } else {
                ret = false;
            }
        } else {
            ret = false;
        }
        return ret;
    },
    /// <summary>
    /// 验证营业执照是否合法 三证合一
    /// @param busCode 营业执照号码   
    /// @returns boolean
    /// </summary>
    isBizLicense: function (busCode) {
        this.firstarray = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
        this.firstkeys = [3, 7, 9, 10, 5, 8, 4, 2];
        this.secondarray = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'T', 'U', 'W', 'X', 'Y'];
        this.secondkeys = [1, 3, 9, 27, 19, 26, 16, 17, 20, 29, 25, 13, 8, 24, 10, 30, 28];
        this.calc = function (code, array1, array2, b) {
            var count = 0;
            for (var i = 0; i < array2.length; i++) {
                var a = code[i];
                count += array2[i] * array1.indexOf(a);
            }
            return b - count % b;
        }

        var code = busCode.toUpperCase();
        /*统一代码由十八位的阿拉伯数字或大写英文字母（不使用I、O、Z、S、V）组成。
        第1位：登记管理部门代码（共一位字符）
        第2位：机构类别代码（共一位字符）
        第3位~第8位：登记管理机关行政区划码（共六位阿拉伯数字）
        第9位~第17位：主体标识码（组织机构代码）（共九位字符）
        第18位：校验码​（共一位字符 */
        if (code.length != 18) { return false; }
        var reg = /^\w\w\d{6}\w{9}\w$/;
        if (!reg.test(code)) {
            return false;
        }
        /*
         登记管理部门代码：使用阿拉伯数字或大写英文字母表示。​
         机构编制：1​
         民政：5​
         工商：9​
         其他：Y
         */
        reg = /^[1,5,9,Y]\w\d{6}\w{9}\w$/;
        if (!reg.test(code)) {
            return false;
        }
        /*
         机构类别代码：使用阿拉伯数字或大写英文字母表示。​
         机构编制机关：11打头​​
         机构编制事业单位：12打头​
         机构编制中央编办直接管理机构编制的群众团体：13打头​​
         机构编制其他：19打头​
         民政社会团体：51打头​
         民政民办非企业单位：52打头​
         民政基金会：53打头​
         民政其他：59打头​
         工商企业：91打头​
         工商个体工商户：92打头​
         工商农民专业合作社：93打头​
         其他：Y1打头​
         */
        reg = /^(11|12|13|19|51|52|53|59|91|92|93|Y1)\d{6}\w{9}\w$/;
        if (!reg.test(code)) {
            return false;
        }
        /*
         登记管理机关行政区划码：只能使用阿拉伯数字表示。按照GB/T 2260编码。​
         例如：四川省成都市本级就是510100；四川省自贡市自流井区就是510302。​
         */
        reg = /^(11|12|13|19|51|52|53|59|91|92|93|Y1)\d{6}\w{9}\w$/;
        if (!reg.test(code)) {
            return false;
        }
        /*
         主体标识码（组织机构代码）：使用阿拉伯数字或英文大写字母表示。按照GB 11714编码。
         在实行统一社会信用代码之前，以前的组织机构代码证上的组织机构代码由九位字符组成。格式为XXXXXXXX-Y。前面八位被称为“本体代码”；最后一位被称为“校验码”。校验码和本体代码由一个连字号（-）连接起来。以便让人很容易的看出校验码。但是三证合一后，组织机构的九位字符全部被纳入统一社会信用代码的第9位至第17位，其原有组织机构代码上的连字号不带入统一社会信用代码。
         原有组织机构代码上的“校验码”的计算规则是：​
         例如：某公司的组织机构代码是：59467239-9。那其最后一位的组织机构代码校验码9是如何计算出来的呢？
         第一步：取组织机构代码的前八位本体代码为基数。5 9 4 6 7 2 3 9
         提示：如果本体代码中含有英文大写字母。则A的基数是10，B的基数是11，C的基数是12，依此类推，直到Z的基数是35。
         第二步：​​取加权因子数值。因为组织机构代码的本体代码一共是八位字符。则这八位的加权因子数值从左到右分别是：3、7、9、10、5、8、4、2。​
         第三步：本体代码基数与对应位数的因子数值相乘。​
         5×3＝15，9×7＝63，4×9＝36，6×10＝60，
         7×5＝35，2×8＝16，3×4=12，9×2＝18​​
         第四步：将乘积求和相加。​
         15+63+36+60+35+16+12+18=255
         第五步：​将和数除以11，求余数。​​
         255÷11=33，余数是2。​​
         */
        var firstkey = this.calc(code.substr(8),
            this.firstarray,
            this.firstkeys,
            11);
        /*
         第六步：用阿拉伯数字11减去余数，得求校验码的数值。当校验码的数值为10时，校验码用英文大写字母X来表示；当校验码的数值为11时，校验码用0来表示；其余求出的校验码数值就用其本身的阿拉伯数字来表示。​
         11-2＝9，因此此公司完整的组织机构代码为 59467239-9。​​
         */
        var firstword;
        if (firstkey < 10) {
            firstword = firstkey;
        } if (firstkey == 10) {
            firstword = 'X';
        } else if (firstkey == 11) {
            firstword = '0';
        }
        if (firstword != code.substr(16, 1)) {
            return false;
        }
        /*
         校验码：使用阿拉伯数字或大写英文字母来表示。校验码的计算方法参照 GB/T 17710。
         例如：某公司的统一社会信用代码为91512081MA62K0260E，那其最后一位的校验码E是如何计算出来的呢？
         第一步：取统一社会信用代码的前十七位为基数。9 1 5 1 2 0 8 1 21 10 6 2 19 0 2 6 0提示：如果前十七位统一社会信用代码含有英文大写字母（不使用I、O、Z、S、V这五个英文字母）。则英文字母对应的基数分别为：A=10、B=11、C=12、D=13、E=14、F=15、G=16、H=17、J=18、K=19、L=20、M=21、N=22、P=23、Q=24、R=25、T=26、U=27、W=28、X=29、Y=30​
         第二步：​​取加权因子数值。因为统一社会信用代码前面前面有十七位字符。则这十七位的加权因子数值从左到右分别是：1、3、9、27、19、26、16、17、20、29、25、13、8、24、10、30、2​8
         第三步：基数与对应位数的因子数值相乘。​
         9×1=9，1×3=3，5×9=45，1×27=27，2×19=38，0×26=0，8×16=128​
         1×17=17，21×20=420，10×29=290，6×25=150，2×13=26，19×8=152​
         0×23=0，2×10=20，6×30=180，0×28=0
         第四步：将乘积求和相加。​9+3+45+27+38+0+128+17+420+290+150+26+152+0+20+180+0=1495
         第五步：​将和数除以31，求余数。​​
         1495÷31=48，余数是17。​​
         */
        var secondkey = this.calc(code,
            this.secondarray,
            this.secondkeys,
            31);
        /*
         第六步：用阿拉伯数字31减去余数，得求校验码的数值。当校验码的数值为0~9时，就直接用该校验码的数值作为最终的统一社会信用代码的校验码；如果校验码的数值是10~30，则校验码转换为对应的大写英文字母。对应关系为：A=10、B=11、C=12、D=13、E=14、F=15、G=16、H=17、J=18、K=19、L=20、M=21、N=22、P=23、Q=24、R=25、T=26、U=27、W=28、X=29、Y=30
         因为，31-17＝14，所以该公司完整的统一社会信用代码为 91512081MA62K0260E。​​
         */
        var secondword = this.secondarray[secondkey];
        if (secondword == undefined || secondword != code.substr(17, 1)) {
            return false;
        }
        var word = code.substr(0, 16) + firstword + secondword;
        if (code != word) {
            // return false;
        }
        return true;
    },
    /// <summary>
    /// 数据库重复验证  
    /// </summary>
    isRegister: function (params) {
        var bdata = true;
        $.ajax({
            type: "get",
            url: params.url.indexOf("?") == -1 ? params.url + "?m=" + Math.random() : params.url + "&m=" + Math.random(),
            data: params.data,
            async: false,
            dataType: "json",
            success: function (result) {
                if (undefined != result && "XMLHttpRequest.LoginOut" == result) {
                    alert("登录超时，请重新登录");
                    window.location.href = App_G.Util.getDomain();
                }
                bdata = result
            }
        });
        return bdata;
    }
};

/// <summary>
/// 初始化次数
/// </summary>
yw.valid.initcount = 0;

/// <summary>
/// 触发提交验证
/// </summary>
yw.valid.submiteles = [];

/// <summary>
/// 指定验证项数组
/// </summary>
yw.valid.ves = {
    //页面普通元素
    elementItems: new Array(),
    //必须不重复的
    distinctItems: new Array(),
    //必须重复的
    relementItems: new Array(),
    //页面元素索引
    indexOf: function (item) {
        for (var i = 0; i < this.elementItems.length; i++) {
            if (this.elementItems[i] == item) {
                //console.log("存在的ID" + this.elementItems[i].optionsId + "传入的ID" + item.optionsId);
                return i;
            }
        }
        return -1;
    },
    delete: function (e) {
        var that = this
        var items = [];
        $.each(that.elementItems, function (i, o) {
            if (o.e == e) {
                items.push(o)
            }
        });

        $.each(items, function (i, o) {
            var index = that.indexOf(o)
            if (index != -1) {
                that.elementItems.splice(index, 1);
            }
        });
    },
    deleteItem: function (Id) {
        var item = null;
        $.each(this.elementItems, function (i, o) {
            if (o.optionsId == Id) {
                item = o;
                return;
            }
        });

        if (item != null && this.indexOf(item) != -1) {
            this.elementItems.splice(this.indexOf(item), 1);
        }
    },
    insertItems: function (item) {
        //页面必须重复
        if (item.isrepeatvld) {
            this.relementItems.push(item);
        }
        //页面必须不重复的
        if (item.isDistinct) {
            this.distinctItems.push(item);
        }

        this.elementItems.push(item);
    },
    //验证必须重复的
    validateRepeat: function (obj) {
        if (this.relementItems.length > 1) {
            for (var i = 0; i < this.relementItems.length; i++) {
                if (obj.prop("id") != this.relementItems[i].optionsId && $("#" + this.relementItems[i].optionsId).val() != "" && obj.val() != $("#" + this.relementItems[i].optionsId).val()) {
                    return false;
                }
            }
        }
        return true;
    },
    //验证必须不重复的
    validateDistinct: function (obj) {
        var is_check = true;
        if (this.distinctItems.length > 1) {
            for (var i = 0; i < this.distinctItems.length; i++) {
                if (obj.prop("id") != this.distinctItems[i].optionsId && $("#" + this.distinctItems[i].optionsId).val() != "" && obj.val() == $("#" + this.distinctItems[i].optionsId).val()) {
                    is_check = false;
                    break;
                }
            }
        }
        return is_check;
    },
    validateSelect: function (val) {
        for (var i = 0; i < this.elementItems.length; i++) {
            if (this.elementItems[i].selectvld && $("#" + this.elementItems[i].optionsId).val().length > 0) {
                return false;
            }
        }
        return true;
    },
    validateItems: function (item) {
        for (var i = 0; i < this.elementItems.length; i++) {
            if (this.elementItems[i] == item)
                return true;
        }
        return false;
    },
    appointvalidate: false
};

/// <summary>
/// 全局配置
/// @param  {Object}.{String}       options.data           处理页面表单
/// @param  {Object}.{Array}        options.data.attr      自定义标签名称 [{ attr:"data-val",data : entity },{ attr:"data-val",data : entity } ]
/// @param  {Object}.{String}       options.data.data      处理页面表单
/// </summary>
yw.valid.config = function (options) {

    //提交元素数组
    if (App_G.Util.isArray(options.submiteles)) {
        $.each(options.submiteles, function (i, o) {
            if ($(o).length) {
                yw.valid.submiteles.push($(o));
            }
        });
    } else if (App_G.Util.isJQuery(options.submiteles)) {
        yw.valid.submiteles.push(options.submiteles);
    } else if (App_G.Util.isString(options.submiteles)) {
        yw.valid.submiteles.push($(options.submiteles));
    }

    //全局提交绑定事件
    $.each(yw.valid.submiteles, function (i, o) {
        $(o).click(function () {
            if (yw.valid.validates(-1)) {
                options.vsuccess($(this), o);
                return true;
            } else {
                if (options.verror != undefined) {
                    options.verror();
                }
                return false;
            }
        });
    });

    //校验数据格式
    if (options.data != undefined && App_G.Util.isArray(options.data)) {
        //循环绑定
        $.each(options.data, function (i, o) {
            App_G.Mapping.Bind(o.attr, o.data);
        });

        if (options.load != undefined) {
            options.load(options.data)
        }
    }

};

/// <summary>
/// validate v1.0
/// @author             叶委  
/// @create date        2013-05-23 
/// @update date v1.0   2015-08-25
/// @groupid            验证组ID
/// </summary>
yw.valid.getValidate = function (options) {

    var opt = {}, current;

    yw.valid.initcount++;

    if (options == undefined) options = {};

    if (undefined == options.selector) current = $(document); else current = $(options.selector);

    //局部页面提交元素
    opt.submiteles = [];
    if (undefined != options.submiteles) {
        opt.submiteles.push(options.submiteles);
    }

    if (opt.submiteles.length) {
        //局部提交绑定事件
        $.each(opt.submiteles, function (i, o) {
            if (App_G.Util.isJQuery(options.submiteles)) {
                o = o.selector;
            }

            //该元素得到焦点事件
            current.on("click", o, function (e) {
                //e.stopPropagation();
                if (yw.valid.validates(opt.groupId)) {
                    options.vsuccess(o);
                    return true;
                } else {
                    options.verror();
                    return false;
                }
            });
        });
    }



    /// <summary>
    /// Author   yewei  
    /// Creation date    2013/05/23   
    /// Updated  date    2018/02/01
    /// @param  {String} ---                                           选择器（匿名参数）
    /// @param  {Object}                      options                  匿名参数对象
    /// @param  {Object}.{String}             options.selector         选择器上下文
    /// @param  {Object}.{String}             options.tabindex         验证错误提示选项卡
    /// @param  {Object}.{String}             options.vtype            验证类型（内置20多种正则，验证常用数据格式）
    /// @param  {Object}.{String}             options.model            模式 0:简洁模式，1:后置图标模式(默认为一)，2:自定义模式
    /// @param  {Object}.{function}           options.focus            得到焦点参数对象 { focus.msg : "", focus.func : function() { } }
    /// @param  {Object}.{Object}.{String}    options.focus.msg        得到焦点提示信息(不适用于模式2)
    /// @param  {Object}.{Object}.{function}  options.focus.func       得到焦点自定义回调事件
    /// @param  {Object}.{function}           options.blur             失去焦点自定义回调事件 { blur.msg : "", blur.func : function(){} }
    /// @param  {Object}.{Object}.{String}    options.blur.msg         失去焦点提示信息(不适用于模式2)
    /// @param  {Object}.{Object}.{function}  options.blur.func        失去焦点自定义回调事件
    /// @param  {Object}.{String}             options.nullmsg          为空提示信息
    /// @param  {Object}.{String}             options.othermsg         其他验证信息（例如重复信息验证）
    /// @param  {Object}.{Boolean}            options.isInit           是否初始化信息
    /// @param  {Object}.{Boolean}            options.isTrim           是否清楚首尾空格
    /// @param  {Object}.{String}             options.aftereId         指定在某个页面元素后面提示信息
    /// @param  {Object}.{String}             options.groupeId         页面元素使用同一组提示信息元素
    /// @param  {Object}.{Boolean}            options.selectvld        选中验证，不填不验证，反之验证
    /// @param  {Object}.{Object}             options.repeatvld        重复数据验证参数对象
    /// @param  {Object}.{Object}.{String}    options.repeatvld.url    重复数据验证，请求地址
    /// @param  {Object}.{Object}.{String}    options.repeatvld.msg    重复数据验证，数据存在时提示信息
    /// @param  {Object}.{Object}.{String}    options.repeatvld.func   重复数据验证，数据存在时提示信息customData
    /// @param  {Object}.{Boolean}            options.isrepeatvld      页面元素值必须重复验证
    /// @param  {Object}.{Boolean}            options.isDistinct       页面元素值非必须重复验证
    /// @param  {Object}.{Array}              options.submiteles       触发提交表单元素(组)
    /// @param  {Object}.{function}           options.vsuccess         验证成功执行
    /// @param  {Object}.{function}           options.verror           验证失败执行
    /// </summary>
    opt.valid = function (e, options) {

        //默认初始化
        if (options.focus == undefined) { options.focus = {} };
        if (options.blur == undefined) { options.blur = {} };

        options["e"] = e;
        options["groupId"] = opt.groupId;

        //克隆参数，处理统一对象共享传递
        var _options = $.extend(true, {}, options), current;
        if (undefined == _options.selector) current = $(document); else current = $(_options.selector);
        var $this = current.find(e);
        if (undefined == $this || $this.length == 0) {
            //如果元素动态创建到页面外的iframe
            if (self.frameElement && self.frameElement.tagName == "IFRAME") {
                $this = $(e, parent.document);
            } else {
                $this = $(e);
            }
        }

        if ($this.length) {

            $.each($this, function (i, o) {

                if (undefined == _options.model) _options.model = 1;

                //设置ID
                if (undefined == $(o).attr("id")) {
                    $(o).attr("id", new Date().getTime().toString() + App_G.Util.getRandomNum(1, 1000000));
                }

                _options.optionsId = $(o).attr("id");

                //克隆参数，处理统一对象共享传递
                var tmp_options = $.extend(true, {}, _options);
                tmp_options.optionsId = $(o).attr("id");
                if (yw.valid.ves.indexOf(_options) == -1) {
                    yw.valid.ves.insertItems(tmp_options);
                } else {
                    yw.valid.ves.deleteItem(_options.optionsId);
                    yw.valid.ves.insertItems(tmp_options);
                }

                //元素ID
                var ele_id = "#" + $(o).attr("id");

                switch ($(o).prop("type")) {
                    //文本输入框
                    case "text":
                    case "tel":
                    case "password":
                    case "textarea":
                        //该元素得到焦点事件
                        current.on("focus", ele_id, function (e) {
                            var obj = $(ele_id);;
                            if (!obj.length) {
                                try {
                                    obj = $(ele_id, window.parent.document);
                                } catch (e) { }
                            }
                            yw.valid.msg.focus(obj, _options);
                        });

                        //失去焦点事件
                        current.on("blur", ele_id, function (e) {
                            //清除首尾空格
                            if (_options.isTrim) $(ele_id).val($.trim($(ele_id).val()));
                            yw.valid.validate(ele_id, _options);
                        });
                        break;
                    //下拉选择框
                    case "select-one":
                        //选择改变事件 
                        current.on("change", ele_id, function (e) {
                            yw.valid.validate(ele_id, _options);
                        });
                        break;
                    //复选框
                    case "checkbox":
                        //单击事件
                        current.on("click", ele_id, function (e) {
                            var obj = $(ele_id);
                            if (!obj.length) {
                                obj = $(ele_id, parent.document);
                            }

                            if (obj.is(':checked')) {
                                yw.valid.msg.success(obj, _options);
                            } else {
                                yw.valid.msg.error(obj, _options);
                            }
                        });
                        break;
                    default:
                        break;
                }

                //初始焦点提示信息
                if (_options.isInit) {
                    yw.valid.msg.focus($("#" + $(o).attr("id")), _options);
                }

            });

        }
    }

    //执行局部（分组）验证
    opt.valid.executeGroup = function (data) {
        if (yw.valid.validates(opt.groupId)) {
            options.vsuccess(data);
            return true;
        } else {
            if (options.verror != undefined) {
                options.verror(data);
            }
            return false;
        }
    }

    //执行验证所有
    opt.valid.execute = function (data) {
        if (yw.valid.validates(-1)) {
            options.vsuccess(data);
            return true;
        } else {
            if (options.verror != undefined) {
                options.verror(data);
            }
            return false;
        }
    }

    //删除验证项
    opt.valid.delete = function (e) {
        yw.valid.ves.delete(e);
    }

    //验证需要验证的项
    yw.valid.validates = function (groupId) {
        var ispass = true;
        $.each(yw.valid.ves.elementItems, function (i, o) {
            if (o.groupId == groupId || groupId == -1) {
                if (!yw.valid.validate("#" + o.optionsId, o) && ispass) {
                    var index = $("#" + o.optionsId).parents(".tab-pane").index();
                    if (index != -1) {
                        //tab选项卡切换
                        $("ul.nav-tabs").children().eq(index).addClass("active").siblings().removeClass("active");
                        $("div.tab-content").children().eq(index).addClass("active").siblings().removeClass("active");
                    }
                    ispass = false;
                }
            }
        });
        return ispass;
    }

    //验证对象
    yw.valid.validate = function (selector, options) {

        var obj = $(selector);
        if (!obj.length) {
            try {
                obj = $(selector, window.parent.document);
            } catch (e) { }
        }

        if (!obj.length) return true;

        /// <summary>
        /// Author   yewei  
        /// 回传参数状态码说明
        /// Creation date    2013/05/23   
        /// Updated  date    2018/02/01
        /// 状态 0：数据不能为空 ，1：数据异步校验已存在数据，
        /// 11：时页面元素值重复，200：验证通过，500：为验证失败
        /// </summary>
        //验证类型通过
        if (options.vtype($.trim(obj.val()), obj)) {

            var ispass = false;

            //必须重复验证
            if (options.isrepeatvld) {
                if (!yw.valid.ves.validateRepeat(obj)) {
                    if (options.model == 2 && options.blur.func != undefined) {
                        return options.blur.func(obj, 11);
                    } else {
                        return yw.valid.msg.other(obj, options);
                    }
                }
            }

            //必须非重复验证
            if (options.isDistinct) {
                if (!yw.valid.ves.validateDistinct(obj)) {
                    if (options.model == 2 && options.blur.func != undefined) {
                        return options.blur.func(obj, 11);
                    } else {
                        return yw.valid.msg.other(obj, options);
                    }
                }
            }

            //如果异步验证
            if (options.repeatvld != null && options.repeatvld != undefined && !ispass) {

                //自定义模式
                if (options.model == 2 && options.blur.func != undefined) {

                } else {
                    yw.valid.msg.detect(obj, { aftereId: options.aftereId }, "检测中...");
                }

                //将获取到的参数赋值
                options.repeatvld.data = {};
                options.repeatvld.data[obj.attr("id")] = obj.val();

                //自定义参数
                if ((typeof options.repeatvld.func == 'function') && options.repeatvld.func.constructor == Function) {
                    var cdata = options.repeatvld.func(obj);
                    for (var attr in cdata) {
                        options.repeatvld.data[attr] = cdata[attr];
                    }
                }

                //异步验证，数据库存在该记录
                var msg = verifyType.isRegister(options.repeatvld);

                //返回为对象则为 自定义信息
                if ((typeof msg == 'object')) {
                    if (msg.status == "1") {
                        if (options.model == 2 && options.blur.func != undefined) {
                            return options.blur.func(obj, 1);
                        } else {
                            return yw.valid.msg.custom(obj, options, msg.message);
                        }
                    }
                } else {
                    if (msg == "1") {
                        if (options.model == 2 && options.blur.func != undefined) {
                            return options.blur.func(obj, 1);
                        } else {
                            return yw.valid.msg.custom(obj, options, options.repeatvld.msg);
                        }
                    }
                }
            }

            //如果为自定义模式
            if (options.model == 2 && options.blur.func != undefined) {
                return options.blur.func(obj, 200);
            } else {
                return yw.valid.msg.success(obj, options);
            }

        } else {

            //没填写不做任何操作
            if (options.selectvld) {
                if ($.trim(obj.val()).length == 0) {
                    if (options.model == 2 && options.blur.func != undefined) {
                        return options.blur.func(obj, 200);
                    } else {
                        return yw.valid.msg.normal(obj, options);
                    }
                }
            }

            if (!$.trim(obj.val()).length > 0) {

                if (options.model == 2 && options.blur.func != undefined) {
                    return options.blur.func(obj, 0);
                } else {
                    if (undefined == options.blur.nullmsg) {
                        var opt = { aftereId: options.aftereId, model: options.model };
                        opt.blur = {};
                        opt.blur.msg = (undefined == options.blur.nullmsg) ? "不能为空" : options.blur.nullmsg;
                        return yw.valid.msg.error(obj, opt);
                    } else {
                        return yw.valid.msg.error(obj, options);
                    }
                }
            }

            if (options.model == 2 && options.blur.func != undefined) {
                return options.blur.func(obj, 500);
            } else {
                return yw.valid.msg.error(obj, options);
            }

        }
    }

    //验证消息
    yw.valid.msg = {
        clear: function (obj) {
            obj.parent().removeClass("control-group detect");
            obj.parent().removeClass("control-group error");
            obj.parent().removeClass("control-group warning");
            obj.parent().removeClass("control-group success");
        },
        custom: function (obj, options, msg) {
            if (options.model == 0) {
                obj.css({ "border": "1px solid #ED888B", "color": "#bd4247" });
            } else {
                this._message(obj, options, msg);
                obj.parent().addClass("control-group error");
            }
            return false;
        },
        other: function (obj, options) {
            if (options.model == 0) {
                obj.css({ "border": "1px solid #ED888B", "color": "#bd4247" });
            } else {
                this._message(obj, options, options.othermsg);
                obj.parent().addClass("control-group error");
            }
            return false;
        },
        error: function (obj, options) {
            if (options.model == 0) {
                obj.css({ "border": "1px solid #ED888B", "color": "#bd4247" });
            } else {
                this._message(obj, options, options.blur.msg);
                obj.parent().addClass("control-group error");
            }
            return false;
        },
        success: function (obj, options) {
            if (options.model == 0) {
                obj.css({ "border": "1px solid #669533", "color": "#669533" });
            } else {
                this._message(obj, options, "");
                obj.parent().addClass("control-group success");
            }
            return true;
        },
        focus: function (obj, options) {
            if (options.model == 0) {
                obj.css({ "border": "1px solid #1c628b", "color": "#1c628b" });
            } else if (options.model == 2 && options.blur.func != undefined) {
                return options.focus.func(obj);
            } else {
                this._message(obj, options, options.focus.msg);
                obj.parent().addClass("control-group warning");
            }
            return true;
        },
        detect: function (obj, options, msg) {
            if (options.model == 0) {

            } else {
                this._message(obj, options, msg);
                obj.parent().addClass("control-group detect");
            }
            return true;
        },
        normal: function (obj) {
            $("#" + obj.prop("id") + "_span").remove();
            //obj.removeClass();
            return true;
        },
        _message: function (obj, options, message) {
            this.clear(obj);
            if (options.groupeId != null && options.groupeId != undefined) {
                //删除提示信息
                this.removeSpan("#" + options.groupeId + "_span");
                var span = $(this.spanmessage.replace("{0}", message)).prop("id", options.groupeId + "_span");

                var obj = $("#" + options.aftereId);
                if (!obj.length) {
                    obj = $("#" + options.aftereId, window.parent.document);
                }

                span.insertAfter(obj);
            } else {
                if (options.aftereId != null && options.aftereId != undefined) {
                    //删除提示信息
                    this.removeSpan("#" + obj.prop("id") + "_span");
                    var span = $(this.spanmessage.replace("{0}", message)).prop("id", obj.prop("id") + "_span");

                    var obj = $("#" + options.aftereId);
                    if (!obj.length) {
                        obj = $("#" + options.aftereId, window.parent.document);
                    }

                    span.insertAfter(obj);
                }
                else {
                    //删除提示信息
                    this.removeSpan("#" + obj.prop("id") + "_span");
                    var span = $(this.spanmessage.replace("{0}", message)).prop("id", obj.prop("id") + "_span");
                    span.insertAfter(obj);
                }
            }
        },
        removeSpan: function (selector) {
            var span = $(selector);
            if (!span.length) {
                try {
                    span = $(selector, window.parent.document);
                } catch (e) { }
            }
            span.remove();
        },
        //提示span
        spanmessage: "<span class='help-inline'>&nbsp;{0}</span>"
    }

    //设置组ID
    opt.groupId = "group_" + yw.valid.initcount;

    return opt;
};
