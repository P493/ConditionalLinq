using ConditionalLinq.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ConditionalLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            throw new NotSupportedException("just demo code here");
        }

        /// <summary>
        /// sometimes we need result
        /// </summary>
        public IList GetDataList()
        {
            return GetQuery().ToList();
        }

        /// <summary>
        /// sometimes we just need the query, could use object as IQueryable<>'s type parameter since we prefer to create anonymous type with linq
        /// </summary>
        public IQueryable<object> GetQuery()
        {
            // pretend we are using linq to sql
            var db_customers = new List<Customer>().AsQueryable();
            var db_orders = new List<Order>().AsQueryable();

            // query condition could be posted from front end or control's value like System.Web.UI.WebControls.DropDownList in a gui application
            var queryDto = new QueryDto();

            return
                (from o in db_orders
                join c in db_customers on o.CustomerId equals c.Id
                select new
                {
                    CustomerName = c.Name,
                    CustomerActive = c.Active,
                    o.State,
                    o.CreationDate,
                    o.ExtraProperty1,
                    o.ExtraProperty2,
                })
                .MayWhere(queryDto.StartDate, v => t => t.CreationDate >= v)
                .MayWhere(queryDto.EndDate, v => t => t.CreationDate <= v)
                .MayWhereString(queryDto.CustomerName, v => t => t.CustomerName.Contains(v))
                .MayWhereBool(queryDto.CustomerActive, v => t => t.CustomerActive == v)
                .MayWhereInt(queryDto.State, v => t => t.State == v)
                .MayWhere(
                    queryDto.NeedMoreCondition,
                    q => q.MayWhere(queryDto.SelectedValue1, (int v) => t => t.ExtraProperty1 == v)
                        .MayWhere(queryDto.SelectedValue2, (bool v) => t => t.ExtraProperty2 == v)
                );
        }

        /// <summary>
        /// not generally used like this, enough for demo
        /// </summary>
        public SqlCommand GetSqlCommand()
        {
            var con = new SqlConnection("server=localhost;uid=sa;pwd=sa;database=not_exist");

            string name = "abc";
            string orderFields = "id,name";

            return new ConditionalSqlBuilder("select * from customer where active = 1")
                .MayAppend(name, "@name", " and name like '%@name%' ")
                .MayAppend(orderFields, "@orderFields", " order by @orderFields ")
                .ToSqlCommand(con);
        }
    }
}
