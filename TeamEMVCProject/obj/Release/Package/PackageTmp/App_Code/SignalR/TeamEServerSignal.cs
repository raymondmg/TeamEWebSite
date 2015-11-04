using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using TeamEMVCProject.Models;

namespace TeamEServerSignal
{
    /// <summary>
    /// 浏览器服务器端
    /// </summary>
    [HubName("TeamEChatClient")]
    public class TeamEServerSignal : Hub
    {
        /// <summary>
        /// 已经连接在线的用户ID   每个用户ID对应未读消息数目
        /// </summary>
        public static class CustomerHandler
        {
            //所有用户的ID信息 
            public static Dictionary<string, Customer> ConnectedUserId = new Dictionary<string, Customer>();
        }

        /// <summary>
        /// 所有聊天内容   包含每个时间段的聊天内容  
        /// </summary>
        public static class ChatContantHandler
        {
            //用户对话信息列表   key-发表时间   value-一段时间内所有用户对话信息
            public static Dictionary<string, List<ChatContent>> ChatAllInfo = new Dictionary<string, List<ChatContent>>();
            //用户对话内容的临时存储列表
            public static List<ChatContent> tempChatInfo = new List<ChatContent>();
            //设置时间间隔为60秒
            public static System.Timers.Timer t = new System.Timers.Timer(60000);
            //用户是否有人说话
            public static bool IsUserSaySth = false;
            //上次用户说话数量
            private static long LastChatInfoCount = 0;
            //当前时间段的标签内容
            public static string preTime;

            /// <summary>
            /// 清除当前服务器中所有聊天信息
            /// </summary>
            public static void ClearAllChatInfo()
            {
                if (ChatAllInfo.Count == 0) return;

                ChatAllInfo.Clear();
            }

            /// <summary>
            /// 是否发送信息前置时间处理
            /// </summary>
            public static bool MsgTimeHandler()
            {
                //判断是否需要在发送信息前添加时间
                if (NeedAddTimeBeforeMsg())
                {
                    //需要添加
                    //用户说话=true
                    IsUserSaySth = true;
                    //向所有客户端传送时间标记
                    return true;
                }
                return false;
                //不需要添加，返回
            }

            /// <summary>
            /// 是否需要在信息前添加时间
            /// </summary>
            /// <returns></returns>
            private static bool NeedAddTimeBeforeMsg()
            {
                //若用户一直没有说话，那么返回需要在信息前添加时间
                return !IsUserSaySth;
            }

            /// <summary>
            /// 间隔时间到触发该事件
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public static void Timer_TimesUp(object sender, System.Timers.ElapsedEventArgs e)
            {
                //60s到，进行检测是否有用户发言
                //当前存储对话信息数量不是上次存储对话信息数量
                if (ChatAllInfo.Count != LastChatInfoCount)
                {
                    //有用户发言
                    IsUserSaySth = true;
                    //将上次存储对话信息数量更新
                    LastChatInfoCount = ChatAllInfo.Count;
                }
                else
                {
                    //对话信息数量一致
                    //没有用户说过话
                    IsUserSaySth = false;
                }
            }

            /// <summary>
            /// 设置当前时间段标签
            /// </summary>
            /// <param name="sptl"></param>
            public static void setPreTimeLabel(string sptl)
            {
                if (sptl == null) return;

                preTime = sptl;
                //时间标签设置时，将该标签作为键存入所有聊天内容中
                //直接将临时对话列表中的内容添加进来
                ChatAllInfo.Add(preTime, tempChatInfo);
                //清空临时列表等待下次新的时间标签传来
                tempChatInfo.Clear();
                tempChatInfo = new List<ChatContent>();
            }
        }

        /// <summary>
        /// 用户进入页面时进行与服务器的链接操作
        /// </summary>
        /// <param name="name"></param>
        public void userConnected(string name, string imgSrc)
        {
            //当有用户连接进入时且当前聊天室并没有用户，开启显示前置时间器
            if (CustomerHandler.ConnectedUserId.Count == 0)
            {
                //初始化设置
                ChatContantHandler.t.Elapsed += new System.Timers.ElapsedEventHandler(ChatContantHandler.Timer_TimesUp);//每隔设定的时间激活设定函数
                ChatContantHandler.t.AutoReset = true;  //设置每隔固定秒数进行激活
                //开启计时器
                ChatContantHandler.t.Enabled = true; //是否触发Elapsed事件
                ChatContantHandler.t.Start();
                //当有用户新进入的时候重置显示时间列表
                ChatContantHandler.IsUserSaySth = false;
            }

            //编码
            name = HttpUtility.HtmlEncode(name);
            imgSrc = HttpUtility.HtmlEncode(imgSrc);
            //string message = "用户 " + name + " 登录";
            //用户初始化
            Customer u = new Customer();
            u.Name = name;
            u.Picture = imgSrc;

            //发送信息给其他人
            //Clients.Others.addList(Context.ConnectionId, name);
            //Clients.Others.hello(message);

            //检测是否已经有聊天内容
            if (ChatContantHandler.ChatAllInfo.Count != 0)
            {
                //循环获取已有的聊天时间段，将每个时间段以及时间段对应的信息发送给当前客户端
                foreach (var t in ChatContantHandler.ChatAllInfo)
                {
                    Clients.Caller.getList(t.Key, t.Value.Select(p => new { message = p.Content, image = p.imgSrc }).ToList());
                    //未读信息数目增加
                    u.UnReadMsgCount += t.Value.Count;
                }
            }

            //新增目前使用者上线清单
            CustomerHandler.ConnectedUserId.Add(Context.ConnectionId, u);
            //告知客户端当前客户端有多少信息未读
            Clients.Caller.initUnReadMsg(u.UnReadMsgCount);

        }


        /// <summary>
        /// 发送消息给所有人
        /// </summary>
        /// <param name="message">信息</param>
        public void sendAllMessage(string message)
        {
            //信息以及头像
            message = HttpUtility.HtmlEncode(message);

            var img = CustomerHandler.ConnectedUserId[Context.ConnectionId].Picture;
            //处理Msg，如果在时间间隔1min中之内没有人说话，那么之后的发话人的内容前将会添加上当前发送消息的时间
            if (ChatContantHandler.MsgTimeHandler())
            {
                //告知其他客户端添加上一个前置的标记
                Clients.Others.AddPreTimeLabel();
                //通知客户端后，等待客户端回传时间信息才可以记录，因此先自动记录当前丢失的信息
                ChatContantHandler.tempChatInfo.Add(new ChatContent(message, img));
            }

            //有当前的时间标签
            if (ChatContantHandler.preTime != null || ChatContantHandler.preTime != "")
            {
                //将持续对话的内容不断添加在此时时间标签下的临时列表之中
                ChatContantHandler.ChatAllInfo[ChatContantHandler.preTime].Add(new ChatContent(message, img));
            }

            //广播给他人
            Clients.Others.sendAllMessge(message, img);
        }

        /// <summary>
        /// 客户端告知服务器端当前时间段的标签内容
        /// </summary>
        public void setPreTimeLabel(string _lb)
        {
            ChatContantHandler.setPreTimeLabel(_lb);
        }

        /// <summary>
        /// 客户端向服务器请求自己信息是否发送出去
        /// </summary>
        public void judgeCanMeSendMsg(string imgSrc, string sendInfo)
        {
            var canSend = true;
            var havePreTime = !ChatContantHandler.IsUserSaySth;
            var _img = imgSrc;
            var _info = sendInfo;
            //服务器收到信息并进行返回信息操作
            Clients.Caller.OwnSendMsgSuccess(canSend, havePreTime, _img, _info);
        }

        /// <summary>
        /// 更新服务器上的未读信息数量
        /// </summary>
        /// <param name="count"></param>
        public void UpdateServerUnReadMsg(int count)
        {
            //获取到当前发送消息的用户并设置其新的数量
            CustomerHandler.ConnectedUserId[Context.ConnectionId].UnReadMsgCount = count;
        }


        //发送信息给特定人
        public void sendMessage(string ToId, string message)
        {
            message = HttpUtility.HtmlEncode(message);
            var fromName = CustomerHandler.ConnectedUserId.Where(p => p.Key == Context.ConnectionId).FirstOrDefault().Value;
            message = fromName + " <span style='color:red'>悄悄对你说</span>：" + message;
            Clients.Client(ToId).sendMessage(message);
        }

        //当使用者断线时执行
        public override Task OnDisconnected(bool stopCalled)
        {
            //当使用者离开时，移除在清单内的ConnectionId
            Clients.All.removeList(Context.ConnectionId);
            CustomerHandler.ConnectedUserId.Remove(Context.ConnectionId);

            //移除使用者后，若当前列表中已经没有使用者，则停止显示前置时间计时器
            if (CustomerHandler.ConnectedUserId.Count == 0)
            {
                ChatContantHandler.t.Stop();
            }
            return base.OnDisconnected(stopCalled);


        }

    }
}