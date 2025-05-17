using Content.Manager.Core.WebApi.Model.Parameters;
using Content.Manager.Core.WebApi.Model.Series;
using LI.Data.Framework;
using System.Data;

namespace Asp.Net.Core.WebApi.CrudOperations.Services
{
    public static class SeriesServiceProvider
    {
        #region Methods

        public static SeriesResponse GetAll(SeriesQueryParameters queryParameters)
        {
            SeriesResponse response = new();
            var connectionString = GetConnection();
            var series = GetAllSeries(connectionString, queryParameters);
            if(series != null && series.Tables.Count > 0 && series.Tables[0].Rows.Count > 0)
            {
                var totalItems = 0;
                foreach(var data in series.Tables[0].AsEnumerable())
                {
                    response.SeriesItems.Add(new SeriesItem
                    {
                        Id = data.Field<int>("Id"),
                        Series = data.Field<string>("Series"),
                        Publisher = data.Field<string>("Publisher"),
                        SortingPriority = data.Field<int>("SortingPriority")

                    });
                    totalItems = data.Field<int>("TotalRows");
                }
                response.TotalItems = totalItems;
            }
            return response;
        }

        private static string GetConnection()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();
            var connectionString = config["db.dev.conn"];
            return connectionString;
        }

        private static DataSet GetAllSeries(string connectionString, SeriesQueryParameters parameters)
        {
            using(var objDd = new MsSqlClient(connectionString))
            {
                objDd.AddParameter("@PageNo", parameters.Start <= 0 ? 1 : parameters.Start);
                objDd.AddParameter("@PageSize", parameters.Rows <= 0 ? 10 : parameters.Rows);
                objDd.AddParameter("@SortColumn", string.IsNullOrWhiteSpace(parameters.Field)? "Series": parameters.Field);
                objDd.AddParameter("@SortOrder", string.IsNullOrWhiteSpace(parameters.Sort) ? "ASC" : parameters.Sort);
                return objDd.ExecuteDataSet("uspGetAllSeries", CommandType.StoredProcedure);
            }
        }   
        
        #endregion                                                                          
    }                                                                                       
}                                                                                           
                                                                                            