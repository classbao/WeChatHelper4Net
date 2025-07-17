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
        /// 最多连续字符数
        /// </summary>
        /// <param name="input">目标字符串</param>
        /// <returns></returns>
        public static int MaximumOfConsecutiveCharacters(string input)
        {
            if(string.IsNullOrEmpty(input))
                return 0;

            int maxCount = 1;
            int currentCount = 1;

            for(int i = 1; i < input.Length; i++)
            {
                if(input[i] == input[i - 1])
                {
                    currentCount++;
                    maxCount = Math.Max(maxCount, currentCount);
                }
                else
                {
                    currentCount = 1;
                }
            }

            return maxCount;
        }

        /// <summary>
        /// 创建随机代码（数字与字母组成）
        /// </summary>
        /// <param name="length">随机代码长度</param>
        /// <param name="onlyUpper">英文字符仅大写</param>
        /// <returns>随机代码</returns>
        public static string GenerateRandomCode(int length, bool onlyUpper = true)
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
                if(MaximumOfConsecutiveCharacters(code.ToString()) > length - 1) //至少有一个字符不相同
                {
                    return GenerateRandomCode(length, onlyUpper);
                }
            }

            return code.ToString();
        }

        /// <summary>
        /// 生成随机纯数字字符串
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string GenerateRandomNum(int length)
        {
            StringBuilder num = new StringBuilder();
            for(int i = 0; i < length; i++)
            {
                num.Append(basechar[random.Next(0, 10)]);
            }

            if(length > 1 && !string.IsNullOrEmpty(num.ToString()))
            {
                if(MaximumOfConsecutiveCharacters(num.ToString()) > length - 1) //至少有一个字符不相同
                {
                    return GenerateRandomNum(length);
                }
            }

            return num.ToString();
        }

    }
}
