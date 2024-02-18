// Ignore Spelling: Api

using CM.WebAPI.Model.Category;
using CM.WebAPI.Model.Facet;
using CM.WebAPI.Model.Parameters;
using System.Data;


namespace CM.WebAPI.Services
{
    public static class FacetsServiceProvider
    {

        #region Methods

        public static FacetsResponse GetAll(FacetsQueryParameters parameters)
        {
            var res = new FacetsResponse();
            var connectionString = GetConnection();
            var facets = GetAllFacets(connectionString, parameters);
            if (facets != null && facets.Tables.Count > 0 && facets.Tables[0].Rows.Count > 0)
            {
                var totalItems = 0;
                foreach (var facet in facets.Tables[0].AsEnumerable())
                {
                    res.Facets.Add(new FacetsItem
                    {
                        Id = facet.Field<int>("ID"),
                        SolrField = facet.Field<string>("SolrField"),
                        FacetName = facet.Field<string>("Name"),
                        Description = facet.Field<string?>("Description"),

                    });
                    totalItems = facet.Field<int>("TotalRows");
                }
                res.TotalItems = totalItems;
            }
            return res;
        }

        public static FacetsResponse GetAllFacetCategoryRelation(FacetsQueryParameters parameters)
        {
            var res = new FacetsResponse();
            var connectionString = GetConnection();
            var facets = GetAllFacetCategoryRelation(connectionString, parameters);
            if (facets != null && facets.Tables.Count > 0 && facets.Tables[0].Rows.Count > 0)
            {
                var totalItems = 0;
                foreach (var facet in facets.Tables[0].AsEnumerable())
                {
                    res.Facets.Add(new FacetsItem
                    {
                        Id = facet.Field<int>("ID"),
                        SolrField = facet.Field<string>("SolrField"),
                        FacetName = facet.Field<string>("Name"),
                        Description = facet.Field<string?>("Description"),

                    });
                    totalItems = facet.Field<int>("TotalRows");
                }
                res.TotalItems = totalItems;
            }
            return res;
        }

        public static FacetsResponse GetFacetCategoryMapping(string categoryName, int categoryId, FacetsQueryParameters parameters)
        {
            var res = new FacetsResponse();
            var connectionString = GetConnection();
            var facets = GetFacetCategoryMapping(categoryName, categoryId, connectionString, parameters);
            if (facets != null && facets.Tables.Count > 0 && facets.Tables[0].Rows.Count > 0)
            {
                var totalItems = 0;
                foreach (var facet in facets.Tables[0].AsEnumerable())
                {
                    res.Facets.Add(new FacetsItem
                    {
                        Id = facet.Field<int>("ID"),
                        SolrField = facet.Field<string>("SolrField"),
                        FacetName = facet.Field<string>("Name"),
                        Description = facet.Field<string?>("Description"),

                    });
                    totalItems = facet.Field<int>("TotalRows");
                }
                res.TotalItems = totalItems;
            }
            return res;
        }

        public static FacetsResponse GetFacetCategoryPublicationTitles(FacetsQueryParameters parameters)
        {
            var res = new FacetsResponse();
            var connectionString = GetConnection();
            var facets = GetFacetCategoryPublicationTitles(connectionString, parameters);
            if (facets != null && facets.Tables.Count > 0 && facets.Tables[0].Rows.Count > 0)
            {
                var totalItems = 0;
                foreach (var facet in facets.Tables[0].AsEnumerable())
                {
                    res.Facets.Add(new FacetsItem
                    {
                        Id = facet.Field<int>("ID"),
                        SolrField = facet.Field<string>("SolrField"),
                        FacetName = facet.Field<string>("Name"),
                        Description = facet.Field<string?>("Description"),

                    });
                    totalItems = facet.Field<int>("TotalRows");
                }
                res.TotalItems = totalItems;
            }
            return res;
        }

        public static int Create(CategoriesItem categoriesItem)
        {
            var connectionString = GetConnection();
            return CreateCategory(connectionString, categoriesItem);
        }

        public static CategoriesResponse GetByName(string name)
        {
            var res = new CategoriesResponse();
            var connectionString = GetConnection();
            var categories = GetCategoriesByName(connectionString, name);
            if (categories != null && categories.Tables.Count > 0 && categories.Tables[0].Rows.Count > 0)
            {
                foreach (var accessCode in categories.Tables[0].AsEnumerable())
                {
                    res.Categories.Add(new CategoriesItem
                    {
                        Id = accessCode.Field<int>("ID"),
                        SortingPriority = accessCode.Field<int>("SortingPrio"),
                        Value = accessCode.Field<string>("Value"),
                        SearchNumberNT = accessCode.Field<bool>("SearchNumberNT"),
                        SearchNumber = accessCode.Field<string?>("SearchNumber"),
                        Notes = accessCode.Field<string?>("Notes"),
                        DateCreated = accessCode.Field<DateTime>("DateCreated").ToString("yyyy-MM-ddTHH:mm:ss"),
                        LastModified = accessCode.Field<DateTime>("LastModified").ToString("yyyy-MM-ddTHH:mm:ss"),
                    });
                }
                res.TotalItems = res.Categories.Count;
            }

            return res;

        }

        public static CategoriesItem? GetById(int id)
        {
            var res = new CategoriesResponse();
            var connectionString = GetConnection();
            var categories = GetCategoriesById(connectionString, id);
            if (categories != null && categories.Tables.Count > 0 && categories.Tables[0].Rows.Count > 0)
            {
                foreach (var accessCode in categories.Tables[0].AsEnumerable())
                {
                    res.Categories.Add(new CategoriesItem
                    {
                        Id = accessCode.Field<int>("ID"),
                        SortingPriority = accessCode.Field<int>("SortingPrio"),
                        Value = accessCode.Field<string>("Value"),
                        SearchNumberNT = accessCode.Field<bool>("SearchNumberNT"),
                        SearchNumber = accessCode.Field<string?>("SearchNumber"),
                        Notes = accessCode.Field<string?>("Notes"),
                        DateCreated = accessCode.Field<DateTime>("DateCreated").ToString("yyyy-MM-ddTHH:mm:ss"),
                        LastModified = accessCode.Field<DateTime>("LastModified").ToString("yyyy-MM-ddTHH:mm:ss"),
                    });
                }
                res.TotalItems = res.Categories.Count;
            }

            return res.Categories.FirstOrDefault();

        }

        public static bool Update(int Id, CategoriesItem categoryItem)
        {

            var connectionString = GetConnection();
            using (var objDd = new MsSqlClient(connectionString))
            {

                objDd.AddParameter("@Id", Id);
                objDd.AddParameter("@SortingPrio", categoryItem.SortingPriority);
                objDd.AddParameter("@Value", string.IsNullOrEmpty(categoryItem.Value) ? DBNull.Value : categoryItem.Value);
                objDd.AddParameter("@SearchNumberNT", categoryItem.SearchNumberNT);
                objDd.AddParameter("@Notes", string.IsNullOrEmpty(categoryItem.Notes) ? DBNull.Value : categoryItem.Notes);
                objDd.AddParameter("@DateCreated", string.IsNullOrEmpty(categoryItem.DateCreated) ? DBNull.Value : categoryItem.DateCreated);
                objDd.AddParameter("@Lastmodified", string.IsNullOrEmpty(categoryItem.LastModified) ? DBNull.Value : categoryItem.LastModified);
                objDd.AddParameter("@SearchNumber", string.IsNullOrEmpty(categoryItem.SearchNumber) ? DBNull.Value : categoryItem.SearchNumber);
                return objDd.ExecuteNonQuery("uspUpdateCategory", CommandType.StoredProcedure) > 0;
            }

        }

        public static bool Remove(CategoriesItem categoryItem)
        {
            var connectionString = GetConnection();
            return RemoveCategory(connectionString, categoryItem != null ? categoryItem.Id : 0) > 0;

        }

        private static string GetConnection()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();
            var connectionString = config["db.dev.conn"];
            return connectionString;
        }

        private static DataSet GetAllFacets(string connectionString, QueryParameters parameters)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {


                objDd.AddParameter("@PageNo", parameters.Start <= 0 ? 1 : parameters.Start);
                objDd.AddParameter("@PageSize", parameters.Rows <= 0 ? 10 : parameters.Rows);
                return objDd.ExecuteDataSet("uspGetAllFacets", CommandType.StoredProcedure);
            }
        }

        private static DataSet GetCategoriesByName(string connectionString, string name)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {
                objDd.AddParameter("@name", name);
                return objDd.ExecuteDataSet("uspGetCategoriesByName", CommandType.StoredProcedure);
            }
        }

        private static DataSet GetCategoriesById(string connectionString, int id)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {
                objDd.AddParameter("@Id", id);
                return objDd.ExecuteDataSet("uspGetCategoriesById", CommandType.StoredProcedure);
            }
        }

        private static int RemoveCategory(string connectionString, int accessCodeId)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {

                objDd.AddParameter("@Id", accessCodeId);
                return objDd.ExecuteNonQuery("uspRemoveCategoryById", CommandType.StoredProcedure);
            }
        }

        private static int CreateCategory(string connectionString, CategoriesItem categoryItem)
        {
            if (categoryItem != null)
            {
                using (var objDd = new MsSqlClient(connectionString))
                {

                    objDd.AddParameter("@Id", categoryItem.Id);
                    objDd.AddParameter("@SortingPrio", categoryItem.SortingPriority);
                    objDd.AddParameter("@Value", string.IsNullOrEmpty(categoryItem.Value) ? DBNull.Value : categoryItem.Value);
                    objDd.AddParameter("@SearchNumberNT", categoryItem.SearchNumberNT);
                    objDd.AddParameter("@Notes", string.IsNullOrEmpty(categoryItem.Notes) ? DBNull.Value : categoryItem.Notes);
                    objDd.AddParameter("@DateCreated", string.IsNullOrEmpty(categoryItem.DateCreated) ? DBNull.Value : categoryItem.DateCreated);
                    objDd.AddParameter("@Lastmodified", string.IsNullOrEmpty(categoryItem.LastModified) ? DBNull.Value : categoryItem.LastModified);
                    objDd.AddParameter("@Name", string.IsNullOrEmpty(categoryItem.FacetName) ? DBNull.Value : categoryItem.FacetName);
                    objDd.AddParameter("@SearchNumber", string.IsNullOrEmpty(categoryItem.SearchNumber) ? DBNull.Value : categoryItem.SearchNumber);
                    return objDd.ExecuteNonQuery("uspAddCategory", CommandType.StoredProcedure);
                }
            }
            return 0;

        }

        private static DataSet GetFacetCategoryPublicationTitles(string connectionString, FacetsQueryParameters parameters)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {


                objDd.AddParameter("@PageNo", parameters.Start <= 0 ? 1 : parameters.Start);
                objDd.AddParameter("@PageSize", parameters.Rows <= 0 ? 10 : parameters.Rows);
                return objDd.ExecuteDataSet("uspGetFacetCategoryPublicationTitles", CommandType.StoredProcedure);
            }
        }

        private static DataSet GetFacetCategoryMapping(string categoryName, int categoryId, string connectionString, FacetsQueryParameters parameters)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {
                objDd.AddParameter("@SearchValue", string.IsNullOrWhiteSpace(parameters.Q) ? DBNull.Value : parameters.Q);
                objDd.AddParameter("@CategoryName", categoryName);
                objDd.AddParameter("@CategoryId", categoryId);
                objDd.AddParameter("@PageNo", parameters.Start <= 0 ? 1 : parameters.Start);
                objDd.AddParameter("@PageSize", parameters.Rows <= 0 ? 10 : parameters.Rows);
                objDd.AddParameter("@SortOrder", string.IsNullOrWhiteSpace(parameters.Sort) ? "ASC" : parameters.Sort);
                return objDd.ExecuteDataSet("uspGetFacetCategoryMapping", CommandType.StoredProcedure);
            }

        }

        private static DataSet GetAllFacetCategoryRelation(string connectionString, FacetsQueryParameters parameters)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {


                objDd.AddParameter("@PageNo", parameters.Start <= 0 ? 1 : parameters.Start);
                objDd.AddParameter("@PageSize", parameters.Rows <= 0 ? 10 : parameters.Rows);
                return objDd.ExecuteDataSet("uspGetAllFacetCategoryRelation", CommandType.StoredProcedure);
            }
        }

        #endregion
    }
}
