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
        private static readonly object ForLock = new object();
        static void Main(string[] args)
        {
            var Program = new Program();
            //Program.StoryStart();
            //ConsoleCharacter("能行吗，就这么点代码");
            Testparallel();
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
                        ConsoleCharacter(item.Name + plot+"!");
                        if (i == 1 && !HasFirst)
                        {
                            HasFirst = true;
                            ConsoleCharacter("天龙八部就此拉开序幕");
                        }
                    }
                    if (!HasEnd && !cts.IsCancellationRequested)
                    {
                        HasEnd = true;
                        ConsoleCharacter(item.Name+"已经做好准备了。。。。。");
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
                    ConsoleCharacter("灭世年份:"+ currentYear);
                    if (currentYear==2019)
                    {
                        ConsoleCharacter("天降雷霆灭世，天龙八部的故事就此结束....");
                        cts.Cancel();
                    }
                }
            },cts.Token);
            //Story.ContinueWhenAny(PlotList.ToArray(), t => ConsoleCharacter("天龙八部就此拉开序幕"));
            Story.ContinueWhenAll(PlotList.ToArray(), t => {
                if (!cts.IsCancellationRequested)
                {
                    ConsoleCharacter("中原群雄大战辽兵，忠义两难一死谢天！");
                }
                stopwatch.Stop();
                ConsoleCharacter("总用时"+stopwatch.ElapsedMilliseconds);
            });
        }
        #endregion
        public static void ConsoleCharacter(string chars)
        {
            lock (ForLock)
            {
                foreach (var item in chars)
                {
                    Console.Write(item);
                    Thread.Sleep(300);
                }
                Console.WriteLine();
            }
        }
        public static void Testparallel()
        {
            //Action<int> action = new Action<int>((a) =>
            //{
            //    Console.WriteLine("Obj"+a+"using "+Thread.CurrentThread.ManagedThreadId.ToString("00"));
            //    Thread.Sleep(1000);
            //});
            ////parallelOptions 可以控制并发数量
            //ParallelOptions parallelOptions = new ParallelOptions();
            //parallelOptions.MaxDegreeOfParallelism = 10;
            //Parallel.For(0,1000, action);


            {
                //List<int> list = new List<int>();
                //for (int i = 0; i < 10000; i++)
                //{
                //    list.Add(i);
                //}
                //完成10000个任务  但是只要11个线程  
                Action<int> action = i =>
                {
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString("00"));
                    Thread.Sleep(new Random(i).Next(100, 300));
                };
                List<Task> taskList = new List<Task>();
                for (int i = 0; i < 10000; i++)
                {

                //}
                //foreach (var i in list)
                //{
                    int k = i;
                    taskList.Add(Task.Run(() => action.Invoke(k)));
                    if (taskList.Count > 5)
                    {
                        Task.WaitAny(taskList.ToArray());
                        taskList = taskList.Where(t => t.Status != TaskStatus.RanToCompletion).ToList();
                    }
                }
                Task.WhenAll(taskList.ToArray());
            }
        }

    }
}
