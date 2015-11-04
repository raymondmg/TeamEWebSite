//////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////																		//
////    			        //////////								                                        //
//////////////////////////////////////										                                //
//////////////////////////////////////	Copyright 2015, 					                                //
//////////////////////////////////////	Last vist: 								                            //
//////////////////////////////////////																		//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////

//服务器连接工具方法
(function ()
{
    // #region 工具方法

    function getFunctionName(func, notNameIsThrow)
    {
        var res = null;

        if (is.not.null(func))
            res = func.name || func.toString().match(/function\s*([^(]*)\(/)[1] || null;

        if (!res && notNameIsThrow)
            throw { "functionObject": func, "msg": "获取function名称失败" };

        return res;
    }

    function getObjectInFunctionNames(obj)
    {
        var funcs = [];
        if (obj)
        {
            for (var funcName in obj)
            {
                funcs.push(is.function(obj[funcName]) ? funcName : []);
            }
        }

        return funcs;
    }

    function getObjectInFunctions(obj)
    {
        var funcs = [];
        if (obj)
        {
            for (var funcName in obj)
            {
                var fun = obj[funcName];
                if (is.function(fun))
                    funcs.push({ "name": funcName, "function": fun });
            }
        }

        return funcs;
    }

    function getArrayObjectInFunctions(objs)
    {
        var funcs = [];
        if (objs)
        {
            for (var i = 0; i < objs.length; i++)
            {
                var item = objs[i];

                var res = is.function(item) ? [{ "name": getFunctionName(item, true), "function": item }]
                    : is.object(item) ? getObjectInFunctions(item) : [];
                Array.prototype.push.apply(funcs, res);
            }
        }

        return funcs;
    }

    function log()
    {
        if (this.debug)
        {
            if (!console || !console.log) return;

            console.log(arguments);
        }
    }

    // #endregion

    //根据连接对象名称与服务器进行连接
    function getConnection(connectionName)
    {
        return $.connection[connectionName];
    }

    //注册方法到服务器
    function registerFunctionToServer(conn, funcName, func)
    {
        if (conn && funcName && is.function(func))
            conn.client[funcName] = func;
    }

    //注册方法到服务器
    function registerFunctionToServerByFunc(conn, func)
    {
        if (conn && func)
        {
            var regFuns = is.function(func) ? [{ "name": getFunctionName(func, true), "function": func }] :
                is.array(func) ? getArrayObjectInFunctions(func) :
                is.object(func) ? getObjectInFunctions(func) : [];

            for (var i = 0; i < regFuns.length; i++)
            {
                var funcInfo = regFuns[i];
                if (funcInfo.name)
                    registerFunctionToServer(conn, funcInfo.name, funcInfo.function);
                else
                    throw "registerFunctionToServerByFunc:没有获取到参数func的方法名";
        }
        }
    }

    //获取服务器的方法
    function getServerFunctionNames(conn)
    {
        var res = (conn && conn.server) ? getObjectInFunctionNames(conn.server) : [];
        return res;
    }

    //获取服务器的方法
    function getServerFunction(conn)
    {
        var res = (conn && conn.server) ? getObjectInFunctions(conn.server) : [];

        return res;
        }

    //根据方法名称获取服务器的方法
    function getServerFunctionByFuncName(conn, funcName)
    {
        if (!conn || !funcName) return function () { };

        var func = conn.server[funcName];
        if (is.function(func))
            return func;

        return function () { };
    }

    function getClientFunction(conn)
    {
        var res = (conn && conn.client) ? getObjectInFunctions(conn.client) : [];

        return res;
    }

    function getClientFunctionNames(conn)
    {
        var res = (conn && conn.client) ? getObjectInFunctionNames(conn.client) : [];
        return res;
    }

    //连接服务器
    function start(successfunc)
    {
        $.connection.hub.start().fail(function (res) { console.log("connection start fail!", res); })
        .done(function (res)
        {
            //连接成功
            console.log("connection start success!");
            //连接成功回调
            if (is.function(successfunc)) successfunc();
        });
    }

    //聊天客户端
    function wppChatClient(connectionName)
    {
        var self = this;
        this.debug = false;
        this._chatClient = getConnection(connectionName);
        this.registerFunctionToServer = function (funcName, func)
        {
            registerFunctionToServer(this._chatClient, funcName, func);
            return this;
        };
        this.registerFunctionToServerByFunc = function (func)
        {
            registerFunctionToServerByFunc(this._chatClient, func);
            return this;
        };
        this.getServerFunctionNames = function ()
        {
            var res = getServerFunctionNames(this._chatClient);
            return res;
        };
        this.getServerFunction = function ()
        {
            var res = getServerFunction(this._chatClient);
            return res;
        };
        this.getClientFunctionNames = function ()
        {
            var res = getClientFunctionNames(this._chatClient);
            return res;
        };
        this.getClientFunction = function ()
        {
            var res = getClientFunction(this._chatClient);
            return res;
        };
        //根据服务器方法名称获取服务器方法
        this.invokeServerFunction = function (funcName)
        {
            return getServerFunctionByFuncName(this._chatClient, funcName);
        };
        this.invokeDirectServerFunction = function (funcName)
        {
            return getServerFunctionByFuncName(this._chatClient, funcName).apply(this._chatClient.server, arguments);
        };
        this.invoke = this.invokeServerFunction;
        this.start = start;
        this.log = log;
        return this;
    }

    window.wppChatClient = wppChatClient;
})();


//WPP 聊天系统初始化
var chat= {};

chat._init = function ()
{
    //当异步读取完SignalR的类库后执行
    var initChat = $.Deferred();
    $.when($.ajax("../Scripts/jquery.signalR-2.2.0.min.js", 'script'))
    .then(function ()
    {
        $.when($.ajax("../signalr/hubs", 'script')).then(function ()
        {
           
            //设置聊天面板的客户端对象
// ReSharper disable once InconsistentNaming
            var client = chat.client = new window.wppChatClient('TeamEChatClient');

            client.debug = true;
            
            //取得已经发生的所有聊天信息内容
            chat.getList = function (userTime, userList)
            {
                //显示当前对应的时间段内容,不需要传送告知服务器
                chatConfig.addTimeLabel(userTime, false);
                //将当前时间段内对应的内容显示
                $.each(userList, function (index, data)
                {
                    chatConfig.UserAcceptMsg(data.image, data.message);
                });
            };

            //初始化未读信息
            chat.initUnReadMsg = function (unReadCount)
            {
               chatConfig.updateUnReadMsgCount(unReadCount);
            }

            //消息是否需要加上前置时间标记
            chat.AddPreTimeLabel = function ()
            {
               chatConfig.ChatPanelAddPreTimeLabel();
            }

            ////聊天版聊天信息监听
            chat.sendAllMessge = function (message, imgSrc)
            {
                chatConfig.UserAcceptMsg(imgSrc, message);
            }

            ////服务器告知客户端信息发送成功
            chat.OwnSendMsgSuccess = function (canSend, havePreTime, _img, _info)
            {
                //alert("receive");
                console.log("是否可以发送" + canSend + "是否添加前置时间：" + havePreTime + "头像:" + _img + "信息" + _info);
                //用户可以将自己发送的信息添加到面板上
                chatConfig.UserCanSendMsg(canSend, havePreTime, _img, _info);
            }
   
            //获取所有聊天信息内容方法注册
            client.registerFunctionToServer("getList", function (userTime, userList)
            {
                console.log("获取到已有的聊天信息内容。。。");
                chat.getList(userTime, userList);
            });

            //显示初始化的聊天信息未读数目方法注册
            client.registerFunctionToServer("initUnReadMsg", function (unReadCount)
            {
                console.log("获取到已有的聊天信息未读数目。。。");
                chat.initUnReadMsg(unReadCount);
            });

            //监听消息是否需要加上前置时间标记注册
            client.registerFunctionToServer("AddPreTimeLabel", function ()
            {
                console.log("获取到时间标记。。。");
                chat.AddPreTimeLabel();
            });

            //发送消息给所有人
            client.registerFunctionToServer("sendAllMessge", function (message, imgSrc)
            {
                console.log("获取到给所有人的发送信息。。。");
                chat.sendAllMessge(message, imgSrc);
            });

            //客户端信息发送成功
            client.registerFunctionToServer("OwnSendMsgSuccess", function (canSend, havePreTime, _img, _info)
            {
                console.log("获取客户端信息发送成功。。。");
                chat.OwnSendMsgSuccess(canSend, havePreTime, _img, _info);
            });

            

            //服务器连接成功
            client.start(function ()
            {
                //初始化聊天界面状态
                chatConfig.initChatPanelState();
                //连接服务器，初始化信息
                chatConfig.ConnectToServer();
                //监听聊天面板状态
                chatPanelConfig.RegisterChatPanelListener();
         
            });
        });
    });


};

//聊天变量
var chatPublicVariables =
    {
        //当前滑动窗口top
        currentScroTop: 0,
        //当前窗口状态
        currentState: "chat",
        //当前登录浏览器的用户ID
        userID: "",
        //信息
        result: "",
        //右侧面板当前状态   打开/关闭
        RightMenuState: false,
        //当前浏览器用户未读信息数目
        CurrentUnReadMsgCount: 0,
        //当前浏览器客户端用户头像
        userImg: ""
    }


//聊天配置
var chatConfig =
    {
        //用户聊天数据类型绑定
        UserChatData: function (img, info)
        {
            this.imgSrc = img;
            this.userInfo = info;
        },
        //初始化界面显示状态
        initChatPanelState: function ()
        {
            //TODO显示当前日期
            $("#_spanDate").text("星期" + new Date().getDay().toString());
            //设定初始化时处于用户聊天面板
            chatPanelConfig.changeSelectedState("chat");
        },
        //浏览器初始化服务器信息
        ConnectToServer: function ()
        {
            //获取用户输入登录名
            while (chatPublicVariables.userID.length == 0) {
                //chatPublicVariables.userID = window.prompt("Test:请输入登录名称");
                chatPublicVariables.userID = "testID";
            }
            //随机生成一个用户头像
            chatPublicVariables.userImg = chatPanelTools.RandomUserImg();
            //将当前登录用户注册到服务器
            chat.client.invokeServerFunction("userConnected")(chatPublicVariables.userID, chatPublicVariables.userImg);
        },

        //获取并添加当前时间到聊天面板上
        addTimeLabel: function (time, sendToServer)
        {
            //获取显示聊天信息的层
            var $rootDiv = $("#wpp_ui_chat-messages");
            var timeLabel = $("<label>");
            var timeSpan = $("<span>");
            //当前显示的时间
            timeSpan.text(time);
            //将该时间显示在用户页面之中
            timeSpan.appendTo(timeLabel);
            timeLabel.appendTo($rootDiv);
            //是否将当前内容传给服务器
            if (sendToServer)
            {
                //将添加的时间span内容告知服务器
                chat.client.invokeServerFunction("setPreTimeLabel")(time.toString());
            }
        },

        //用户接收到他人发送的信息
        UserAcceptMsg: function (imgSrc, sendInfo)
        {
            //聊天数据绑定
            var data = new chatConfig.UserChatData(imgSrc, sendInfo);
            chatConfig.loadChatInfo(data, false);
            //更新面板显示
            chatPanelTools.updateScrollTop();
            //统计量显示
            //若用户当前未打开右侧面板  ，统计量增加
            chatConfig.UnReadMsgHandler();
        },

        //读取信息
        loadChatInfo: function (data, right)
        {
            //数据存在
            if (data)
            {

                //获取显示聊天信息的层
                var $rootDiv = $("#wpp_ui_chat-messages");
                var msgDiv = $("<div>");

                if (right)
                {
                    msgDiv.addClass("message right");
                }
                else
                {
                    //若生成的是其他用户的信息，则要绑定点击事件
                    msgDiv.addClass("message");

                }

                msgDiv.appendTo($rootDiv);

                //添加头像和个人信息    
                var UserImg = $("<img>");
                UserImg.attr("src", data.imgSrc);
                UserImg.appendTo(msgDiv);

                var UserInfo = $("<div>");
                UserInfo.addClass("bubble");
                UserInfo.text(data.userInfo)
                UserInfo.appendTo(msgDiv);

                var Corner = $("<div>");
                Corner.addClass("corner");
                Corner.appendTo(UserInfo);


                //var ChatPublishTime = $("<span>");

                ////显示发表的时间
                ////var PublishTime = new Date();
                ////var _pbTime = PublishTime.toLocaleString();
                //ChatPublishTime.text("Now");
                //ChatPublishTime.appendTo(UserInfo);
            }
        },

        //未读信息处理
        UnReadMsgHandler: function ()
        {
            //若右侧面板打开，则将未读消息清空，并更新服务器数据
            if (chatPublicVariables.RightMenuState)
            {
                //清空未读的数目
                chatConfig.updateUnReadMsgCount(0);
            }
            else
            {
                //当右侧面板未打开，每当有一条数据传输来就将未读信息++
                chatConfig.updateUnReadMsgCount(++chatPublicVariables.CurrentUnReadMsgCount);
            }
        },

        //更新未读数目  同时更新服务器上的数据
        updateUnReadMsgCount: function (unRead)
        {
            chatPublicVariables.CurrentUnReadMsgCount = unRead;
        },
     
        //给聊天面板上添加上用户发话的前置时间
        ChatPanelAddPreTimeLabel: function ()
        {
            var myDate = new Date();
            var _preTime = myDate.toLocaleTimeString();
            chatConfig.addTimeLabel(_preTime, true);
        },

        //清除当前浏览器客户端的所有聊天信息
        ClearClientChatMsg: function ()
        {
            $("#wpp_ui_userChatPanel #wpp_ui_chat-messages").html("");
        },
        //当前用户发送信息
        UserSendMsg: function (imgSrc, sendInfo)
        {
            console.log("用户头像" + imgSrc + "用户信息" + sendInfo);
            //alert(imgSrc.toString());
            //向服务器发送请求，检测自己是否可以发送信息
            chat.client.invokeServerFunction("judgeCanMeSendMsg")(imgSrc.toString(), sendInfo.toString());
        },

        //服务器告知客户端是否可以发送信息(以及获取是否含有时间)
        UserCanSendMsg: function (canSend, havePreTime, imgSrc, sendInfo)
        {
            console.log("是否可以发送显示" + canSend);
            if (canSend)
            {
                console.log("可以发送当前信息");
                //可以
                if (havePreTime)
                {
                    chatConfig.ChatPanelAddPreTimeLabel();
                }
                //添加自己的信息到面板上
                var data = new chatConfig.UserChatData(imgSrc, sendInfo);
                console.log("用户可以发送当前信息，头像：" + data.imgSrc + "信息是:" + data.userInfo);
                chatConfig.loadChatInfo(data, true);
                //更新面板显示
                chatPanelTools.updateScrollTop();
                //服务器广播所有人发送的信息
                chat.client.invokeServerFunction("sendAllMessage")(chatPublicVariables.result);
            }
            else
            {
                return;
            }
        }

    };

//聊天面板显示配置
var chatPanelConfig =
    {
        //显示聊天信息
        //切换到用户选择状态
        changeSelectedState: function (state)
        {
            switch (state)
            {
                case "chat":
                    chatPublicVariables.currentState = "chat";
                    chatPanelConfig.FriendNormal();
                    chatPanelConfig.ChatActive();
                    break;
                case "friend":
                    chatPublicVariables.currentState = "friend";
                    chatPanelConfig.ChatNormal();
                    chatPanelConfig.FriendActive();
                    break;
            }
        },

        //聊天面板隐藏
        ChatPanelHide: function ()
        {

        },

        //聊天面板显示
        ChatPanelShow: function ()
        {
            //添加animate类使得聊天面板可见
            //$("chat-messages")
        },

        //朋友面板隐藏
        FriendPanelHide: function ()
        {

        },

        //朋友面板显示
        FriendPanelShow: function ()
        {

        },

        //聊天选项激活
        ChatActive: function ()
        {
            if ($("#wpp_ui_topmenu_chat").hasClass("chats"))
            {
                $("#wpp_ui_topmenu_chat").removeClass("chats");
            }
            if (!$("#wpp_ui_topmenu_chat").hasClass("chatsActive"))
            {
                $("#wpp_ui_topmenu_chat").addClass("chatsActive");
            }

            //两个面板切换
            $('#friends').fadeOut(100, function ()
            {
                $("#wpp_ui_userChatPanel").addClass("animate");
                $('#wpp_ui_userChatPanel').fadeIn();
            });
        },

        //聊天选项正常显示
        ChatNormal: function ()
        {
            if ($("#wpp_ui_topmenu_chat").hasClass("chatsActive"))
            {
                $("#wpp_ui_topmenu_chat").removeClass("chatsActive");
            }
            if (!$("#wpp_ui_topmenu_chat").hasClass("chats"))
            {
                $("#wpp_ui_topmenu_chat").addClass("chats");
            }
        },

        //好友选项激活
        FriendActive: function ()
        {
            if ($("#wpp_ui_topmenu_friends").hasClass("friends"))
            {
                $("#wpp_ui_topmenu_friends").removeClass("friends");
            }
            if (!$("#wpp_ui_topmenu_friends").hasClass("friendsActive"))
            {
                $("#wpp_ui_topmenu_friends").addClass("friendsActive");
            }

            //面板切换
            $("#wpp_ui_userChatPanel").removeClass("animate");

            setTimeout(function ()
            {
                //两个面板切换
                $('#wpp_ui_userChatPanel').fadeOut();
                $('#wpp_ui_friends').fadeIn()
            }, 50);
        },

        //好友选项正常显示
        FriendNormal: function ()
        {
            if ($("#wpp_ui_topmenu_friends").hasClass("friendsActive"))
            {
                $("#wpp_ui_topmenu_friends").removeClass("friendsActive");
            }
            if (!$("#wpp_ui_topmenu_friends").hasClass("friends"))
            {
                $("#wpp_ui_topmenu_friends").addClass("friends");
            }
        },

        //注册聊天面板监听
        RegisterChatPanelListener: function ()
        {
            //控制滑动
            // 打开右侧面板    右侧面板打开，如果有未读消息，则清空
            $("#wpp_ui_chatbtn").on('click', function (event)
            {
                event.preventDefault();
                event.stopPropagation();
                //根据面板状态显示未读数目
                switch (es.chatpanel.state)
                {
                    case 'hide':
                        chatPublicVariables.RightMenuState = false;
                        break;
                    case 'show':
                        //打开时进行右侧未读消息处理
                        chatPublicVariables.RightMenuState = true;
                        $("#wpp_ui_chat-messages").scrollTop(chatPublicVariables.currentScroTop);
                        chatConfig.UnReadMsgHandler();
                        break;
                }

            });

            //发送信息：聚焦
            $('#wpp_ui_sendmessage input').focus(function ()
            {
                if ($(this).val() == '此处填写信息...')
                {
                    $(this).val('');
                }
            });
            //发送信息：退出聚焦
            $('#wpp_ui_sendmessage input').focusout(function ()
            {
                if ($(this).val() == '')
                {
                    $(this).val('此处填写信息...');
                }
            });


            //发送按钮点击事件
            $("#wpp_ui_btn_user_send").click(function ()
            {
                chatPanelEventListener.sendMsgEvent();

            });

            //点击切换到朋友列表
            $("#topmenu_friends").click(function ()
            {
                if (chatPublicVariables.currentState == "friend") return;
                //切换状态以及面板
                chatPanelConfig.changeSelectedState("friend");
            });

            //点击切换到聊天室面板
            //聊天室面板点击
            $('#topmenu_chat').click(function ()
            {
                if (chatPublicVariables.currentState == "chat") return;
                //切换状态
                chatPanelConfig.changeSelectedState("chat");

            });

            $(document).on('keyup', function (e) {
                if (e.keyCode === 13) {
                    chatPanelEventListener.sendMsgEvent();
                }
            });
        }
    }


//聊天面板工具方法
var chatPanelTools =
    {
        //测试头像使用
        //temp随机生成用户的头像
        RandomUserImg: function ()
        {
            var imgSrc = "../assets/images/ChatPanel/" + chatPanelTools.GetRandomNum(1, 6).toString() + ".jpg";
            return imgSrc;
        },

        //获取区间内的随机数
        GetRandomNum: function (Min, Max)
        {
            var Range = Max - Min;
            var Rand = Math.random();
            return (Min + Math.round(Rand * Range));
        },
        //更新scrolltop
        updateScrollTop: function ()
        {
            chatPublicVariables.currentScroTop = chatPublicVariables.currentScroTop + 90;
            $("#wpp_ui_chat-messages").scrollTop(chatPublicVariables.currentScroTop);
        }
    }

var chatPanelEventListener =
{
    sendMsgEvent: function ()
    {
        //alert("send");
        //获取用户输入的信息
        chatPublicVariables.result = $("#wpp_ui_i_user_sendmsg").val();
        console.log("用户输入" + chatPublicVariables.result);
        //若用户没有输入任何信息，提示
        if (chatPublicVariables.result == "" || chatPublicVariables.result == "此处填写信息...") {
            alert("请输入发送信息！");
            return;
        }
        //将该条信息添加到用户聊天面板上
        chatConfig.UserSendMsg(chatPublicVariables.userImg, chatPublicVariables.result);
        //清空输入框信息
        $("#wpp_ui_i_user_sendmsg").val("");
    }
}

var jiathis_config = {
    url: "http://dev.dwechat.com/src/index.html?e=AdobeEdgeProject/publish/web/AdobeEdgeProject",
    summary: "from www.digi-tiger.com",
    title: "digitiger"
}

///*********************************
// **根据当前用户屏幕进行聊天窗体调整
// **********************************/
////当窗口大小变化时
//window.onresize = onWindowResize();
////根据浏览器窗口大小对聊天面板进行设置
//function onWindowResize() {
//    var height = (document.body.clientHeight - 191).toString() + "px";
//    $("#wpp_ui_chat-messages").css("height", height);
//}

