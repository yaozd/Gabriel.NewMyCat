﻿using System;
using System.Text;
using Gabriel.NewMyCat.Message;
using Gabriel.NewMyCat.Util;

namespace Gabriel.NewMyCat
{
    /// <summary>
    /// 信息打印
    /// </summary>
    public class MessagePrint
    {
        public static string PlainTextMessage(Context ctx)
        {
            var item = ctx.Transaction;
            var text = new StringBuilder();
            text.AppendLine();
            text.AppendLine("Guid:"+ctx.Guid);
            text.AppendFormat("根节点[是否异常:{6}]：事务名称={0}({5}ms)；类型={1}；分类={2}；开始时间={3:yyyy-MM-dd hh:mm:ss ffff}；结束时间={4:yyyy-MM-dd hh:mm:ss ffff}", 
                item.Name, 
                item.Type, 
                item.Category,
                ctx.BeginTime, 
                ctx.EndTime,
                ctx.TimeSpanInMilliseconds,
                ctx.IsException);
            text.AppendLine(PlainTextTransaction(ctx.Transaction, 0));
            return text.ToString();
        }

        private static string PlainTextTransaction(Transaction t, int n)
        {
            n++; //记录数据的层次
            var text = "";
            var countE = t.Evens.Count;
            var currentE = t.Evens;
            for (int i = 0; i < countE; i++)
            {
                var item = currentE[i];
                var textFormat= 
                    item.BeginOrEnd? string.Format("**E**名称={0}：", item.Name):
                    string.Format("**E**名称={0}({3}ms)；描述信息={1}；时间={2:HH:mm:ss ffff}。", item.Name, item.Description, item.Time, item.TimeSpanInMilliseconds);
                var evenText = PlainTextSpace(n) + textFormat;
                text = text + evenText + PlainTextEven(currentE[i], n);
            }
            return text;
        }

        private static string PlainTextEven(Even e, int n)
        {
            n++;
            var text = "";
            var countT = e.Transactions.Count;
            var currentT = e.Transactions;
            for (int i = 0; i < countT; i++)
            {
                var item = currentT[i];
                var textTransaction = PlainTextSpace(n) +
                                      string.Format("--T--名称={0}({4}ms)；类型={1}；分类={2}；时间={3:HH:mm:ss ffff}。", item.Name, item.Type,item.Category, item.Time, item.TimeSpanInMilliseconds);
                text = text + textTransaction + PlainTextTransaction(item, n);
            }
            return text;
        }

        private static string PlainTextSpace(int n)
        {
            return Environment.NewLine + "".PadLeft(n*5, ' ');
        }
    }
}