using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatHelper4Net;
using WeChatHelper4Net.Models.Menu;
using WeChatHelper4Net.Models.Menu.Base;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var c = new Click("xiong", "aaa");

            //Console.WriteLine(new Click("xiong", "aaa").ToJson());

            //var subbutton = new SubButton("二级菜单", new List<SingleButton>()
            //{
            //    new Click("xiong", "aaa"),
            //    new View("xue", "bbb"),
            //});
            //string subbuttonJson = subbutton.ToJson();
            //Console.WriteLine(subbuttonJson);

            var button = new Button(new List<BaseButton>()
            {
                new Click("今日歌曲", "V1001_TODAY_MUSIC"),
                new SubButton("菜单", new List<SingleButton>()
                {
                    new View("搜索", "http://www.soso.com/"),
                    new MiniProgram() {  name="wxa",
                        url="http://mp.weixin.qq.com",
                        appid="wx286b93c14bbf93aa",
                        pagepath="pages/lunar/index" },
                    new Click("赞一下我们", "V1001_GOOD"),
                    new Pic_weixin("相册","rselfmenu_1_2",new List<SingleButton>()
                        {
                            new Click("相册111", "xc111"),
                            new View("相册222", "xc222"),
                        })
                })
            });
            string buttonJson = button.ToJson();
            Console.WriteLine(buttonJson);

            Console.ReadKey();
        }
    }
}
