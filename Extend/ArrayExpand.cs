using System;
using System.Collections.Generic;


public static class ArrayExpand
    {
        /// <summary>
        /// 判断集合中是否存在该值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool Contains<T>(this IEnumerable<T> items,Func<T,bool> func) {
            foreach (var item in items) {
                if (func(item)) {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 查询参数对象在集合中的下标
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static int IndexOf<T>(this IEnumerable<T> items, Func<T, bool> func)
        {
            var index = 0;
            foreach (var item in items)
            {
                if (func(item))
                {
                    return index;
                }
                index++;
            }
           
            return 0;
        }
    }

