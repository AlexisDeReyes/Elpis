﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kayak;
using Kayak.Http;

namespace Kayak.Tests.Net
{
    class SocketDelegate : ISocketDelegate
    {
        //public Action OnTimeoutAction;
        public Action<Exception> OnErrorAction = null;
        public Action OnEndAction = null;
        public Func<ArraySegment<byte>, Action, bool> OnDataAction = null;
        public Action OnConnectedAction = null;
        public Action OnCloseAction = null;

        public Exception Exception;
        public int NumOnConnectedEvents;
        public bool GotOnEnd;
        public bool GotOnClose;

        public DataBuffer Buffer;

        public SocketDelegate()
        {
            Buffer = new DataBuffer();
        }

        //void OnTimeout(IServer server)
        //{
        //    if (OnTimeoutAction != null)
        //        OnTimeoutAction();
        //}

        public void OnError(ISocket socket, Exception e)
        {
            Exception = e;
            if (OnErrorAction != null)
                OnErrorAction(e);
        }

        public void OnEnd(ISocket socket)
        {
            if (GotOnEnd)
                throw new Exception("Socket delegate previously got OnEnd");

            GotOnEnd = true;

            if (OnEndAction != null)
                OnEndAction();
        }

        public bool OnData(ISocket server, ArraySegment<byte> data, Action continuation)
        {
            Buffer.Add(data);

            if (OnDataAction != null)
                return OnDataAction(data, continuation);

            return false;
        }


        public void OnConnected(ISocket socket)
        {
            NumOnConnectedEvents++;
            if (OnConnectedAction != null)
                OnConnectedAction();
        }

        public void OnClose(ISocket socket)
        {
            if (GotOnClose)
                throw new Exception("Socket delegate previously got OnClose");

            GotOnClose = true;

            if (OnCloseAction != null)
                OnCloseAction();
        }
    }
}
