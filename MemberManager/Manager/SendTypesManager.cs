using MemberManager.Context;
using MemberManager.Models.DbModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MemberManager.Manager
{
    public class SendTypesManager : AbstractAppEntityManager<SendTypes>
    {
        public SendTypesManager(MemberContext _db,
IHttpContextAccessor _httpContextAccessor) : base(_db)
        {
        }

        public override IQueryable<SendTypes> GetEntitiesQ()
        {
            return db.SendTypes.Where(m => !m.removed);
        }

        public override SendTypes GetById(Int64 id)
        {
            return db.SendTypes.Where(m => !m.removed && m.id == id).FirstOrDefault();
        }

        public async Task<List<SendTypes>> ExcuteQuery(Criteria criteria)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            StringBuilder sql = new StringBuilder();
            sql.Append(" Select * From [" + SendTypes.TABLE_NAME + "] as st ");
            sql.Append(" Where 1 = 1 ");
            
            if(!string.IsNullOrWhiteSpace(criteria.name))
            {
                sql.Append(" And st.[name] like @name");
                parameters.Add(new SqlParameter("name","%" + criteria.name + "%"));
            }

            sql.Append(" And st.removed = 0 Order By st.Sort ");

            return await db.SendTypes.FromSqlRaw(sql.ToString(),parameters.ToArray()).ToListAsync();
        }

        public class Criteria
        {
            public string name { get; set; }
        }
    }
}
