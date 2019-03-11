// Reference: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\mscorlib.dll
// Reference: C:\Windows\Microsoft.Net\assembly\GAC_MSIL\System.Core\v4.0_4.0.0.0__b77a5c561934e089\System.Core.dll
// Reference: C:\Windows\Microsoft.Net\assembly\GAC_MSIL\System.Configuration\v4.0_4.0.0.0__b03f5f7f11d50a3a\System.Configuration.dll
// Reference: C:\Windows\Microsoft.Net\assembly\GAC_MSIL\System\v4.0_4.0.0.0__b77a5c561934e089\System.dll
// Reference: C:\Windows\Microsoft.Net\assembly\GAC_MSIL\System.Xml\v4.0_4.0.0.0__b77a5c561934e089\System.Xml.dll
// Reference: C:\Windows\Microsoft.Net\assembly\GAC_MSIL\System.ComponentModel.Composition\v4.0_4.0.0.0__b77a5c561934e089\System.ComponentModel.Composition.dll
// Reference: D:\Hoteli\dist\HoteliRhetosServer\bin\Autofac.Integration.Wcf.dll
// Reference: C:\Windows\Microsoft.Net\assembly\GAC_MSIL\System.ServiceModel.Activation\v4.0_4.0.0.0__31bf3856ad364e35\System.ServiceModel.Activation.dll
// Reference: C:\Windows\Microsoft.Net\assembly\GAC_MSIL\System.ServiceModel\v4.0_4.0.0.0__b77a5c561934e089\System.ServiceModel.dll
// Reference: C:\Windows\Microsoft.Net\assembly\GAC_MSIL\System.ServiceModel.Web\v4.0_4.0.0.0__31bf3856ad364e35\System.ServiceModel.Web.dll
// Reference: C:\Windows\Microsoft.Net\assembly\GAC_64\System.Web\v4.0_4.0.0.0__b03f5f7f11d50a3a\System.Web.dll
// Reference: D:\Hoteli\dist\HoteliRhetosServer\bin\Rhetos.Interfaces.dll
// Reference: D:\Hoteli\dist\HoteliRhetosServer\bin\Plugins\Rhetos.Dom.DefaultConcepts.Interfaces.dll
// Reference: D:\Hoteli\dist\HoteliRhetosServer\bin\Rhetos.Logging.Interfaces.dll
// Reference: D:\Hoteli\dist\HoteliRhetosServer\bin\Rhetos.Processing.Interfaces.dll
// Reference: D:\Hoteli\dist\HoteliRhetosServer\bin\Rhetos.Utilities.dll
// Reference: D:\Hoteli\dist\HoteliRhetosServer\bin\Rhetos.XmlSerialization.dll
// Reference: D:\Hoteli\dist\HoteliRhetosServer\bin\Rhetos.Web.dll
// Reference: D:\Hoteli\dist\HoteliRhetosServer\bin\Plugins\Rhetos.RestGenerator.dll
// Reference: D:\Hoteli\dist\HoteliRhetosServer\bin\Generated\ServerDom.Model.dll
// Reference: D:\Hoteli\dist\HoteliRhetosServer\bin\Generated\ServerDom.Orm.dll
// Reference: D:\Hoteli\dist\HoteliRhetosServer\bin\Generated\ServerDom.Repositories.dll
// Reference: D:\Hoteli\dist\HoteliRhetosServer\bin\Autofac.dll
// Reference: D:\Hoteli\dist\HoteliRhetosServer\bin\Plugins\Rhetos.Processing.DefaultCommands.Interfaces.dll
// Reference: D:\Hoteli\dist\HoteliRhetosServer\bin\Newtonsoft.Json.dll
// Reference: D:\Hoteli\dist\HoteliRhetosServer\bin\Plugins\Rhetos.Dom.DefaultConcepts.dll
// CompilerOptions: ""


using Autofac;
using Module = Autofac.Module;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.RestGenerator.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Routing;

namespace Rhetos.Rest
{
    public class RestServiceHostFactory : Autofac.Integration.Wcf.AutofacServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            RestServiceHost host = new RestServiceHost(serviceType, baseAddresses);

            return host;
        }
    }

    public class RestServiceHost : ServiceHost
    {
        private Type _serviceType;

        public RestServiceHost(Type serviceType, Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            _serviceType = serviceType;
        }

        protected override void OnOpening()
        {
            base.OnOpening();
            this.AddServiceEndpoint(_serviceType, new WebHttpBinding("rhetosWebHttpBinding"), string.Empty);
            this.AddServiceEndpoint(_serviceType, new BasicHttpBinding("rhetosBasicHttpBinding"), "SOAP");

            ((ServiceEndpoint)(Description.Endpoints.Where(e => e.Binding is WebHttpBinding).Single())).Behaviors.Add(new WebHttpBehavior()); 
            if (Description.Behaviors.Find<Rhetos.Web.JsonErrorServiceBehavior>() == null)
                Description.Behaviors.Add(new Rhetos.Web.JsonErrorServiceBehavior());
        }
    }

    [System.ComponentModel.Composition.Export(typeof(Module))]
    public class RestServiceModuleConfiguration : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ServiceUtility>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceHoteliHotel>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceHoteliHotelInfo>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceHoteliTipSobe>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceHoteliHotelRezervacijeZaSobu>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceHoteliSoba>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceHoteliGost>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceHoteliGostPrijatelj>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceHoteliGostPoslovno>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceHoteliRezervacija>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceHoteliRezervacijaGrid>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceHoteliHotelGrid>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceHoteliGostRezervacija>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceHoteliRacun>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceHoteliUsluga>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceHoteliStavka>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonAutoCodeCache>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonFilterId>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonKeepSynchronizedMetadata>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonExclusiveLock>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonSetLock>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonReleaseLock>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonLogReader>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonLogRelatedItemReader>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonLog>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonAddToLog>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonLogRelatedItem>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonRelatedEventsSource>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonClaim>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonMyClaim>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonPrincipal>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonPrincipalHasRole>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonRole>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonRoleInheritsRole>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonPrincipalPermission>().InstancePerLifetimeScope();
            builder.RegisterType<RestServiceCommonRolePermission>().InstancePerLifetimeScope();
            /*InitialCodeGenerator.ServiceRegistrationTag*/
            base.Load(builder);
        }
    }

    [System.ComponentModel.Composition.Export(typeof(Rhetos.IService))]
    public class RestServiceInitializer : Rhetos.IService
    {
        public void Initialize()
        {
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Hoteli/Hotel", 
                new RestServiceHostFactory(), typeof(RestServiceHoteliHotel)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Hoteli/HotelInfo", 
                new RestServiceHostFactory(), typeof(RestServiceHoteliHotelInfo)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Hoteli/TipSobe", 
                new RestServiceHostFactory(), typeof(RestServiceHoteliTipSobe)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Hoteli/HotelRezervacijeZaSobu", 
                new RestServiceHostFactory(), typeof(RestServiceHoteliHotelRezervacijeZaSobu)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Hoteli/Soba", 
                new RestServiceHostFactory(), typeof(RestServiceHoteliSoba)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Hoteli/Gost", 
                new RestServiceHostFactory(), typeof(RestServiceHoteliGost)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Hoteli/GostPrijatelj", 
                new RestServiceHostFactory(), typeof(RestServiceHoteliGostPrijatelj)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Hoteli/GostPoslovno", 
                new RestServiceHostFactory(), typeof(RestServiceHoteliGostPoslovno)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Hoteli/Rezervacija", 
                new RestServiceHostFactory(), typeof(RestServiceHoteliRezervacija)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Hoteli/RezervacijaGrid", 
                new RestServiceHostFactory(), typeof(RestServiceHoteliRezervacijaGrid)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Hoteli/HotelGrid", 
                new RestServiceHostFactory(), typeof(RestServiceHoteliHotelGrid)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Hoteli/GostRezervacija", 
                new RestServiceHostFactory(), typeof(RestServiceHoteliGostRezervacija)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Hoteli/Racun", 
                new RestServiceHostFactory(), typeof(RestServiceHoteliRacun)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Hoteli/Usluga", 
                new RestServiceHostFactory(), typeof(RestServiceHoteliUsluga)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Hoteli/Stavka", 
                new RestServiceHostFactory(), typeof(RestServiceHoteliStavka)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/AutoCodeCache", 
                new RestServiceHostFactory(), typeof(RestServiceCommonAutoCodeCache)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/FilterId", 
                new RestServiceHostFactory(), typeof(RestServiceCommonFilterId)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/KeepSynchronizedMetadata", 
                new RestServiceHostFactory(), typeof(RestServiceCommonKeepSynchronizedMetadata)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/ExclusiveLock", 
                new RestServiceHostFactory(), typeof(RestServiceCommonExclusiveLock)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/SetLock", 
                new RestServiceHostFactory(), typeof(RestServiceCommonSetLock)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/ReleaseLock", 
                new RestServiceHostFactory(), typeof(RestServiceCommonReleaseLock)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/LogReader", 
                new RestServiceHostFactory(), typeof(RestServiceCommonLogReader)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/LogRelatedItemReader", 
                new RestServiceHostFactory(), typeof(RestServiceCommonLogRelatedItemReader)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/Log", 
                new RestServiceHostFactory(), typeof(RestServiceCommonLog)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/AddToLog", 
                new RestServiceHostFactory(), typeof(RestServiceCommonAddToLog)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/LogRelatedItem", 
                new RestServiceHostFactory(), typeof(RestServiceCommonLogRelatedItem)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/RelatedEventsSource", 
                new RestServiceHostFactory(), typeof(RestServiceCommonRelatedEventsSource)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/Claim", 
                new RestServiceHostFactory(), typeof(RestServiceCommonClaim)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/MyClaim", 
                new RestServiceHostFactory(), typeof(RestServiceCommonMyClaim)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/Principal", 
                new RestServiceHostFactory(), typeof(RestServiceCommonPrincipal)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/PrincipalHasRole", 
                new RestServiceHostFactory(), typeof(RestServiceCommonPrincipalHasRole)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/Role", 
                new RestServiceHostFactory(), typeof(RestServiceCommonRole)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/RoleInheritsRole", 
                new RestServiceHostFactory(), typeof(RestServiceCommonRoleInheritsRole)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/PrincipalPermission", 
                new RestServiceHostFactory(), typeof(RestServiceCommonPrincipalPermission)));
            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Rest/Common/RolePermission", 
                new RestServiceHostFactory(), typeof(RestServiceCommonRolePermission)));
            /*InitialCodeGenerator.ServiceInitializationTag*/
        }

        public void InitializeApplicationInstance(System.Web.HttpApplication context)
        {
        }
    }


    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceHoteliHotel
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Hoteli.Hotel*/

        public RestServiceHoteliHotel(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Hoteli.Hotel*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Hoteli.Hotel*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Hoteli.Hotel*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Hoteli.Hotel> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Hotel>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Hoteli.Hotel> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Hotel>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Hotel>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Hoteli.Hotel> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Hoteli.Hotel>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Hoteli.Hotel GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Hoteli.Hotel>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertHoteliHotel(Hoteli.Hotel entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateHoteliHotel(string id, Hoteli.Hotel entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteHoteliHotel(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Hoteli.Hotel { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Hoteli.Hotel*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceHoteliHotelInfo
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Hoteli.HotelInfo*/

        public RestServiceHoteliHotelInfo(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Hoteli.HotelInfo*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Hoteli.HotelInfo*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Hoteli.HotelInfo*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Hoteli.HotelInfo> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.HotelInfo>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Hoteli.HotelInfo> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.HotelInfo>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.HotelInfo>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Hoteli.HotelInfo> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Hoteli.HotelInfo>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Hoteli.HotelInfo GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Hoteli.HotelInfo>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        /*DataStructureInfo AdditionalOperations Hoteli.HotelInfo*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceHoteliTipSobe
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Hoteli.TipSobe*/

        public RestServiceHoteliTipSobe(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Hoteli.TipSobe*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Hoteli.TipSobe*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                Tuple.Create("Hoteli.NazivMisspelled", typeof(Hoteli.NazivMisspelled)),
                Tuple.Create("NazivMisspelled", typeof(Hoteli.NazivMisspelled)),
                /*DataStructureInfo FilterTypes Hoteli.TipSobe*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Hoteli.TipSobe> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.TipSobe>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Hoteli.TipSobe> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.TipSobe>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.TipSobe>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Hoteli.TipSobe> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Hoteli.TipSobe>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Hoteli.TipSobe GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Hoteli.TipSobe>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertHoteliTipSobe(Hoteli.TipSobe entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateHoteliTipSobe(string id, Hoteli.TipSobe entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteHoteliTipSobe(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Hoteli.TipSobe { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Hoteli.TipSobe*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceHoteliHotelRezervacijeZaSobu
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Hoteli.HotelRezervacijeZaSobu*/

        public RestServiceHoteliHotelRezervacijeZaSobu(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Hoteli.HotelRezervacijeZaSobu*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Hoteli.HotelRezervacijeZaSobu*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Hoteli.HotelRezervacijeZaSobu*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Hoteli.HotelRezervacijeZaSobu> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.HotelRezervacijeZaSobu>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Hoteli.HotelRezervacijeZaSobu> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.HotelRezervacijeZaSobu>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.HotelRezervacijeZaSobu>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Hoteli.HotelRezervacijeZaSobu> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Hoteli.HotelRezervacijeZaSobu>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Hoteli.HotelRezervacijeZaSobu GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Hoteli.HotelRezervacijeZaSobu>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        /*DataStructureInfo AdditionalOperations Hoteli.HotelRezervacijeZaSobu*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceHoteliSoba
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Hoteli.Soba*/

        public RestServiceHoteliSoba(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Hoteli.Soba*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Hoteli.Soba*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                Tuple.Create("Hoteli.SearchPenthausFilterBy", typeof(Hoteli.SearchPenthausFilterBy)),
                Tuple.Create("SearchPenthausFilterBy", typeof(Hoteli.SearchPenthausFilterBy)),
                Tuple.Create("Hoteli.SearchPenthaus", typeof(Hoteli.SearchPenthaus)),
                Tuple.Create("SearchPenthaus", typeof(Hoteli.SearchPenthaus)),
                /*DataStructureInfo FilterTypes Hoteli.Soba*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Hoteli.Soba> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Soba>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Hoteli.Soba> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Soba>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Soba>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Hoteli.Soba> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Hoteli.Soba>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Hoteli.Soba GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Hoteli.Soba>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertHoteliSoba(Hoteli.Soba entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateHoteliSoba(string id, Hoteli.Soba entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteHoteliSoba(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Hoteli.Soba { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Hoteli.Soba*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceHoteliGost
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Hoteli.Gost*/

        public RestServiceHoteliGost(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Hoteli.Gost*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Hoteli.Gost*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                Tuple.Create("Hoteli.Email_RegExMatchFilter", typeof(Hoteli.Email_RegExMatchFilter)),
                Tuple.Create("Email_RegExMatchFilter", typeof(Hoteli.Email_RegExMatchFilter)),
                /*DataStructureInfo FilterTypes Hoteli.Gost*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Hoteli.Gost> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Gost>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Hoteli.Gost> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Gost>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Gost>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Hoteli.Gost> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Hoteli.Gost>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Hoteli.Gost GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Hoteli.Gost>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertHoteliGost(Hoteli.Gost entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateHoteliGost(string id, Hoteli.Gost entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteHoteliGost(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Hoteli.Gost { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Hoteli.Gost*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceHoteliGostPrijatelj
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Hoteli.GostPrijatelj*/

        public RestServiceHoteliGostPrijatelj(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Hoteli.GostPrijatelj*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Hoteli.GostPrijatelj*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Hoteli.GostPrijatelj*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Hoteli.GostPrijatelj> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.GostPrijatelj>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Hoteli.GostPrijatelj> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.GostPrijatelj>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.GostPrijatelj>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Hoteli.GostPrijatelj> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Hoteli.GostPrijatelj>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Hoteli.GostPrijatelj GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Hoteli.GostPrijatelj>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertHoteliGostPrijatelj(Hoteli.GostPrijatelj entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateHoteliGostPrijatelj(string id, Hoteli.GostPrijatelj entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteHoteliGostPrijatelj(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Hoteli.GostPrijatelj { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Hoteli.GostPrijatelj*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceHoteliGostPoslovno
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Hoteli.GostPoslovno*/

        public RestServiceHoteliGostPoslovno(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Hoteli.GostPoslovno*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Hoteli.GostPoslovno*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Hoteli.GostPoslovno*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Hoteli.GostPoslovno> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.GostPoslovno>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Hoteli.GostPoslovno> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.GostPoslovno>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.GostPoslovno>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Hoteli.GostPoslovno> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Hoteli.GostPoslovno>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Hoteli.GostPoslovno GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Hoteli.GostPoslovno>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertHoteliGostPoslovno(Hoteli.GostPoslovno entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateHoteliGostPoslovno(string id, Hoteli.GostPoslovno entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteHoteliGostPoslovno(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Hoteli.GostPoslovno { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Hoteli.GostPoslovno*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceHoteliRezervacija
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Hoteli.Rezervacija*/

        public RestServiceHoteliRezervacija(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Hoteli.Rezervacija*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Hoteli.Rezervacija*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                Tuple.Create("Hoteli.VrijemeOdPrijeVrijemeDo", typeof(Hoteli.VrijemeOdPrijeVrijemeDo)),
                Tuple.Create("VrijemeOdPrijeVrijemeDo", typeof(Hoteli.VrijemeOdPrijeVrijemeDo)),
                /*DataStructureInfo FilterTypes Hoteli.Rezervacija*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Hoteli.Rezervacija> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Rezervacija>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Hoteli.Rezervacija> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Rezervacija>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Rezervacija>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Hoteli.Rezervacija> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Hoteli.Rezervacija>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Hoteli.Rezervacija GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Hoteli.Rezervacija>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertHoteliRezervacija(Hoteli.Rezervacija entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateHoteliRezervacija(string id, Hoteli.Rezervacija entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteHoteliRezervacija(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Hoteli.Rezervacija { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Hoteli.Rezervacija*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceHoteliRezervacijaGrid
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Hoteli.RezervacijaGrid*/

        public RestServiceHoteliRezervacijaGrid(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Hoteli.RezervacijaGrid*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Hoteli.RezervacijaGrid*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Hoteli.RezervacijaGrid*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Hoteli.RezervacijaGrid> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.RezervacijaGrid>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Hoteli.RezervacijaGrid> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.RezervacijaGrid>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.RezervacijaGrid>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Hoteli.RezervacijaGrid> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Hoteli.RezervacijaGrid>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Hoteli.RezervacijaGrid GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Hoteli.RezervacijaGrid>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        /*DataStructureInfo AdditionalOperations Hoteli.RezervacijaGrid*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceHoteliHotelGrid
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Hoteli.HotelGrid*/

        public RestServiceHoteliHotelGrid(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Hoteli.HotelGrid*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Hoteli.HotelGrid*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Hoteli.HotelGrid*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Hoteli.HotelGrid> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.HotelGrid>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Hoteli.HotelGrid> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.HotelGrid>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.HotelGrid>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Hoteli.HotelGrid> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Hoteli.HotelGrid>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Hoteli.HotelGrid GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Hoteli.HotelGrid>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        /*DataStructureInfo AdditionalOperations Hoteli.HotelGrid*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceHoteliGostRezervacija
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Hoteli.GostRezervacija*/

        public RestServiceHoteliGostRezervacija(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Hoteli.GostRezervacija*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Hoteli.GostRezervacija*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Hoteli.GostRezervacija*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Hoteli.GostRezervacija> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.GostRezervacija>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Hoteli.GostRezervacija> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.GostRezervacija>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.GostRezervacija>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Hoteli.GostRezervacija> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Hoteli.GostRezervacija>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Hoteli.GostRezervacija GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Hoteli.GostRezervacija>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertHoteliGostRezervacija(Hoteli.GostRezervacija entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateHoteliGostRezervacija(string id, Hoteli.GostRezervacija entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteHoteliGostRezervacija(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Hoteli.GostRezervacija { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Hoteli.GostRezervacija*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceHoteliRacun
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Hoteli.Racun*/

        public RestServiceHoteliRacun(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Hoteli.Racun*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Hoteli.Racun*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                Tuple.Create("Hoteli.Popust_MaxValueFilter", typeof(Hoteli.Popust_MaxValueFilter)),
                Tuple.Create("Popust_MaxValueFilter", typeof(Hoteli.Popust_MaxValueFilter)),
                Tuple.Create("Hoteli.Popust_MinValueFilter", typeof(Hoteli.Popust_MinValueFilter)),
                Tuple.Create("Popust_MinValueFilter", typeof(Hoteli.Popust_MinValueFilter)),
                /*DataStructureInfo FilterTypes Hoteli.Racun*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Hoteli.Racun> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Racun>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Hoteli.Racun> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Racun>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Racun>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Hoteli.Racun> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Hoteli.Racun>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Hoteli.Racun GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Hoteli.Racun>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertHoteliRacun(Hoteli.Racun entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateHoteliRacun(string id, Hoteli.Racun entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteHoteliRacun(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Hoteli.Racun { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Hoteli.Racun*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceHoteliUsluga
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Hoteli.Usluga*/

        public RestServiceHoteliUsluga(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Hoteli.Usluga*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Hoteli.Usluga*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Hoteli.Usluga*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Hoteli.Usluga> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Usluga>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Hoteli.Usluga> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Usluga>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Usluga>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Hoteli.Usluga> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Hoteli.Usluga>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Hoteli.Usluga GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Hoteli.Usluga>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertHoteliUsluga(Hoteli.Usluga entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateHoteliUsluga(string id, Hoteli.Usluga entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteHoteliUsluga(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Hoteli.Usluga { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Hoteli.Usluga*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceHoteliStavka
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Hoteli.Stavka*/

        public RestServiceHoteliStavka(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Hoteli.Stavka*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Hoteli.Stavka*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                Tuple.Create("Hoteli.SystemRequiredBrojStavke", typeof(Hoteli.SystemRequiredBrojStavke)),
                Tuple.Create("SystemRequiredBrojStavke", typeof(Hoteli.SystemRequiredBrojStavke)),
                /*DataStructureInfo FilterTypes Hoteli.Stavka*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Hoteli.Stavka> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Stavka>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Hoteli.Stavka> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Stavka>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Hoteli.Stavka>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Hoteli.Stavka> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Hoteli.Stavka>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Hoteli.Stavka GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Hoteli.Stavka>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertHoteliStavka(Hoteli.Stavka entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateHoteliStavka(string id, Hoteli.Stavka entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteHoteliStavka(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Hoteli.Stavka { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Hoteli.Stavka*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonAutoCodeCache
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Common.AutoCodeCache*/

        public RestServiceCommonAutoCodeCache(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Common.AutoCodeCache*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Common.AutoCodeCache*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Common.AutoCodeCache*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Common.AutoCodeCache> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Common.AutoCodeCache>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Common.AutoCodeCache> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.AutoCodeCache>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.AutoCodeCache>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Common.AutoCodeCache> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Common.AutoCodeCache>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Common.AutoCodeCache GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Common.AutoCodeCache>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertCommonAutoCodeCache(Common.AutoCodeCache entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateCommonAutoCodeCache(string id, Common.AutoCodeCache entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteCommonAutoCodeCache(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Common.AutoCodeCache { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Common.AutoCodeCache*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonFilterId
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Common.FilterId*/

        public RestServiceCommonFilterId(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Common.FilterId*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Common.FilterId*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Common.FilterId*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Common.FilterId> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Common.FilterId>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Common.FilterId> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.FilterId>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.FilterId>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Common.FilterId> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Common.FilterId>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Common.FilterId GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Common.FilterId>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertCommonFilterId(Common.FilterId entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateCommonFilterId(string id, Common.FilterId entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteCommonFilterId(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Common.FilterId { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Common.FilterId*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonKeepSynchronizedMetadata
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Common.KeepSynchronizedMetadata*/

        public RestServiceCommonKeepSynchronizedMetadata(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Common.KeepSynchronizedMetadata*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Common.KeepSynchronizedMetadata*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Common.KeepSynchronizedMetadata*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Common.KeepSynchronizedMetadata> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Common.KeepSynchronizedMetadata>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Common.KeepSynchronizedMetadata> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.KeepSynchronizedMetadata>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.KeepSynchronizedMetadata>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Common.KeepSynchronizedMetadata> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Common.KeepSynchronizedMetadata>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Common.KeepSynchronizedMetadata GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Common.KeepSynchronizedMetadata>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertCommonKeepSynchronizedMetadata(Common.KeepSynchronizedMetadata entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateCommonKeepSynchronizedMetadata(string id, Common.KeepSynchronizedMetadata entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteCommonKeepSynchronizedMetadata(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Common.KeepSynchronizedMetadata { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Common.KeepSynchronizedMetadata*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonExclusiveLock
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Common.ExclusiveLock*/

        public RestServiceCommonExclusiveLock(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Common.ExclusiveLock*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Common.ExclusiveLock*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Common.ExclusiveLock*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Common.ExclusiveLock> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Common.ExclusiveLock>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Common.ExclusiveLock> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.ExclusiveLock>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.ExclusiveLock>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Common.ExclusiveLock> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Common.ExclusiveLock>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Common.ExclusiveLock GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Common.ExclusiveLock>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertCommonExclusiveLock(Common.ExclusiveLock entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateCommonExclusiveLock(string id, Common.ExclusiveLock entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteCommonExclusiveLock(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Common.ExclusiveLock { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Common.ExclusiveLock*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonSetLock
    {
        private ServiceUtility _serviceUtility;

        public RestServiceCommonSetLock(ServiceUtility serviceUtility) 
        {
            _serviceUtility = serviceUtility;
        }

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void ExecuteCommonSetLock(Common.SetLock action)
        {
            _serviceUtility.Execute<Common.SetLock>(action);
        }
    }


    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonReleaseLock
    {
        private ServiceUtility _serviceUtility;

        public RestServiceCommonReleaseLock(ServiceUtility serviceUtility) 
        {
            _serviceUtility = serviceUtility;
        }

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void ExecuteCommonReleaseLock(Common.ReleaseLock action)
        {
            _serviceUtility.Execute<Common.ReleaseLock>(action);
        }
    }


    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonLogReader
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Common.LogReader*/

        public RestServiceCommonLogReader(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Common.LogReader*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Common.LogReader*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Common.LogReader*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Common.LogReader> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Common.LogReader>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Common.LogReader> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.LogReader>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.LogReader>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Common.LogReader> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Common.LogReader>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Common.LogReader GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Common.LogReader>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        /*DataStructureInfo AdditionalOperations Common.LogReader*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonLogRelatedItemReader
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Common.LogRelatedItemReader*/

        public RestServiceCommonLogRelatedItemReader(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Common.LogRelatedItemReader*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Common.LogRelatedItemReader*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Common.LogRelatedItemReader*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Common.LogRelatedItemReader> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Common.LogRelatedItemReader>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Common.LogRelatedItemReader> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.LogRelatedItemReader>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.LogRelatedItemReader>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Common.LogRelatedItemReader> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Common.LogRelatedItemReader>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Common.LogRelatedItemReader GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Common.LogRelatedItemReader>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        /*DataStructureInfo AdditionalOperations Common.LogRelatedItemReader*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonLog
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Common.Log*/

        public RestServiceCommonLog(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Common.Log*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Common.Log*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Common.Log*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Common.Log> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Common.Log>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Common.Log> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.Log>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.Log>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Common.Log> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Common.Log>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Common.Log GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Common.Log>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertCommonLog(Common.Log entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateCommonLog(string id, Common.Log entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteCommonLog(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Common.Log { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Common.Log*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonAddToLog
    {
        private ServiceUtility _serviceUtility;

        public RestServiceCommonAddToLog(ServiceUtility serviceUtility) 
        {
            _serviceUtility = serviceUtility;
        }

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void ExecuteCommonAddToLog(Common.AddToLog action)
        {
            _serviceUtility.Execute<Common.AddToLog>(action);
        }
    }


    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonLogRelatedItem
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Common.LogRelatedItem*/

        public RestServiceCommonLogRelatedItem(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Common.LogRelatedItem*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Common.LogRelatedItem*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                Tuple.Create("Common.SystemRequiredLog", typeof(Common.SystemRequiredLog)),
                Tuple.Create("SystemRequiredLog", typeof(Common.SystemRequiredLog)),
                /*DataStructureInfo FilterTypes Common.LogRelatedItem*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Common.LogRelatedItem> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Common.LogRelatedItem>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Common.LogRelatedItem> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.LogRelatedItem>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.LogRelatedItem>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Common.LogRelatedItem> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Common.LogRelatedItem>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Common.LogRelatedItem GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Common.LogRelatedItem>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertCommonLogRelatedItem(Common.LogRelatedItem entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateCommonLogRelatedItem(string id, Common.LogRelatedItem entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteCommonLogRelatedItem(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Common.LogRelatedItem { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Common.LogRelatedItem*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonRelatedEventsSource
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Common.RelatedEventsSource*/

        public RestServiceCommonRelatedEventsSource(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Common.RelatedEventsSource*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Common.RelatedEventsSource*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Common.RelatedEventsSource*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Common.RelatedEventsSource> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Common.RelatedEventsSource>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Common.RelatedEventsSource> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.RelatedEventsSource>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.RelatedEventsSource>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Common.RelatedEventsSource> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Common.RelatedEventsSource>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Common.RelatedEventsSource GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Common.RelatedEventsSource>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        /*DataStructureInfo AdditionalOperations Common.RelatedEventsSource*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonClaim
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Common.Claim*/

        public RestServiceCommonClaim(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Common.Claim*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Common.Claim*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                Tuple.Create("Rhetos.Dom.DefaultConcepts.ActiveItems", typeof(Rhetos.Dom.DefaultConcepts.ActiveItems)),
                Tuple.Create("ActiveItems", typeof(Rhetos.Dom.DefaultConcepts.ActiveItems)),
                Tuple.Create("Common.SystemRequiredActive", typeof(Common.SystemRequiredActive)),
                Tuple.Create("SystemRequiredActive", typeof(Common.SystemRequiredActive)),
                /*DataStructureInfo FilterTypes Common.Claim*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Common.Claim> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Common.Claim>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Common.Claim> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.Claim>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.Claim>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Common.Claim> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Common.Claim>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Common.Claim GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Common.Claim>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertCommonClaim(Common.Claim entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateCommonClaim(string id, Common.Claim entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteCommonClaim(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Common.Claim { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Common.Claim*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonMyClaim
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Common.MyClaim*/

        public RestServiceCommonMyClaim(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Common.MyClaim*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Common.MyClaim*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                Tuple.Create("Common.Claim", typeof(Common.Claim)),
                Tuple.Create("Claim", typeof(Common.Claim)),
                Tuple.Create("IEnumerable<Common.Claim>", typeof(IEnumerable<Common.Claim>)),
                /*DataStructureInfo FilterTypes Common.MyClaim*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Common.MyClaim> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Common.MyClaim>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Common.MyClaim> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.MyClaim>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.MyClaim>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Common.MyClaim> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Common.MyClaim>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Common.MyClaim GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Common.MyClaim>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        /*DataStructureInfo AdditionalOperations Common.MyClaim*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonPrincipal
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Common.Principal*/

        public RestServiceCommonPrincipal(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Common.Principal*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Common.Principal*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Common.Principal*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Common.Principal> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Common.Principal>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Common.Principal> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.Principal>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.Principal>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Common.Principal> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Common.Principal>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Common.Principal GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Common.Principal>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertCommonPrincipal(Common.Principal entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateCommonPrincipal(string id, Common.Principal entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteCommonPrincipal(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Common.Principal { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Common.Principal*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonPrincipalHasRole
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Common.PrincipalHasRole*/

        public RestServiceCommonPrincipalHasRole(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Common.PrincipalHasRole*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Common.PrincipalHasRole*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                Tuple.Create("Common.SystemRequiredPrincipal", typeof(Common.SystemRequiredPrincipal)),
                Tuple.Create("SystemRequiredPrincipal", typeof(Common.SystemRequiredPrincipal)),
                /*DataStructureInfo FilterTypes Common.PrincipalHasRole*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Common.PrincipalHasRole> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Common.PrincipalHasRole>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Common.PrincipalHasRole> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.PrincipalHasRole>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.PrincipalHasRole>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Common.PrincipalHasRole> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Common.PrincipalHasRole>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Common.PrincipalHasRole GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Common.PrincipalHasRole>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertCommonPrincipalHasRole(Common.PrincipalHasRole entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateCommonPrincipalHasRole(string id, Common.PrincipalHasRole entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteCommonPrincipalHasRole(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Common.PrincipalHasRole { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Common.PrincipalHasRole*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonRole
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Common.Role*/

        public RestServiceCommonRole(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Common.Role*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Common.Role*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                /*DataStructureInfo FilterTypes Common.Role*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Common.Role> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Common.Role>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Common.Role> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.Role>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.Role>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Common.Role> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Common.Role>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Common.Role GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Common.Role>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertCommonRole(Common.Role entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateCommonRole(string id, Common.Role entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteCommonRole(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Common.Role { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Common.Role*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonRoleInheritsRole
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Common.RoleInheritsRole*/

        public RestServiceCommonRoleInheritsRole(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Common.RoleInheritsRole*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Common.RoleInheritsRole*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                Tuple.Create("Common.SystemRequiredUsersFrom", typeof(Common.SystemRequiredUsersFrom)),
                Tuple.Create("SystemRequiredUsersFrom", typeof(Common.SystemRequiredUsersFrom)),
                /*DataStructureInfo FilterTypes Common.RoleInheritsRole*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Common.RoleInheritsRole> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Common.RoleInheritsRole>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Common.RoleInheritsRole> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.RoleInheritsRole>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.RoleInheritsRole>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Common.RoleInheritsRole> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Common.RoleInheritsRole>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Common.RoleInheritsRole GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Common.RoleInheritsRole>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertCommonRoleInheritsRole(Common.RoleInheritsRole entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateCommonRoleInheritsRole(string id, Common.RoleInheritsRole entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteCommonRoleInheritsRole(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Common.RoleInheritsRole { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Common.RoleInheritsRole*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonPrincipalPermission
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Common.PrincipalPermission*/

        public RestServiceCommonPrincipalPermission(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Common.PrincipalPermission*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Common.PrincipalPermission*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                Tuple.Create("Common.SystemRequiredPrincipal", typeof(Common.SystemRequiredPrincipal)),
                Tuple.Create("SystemRequiredPrincipal", typeof(Common.SystemRequiredPrincipal)),
                Tuple.Create("Common.SystemRequiredClaim", typeof(Common.SystemRequiredClaim)),
                Tuple.Create("SystemRequiredClaim", typeof(Common.SystemRequiredClaim)),
                /*DataStructureInfo FilterTypes Common.PrincipalPermission*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Common.PrincipalPermission> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Common.PrincipalPermission>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Common.PrincipalPermission> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.PrincipalPermission>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.PrincipalPermission>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Common.PrincipalPermission> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Common.PrincipalPermission>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Common.PrincipalPermission GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Common.PrincipalPermission>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertCommonPrincipalPermission(Common.PrincipalPermission entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateCommonPrincipalPermission(string id, Common.PrincipalPermission entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteCommonPrincipalPermission(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Common.PrincipalPermission { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Common.PrincipalPermission*/
    }
    
    [System.ServiceModel.ServiceContract]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceCommonRolePermission
    {
        private ServiceUtility _serviceUtility;
        /*DataStructureInfo AdditionalPropertyInitialization Common.RolePermission*/

        public RestServiceCommonRolePermission(ServiceUtility serviceUtility/*DataStructureInfo AdditionalPropertyConstructorParameter Common.RolePermission*/)
        {
            _serviceUtility = serviceUtility;
            /*DataStructureInfo AdditionalPropertyConstructorSetProperties Common.RolePermission*/
        }
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {
                Tuple.Create("Common.SystemRequiredRole", typeof(Common.SystemRequiredRole)),
                Tuple.Create("SystemRequiredRole", typeof(Common.SystemRequiredRole)),
                Tuple.Create("Common.SystemRequiredClaim", typeof(Common.SystemRequiredClaim)),
                Tuple.Create("SystemRequiredClaim", typeof(Common.SystemRequiredClaim)),
                /*DataStructureInfo FilterTypes Common.RolePermission*/
            }
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsResult<Common.RolePermission> Get(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            var data = _serviceUtility.GetData<Common.RolePermission>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<Common.RolePermission> { Records = data.Records };
        }

        [Obsolete]
        [OperationContract]
        [WebGet(UriTemplate = "/Count?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public CountResult GetCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.RolePermission>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult { TotalRecords = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [OperationContract]
        [WebGet(UriTemplate = "/TotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public TotalCountResult GetTotalCount(string filter, string fparam, string genericfilter, string filters, string sort)
        {
            var data = _serviceUtility.GetData<Common.RolePermission>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult { TotalCount = data.TotalCount };
        }

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [OperationContract]
        [WebGet(UriTemplate = "/RecordsAndTotalCount?filter={filter}&fparam={fparam}&genericfilter={genericfilter}&filters={filters}&top={top}&skip={skip}&page={page}&psize={psize}&sort={sort}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RecordsAndTotalCountResult<Common.RolePermission> GetRecordsAndTotalCount(string filter, string fparam, string genericfilter, string filters, int top, int skip, int page, int psize, string sort)
        {
            return _serviceUtility.GetData<Common.RolePermission>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }

        [OperationContract]
        [WebGet(UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Common.RolePermission GetById(string id)
        {
            var result = _serviceUtility.GetDataById<Common.RolePermission>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException("There is no resource of this type with a given ID.") { HttpStatusCode = HttpStatusCode.NotFound, Severe = false };
            return result;
        }

        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public InsertDataResult InsertCommonRolePermission(Common.RolePermission entity)
        {
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult { ID = entity.ID };
        }

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void UpdateCommonRolePermission(string id, Common.RolePermission entity)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException("Given entity ID is not equal to resource ID from URI.");

            _serviceUtility.UpdateData(entity);
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "{id}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void DeleteCommonRolePermission(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException("Invalid format of GUID parametar 'ID'.");
            var entity = new Common.RolePermission { ID = guid };

            _serviceUtility.DeleteData(entity);
        }

/*DataStructureInfo AdditionalOperations Common.RolePermission*/
    }
    /*InitialCodeGenerator.RhetosRestClassesTag*/

}

