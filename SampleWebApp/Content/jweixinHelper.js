/*
 * 说明：针对http://res.wx.qq.com/open/js/jweixin-1.0.0.js文件（微信公众号JS-SDK）的辅助开发。
 * 作者：熊学浩
 * 时间：2015-01-13
*/
wx.ready(function () {
    // config信息验证后会执行ready方法，所有接口调用都必须在config接口获得结果之后，config是一个客户端的异步操作，所以如果需要在页面加载时就调用相关接口，则须把相关接口放在ready函数中调用来确保正确执行。对于用户触发时才调用的接口，则可以直接调用，不需要放在ready函数中。

    //判断当前版本是否支持指定 JS 接口，支持批量判断
    wx.checkJsApi({
        jsApiList: [
            'onMenuShareTimeline',
            'onMenuShareAppMessage',
            'onMenuShareQQ',
            'onMenuShareWeibo',
            'onMenuShareQZone',
            'startRecord',
            'stopRecord',
            'onVoiceRecordEnd',
            'playVoice',
            'pauseVoice',
            'stopVoice',
            'onVoicePlayEnd',
            'uploadVoice',
            'downloadVoice',
            'chooseImage',
            'previewImage',
            'uploadImage',
            'downloadImage',
            'translateVoice',
            'getNetworkType',
            'openLocation',
            'getLocation',
            'hideOptionMenu',
            'showOptionMenu',
            'hideMenuItems',
            'showMenuItems',
            'hideAllNonBaseMenuItem',
            'showAllNonBaseMenuItem',
            'closeWindow',
            'scanQRCode',
            'chooseWXPay',
            'openProductSpecificView',
            'addCard',
            'chooseCard',
            'openCard'
        ],
        success: function (res) {
            if (res) {
                if (typeof (res) == "object" && res.checkResult && typeof (res.checkResult) == "object") {
                    //console.log("getNetworkType：" + res.checkResult.getNetworkType + "\n\r" + "errMsg=" + (res.errMsg || ""));
                }
                else
                    console.log(JSON.stringify(res));
            }
            else
                alert(res);
        }
    });

    if (!!shareData) {
        /* 有分享内容，可以分享 */

        //显示所有功能按钮接口
        //wx.showAllNonBaseMenuItem();

        //显示右上角菜单接口
        //wx.showOptionMenu();

        //批量隐藏功能按钮接口
        wx.hideMenuItems({
            menuList: [
				'menuItem:exposeArticle', //举报

                'menuItem:share:facebook',
                'menuItem:share:weiboApp',
                'menuItem:share:qq',
				'menuItem:share:facebook', //分享到FB
                'menuItem:share:QZone', //分享到 QQ 空间
                 '/menuItem:share:QZone',

				'menuItem:jsDebug', //调试
				//'menuItem:editTag', //编辑标签
				'menuItem:delete', //删除
                'menuItem:copyUrl',
				'menuItem:originPage', //原网页
				'menuItem:openWithQQBrowser', //在QQ浏览器中打开
				'menuItem:openWithSafari', //在Safari中打开
				'menuItem:share:email', //邮件
                'menuItem:share:brand' //一些特殊公众号
            ] // 要隐藏的菜单项，所有menu项见附录3
        });


        /*
		//微信分享信息设置
		var shareData = {
			title: '分享标题-壹学者', // 分享标题
			desc: '分享描述-壹学者', // 分享描述
			link: 'http://www.1xuezhe.exuezhe.com/', // 分享链接
			imgUrl: "http://www.1xuezhe.exuezhe.com/imagesV2/logo.png", // 分享图标
			type: '', // 分享类型,music、video或link，不填默认为link
			dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
			trigger: function (res) {
				// 监听Menu中的按钮点击时触发的方法，该方法仅支持Menu中的相关接口。
			},
			success: function (res) {
				// 接口调用成功时执行的回调函数。
				if (JWeixinHelper.isShareSuccess(res).success) {
					//分享成功
				}
			},
			fail: function (res) {
				// 接口调用失败时执行的回调函数。
			},
			cancel: function (res) {
				// 用户点击取消时的回调函数，仅部分有用户取消操作的api才会用到。
			},
			complete: function (res) {
				// 接口调用完成时执行的回调函数，无论成功或失败都会执行。
			},
		};
		*/

        //获取“分享到朋友圈”按钮点击状态及自定义分享内容接口
        wx.onMenuShareTimeline(shareData);
        //获取“分享给朋友”按钮点击状态及自定义分享内容接口
        wx.onMenuShareAppMessage(shareData);
        //获取“分享到QQ”按钮点击状态及自定义分享内容接口
        //wx.onMenuShareQQ(shareData);
        //获取“分享到腾讯微博”按钮点击状态及自定义分享内容接口
        //wx.onMenuShareWeibo(shareData);
        //获取“分享到QQ空间”按钮点击状态及自定义分享内容接口
        //wx.onMenuShareQZone(shareData);
        //获取“分享到FB”按钮点击状态及自定义分享内容接口
        ////wx.onMenuShareFacebook(shareData);
    }
    else {
        /* 分享内容缺失，不可以分享 */

        //隐藏所有非基础按钮接口
        wx.hideAllNonBaseMenuItem();

        //隐藏右上角菜单接口
        //wx.hideOptionMenu();

        //批量隐藏功能按钮接口
        //wx.hideMenuItems({
        //    menuList: [
        //        'menuItem:share:appMessage',
        //        'menuItem:share:timeline',
        //        'menuItem:share:qq',
        //        'menuItem:share:weiboApp',
        //        'menuItem:favorite',
        //        'menuItem:share:facebook',
        //        'menuItem:share:QZone',

        //        'menuItem:editTag',
        //        'menuItem:delete',
        //        'menuItem:copyUrl',
        //        'menuItem:originPage',
        //        'menuItem:openWithQQBrowser',
        //        'menuItem:openWithSafari',
        //        'menuItem:share:email',
        //        'menuItem:share:brand'
        //    ] // 要隐藏的菜单项，只能隐藏“传播类”和“保护类”按钮，所有menu项见附录3
        //});

        //批量显示功能按钮接口
        //wx.showMenuItems({
        //    menuList: [
        //		'menuItem:setFont', //调整字体
        //		'menuItem:dayMode', //日间模式
        //		'menuItem:nightMode', //夜间模式
        //		'menuItem:refresh', //刷新
        //		'menuItem:profile', //查看公众号（已添加）
        //		'menuItem:addContact', //查看公众号（未添加）

        //		//'menuItem:share:appMessage', //发送给朋友
        //		//'menuItem:share:timeline', //分享到朋友圈
        //		//'menuItem:share:qq', //分享到QQ
        //		//'menuItem:share:weiboApp', //分享到Weibo

        //		//'menuItem:favorite', //收藏
        //		//'menuItem:copyUrl', //复制链接

        //		'menuItem:readMode' //阅读模式
        //    ] // 要显示的菜单项，所有menu项见附录3
        //});


        //获取“分享到朋友圈”按钮点击状态及自定义分享内容接口
        wx.onMenuShareTimeline({
            title: '',
            link: '',
            imgUrl: '',
            trigger: function (res) {
                return false;
            }
        });
        //获取“分享给朋友”按钮点击状态及自定义分享内容接口
        wx.onMenuShareAppMessage({
            title: '',
            desc: '',
            link: '',
            imgUrl: '',
            type: '',
            dataUrl: '',
            trigger: function (res) {
                return false;
            }
        });
        //获取“分享到QQ”按钮点击状态及自定义分享内容接口
        wx.onMenuShareQQ({
            title: '',
            desc: '',
            link: '',
            imgUrl: '',
            trigger: function (res) {
                return false;
            }
        });
        //获取“分享到腾讯微博”按钮点击状态及自定义分享内容接口
        wx.onMenuShareWeibo({
            title: '',
            desc: '',
            link: '',
            imgUrl: '',
            trigger: function (res) {
                return false;
            }
        });
        //获取“分享到QQ空间”按钮点击状态及自定义分享内容接口
        wx.onMenuShareQZone({
            title: '',
            desc: '',
            link: '',
            imgUrl: '',
            trigger: function (res) {
                return false;
            }
        });
        //获取“分享到FB”按钮点击状态及自定义分享内容接口
        //wx.onMenuShareFacebook({
        //    title: '',
        //    desc: '',
        //    link: '',
        //    imgUrl: '',
        //    trigger: function (res) {
        //        return false;
        //    }
        //});
    }

});

wx.error(function (res) {
    // config信息验证失败会执行error函数，如签名过期导致验证失败，具体错误信息可以打开config的debug模式查看，也可以在返回的res参数中查看，对于SPA可以在这里更新签名。
    alert(res.errMsg);
});


/*** JWeixinHelper类库·开始 ***/
//JWeixinHelper类库
function JWeixinHelperLibrary() {
    this.openAddress = function () {
        /* https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_8&index=7 */
        wx.openAddress({
            success: function (res) {
                // 用户成功拉出地址 
            },
            cancel: function (res) {
                // 用户取消拉出地址
            },
            fail: function (res) {
                alert('fail:用户拒绝授权访问相册\n\r' + JSON.stringify(res));
            }
        });
    };
}

//微信分享成功
JWeixinHelperLibrary.prototype.isShareSuccess = function (data) {
    /*
	  在Firefox，chrome，opera，safari，ie9，ie8等高级浏览器直接可以用JSON对象的stringify()和parse()方法。
      JSON.stringify(obj)将JSON转为字符串。JSON.parse(string)将字符串转为JSON格式；
	*/
    //JSON.stringify(data);
    var results = { msg: "", success: false };
    if (data) {
        if (typeof (data) == "object" && data.errMsg) {
            switch (data.errMsg) {
                case "sendAppMsg:ok": //“分享给朋友”
                    results.msg = "sendAppMsg";
                    results.success = true;
                    break;
                case "shareTimeline:ok": //“分享到朋友圈”
                    results.msg = "shareTimeline";
                    results.success = true;
                    break;
                case "shareQQ:ok": //“分享到QQ”
                    results.msg = "shareQQ";
                    results.success = true;
                    break;
                case "shareWeibo:ok": //“分享到腾讯微博”
                    results.msg = "shareWeibo";
                    results.success = true;
                    break;
                default:
                    results.msg = data.errMsg;
                    results.success = false;
                    break;
            }
        }
        else
            results.msg = data.toString();
    }
    return results;
}

//微信扫一扫接口(微信处理结果)
JWeixinHelperLibrary.prototype.scanQRCode0 = function () {
    wx.scanQRCode();
}
//微信扫一扫接口(直接返回扫描结果)
JWeixinHelperLibrary.prototype.scanQRCode1 = function () {
    wx.scanQRCode({
        needResult: 1,
        desc: 'scanQRCode desc',
        success: function (res) {
            alert(JSON.stringify(res));
        },
        fail: function (res) {
            alert(JSON.stringify(res));
        },
        complete: function (res) {
            alert(JSON.stringify(res));
        }
    });
}

var images = {
    localId: [],
    serverId: []
};
//图像接口-拍照或从手机相册中选图接口
JWeixinHelperLibrary.prototype.chooseImage = function () {
    wx.chooseImage({
        success: function (res) {
            // 返回选定照片的本地ID列表，localId可以作为img标签的src属性显示图片
            images.localId = res.localIds;
            alert('已选择 ' + res.localIds.length + ' 张图片\n\r' + JSON.stringify(res));
        },
        cancel: function (res) {
            alert("cancel:用户拒绝授权访问相册\n\r" + JSON.stringify(res));
        },
        fail: function (res) {
            alert('fail:用户拒绝授权访问相册\n\r' + JSON.stringify(res));
        }
    });
}
//图像接口-预览图片接口
JWeixinHelperLibrary.prototype.previewImage = function (currentImageUrl, previewImageUrls) {
    var _tempImageUrls = [];
    if (typeof (previewImageUrls) == "object")
        _tempImageUrls = previewImageUrls;
    else
        _tempImageUrls = previewImageUrls.toString().split(/\s*(,|，)\s*/gi);
    var _previewImageUrls = new Array();
    for (var i = 0 ; i < _tempImageUrls.length; i++) {
        if (!!_tempImageUrls[i] && !/^\s*$/.test(_tempImageUrls[i])) {
            if (/\.(jpg|jpeg|png|gif|bmp)\s*$/ig.test(_tempImageUrls[i])) {
                _previewImageUrls.push(_tempImageUrls[i]);
            }
        }
    }
    if (!!_previewImageUrls && _previewImageUrls.length > 0) {
        currentImageUrl = (!!currentImageUrl) ? currentImageUrl : _previewImageUrls[0];
        wx.previewImage({
            current: currentImageUrl, // 当前显示的图片链接
            urls: _previewImageUrls // 需要预览的图片链接列表
        });
    }
    /*
	wx.previewImage({
      current: 'http://img5.douban.com/view/photo/photo/public/p1353993776.jpg',
      urls: [
        'http://img3.douban.com/view/photo/photo/public/p2152117150.jpg',
        'http://img5.douban.com/view/photo/photo/public/p1353993776.jpg',
        'http://img3.douban.com/view/photo/photo/public/p2152134700.jpg'
      ]
    });
	*/
}
//图像接口-上传图片接口（备注：上传图片有效期3天，可用微信多媒体接口下载图片到自己的服务器，此处获得的 serverId 即 media_id）
JWeixinHelperLibrary.prototype.uploadImage = function () {
    if (images.localId.length == 0) {
        alert('请先使用 chooseImage 接口选择图片');
        return;
    }
    var i = 0, length = images.localId.length;
    images.serverId = [];
    var timeoutUploadImage;
    function upload() {
        if (!!timeoutUploadImage)
            clearTimeout(timeoutUploadImage);
        wx.uploadImage({
            localId: images.localId[i],
            isShowProgressTips: 1, // 默认为1，显示进度提示
            success: function (res) {
                i++;
                alert('已上传：' + i + '/' + length);
                images.serverId.push(res.serverId);
                if (i < length) {
                    timeoutUploadImage = setTimeout(upload(), 500);
                    //upload();
                }
                else {
                    if (!!timeoutUploadImage)
                        clearTimeout(timeoutUploadImage);
                    //下载文件接口下载图片
                }
            },
            fail: function (res) {
                alert(JSON.stringify(res));
            },
            complete: function (res) {
                alert(JSON.stringify(res));
            }
        });
    }
    upload();
}
//图像接口-下载图片接口
JWeixinHelperLibrary.prototype.downloadImage = function () {
    if (images.serverId.length === 0) {
        alert('请先使用 uploadImage 上传图片');
        return;
    }
    var i = 0, length = images.serverId.length;
    images.localId = [];
    function download() {
        wx.downloadImage({
            serverId: images.serverId[i],
            isShowProgressTips: 1, // 默认为1，显示进度提示
            success: function (res) {
                i++;
                alert('已下载：' + i + '/' + length);
                images.localId.push(res.localId);
                if (i < length) {
                    download();
                }
            },
            fail: function (res) {
                alert(JSON.stringify(res));
            },
            complete: function (res) {
                alert(JSON.stringify(res));
            }
        });
    }
    download();
}

var voice = {
    localId: '',
    serverId: ''
};
//音频接口-开始录音接口
JWeixinHelperLibrary.prototype.startRecord = function () {
    wx.startRecord({
        success: function (res) {
            //正在录音……
        },
        cancel: function (res) {
            if (res.errMsg == "startRecord:cancel") {
                alert('用户拒绝授权录音');
            }
            else {
                alert('cancel:用户拒绝授权录音\n\r' + JSON.stringify(res));
            }
        },
        fail: function (res) {
            if (res.errMsg == "startRecord:recording") {
                JWeixinHelper.stopRecord();
            }
            else {
                alert('fail:用户拒绝授权录音\n\r' + JSON.stringify(res));
            }
        }
    });
}
//音频接口-停止录音
JWeixinHelperLibrary.prototype.stopRecord = function () {
    wx.stopRecord({
        success: function (res) {
            voice.localId = res.localId;
        },
        fail: function (res) {
            alert(JSON.stringify(res));
        }
    });
}
/*
// 监听录音自动停止
wx.onVoiceRecordEnd({
	complete: function (res) {
		voice.localId = res.localId;
		alert('录音时间已超过一分钟');
	}
});
*/
//音频接口-播放音频
JWeixinHelperLibrary.prototype.playVoice = function () {
    if (voice.localId == '') {
        alert('请先使用 startRecord 接口录制一段声音');
        return;
    }
    wx.playVoice({
        localId: voice.localId
    });
}
//音频接口-暂停播放音频
JWeixinHelperLibrary.prototype.pauseVoice = function () {
    wx.pauseVoice({
        localId: voice.localId
    });
}
//音频接口-停止播放音频
JWeixinHelperLibrary.prototype.stopVoice = function () {
    wx.stopVoice({
        localId: voice.localId
    });
}
// 监听语音播放完毕接口
wx.onVoicePlayEnd({
    success: function (res) {
        var localId = res.localId; // 返回音频的本地ID
    },
    complete: function (res) {
        alert('录音（' + res.localId + '）播放结束');
    }
});
//音频接口-上传语音接口
JWeixinHelperLibrary.prototype.uploadVoice = function () {
    if (voice.localId == '') {
        alert('请先使用 startRecord 接口录制一段声音');
        return;
    }
    wx.uploadVoice({
        localId: voice.localId, // 需要上传的音频的本地ID，由stopRecord接口获得
        isShowProgressTips: 1, // 默认为1，显示进度提示
        success: function (res) {
            alert('上传语音成功，serverId 为' + res.serverId);
            voice.serverId = res.serverId;
        },
        fail: function (res) {
            alert(JSON.stringify(res));
        },
        complete: function (res) {
            alert(JSON.stringify(res));
        }
    });
}
//音频接口-下载语音接口
JWeixinHelperLibrary.prototype.downloadVoice = function () {
    if (voice.serverId == '') {
        alert('请先使用 uploadVoice 上传声音');
        return;
    }
    wx.downloadVoice({
        serverId: voice.serverId, // 需要下载的音频的服务器端ID，由uploadVoice接口获得
        isShowProgressTips: 1, // 默认为1，显示进度提示
        success: function (res) {
            alert('下载语音成功，localId 为' + res.localId);
            voice.localId = res.localId;
        },
        fail: function (res) {
            alert(JSON.stringify(res));
        },
        complete: function (res) {
            alert(JSON.stringify(res));
        }
    });
}

//智能接口-识别音频并返回识别结果接口
JWeixinHelperLibrary.prototype.translateVoice = function () {
    if (voice.localId == '') {
        alert('请先使用 startRecord 接口录制一段声音');
        return;
    }
    wx.translateVoice({
        localId: voice.localId,
        isShowProgressTips: 1, // 默认为1，显示进度提示
        complete: function (res) {
            if (res.hasOwnProperty('translateResult')) {
                alert('识别结果：' + res.translateResult);
            } else {
                alert('无法识别');
            }
        }
    });
}

//设备信息-获取网络状态接口
JWeixinHelperLibrary.prototype.getNetworkType = function () {
    wx.getNetworkType({
        success: function (res) {
            var networkType = res.networkType; // 返回网络类型2g，3g，4g，wifi
            alert(res.networkType);
        },
        cancel: function (res) {
            alert('用户拒绝授权获取网络状态');
        },
        fail: function (res) {
            alert(JSON.stringify(res));
        }
    });
}

var UserLocation = {
    latitude: 0.0, // 纬度，浮点数，范围为90 ~ -90
    longitude: 0.0, // 经度，浮点数，范围为180 ~ -180。
    scale: 14,
    speed: 0.0, // 速度，以米/每秒计
    accuracy: 0.0, // 位置精度
    name: "",
    address: "",
    infoUrl: "http://www.1xuezhe.exuezhe.com/"
};
//地理位置-获取地理位置接口
JWeixinHelperLibrary.prototype.getLocation = function () {
    wx.getLocation({
        success: function (res) {
            UserLocation.latitude = res.latitude; // 纬度，浮点数，范围为90 ~ -90
            UserLocation.longitude = res.longitude; // 经度，浮点数，范围为180 ~ -180。
            UserLocation.speed = res.speed; // 速度，以米/每秒计
            UserLocation.accuracy = res.accuracy; // 位置精度
            alert(JSON.stringify(res));
        },
        cancel: function (res) {
            alert('用户拒绝授权获取地理位置');
        },
        fail: function (res) {
            alert(JSON.stringify(res));
        }
    });
}
//地理位置-使用微信内置地图查看位置接口
JWeixinHelperLibrary.prototype.openLocation = function () {
    if (UserLocation.latitude == 0.0 || UserLocation.longitude == 0.0) {
        alert('请先使用 getLocation接口 获取地理位置信息');
        return;
    }
    wx.openLocation({
        latitude: UserLocation.latitude, // 纬度，浮点数，范围为90 ~ -90
        longitude: UserLocation.longitude, // 经度，浮点数，范围为180 ~ -180。
        name: UserLocation.name, // 位置名
        address: UserLocation.address, // 地址详情说明
        scale: UserLocation.scale, // 地图缩放级别,整形值,范围从1~28。默认为最大
        infoUrl: UserLocation.infoUrl // 在查看位置界面底部显示的超链接,可点击跳转
    });
}

/*** JWeixinHelper类库·结束 ***/
//模拟类的实例
var JWeixinHelper = new JWeixinHelperLibrary();
if (!JWeixinHelper || typeof (JWeixinHelper) == "undefined" || JWeixinHelper == NaN) {
    var JWeixinHelper = new JWeixinHelperLibrary();
}