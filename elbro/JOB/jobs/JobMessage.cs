﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace elbro
{
    public class JobMessage : JobBase, IMessageContext
    {
        #region [ VARIABLE ]

        readonly QueueThreadSafe<Guid> ResponseIds;
        readonly DictionaryThreadSafe<Guid, Message> ResponseMessages;
        readonly DictionaryThreadSafe<Guid, Message> RequestMessages;

        public const string REQUEST_MSG_GROUP = "REQUEST_MSG_GROUP";

        readonly QueueThreadSafe<Message> Messages;

        readonly DictionaryThreadSafe<Guid, List<Guid>> RequestMessageGroup;
        readonly DictionaryThreadSafe<Guid, int> RequestMessageGroupTotal;
        readonly DictionaryThreadSafe<Guid, Func<IJobHandle, Guid, bool>> RequestMessageGroupAction
            = new DictionaryThreadSafe<Guid, Func<IJobHandle, Guid, bool>>();

        readonly ListThreadSafe<Guid> MsgTimeOutExpire;
        readonly ListDoubleThreadSafe<long, Guid> MsgTimeOut;
        readonly System.Threading.Timer timer;
        static readonly Func<List<long>, long, int[]> FUNC_TIME_OUT_FILTER = (list, valueCompare) =>
        {
            List<int> ls = new List<int>();

            return ls.ToArray();
        };

        #endregion

        public JobMessage(IJobContext jobContext) : base(jobContext, JOB_TYPE.MESSAGE)
        {
            this.MsgTimeOutExpire = new ListThreadSafe<Guid>();
            this.MsgTimeOut = new ListDoubleThreadSafe<long, Guid>();
            this.Messages = new QueueThreadSafe<Message>();
            //this.RequestMessage = man;

            this.ResponseIds = new QueueThreadSafe<Guid>();
            this.ResponseMessages = new DictionaryThreadSafe<Guid, Message>();
            this.RequestMessageGroup = new DictionaryThreadSafe<Guid, List<Guid>>();
            this.RequestMessageGroupTotal = new DictionaryThreadSafe<Guid, int>();

            timer = new System.Threading.Timer(new System.Threading.TimerCallback((obj) =>
            {
                ListDoubleThreadSafe<long, Guid> ls = (ListDoubleThreadSafe<long, Guid>)obj;
                long timeStart = DateTime.Now.Ticks / 1000;
                Guid[] ids = ls.FindItem1LessThanAndRemove(FUNC_TIME_OUT_FILTER, timeStart);
                if (ids.Length > 0)
                {
                    foreach (var t in ids)
                        Tracer.WriteLine("RESPONSE TIME_OUT: {0}", t);

                    this.MsgTimeOutExpire.AddRange(ids);
                }
            }), this.MsgTimeOut, 1000, 1000);
        }
        
        public override void f_init()
        {
            Tracer.WriteLine("J{0} JOB_MESSAGE: SIGNAL -> INITED", this.f_getId());
        }
        public override void f_processMessageCallbackResult(Guid messageId)
        {
            Thread.Sleep(1000);
        }
        public override Guid f_processMessage(Message m)
        {
            //Message m = null;
            //m = this.Messages.Dequeue(null);
            //if (m != null)
            //{
            //    //Guid id = m.GetMessageId();
            //    //if (this.MsgTimeOutExpire.Count > 0 && this.MsgTimeOutExpire.IndexOf(id) != -1)
            //    //{
            //    //    this.MsgTimeOutExpire.Remove(id);
            //    //    return;
            //    //}

            //    //this.ResponseIds.Enqueue(id);
            //    //this.ResponseMessages.Add(id, m);

            //    //Guid groupId = m.GetGroupId();
            //    //if (this.RequestMessageGroup.ContainsKey(groupId))
            //    //{
            //    //    this.RequestMessageGroup.Remove(groupId);
            //    //    if (this.RequestMessageGroup.Count == 0)
            //    //    {
            //    //        System.Tracer.WriteLine("JOB_MESSAGE:  DONE GROUP {0} = {1}", groupId, this.RequestMessageGroupTotal[groupId]);
            //    //        this.RequestMessageGroupAction[groupId](this.MsgContext, this.Handle, groupId);
            //    //    }
            //    //}
            //}
            return Guid.Empty;
        }

        #region [ IMessageContext ]

        public void f_responseMessage(Message m)
        {
            Guid id = m.GetMessageId();
            this.ResponseIds.Enqueue(id);
            this.ResponseMessages.Add(id, m);
        }

        public Message f_responseMessageGet(Guid id)
        {
            if (this.ResponseMessages.ContainsKey(id))
                return this.ResponseMessages[id];
            return null;
        }

        public void f_sendRequestMessages(JOB_TYPE type, Message[] messages)
        {
            foreach (Message m in messages)
            {
                Guid id = m.GetMessageId();
                if (m.f_getTimeOut() > 0)
                {
                    long timeOut = DateTime.Now.Ticks / 1000 + m.f_getTimeOut();
                    this.MsgTimeOut.Add(timeOut, id);
                }
                this.RequestMessages.Add(id, m);
            }
        }

        #endregion

        ~JobMessage()
        {
            this.MsgTimeOut.Clear();
            this.ResponseMessages.Clear();
            this.RequestMessageGroup.Clear();
        }
    }
}
