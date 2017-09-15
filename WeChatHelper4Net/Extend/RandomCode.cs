using System;
using System.Text;

namespace WeChatHelper4Net.Extend
{
    /// <summary>
	/// RandomCode 随机代码
	/// 熊学浩
	/// </summary>
	public class RandomCode
    {
        /// <summary>
		/// 随机代码基本字符集
		/// </summary>
		private static readonly string[] basechar ={"0","1","2","3","4","5","6","7","8","9",
                                    "A","B","C","D","E","F","G","H","I","J","K","L","M","N",
                                    "O","P","Q","R","S","T","U","V","W","X","Y","Z",
                                    "a","b","c","d","e","f","g","h","i","j","k","l","m","n",
                                    "o","p","q","r","s","t","u","v","w","x","y","z"
                                   };
        /// <summary>
        /// 静态只读的随机对象，确保连续产生的随机数的随机性
        /// </summary>
        public static readonly Random random = new Random();

        /// <summary>
        /// 字符串中连续相同的字符的个数
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns>字符串中连续相同的字符的个数</returns>
        public static int sameChar(string text)
        {
            if(!string.IsNullOrEmpty(text) && text.Length > 1)
            {
                int flag = 0; //字符串中相同的字符数
                char[] ArrChar = text.ToCharArray();
                for(int i = 0; i < ArrChar.Length - 1; i++) //判断连续相同的字符（末尾字符不用判断）
                {
                    if(ArrChar[i] == ArrChar[i + 1])
                        flag++;
                }
                return flag;
            }
            return 0;
        }

        /// <summary>
        /// 创建随机代码（数字与字母组成）
        /// </summary>
        /// <param name="length">随机代码长度</param>
        /// <param name="onlyUpper">英文字符仅大写</param>
        /// <returns>随机代码</returns>
        public static string createRandomCode(int length, bool onlyUpper = true)
        {
            int range = 0;
            if(onlyUpper)
            {
                range = 36;
            }
            else
            {
                range = basechar.Length;
            }

            StringBuilder code = new StringBuilder();
            for(int i = 0; i < length; i++)
            {
                code.Append(basechar[random.Next(0, range)]);
            }

            if(length > 1 && !string.IsNullOrEmpty(code.ToString()))
            {
                if(sameChar(code.ToString()) > length - 1) //至少有一个字符不相同
                {
                    return createRandomCode(length, onlyUpper);
                }
            }

            return code.ToString();
        }
    }
}
