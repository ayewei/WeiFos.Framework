// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


//Test1();
//Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ms")}：开始" + Thread.CurrentThread.ManagedThreadId);

// 调用同步步步方法
//SyncTestMethod();

// 调用异步步方法
//AsyncTestMethod();

//Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ms")}：结束" + Thread.CurrentThread.ManagedThreadId);

Console.WriteLine("请输入共几个数字");
int num = int.Parse(Console.ReadLine());
Console.WriteLine("请输入要相加的数字");
int num1 = int.Parse(Console.ReadLine());
List<string> arr = new List<string>();
for (int i = 1; i <= num; i++)
{
    string tmp = "";
    for (int j = 1; j <= i; j++)
    {
        tmp += num1.ToString();
    }
    arr.Add(tmp);
}

int sum = 0;
foreach (var a in arr)
{
    sum += int.Parse(a);
}

Console.WriteLine(sum);


test1();

Console.ReadKey();







static bool checke(string str, string str1)
{
    Array.Sort(str.ToCharArray());
    string astr = str.ToString();

    Array.Sort(str1.ToCharArray());
    string astr1 = str1.ToString();

    return astr.Equals(astr1);
}


static void test1()
{

    List<string> jiadui = new List<string>();
    jiadui.Add("a");
    jiadui.Add("b");
    jiadui.Add("c");

    List<string> yidui = new List<string>();
    yidui.Add("x");
    yidui.Add("y");
    yidui.Add("z");

    string tmpdui1 = "ax", tmpdui2 = "cx", tmpdui3 = "cz";

    List<string> tdui = new List<string>();
    tdui.Add("bx");
    tdui.Add("cy");

    List<string> tmp = new List<string>();
    foreach (string j in jiadui)
    {
        foreach (string y in yidui)
        {
            string ddd = j + y;
            if (!checke(ddd, tmpdui1) && !checke(ddd, tmpdui2) && !checke(ddd, tmpdui3))
            {
                tmp.Add(j + y);
            }
        }
    }

    foreach (string t in tdui)
    {
        if (tmp.Contains(t))
        {
            tmp.Remove(t);
        }
    }

    List<string> tmp11 = new List<string>();
    foreach (string jjj in tmp)
    {
        foreach (string iii in tdui)
        {
            bool has = false;
            foreach (char i1 in iii.ToCharArray())
            {
                if (jjj.Contains(i1.ToString()))
                {
                    has = true;
                }
            }

            if (has && !tmp11.Contains(jjj))
            {
                tmp11.Add(jjj);
            }
        }
    }

    List<string> tmp12 = new List<string>();
    foreach (string jjj in tmp)
    {
        if (!tmp11.Contains(jjj))
        {
            tmp12.Add(jjj);
        }
    }

    tdui.AddRange(tmp12);
}



static void Test1()
{
    var order1 = Task.Run(() =>
    {
        Console.WriteLine("Order 1");
    });

    // 匿名委托将等待 order1 执行完成后执行，并将 order1 对象作为参数传入
    order1.ContinueWith((task) =>
    {
        Console.WriteLine("Order 1 Is Completed");
    });

    var t1 = Task.Run(() => { Task.Delay(1500).Wait(); Console.WriteLine("t1"); });
    var t2 = Task.Run(() => { Task.Delay(2000).Wait(); Console.WriteLine("t2"); });
    var t3 = Task.Run(() => { Task.Delay(3000).Wait(); Console.WriteLine("t3"); });
    Task.WaitAll(t1, t2, t3);
    // t1,t2,t3 完成后输出下面的消息
    Console.WriteLine("t1,t2,t3 Is Complete");

    var t4 = Task.Run(() => { Task.Delay(1500).Wait(); Console.WriteLine("t4"); });
    var t5 = Task.Run(() => { Task.Delay(2000).Wait(); Console.WriteLine("t5"); });
    var t6 = Task.Run(() => { Task.Delay(3000).Wait(); Console.WriteLine("t6"); });
    Task.WaitAny(t4, t5, t6);
    // 当任意任务完成时，输出下面的消息，目前按延迟时间计算，在 t4 完成后立即输出下面的信息
    Console.WriteLine("t4,t5,t6 Is Complete");

    var t7 = Task.Run(() => { Task.Delay(1500).Wait(); Console.WriteLine("t7"); });
    var t8 = Task.Run(() => { Task.Delay(2000).Wait(); Console.WriteLine("t8"); });
    var t9 = Task.Run(() => { Task.Delay(3000).Wait(); Console.WriteLine("t9"); });
    var whenAll = Task.WhenAll(t7, t8, t9);
    // WhenAll 不会等待，所以这里必须显示指定等待
    whenAll.Wait();
    // 当所有任务完成时，输出下面的消息
    Console.WriteLine("t7,t8,t9 Is Complete");

    var t10 = Task.Run(() => { Task.Delay(1500).Wait(); Console.WriteLine("t10"); });
    var t11 = Task.Run(() => { Task.Delay(2000).Wait(); Console.WriteLine("t11"); });
    var t12 = Task.Run(() => { Task.Delay(3000).Wait(); Console.WriteLine("t12"); });
    var whenAny = Task.WhenAll(t10, t11, t12);
    // whenAny 不会等待，所以这里必须显示指定等待
    whenAny.Wait();
    // 当任意任务完成时，输出下面的消息，目前按延迟时间计算，在 t10 完成后立即输出下面的信息
    Console.WriteLine("t10,t11,t12 Is Complete");
}


/// <summary>
/// 同步方法
/// </summary>
static void SyncTestMethod()
{
    for (int i = 0; i < 10; i++)
    {
        var str = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ms")}:SyncTestMethod{i}{"当前线程:" + Thread.CurrentThread.ManagedThreadId}";
        Console.WriteLine(str);
        Thread.Sleep(1000);
    }
}

/// <summary>
/// 异步方法
/// </summary>
/// <returns></returns>
static async Task AsyncTestMethod()
{
    await Task.Run(() =>
    {
        for (int i = 0; i < 10; i++)
        {
            var str = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ms")}:AsyncTestMethod{i}{"当前线程:" + Thread.CurrentThread.ManagedThreadId}";
            Console.WriteLine(str);
            Thread.Sleep(1000);
        }
    });
}

