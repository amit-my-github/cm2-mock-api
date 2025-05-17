using Content.Manager.Core.WebApi.Model.Parameters;
using Content.Manager.Core.WebApi.Model.Publishers;
using Content.Manager.Core.WebApi.Model.Publishers;
using LI.Data.Framework;
using System.Data;

namespace Asp.Net.Core.WebApi.CrudOperations.Services
{
    public static class PublisherServiceProvider
    {


        #region Methods

        public static PublishersResponse GetAll(PublisherQueryParameters parameters)
        {
            var connectionString = GetConnection();
            var res = new PublishersResponse();
            {
                var publishers = GetAllPublishers(connectionString, parameters);
                if (publishers != null && publishers.Tables.Count > 0 && publishers.Tables[0].Rows.Count > 0)
                {
                    foreach (var publisher in publishers.Tables[0].AsEnumerable())
                    {
                        res.Publishers.Add(new PublisherItem
                        {
                            TotalRows = publisher.Field<int>("TotalRows"),
                            Id = publisher.Field<int>("ID"),
                            DateCreated = publisher.Field<DateTime>("DateCreated").ToString("yyyy-MM-ddTHH:mm:ss"),
                            Status = publisher.Field<string>("Status"),
                            Name = publisher.Field<string?>("Name"),
                            Notes = publisher.Field<string?>("Notes"),
                            LastModified = publisher.Field<DateTime>("Lastmodified").ToString("yyyy-MM-ddTHH:mm:ss"),
                        });
                    }
                    res.TotalItems = res.Publishers.First().TotalRows;
                }

            }
            return res;
        }

        public static PublisherItem? GetById(int id)
        {
            var connectionString = GetConnection();
            var res = new PublishersResponse();
            {
                var publishers = GetPublisherById(connectionString, id);
                if (publishers != null && publishers.Tables.Count > 0 && publishers.Tables[0].Rows.Count > 0)
                {
                    foreach (var publisher in publishers.Tables[0].AsEnumerable())
                    {
                        res.Publishers.Add(new PublisherItem
                        {

                            Id = publisher.Field<int>("ID"),
                            DateCreated = publisher.Field<DateTime>("DateCreated").ToString("yyyy-MM-ddTHH:mm:ss"),
                            Status = publisher.Field<string>("Status"),
                            Name = publisher.Field<string?>("Name"),
                            Notes = publisher.Field<string?>("Notes"),
                            LastModified = publisher.Field<DateTime>("Lastmodified").ToString("yyyy-MM-ddTHH:mm:ss"),
                        });
                    }

                    res.TotalItems = res.Publishers.Count;
                }

            }
            return res.Publishers.FirstOrDefault();
        }

        public static int Create(PublisherItem publisher)
        {
            var connectionString = GetConnection();
            return CreatePublisher(connectionString, publisher);
        }

        public static bool Update(int Id, PublisherItem publisher)
        {
            var connectionString = GetConnection();
            using (var objDd = new MsSqlClient(connectionString))
            {

                objDd.AddParameter("@Id", @Id);
                objDd.AddParameter("@Name", string.IsNullOrEmpty(publisher.Name) ? DBNull.Value : publisher.Name);
                objDd.AddParameter("@Status", string.IsNullOrEmpty(publisher.Status) ? DBNull.Value : publisher.Status);
                objDd.AddParameter("@DateCreated", string.IsNullOrEmpty(publisher.DateCreated) ? DBNull.Value : publisher.DateCreated);
                objDd.AddParameter("@Lastmodified", string.IsNullOrEmpty(publisher.LastModified) ? DBNull.Value : publisher.LastModified);
                objDd.AddParameter("@Notes", string.IsNullOrEmpty(publisher.Notes) ? DBNull.Value : publisher.Notes);
                return objDd.ExecuteNonQuery("uspUpdatePublisher", CommandType.StoredProcedure) > 0;
            }

        }

        public static bool Remove(PublisherItem publisher)
        {
            var connectionString = GetConnection();
            return RemovePublisher(connectionString, publisher != null ? publisher.Id : 0) > 0;
        }

        private static int RemovePublisher(string connectionString, int Id)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {

                objDd.AddParameter("@Id", Id);
                return objDd.ExecuteNonQuery("uspRemovePublisherById", CommandType.StoredProcedure);
            }
        }

        private static string GetConnection()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();
            var connectionString = config["db.dev.conn"];
            return connectionString;
        }

        private static DataSet GetAllPublishers(string connectionString, PublisherQueryParameters parameters)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {

                objDd.AddParameter("@SearchValue", string.IsNullOrWhiteSpace(parameters.Q) ? DBNull.Value : parameters.Q);
                objDd.AddParameter("@PageNo", parameters.Start <= 0 ? 1 : parameters.Start);
                objDd.AddParameter("@PageSize", parameters.Rows <= 0 ? 10 : parameters.Rows);
                objDd.AddParameter("@SortColumn", string.IsNullOrWhiteSpace(parameters.Field) ? "Name" : parameters.Field);
                objDd.AddParameter("@SortOrder", string.IsNullOrWhiteSpace(parameters.Sort) ? "ASC" : parameters.Sort);
                if (parameters.IsFree.HasValue)
                {
                    objDd.AddParameter("@Status", parameters.IsFree.Value? "Y":"N");
                }
                return objDd.ExecuteDataSet("uspGetAllPublishers", CommandType.StoredProcedure);
            }
        }

        private static DataSet GetPublisherById(string connectionString, int Id)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {
                objDd.AddParameter("@Id", Id);
                return objDd.ExecuteDataSet("uspGetPublisherById", CommandType.StoredProcedure);
            }
        }

        private static int CreatePublisher(string connectionString, PublisherItem publisher)
        {
            if (publisher != null)
            {
                using (var objDd = new MsSqlClient(connectionString))
                {

                    objDd.AddParameter("@Name", string.IsNullOrEmpty(publisher.Name) ? DBNull.Value : publisher.Name);
                    objDd.AddParameter("@Status", string.IsNullOrEmpty(publisher.Status) ? DBNull.Value : publisher.Status);
                    objDd.AddParameter("@DateCreated", string.IsNullOrEmpty(publisher.DateCreated) ? DBNull.Value : publisher.DateCreated);
                    objDd.AddParameter("@Lastmodified", string.IsNullOrEmpty(publisher.LastModified) ? DBNull.Value : publisher.LastModified);
                    objDd.AddParameter("@Notes", string.IsNullOrEmpty(publisher.Notes) ? DBNull.Value : publisher.Notes);
                    return objDd.ExecuteNonQuery("uspAddPublisher", CommandType.StoredProcedure);
                }
            }
            return 0;

        }


        #endregion
    }
}
