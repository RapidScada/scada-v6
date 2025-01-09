using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Response
{
    /// <summary>
    /// 分页
    /// </summary>
    public class PaginationResult
    {
        /*
         * layui table 分页返回格式
         {
  "code": 0,
  "data": [{}, {}],
  "msg": "",
  "count": 1000
}
         */
        /// <summary>
        /// 构造
        /// </summary>
        public PaginationResult()
        {

        }
        /// <summary>
        /// 构造
        /// </summary>
        public PaginationResult(int count,int pageNum,int pageSize, object dataSource)
            :this(count, dataSource)
        {
            PageNum = pageNum;
            PageSize = pageSize;
        }

        /// <summary>
        /// 构造
        /// </summary>
        public PaginationResult(int count, object dataSource)
        {
            Count = count;
            Data = dataSource;
        }
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; } = 0;
        /// <summary>
        /// 总计
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageNum { get; set; }
        /// <summary>
        /// 分页条数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 空分页
        /// </summary>
        public static PaginationResult Empty(int pageNum, int pageSize)
        {
            return new PaginationResult()
            {
                PageNum =pageNum,
                PageSize =pageSize
            };
        }
    }
}
