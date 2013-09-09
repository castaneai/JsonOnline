using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonOnline
{
    /// <summary>
    /// 通信メッセージに型名を添えて包んだもの
    /// 実際のネットワーク上で送受信する単位となる
    /// </summary>
    internal class Packet
    {
        /// <summary>
        /// メッセージの型名
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// メッセージの内容（JSON文字列）
        /// </summary>
        public string Data { get; set; }
    }
}
