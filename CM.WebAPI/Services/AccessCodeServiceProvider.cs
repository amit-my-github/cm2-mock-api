// Ignore Spelling: Api

using CM.WebAPI.Model.AccessCodes;
using CM.WebAPI.Model.Parameters;
using System.Data;


namespace CM.WebAPI.Services
{
    public static class AccessCodeServiceProvider
    {

        #region Methods

        public static AccessCodeResponse GetAll(AccessCodeQueryParameters parameters)
        {
            var res = new AccessCodeResponse();
            var connectionString = GetConnection();
            var accessCodes = GetAllAccessCodes(connectionString, parameters);
            if (accessCodes != null && accessCodes.Tables.Count > 0 && accessCodes.Tables[0].Rows.Count > 0)
            {
                var totalItems = 0;
                foreach (var accessCode in accessCodes.Tables[0].AsEnumerable())
                {
                    res.AccessCodes.Add(new AccessCodeItem
                    {
                        Id = accessCode.Field<int>("AccessCodeID"),
                        CreatedDate = accessCode.Field<DateTime>("DateCreated").ToString("yyyy-MM-ddTHH:mm:ss"),
                        IsFree = accessCode.Field<bool>("IsFree"),
                        Description = accessCode.Field<string?>("Description"),
                        AccessCode = accessCode.Field<string?>("AccessCode"),
                        LastModified = accessCode.Field<DateTime>("Lastmodified").ToString("yyyy-MM-ddTHH:mm:ss"),
                    });
                    totalItems = accessCode.Field<int>("TotalRows");
                }
                res.TotalItems = totalItems;
            }
            return res;
        }

        public static int Create(AddAccessCodeRequest accessCode)
        {
            var connectionString = GetConnection();
            return CreateAccessCode(connectionString, accessCode);
        }

        public static AccessCodeItem? GetById(int id)
        {
            var res = new AccessCodeResponse();
            var connectionString = GetConnection();
            var accessCodes = GetAccessCodeById(connectionString, id);
            if (accessCodes != null && accessCodes.Tables.Count > 0 && accessCodes.Tables[0].Rows.Count > 0)
            {
                foreach (var accessCode in accessCodes.Tables[0].AsEnumerable())
                {
                    res.AccessCodes.Add(new AccessCodeItem
                    {
                        Id = accessCode.Field<int>("AccessCodeID"),
                        CreatedDate = accessCode.Field<DateTime>("DateCreated").ToString("yyyy-MM-ddTHH:mm:ss"),
                        IsFree = accessCode.Field<bool>("IsFree"),
                        Description = accessCode.Field<string?>("Description"),
                        AccessCode = accessCode.Field<string?>("AccessCode"),
                        LastModified = accessCode.Field<DateTime>("Lastmodified").ToString("yyyy-MM-ddTHH:mm:ss"),
                    });
                }
                res.TotalItems = res.AccessCodes.Count;
                return res.AccessCodes.First();
            }
            return null;

        }

        public static bool IsValidRequest(int id)
        {
            return IsValid(GetConnection(), id);

        }

        public static AccessCodeItem? GetByCode(string code)
        {
            var res = new AccessCodeResponse();
            var connectionString = GetConnection();
            var accessCodes = GetAccessCodeByAccessCode(connectionString, code);
            if (accessCodes != null && accessCodes.Tables.Count > 0 && accessCodes.Tables[0].Rows.Count > 0)
            {
                foreach (var accessCode in accessCodes.Tables[0].AsEnumerable())
                {
                    res.AccessCodes.Add(new AccessCodeItem
                    {
                        Id = accessCode.Field<int>("AccessCodeID"),
                        CreatedDate = accessCode.Field<DateTime>("DateCreated").ToString("yyyy-MM-ddTHH:mm:ss"),
                        IsFree = accessCode.Field<bool>("IsFree"),
                        Description = accessCode.Field<string?>("Description"),
                        AccessCode = accessCode.Field<string?>("AccessCode"),
                        LastModified = accessCode.Field<DateTime>("Lastmodified").ToString("yyyy-MM-ddTHH:mm:ss"),
                    });
                }
                res.TotalItems = res.AccessCodes.Count;
                return res.AccessCodes.First();
            }
            return new AccessCodeItem();
        }

        public static bool Update(int Id, AddAccessCodeRequest accessCode)
        {
            var connectionString = GetConnection();
            using (var objDd = new MsSqlClient(connectionString))
            {

                objDd.AddParameter("@AccessCodeId", Id);
                objDd.AddParameter("@AccessCode", string.IsNullOrEmpty(accessCode.AccessCode) ? DBNull.Value : accessCode.AccessCode);
                objDd.AddParameter("@Description", string.IsNullOrEmpty(accessCode.Description) ? DBNull.Value : accessCode.Description);
                objDd.AddParameter("@IsFree", accessCode.IsFree);
                objDd.AddParameter("@Lastmodified", DateTime.Now);
                return objDd.ExecuteNonQuery("uspUpdateAccessCode", CommandType.StoredProcedure) > 0;
            }

        }

        public static bool Remove(int id)
        {
            var connectionString = GetConnection();
            return RemoveAccessCode(connectionString, id) > 0;
        }

        private static string GetConnection()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();
            var connectionString = config["db.dev.conn"];
            return connectionString;
        }

        private static DataSet GetAllAccessCodes(string connectionString, AccessCodeQueryParameters parameters)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {

                objDd.AddParameter("@SearchValue", string.IsNullOrWhiteSpace(parameters.Q) ? DBNull.Value : parameters.Q);
                objDd.AddParameter("@PageNo", parameters.Start <= 0 ? 1 : parameters.Start);
                objDd.AddParameter("@PageSize", parameters.Rows <= 0 ? 10 : parameters.Rows);
                objDd.AddParameter("@SortColumn", string.IsNullOrWhiteSpace(parameters.Field) ? "Access Code" : parameters.Field);
                objDd.AddParameter("@SortOrder", string.IsNullOrWhiteSpace(parameters.Sort) ? "ASC" : parameters.Sort);
                if (parameters.IsFree.HasValue)
                {
                    objDd.AddParameter("@IsFree", parameters.IsFree.Value);
                }
                return objDd.ExecuteDataSet("uspGetAllAccessCodes", CommandType.StoredProcedure);
            }
        }

        private static DataSet GetAccessCodeById(string connectionString, int accessCodeId)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {
                objDd.AddParameter("@AccessCodeId", accessCodeId);
                return objDd.ExecuteDataSet("uspGetAccessCodeById", CommandType.StoredProcedure);
            }
        }

        private static DataSet GetAccessCodeByAccessCode(string connectionString, string accessCode)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {

                objDd.AddParameter("@AccessCode", accessCode);
                return objDd.ExecuteDataSet("uspGetAccessCodeByAccessCode", CommandType.StoredProcedure);
            }
        }

        private static int RemoveAccessCode(string connectionString, int accessCodeId)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {

                objDd.AddParameter("@AccessCodeId", accessCodeId);
                return objDd.ExecuteNonQuery("uspRemoveAccessCodeById", CommandType.StoredProcedure);
            }
        }

        private static int CreateAccessCode(string connectionString, AddAccessCodeRequest accessCode)
        {
            if (accessCode != null)
            {
                using (var objDd = new MsSqlClient(connectionString))
                {

                    objDd.AddParameter("@AccessCode", string.IsNullOrEmpty(accessCode.AccessCode) ? DBNull.Value : accessCode.AccessCode);
                    objDd.AddParameter("@Description", string.IsNullOrEmpty(accessCode.Description) ? DBNull.Value : accessCode.Description);
                    objDd.AddParameter("@IsFree", accessCode.IsFree);
                    objDd.AddParameter("@DateCreated", DateTime.Now);
                    return objDd.ExecuteNonQuery("uspAddAccessCode", CommandType.StoredProcedure);
                }
            }
            return 0;

        }

        private static bool IsValid(string connectionString, int accessCodeId)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {
                objDd.AddParameter("@AccessCodeId", accessCodeId);
                var obj = objDd.ExecuteDataSet("uspIsValidAccessCodeId", CommandType.StoredProcedure);
                return obj != null && obj.Tables.Count > 0 && obj.Tables[0].Rows.Count > 0;

            }
        }

        #endregion
    }
}
