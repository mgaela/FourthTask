using FourthTask.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FourthTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var Program = new Program();
            Program.StoryStart();
            Console.ReadLine();
        }
        #region Data
        private string[] QiaoFengEvents =
        {
            "丐帮帮主","契丹人","南宫大王","挂印离开"
        };
        private string[] XuZhuEvents =
        {
            "小和尚","逍遥掌门","灵鹫宫宫主","西夏驸马"
        };
        private string[] DuanYuEvents =
        {
            "钟灵儿","木婉清","王语嫣","大理国王"
        };
        private bool HasFirst = false;
        private bool HasEnd = false;
        private List<Task> PlotList = new List<Task>();
        private void StoryStart()
        {
            SerializeHelper serializeHelper = new SerializeHelper();
            var Lists= serializeHelper.XmlSerialize();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            TaskFactory Story = new TaskFactory();
            CancellationTokenSource cts = new CancellationTokenSource();//bool值 
            foreach (var item in Lists)
            {
                PlotList.Add(Story.StartNew(() =>
                {
                    var i = 0;
                    foreach (var plot in item.Plot.Split(','))
                    {
                        Thread.Sleep(new RandomHelper().GetRandomNumber(1500, 2500));//随机休息一下
                        if (cts.IsCancellationRequested)
                        {
                            return;
                        }
                        i++;
                        Console.WriteLine(item.Name + plot);
                        if (i == 1 && !HasFirst)
                        {
                            HasFirst = true;
                            Console.WriteLine("天龙八部就此拉开序幕");
                        }
                    }
                    if (!HasEnd && !cts.IsCancellationRequested)
                    {
                        HasEnd = true;
                        Console.WriteLine(item.Name,"已经做好准备了。。。。。");
                    }
                }));
            }
            //PlotList.Add(Story.StartNew(()=>
            //{
            //    var i = 0;
            //    foreach (var QiaoFengEvent in QiaoFengEvents)
            //    {
            //        Thread.Sleep(new RandomHelper().GetRandomNumber(1500, 2500));//随机休息一下
            //        if (cts.IsCancellationRequested)
            //        {
            //            return;
            //        }
            //        i++;
            //        Console.WriteLine("乔峰:"+QiaoFengEvent);
            //        if (i==1&& !HasFirst)
            //        {
            //            HasFirst = true;
            //            Console.WriteLine("天龙八部就此拉开序幕");
            //        }
            //    }
            //    if (!HasEnd&& !cts.IsCancellationRequested)
            //    {
            //        HasEnd = true;
            //        Console.WriteLine("乔某已经做好准备了。。。。。");
            //    }
                
            //}, cts.Token));
            //PlotList.Add(Story.StartNew(() =>
            //{
            //    var i = 0;
            //    foreach (var XuZhuEvent in XuZhuEvents)
            //    {
            //        Thread.Sleep(new RandomHelper().GetRandomNumber(1500, 2500));//随机休息一下
            //        if (cts.IsCancellationRequested)
            //        {
            //            return;
            //        }
            //        i++;
            //        Console.WriteLine("虚竹:" + XuZhuEvent);
            //        if (i==1&&!HasFirst)
            //        {
            //            HasFirst = true;
            //            Console.WriteLine("天龙八部就此拉开序幕");
            //        }
            //    }
            //    if (!HasEnd && !cts.IsCancellationRequested)
            //    {
            //        HasEnd = true;
            //        Console.WriteLine("虚竹已经做好准备了。。。。。");
            //    }
            //},cts.Token));
            //PlotList.Add(Story.StartNew(() =>
            //{
            //    var i = 0;
            //    foreach (var DuanYuEvent in DuanYuEvents)
            //    {
            //        Thread.Sleep(new RandomHelper().GetRandomNumber(1500, 2500));//随机休息一下
            //        if (cts.IsCancellationRequested)
            //        {
            //            return;
            //        }
            //        i++;
            //        Console.WriteLine("段誉:" + DuanYuEvent);
            //        if (i == 1 && !HasFirst)
            //        {
            //            HasFirst = true;
            //            Console.WriteLine("天龙八部就此拉开序幕");
            //        }
            //    }
            //    if (!HasEnd && !cts.IsCancellationRequested)
            //    {
            //        HasEnd = true;
            //        Console.WriteLine("段某已经做好准备了。。。。。");
            //    }
            //},cts.Token));
            Story.StartNew(() =>
            {
                while (stopwatch.IsRunning)
                {
                    Thread.Sleep(new RandomHelper().GetRandomNumber(1500, 2500));//随机休息一下
                    var currentYear = new Random().Next(0, 10000);
                    Console.WriteLine("灭世年份:"+ currentYear);
                    if (currentYear==2019)
                    {
                        Console.WriteLine("天降雷霆灭世，天龙八部的故事就此结束....");
                        cts.Cancel();
                    }
                }
            },cts.Token);
            //Story.ContinueWhenAny(PlotList.ToArray(), t => Console.WriteLine("天龙八部就此拉开序幕"));
            Story.ContinueWhenAll(PlotList.ToArray(), t => {
                if (!cts.IsCancellationRequested)
                {
                    Console.WriteLine("中原群雄大战辽兵，忠义两难一死谢天！");
                }
                stopwatch.Stop();
                Console.WriteLine(stopwatch.ElapsedMilliseconds);
            });
        }
        #endregion
    }
}
