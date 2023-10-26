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
        public PaginationResult()
        {
            
        }
        public PaginationResult(int count,int pageNum,int pageSize, object dataSource)
            :this(count, dataSource)
        {
            PageNum = pageNum;
            PageSize = pageSize;    
        }

        public PaginationResult(int count, object dataSource)
        {
            Count = count;
            Data = dataSource;
        }

        public int Code { get; set; } = 0;
        public int Count { get; set; }
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public object Data { get; set; }

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
