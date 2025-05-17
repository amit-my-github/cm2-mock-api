// Ignore Spelling: Api

using Content.Manager.Core.WebApi.Model.Parameters;
using Content.Manager.Core.WebApi.Model.Publication;
using LI.Data.Framework;
using System.Data;
using static Content.Manager.Core.WebApi.Model.Publication.PublicationTitlesOverViewResponse;

namespace Asp.Net.Core.WebApi.CrudOperations.Services
{
    public static class PublicationServiceProvider
    {

        #region Methods

        public static PublicationTitlesOverViewResponse GetAll(PublicationQueryParameters parameters)
        {
            var res = new PublicationTitlesOverViewResponse();
            var connectionString = GetConnection();
            var publications = GetAllPublications(connectionString, parameters);
            if (publications != null && publications.Tables.Count > 0 && publications.Tables[0].Rows.Count > 0)
            {
                var publishers = publications.Tables[0].AsEnumerable().Select(x => x.Field<string>("PublisherName")).Distinct().ToList();
                ICollection<DataRow>? publicationsTitles;
                IList<PublicationTitleOverView>? titles;
                var totalItems = 0;
                foreach (var publisher in publishers)
                {
                    publicationsTitles = publications.Tables[0].AsEnumerable().Where(x => x.Field<string>("PublisherName") == publisher).Select(x => x).ToList();
                    titles = new List<PublicationTitleOverView>();
                    foreach (var publicationTitle in publicationsTitles)
                    {
                        var title = new PublicationTitleOverView
                        {
                            ExternalCode = new List<string>(),
                            Id = publicationTitle.Field<int>("Id"),
                            DisplayCode = publicationTitle.Field<string>("DisplayCode"),
                            DateCreated = publicationTitle.Field<DateTime>("DateCreated").ToString("yyyy-MM-ddTHH:mm:ss"),
                            Status = publicationTitle.Field<string>("Status"),
                            DateModified = publicationTitle.Field<DateTime>("LastModified").ToString("yyyy-MM-ddTHH:mm:ss"),
                            Usage = 0,
                            Type = publicationTitle.Field<string>("Type")
                        };
                        var exts = publicationTitle.Field<string>("ExternalCode").Split(',');
                        foreach (var item in exts)
                        {
                            title.ExternalCode.Add(item);
                        }
                        titles.Add(title);
                        totalItems = publicationTitle.Field<int>("TotalRows");

                    }
                    if (res.PublicationTitles != null)
                    {
                        res.PublicationTitles.Add(new PublicationTitlesOverView
                        {
                            PublisherName = publisher,
                            Titles = titles
                        });
                    }
                }

                res.TotalItems = totalItems;
            }

            return res;
        }

        public static int Create(PublicationTitleItem publicationTitleItem)
        {
            var connectionString = GetConnection();
            CreatePublicationTitle(connectionString, publicationTitleItem);
            var publicationId = MaxId(connectionString);
            var externalCode = publicationTitleItem.ExternalCodes;
            var thesaurus = publicationTitleItem.Thesaurus;
            if (externalCode != null && externalCode.Any())
            {
                foreach (var code in externalCode)
                {
                    AddExternalCode(connectionString, publicationId, code);
                }
            }
            if (thesaurus != null && thesaurus.Any())
            {
                foreach (var item in thesaurus)
                {
                    AddSynonym(connectionString, publicationId, item);
                }
            }
            return publicationId;
        }

        public static PublicationTitleItem? GetById(int id)
        {

            var pub = new PublicationTitleItem();
            var connectionString = GetConnection();
            var publication = GetPublicationDetailsById(connectionString, id);
            if (publication != null && publication.Tables.Count > 0 && publication.Tables[0].Rows.Count > 0)
            {

                pub.Type = publication.Tables[0].Rows[0].Field<string?>("Type");
                pub.Name = publication.Tables[0].Rows[0].Field<string?>("Name");
                pub.DisplayCode = publication.Tables[0].Rows[0].Field<string?>("DisplayCode");
                pub.AccessCodeId = publication.Tables[0].Rows[0].Field<int>("AccessCodeID");
                pub.IsExcludeFromAllInSubscription = publication.Tables[0].Rows[0].Field<bool>("ExcludeFromAllInSubscription");
                pub.IsDisplayOnAccessRightsPage = publication.Tables[0].Rows[0].Field<bool>("IsHiddenOnAccessRights");
                pub.Isbn = publication.Tables[0].Rows[0].Field<string?>("IsbnIssn");
                pub.IsPowerFilter = publication.Tables[0].Rows[0].Field<bool>("PowerFilter");
                pub.MonitorLimit = publication.Tables[0].Rows[0].Field<int>("MonitorLimit");
                pub.IsReference = publication.Tables[0].Rows[0].Field<bool>("IsNaslagwerk");
                pub.IsOverAllTitle = publication.Tables[0].Rows[0].Field<bool>("IsOverAllTitle");
                pub.HasParts = publication.Tables[0].Rows[0].Field<bool>("HasParts");
                if (publication.Tables[0].Rows[0].Field<int?>("PartOf") != null)
                {
                    pub.PartOf = publication.Tables[0].Rows[0].Field<int>("PartOf");
                }

                pub.FeedUrl = publication.Tables[0].Rows[0].Field<string?>("FeedUrl");
                pub.ContentsUrl = publication.Tables[0].Rows[0].Field<string?>("ContentsUrl");
                pub.InSourcePage = publication.Tables[0].Rows[0].Field<bool>("InSourcePage");
                if (publication.Tables[0].Rows[0].Field<string?>("PublicationStart") != null)
                {
                    pub.PublicationStart = publication.Tables[0].Rows[0].Field<string?>("PublicationStart");
                }
                if (publication.Tables[0].Rows[0].Field<string?>("PublicationEnd") != null)
                {
                    pub.PublicationEnd = publication.Tables[0].Rows[0].Field<string>("PublicationEnd");
                }
                if (publication.Tables[0].Rows[0].Field<DateTime?>("Embargo") != null)
                {
                    pub.EmbargoDate = publication.Tables[0].Rows[0].Field<DateTime>("Embargo").ToString("yyyy-MM-ddTHH:mm:ss");
                }
                if (publication.Tables[0].Rows[0].Field<DateTime?>("LastModified") != null)
                {
                    pub.LastModified = publication.Tables[0].Rows[0].Field<DateTime>("LastModified").ToString("yyyy-MM-ddTHH:mm:ss");
                }
                if (publication.Tables[0].Rows[0].Field<DateTime?>("dateCreated") != null)
                {
                    pub.DateCreated = publication.Tables[0].Rows[0].Field<DateTime>("dateCreated").ToString("yyyy-MM-ddTHH:mm:ss");
                }
                pub.PubAvailabilityInBasicService = publication.Tables[0].Rows[0].Field<string?>("PubAvailibilityBS");
                pub.PublicationOrderInformation = publication.Tables[0].Rows[0].Field<string?>("PubOrderInfo");
                pub.PublicationSpecialFeatures = publication.Tables[0].Rows[0].Field<string?>("PubSpecFeatures");
                pub.PublicationDescription = publication.Tables[0].Rows[0].Field<string?>("PubDesc");
                pub.Notes = publication.Tables[0].Rows[0].Field<string?>("notes");

                pub.PublisherID = publication.Tables[0].Rows[0].Field<int>("PublisherID");
                pub.Status = publication.Tables[0].Rows[0].Field<string?>("Status");
                pub.AccessCode = publication.Tables[0].Rows[0].Field<string?>("AccessCode");
                pub.Id = publication.Tables[0].Rows[0].Field<int>("Id");

            }
            if (publication != null && publication.Tables.Count > 0 && publication.Tables[1].Rows.Count > 0)
            {
                pub.ExternalCodes = new List<ExternalCode>();
                foreach (DataRow dt in publication.Tables[1].Rows)
                {
                    pub.ExternalCodes.Add(new ExternalCode { Code = dt.Field<string?>("ExternalCode"), IsPrimary = dt.Field<bool>("IsPrimary") });
                }
            }
            if (publication != null && publication.Tables.Count > 0 && publication.Tables[2].Rows.Count > 0)
            {

                pub.Thesaurus = new List<string>();
                foreach (DataRow dt in publication.Tables[2].Rows)
                {
                    pub.Thesaurus.Add(dt.Field<string?>("Synonym") ?? string.Empty);
                }
            }
            return pub;

        }

        public static PublicationTitleResponse? GetDetailsById(int id)
        {
            var res = new PublicationTitleResponse();
            var connectionString = GetConnection();
            var publication = GetPublicationDetailsById(connectionString, id);
            if (publication != null && publication.Tables.Count > 0 && publication.Tables[0].Rows.Count > 0)
            {
                var pub = new PublicationTitle();
                pub.Id = publication.Tables[0].Rows[0].Field<int>("Id");
                pub.DisplayCode = publication.Tables[0].Rows[0].Field<string?>("DisplayCode");
                pub.ExternalCode = publication.Tables[0].Rows[0].Field<string?>("ExternalCode");
                pub.DateCreated = publication.Tables[0].Rows[0].Field<DateTime>("DateCreated").ToString("yyyy-MM-ddTHH:mm:ss");
                pub.Status = publication.Tables[0].Rows[0].Field<string?>("Status");
                pub.DateModified = publication.Tables[0].Rows[0].Field<DateTime>("LastModified").ToString("yyyy-MM-ddTHH:mm:ss");
                pub.AccessCode = publication.Tables[0].Rows[0].Field<string?>("AccessCode");
                pub.Name = publication.Tables[0].Rows[0].Field<string?>("Name");
                pub.Type = publication.Tables[0].Rows[0].Field<string?>("Type");
                res.Title = pub;

            }
            return res;

        }

        public static bool Update(int Id, PublicationTitleItem publicationTitleItem)
        {

            var connectionString = GetConnection();
            if (publicationTitleItem != null)
            {
                RemovSynonyms(connectionString, Id);
                RemovExternalCode(connectionString, Id);
                using (var objDd = new MsSqlClient(connectionString))
                {
                    objDd.AddParameter("@Type", string.IsNullOrEmpty(publicationTitleItem.Type) ? DBNull.Value : publicationTitleItem.Type);
                    objDd.AddParameter("@Name", string.IsNullOrEmpty(publicationTitleItem.Name) ? DBNull.Value : publicationTitleItem.Name);
                    objDd.AddParameter("@DisplayCode", string.IsNullOrEmpty(publicationTitleItem.DisplayCode) ? DBNull.Value : publicationTitleItem.DisplayCode);
                    objDd.AddParameter("@AccessCodeID", publicationTitleItem.AccessCodeId);
                    objDd.AddParameter("@IsExcludeFromAllInSubscription", publicationTitleItem.IsExcludeFromAllInSubscription);
                    objDd.AddParameter("@IsDisplayOnAccessRightsPage", publicationTitleItem.IsDisplayOnAccessRightsPage);
                    objDd.AddParameter("@Status", string.IsNullOrEmpty(publicationTitleItem.Status) ? "N" : publicationTitleItem.Status);
                    objDd.AddParameter("@Isbn", string.IsNullOrEmpty(publicationTitleItem.Isbn) ? DBNull.Value : publicationTitleItem.Isbn);
                    objDd.AddParameter("@IsPowerFilter", publicationTitleItem.IsPowerFilter);
                    objDd.AddParameter("@MonitorLimit", publicationTitleItem.MonitorLimit);
                    objDd.AddParameter("@IsReference", publicationTitleItem.IsReference);
                    objDd.AddParameter("@IsOverAllTitle", publicationTitleItem.IsOverAllTitle);
                    objDd.AddParameter("@HasParts", publicationTitleItem.HasParts);
                    objDd.AddParameter("@PartOf", publicationTitleItem.PartOf.HasValue ? publicationTitleItem.PartOf : DBNull.Value);
                    objDd.AddParameter("@FeedUrl", string.IsNullOrEmpty(publicationTitleItem.FeedUrl) ? DBNull.Value : publicationTitleItem.FeedUrl);
                    objDd.AddParameter("@ContentsUrl", string.IsNullOrEmpty(publicationTitleItem.ContentsUrl) ? DBNull.Value : publicationTitleItem.ContentsUrl);
                    objDd.AddParameter("@InSourcePage", publicationTitleItem.InSourcePage);
                    objDd.AddParameter("@PublicationStart", string.IsNullOrEmpty(publicationTitleItem.PublicationStart) ? DBNull.Value : publicationTitleItem.PublicationStart);
                    objDd.AddParameter("@EmbargoDate", string.IsNullOrEmpty(publicationTitleItem.EmbargoDate) ? DBNull.Value : publicationTitleItem.EmbargoDate);
                    objDd.AddParameter("@PublicationEnd", string.IsNullOrEmpty(publicationTitleItem.PublicationEnd) ? DBNull.Value : publicationTitleItem.PublicationEnd);
                    objDd.AddParameter("@PubAvailabilityInBasicService", string.IsNullOrEmpty(publicationTitleItem.PubAvailabilityInBasicService) ? DBNull.Value : publicationTitleItem.PubAvailabilityInBasicService);
                    objDd.AddParameter("@PublicationOrderInformation", string.IsNullOrEmpty(publicationTitleItem.PublicationOrderInformation) ? DBNull.Value : publicationTitleItem.PublicationOrderInformation);
                    objDd.AddParameter("@PublicationSpecialFeatures", string.IsNullOrEmpty(publicationTitleItem.PublicationSpecialFeatures) ? DBNull.Value : publicationTitleItem.PublicationSpecialFeatures);
                    objDd.AddParameter("@PublicationDescription", string.IsNullOrEmpty(publicationTitleItem.PublicationDescription) ? DBNull.Value : publicationTitleItem.PublicationDescription);
                    objDd.AddParameter("@ContentManagerNotes", string.IsNullOrEmpty(publicationTitleItem.Notes) ? DBNull.Value : publicationTitleItem.Notes);
                    objDd.AddParameter("@DateCreated", string.IsNullOrEmpty(publicationTitleItem.DateCreated) ? DateTime.Now : publicationTitleItem.DateCreated);
                    objDd.AddParameter("@LastModified", DateTime.Now);
                    objDd.AddParameter("@PublisherID", publicationTitleItem.PublisherID);
                    objDd.AddParameter("@ID", Id);
                    objDd.ExecuteNonQuery("uspUpdatePublicationTitle", CommandType.StoredProcedure);
                }

                var externalCode = publicationTitleItem.ExternalCodes;
                var thesaurus = publicationTitleItem.Thesaurus;
                if (externalCode != null && externalCode.Any())
                {
                    foreach (var code in externalCode)
                    {
                        AddExternalCode(connectionString, Id, code);
                    }
                }
                if (thesaurus != null && thesaurus.Any())
                {
                    foreach (var item in thesaurus)
                    {
                        AddSynonym(connectionString, Id, item);
                    }
                }
                return true;
            }
            return false;

        }

        public static bool Remove(int publicationTitleId)
        {
            var connectionString = GetConnection();
            return RemovePulicationTitle(connectionString, publicationTitleId) > 0;

        }

        public static int GetPublicationTitleId(int publicationTitleId)
        {
            var connectionString = GetConnection();
            using (var objDd = new MsSqlClient(connectionString))
            {
                objDd.AddParameter("@PublicationTitleId", publicationTitleId);
                var publicationDetails = objDd.ExecuteDataSet("uspGetPublicationDetailsById", CommandType.StoredProcedure);
                return publicationDetails != null && publicationDetails.Tables.Count > 0 && publicationDetails.Tables[0].Rows.Count > 0 ? publicationDetails.Tables[0].Rows[0].Field<int>("ID") : 0;
            }

        }

        public static PublicationTitlesResponse GetPublicationTitlesByAccessCode(string accessCode, PublicationQueryParameters parameters)
        {
            var connectionString = GetConnection();
            var res = new PublicationTitlesResponse();

            var publicationTitles = GetPublicationByAccessCode(connectionString, accessCode, parameters);

            if (publicationTitles != null && publicationTitles.Tables.Count > 0 && publicationTitles.Tables[0].Rows.Count > 0)
            {
                if (res.PublicationTitles != null)
                {
                    foreach (DataRow publicationTitle in publicationTitles.Tables[0].AsEnumerable())
                    {
                        res.PublicationTitles.Add(new PublicationTitles
                        {
                            PublisherName = publicationTitle.Field<string>("PublisherName"),
                            Titles = new List<PublicationTitle>
                {
                    new PublicationTitle
                    {
                        Id = publicationTitle.Field<int>("ID"),
                        DisplayCode = publicationTitle.Field<string?>("DisplayCode"),
                        DateCreated = publicationTitle.Field<DateTime>("DateCreated").ToString("yyyy-MM-dd HH:mm"),
                        Status = publicationTitle.Field<string>("Status"),
                        DateModified = publicationTitle.Field<DateTime>("LastModified").ToString("yyyy-MM-dd HH:mm"),
                        Usage = 0,
                        Name = publicationTitle.Field<string>("Name"),
                        Type = publicationTitle.Field<string>("Type"),
                        ExternalCode = publicationTitle.Field<string>("ExternalCode")

                    },

                }
                        });
                    }
                }
                res.TotalItems = publicationTitles.Tables[0].Rows[0].Field<int>("TotalRows");
            }

            return res;
        }
        public static bool IsValidRequest(int id)
        {
            return IsValid(GetConnection(), id);

        }

        private static string GetConnection()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();
            var connectionString = config["db.dev.conn"];
            return connectionString;
        }

        private static DataSet GetAllPublications(string connectionString, PublicationQueryParameters parameters)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {

                objDd.AddParameter("@SearchValue", string.IsNullOrWhiteSpace(parameters.Q) ? DBNull.Value : parameters.Q);
                objDd.AddParameter("@PageNo", parameters.Start <= 0 ? 1 : parameters.Start);
                objDd.AddParameter("@PageSize", parameters.Rows <= 0 ? 10 : parameters.Rows);
                objDd.AddParameter("@SortColumn", string.IsNullOrWhiteSpace(parameters.Field) ? "PublisherName" : parameters.Field);
                objDd.AddParameter("@SortOrder", string.IsNullOrWhiteSpace(parameters.Sort) ? "ASC" : parameters.Sort);
                objDd.AddParameter("@Type", string.IsNullOrWhiteSpace(parameters.Type) ? DBNull.Value : parameters.Type);
                objDd.AddParameter("@ExternalCode", string.IsNullOrWhiteSpace(parameters.ExtCode) ? "" : parameters.ExtCode);
                if (parameters.Feed.HasValue)
                {
                    objDd.AddParameter("@Feed", parameters.Feed.Value ? "Y" : "N");
                }
                if (parameters.IsActive.HasValue)
                {
                    objDd.AddParameter("@Status", parameters.IsActive.Value ? "Y" : "N");
                }
                return objDd.ExecuteDataSet("uspGetAllPublications", CommandType.StoredProcedure);
            }
        }

        private static DataSet GetPublicationDetailsById(string connectionString, int publicationTitleId)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {
                objDd.AddParameter("@PublicationTitleId", publicationTitleId);
                return objDd.ExecuteDataSet("uspGetPublicationDetailsById", CommandType.StoredProcedure);
            }
        }
        private static int MaxId(string connectionString)
        {
            using (var objDd = new MsSqlClient(connectionString))
            {

                var db = objDd.ExecuteDataSet("uspGetMaxPublicationId", CommandType.StoredProcedure);
                return db.Tables.Count > 0 && db.Tables[0].Rows.Count > 0 ? db.Tables[0].Rows[0].Field<int>("ID") : 0;
            }
        }


        private static int RemovePulicationTitle(string connectionString, int pulicationTitleId)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {

                objDd.AddParameter("@PulicationTitleId", pulicationTitleId);
                return objDd.ExecuteNonQuery("uspRemovePulicationTitleById", CommandType.StoredProcedure);
            }
        }

        private static int CreatePublicationTitle(string connectionString, PublicationTitleItem publicationTitleItem)
        {


            if (publicationTitleItem != null)
            {
                using (var objDd = new MsSqlClient(connectionString))
                {
                    objDd.AddParameter("@Type", string.IsNullOrEmpty(publicationTitleItem.Type) ? DBNull.Value : publicationTitleItem.Type);
                    objDd.AddParameter("@Name", string.IsNullOrEmpty(publicationTitleItem.Name) ? DBNull.Value : publicationTitleItem.Name);
                    objDd.AddParameter("@DisplayCode", string.IsNullOrEmpty(publicationTitleItem.DisplayCode) ? DBNull.Value : publicationTitleItem.DisplayCode);
                    objDd.AddParameter("@AccessCodeID", publicationTitleItem.AccessCodeId);
                    objDd.AddParameter("@IsExcludeFromAllInSubscription", publicationTitleItem.IsExcludeFromAllInSubscription);
                    objDd.AddParameter("@IsDisplayOnAccessRightsPage", publicationTitleItem.IsDisplayOnAccessRightsPage);
                    objDd.AddParameter("@Status", string.IsNullOrEmpty(publicationTitleItem.Status) ? "N" : publicationTitleItem.Status);
                    objDd.AddParameter("@Isbn", string.IsNullOrEmpty(publicationTitleItem.Isbn) ? DBNull.Value : publicationTitleItem.Isbn);
                    objDd.AddParameter("@IsPowerFilter", publicationTitleItem.IsPowerFilter);
                    objDd.AddParameter("@MonitorLimit", publicationTitleItem.MonitorLimit);
                    objDd.AddParameter("@IsReference", publicationTitleItem.IsReference);
                    objDd.AddParameter("@IsOverAllTitle", publicationTitleItem.IsOverAllTitle);
                    objDd.AddParameter("@HasParts", publicationTitleItem.HasParts);
                    objDd.AddParameter("@PartOf", publicationTitleItem.PartOf.HasValue ? publicationTitleItem.PartOf : DBNull.Value);
                    objDd.AddParameter("@FeedUrl", string.IsNullOrEmpty(publicationTitleItem.FeedUrl) ? DBNull.Value : publicationTitleItem.FeedUrl);
                    objDd.AddParameter("@ContentsUrl", string.IsNullOrEmpty(publicationTitleItem.ContentsUrl) ? DBNull.Value : publicationTitleItem.ContentsUrl);
                    objDd.AddParameter("@InSourcePage", publicationTitleItem.InSourcePage);
                    objDd.AddParameter("@PublicationStart", string.IsNullOrEmpty(publicationTitleItem.PublicationStart) ? DBNull.Value : publicationTitleItem.PublicationStart);
                    objDd.AddParameter("@EmbargoDate", string.IsNullOrEmpty(publicationTitleItem.EmbargoDate) ? DBNull.Value : publicationTitleItem.EmbargoDate);
                    objDd.AddParameter("@PublicationEnd", string.IsNullOrEmpty(publicationTitleItem.PublicationEnd) ? DBNull.Value : publicationTitleItem.PublicationEnd);
                    objDd.AddParameter("@PubAvailabilityInBasicService", string.IsNullOrEmpty(publicationTitleItem.PubAvailabilityInBasicService) ? DBNull.Value : publicationTitleItem.PubAvailabilityInBasicService);
                    objDd.AddParameter("@PublicationOrderInformation", string.IsNullOrEmpty(publicationTitleItem.PublicationOrderInformation) ? DBNull.Value : publicationTitleItem.PublicationOrderInformation);
                    objDd.AddParameter("@PublicationSpecialFeatures", string.IsNullOrEmpty(publicationTitleItem.PublicationSpecialFeatures) ? DBNull.Value : publicationTitleItem.PublicationSpecialFeatures);
                    objDd.AddParameter("@PublicationDescription", string.IsNullOrEmpty(publicationTitleItem.PublicationDescription) ? DBNull.Value : publicationTitleItem.PublicationDescription);
                    objDd.AddParameter("@ContentManagerNotes", string.IsNullOrEmpty(publicationTitleItem.Notes) ? DBNull.Value : publicationTitleItem.Notes);
                    objDd.AddParameter("@DateCreated", DateTime.Now);
                    objDd.AddParameter("@PublisherID", publicationTitleItem.PublisherID);
                    return objDd.ExecuteNonQuery("uspAddPublicationTitle", CommandType.StoredProcedure);

                }
            }
            return 0;

        }

        private static void AddExternalCode(string connectionString, int publicationId, ExternalCode externalCode)
        {

            if (externalCode != null)
            {
                using (var objDd = new MsSqlClient(connectionString))
                {
                    objDd.AddParameter("@IsPrimary", externalCode.IsPrimary);
                    objDd.AddParameter("@ExternalCode", string.IsNullOrEmpty(externalCode.Code) ? DBNull.Value : externalCode.Code);
                    objDd.AddParameter("@DateCreated", DateTime.Now);
                    objDd.AddParameter("@PublisherID", publicationId);
                    objDd.ExecuteNonQuery("uspAddExternalCode", CommandType.StoredProcedure);

                }
            }


        }

        private static void RemovExternalCode(string connectionString, int Id)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {
                objDd.AddParameter("@Id", Id);
                objDd.ExecuteNonQuery("uspRemovePublicationTitleCodesById", CommandType.StoredProcedure);

            }

        }

        private static void RemovSynonyms(string connectionString, int Id)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {
                objDd.AddParameter("@Id", Id);
                objDd.ExecuteNonQuery("uspRemovePublicationTitleSynonymsById", CommandType.StoredProcedure);

            }

        }

        private static void AddSynonym(string connectionString, int PublicationId, string Synonym)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {
                objDd.AddParameter("@Synonym", string.IsNullOrEmpty(Synonym) ? DBNull.Value : Synonym);
                objDd.AddParameter("@DateCreated", DateTime.Now);
                objDd.AddParameter("@PublicationId", PublicationId);
                objDd.ExecuteNonQuery("uspAddSynonyms", CommandType.StoredProcedure);

            }
        }

        private static DataSet GetPublicationByAccessCode(string connectionString, string accessCode, PublicationQueryParameters parameters)
        {
            using (var objDd = new MsSqlClient(connectionString))
            {
                objDd.AddParameter("@AccessCode", accessCode);
                objDd.AddParameter("@SearchValue", string.IsNullOrWhiteSpace(parameters.Q) ? DBNull.Value : parameters.Q);
                objDd.AddParameter("@PageNo", parameters.Start <= 0 ? 1 : parameters.Start);
                objDd.AddParameter("@PageSize", parameters.Rows <= 0 ? 10 : parameters.Rows);
                objDd.AddParameter("@SortColumn", string.IsNullOrWhiteSpace(parameters.Field) ? "Display code" : parameters.Field);
                objDd.AddParameter("@SortOrder", string.IsNullOrWhiteSpace(parameters.Sort) ? "ASC" : parameters.Sort);
                objDd.AddParameter("@Type", string.IsNullOrEmpty(parameters.Type) ? DBNull.Value : parameters.Type);
                objDd.AddParameter("@Status", string.IsNullOrEmpty(parameters.Status) ? DBNull.Value : parameters.Status);

                return objDd.ExecuteDataSet("uspGetTitlesByAccessCode", CommandType.StoredProcedure);
            }
        }

        private static bool IsValid(string connectionString, int publicationId)
        {

            using (var objDd = new MsSqlClient(connectionString))
            {
                objDd.AddParameter("@PublicationId", publicationId);
                var obj = objDd.ExecuteDataSet("uspIsValidPublicationId", CommandType.StoredProcedure);
                return obj != null && obj.Tables.Count > 0 && obj.Tables[0].Rows.Count > 0;

            }
        }

        #endregion
    }
}
