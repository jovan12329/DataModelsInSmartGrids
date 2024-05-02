using FTN.Common;
using FTN.ServiceContracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkModelClient
{
    public class ClientGda : IDisposable
    {

        private ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();

        private NetworkModelGDAProxy gdaQueryProxy = null;
        private NetworkModelGDAProxy GdaQueryProxy
        {
            get
            {
                if (gdaQueryProxy != null)
                {
                    gdaQueryProxy.Abort();
                    gdaQueryProxy = null;
                }

                gdaQueryProxy = new NetworkModelGDAProxy("NetworkModelGDAEndpoint");
                gdaQueryProxy.Open();

                return gdaQueryProxy;
            }
        }

        public ClientGda()
        {
        }

        #region GDAQueryService



        public IList<string> ServerGids()
        {

            List<DMSType> allConcreteTypes = modelResourcesDesc.AllDMSTypes.ToList();
            allConcreteTypes.Remove(DMSType.MASK_TYPE);
            List<long> ids = new List<long>(0);
            List<string> sids = new List<string>();
            foreach (DMSType type in allConcreteTypes)
            {
                ModelCode m = modelResourcesDesc.GetModelCodeFromType(type);
                ids.AddRange(GetExtentValues(m));
                
            }

            foreach (var e in ids) {
                sids.Add($"0x{e.ToString("x16")}");

            }

            return sids;

        }


        public IList<ModelCode> GetConcreteModelCodes() {

            List<DMSType> allConcreteTypes = modelResourcesDesc.AllDMSTypes.ToList();
            allConcreteTypes.Remove(DMSType.MASK_TYPE);
            List<ModelCode> mcs = new List<ModelCode>();


            foreach (DMSType type in allConcreteTypes)
            {
                ModelCode m = modelResourcesDesc.GetModelCodeFromType(type);
                mcs.Add(m);

            }



            return mcs;
        
        }

            



        public IList<ModelCode> GetModelCodesForSelectedGid(long gid) {

            short type = ModelCodeHelper.ExtractTypeFromGlobalId(gid);
            List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds((DMSType)type);

            

            return properties;

        }


        public IList<ModelCode> GetConcreteMCProperties(ModelCode m) {

            DMSType type = ModelCodeHelper.GetTypeFromModelCode(m);
            List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(type);

            return properties;


        }



        public ResourceDescription GetSelectedPropertiesForGid(long gid, IList<ModelCode> propertiesIds)
        {

           ResourceDescription rd= GdaQueryProxy.GetValues(gid, propertiesIds.ToList());

           return rd;



        }




        public ResourceDescription GetValues(long globalId)
        {
            string message = "Getting values method started.";
            Console.WriteLine(message);
            CommonTrace.WriteTrace(CommonTrace.TraceError, message);


            ResourceDescription rd = null;

            try
            {

                short type = ModelCodeHelper.ExtractTypeFromGlobalId(globalId);
                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds((DMSType)type);

                rd = GdaQueryProxy.GetValues(globalId, properties);

                message = "Getting values method successfully finished.";
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }
            catch (Exception e)
            {
                message = string.Format("Getting values method for entered id = {0} failed.\n\t{1}", globalId, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }
            finally
            {

            }

            return rd;
        }



        public List<long> MyGetExtentValues(ModelCode modelCode, List<ModelCode> ls, out List<ResourceDescription> rdds)
        {
            //string message = "Getting extent values method started.";
            //Console.WriteLine(message);
            //CommonTrace.WriteTrace(CommonTrace.TraceError, message);


            int iteratorId = 0;
            List<long> ids = new List<long>();
            rdds = new List<ResourceDescription>();

            try
            {
                int numberOfResources = 2;
                int resourcesLeft = 0;

                List<ModelCode> properties = ls;

                iteratorId = GdaQueryProxy.GetExtentValues(modelCode, properties);
                resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);




                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                    for (int i = 0; i < rds.Count; i++)
                    {
                        ids.Add(rds[i].Id);

                    }

                    rdds.AddRange(rds);

                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                }

                GdaQueryProxy.IteratorClose(iteratorId);

                //message = "Getting extent values method successfully finished.";
                //Console.WriteLine(message);
                //CommonTrace.WriteTrace(CommonTrace.TraceError, message);

            }
            catch (Exception e)
            {
                string message = string.Format("Getting extent values method failed for {0}.\n\t{1}", modelCode, e.Message);
                //Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }
            finally
            {

            }

            return ids;
        }





        public List<long> GetExtentValues(ModelCode modelCode)
        {
            string message = "Getting extent values method started.";
            Console.WriteLine(message);
            CommonTrace.WriteTrace(CommonTrace.TraceError, message);

            
            int iteratorId = 0;
            List<long> ids = new List<long>();

            try
            {
                int numberOfResources = 2;
                int resourcesLeft = 0;

                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(modelCode);

                iteratorId = GdaQueryProxy.GetExtentValues(modelCode, properties);
                resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);


                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                    for (int i = 0; i < rds.Count; i++)
                    {
                        ids.Add(rds[i].Id);
                        
                    }

                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                }

                GdaQueryProxy.IteratorClose(iteratorId);

                message = "Getting extent values method successfully finished.";
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);

            }
            catch (Exception e)
            {
                message = string.Format("Getting extent values method failed for {0}.\n\t{1}", modelCode, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }
            finally
            {
                
            }

            return ids;
        }

        public List<long> GetRelatedValues(long sourceGlobalId, Association association)
        {
            string message = "Getting related values method started.";
            Console.WriteLine(message);
            CommonTrace.WriteTrace(CommonTrace.TraceError, message);

            List<long> resultIds = new List<long>();



            int numberOfResources = 2;

            try
            {
                List<ModelCode> properties = new List<ModelCode>();


                int iteratorId = GdaQueryProxy.GetRelatedValues(sourceGlobalId, properties, association);
                int resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);



                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                    for (int i = 0; i < rds.Count; i++)
                    {
                        resultIds.Add(rds[i].Id);

                    }

                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                }

                GdaQueryProxy.IteratorClose(iteratorId);

                message = "Getting related values method successfully finished.";
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }
            catch (Exception e)
            {
                message = string.Format("Getting related values method  failed for sourceGlobalId = {0} and association (propertyId = {1}, type = {2}). Reason: {3}", sourceGlobalId, association.PropertyId, association.Type, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }
            finally
            {

            }

            return resultIds;
        }

        #endregion GDAQueryService

        #region Test Methods

        public List<long> TestGetExtentValuesAllTypes()
        {
            string message = "Getting extent values for all DMS types started.";
            Console.WriteLine(message);
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

            List<ModelCode> properties = new List<ModelCode>();
            List<long> ids = new List<long>();

            int iteratorId = 0;
            int numberOfResources = 1000;
            DMSType currType = 0;
            try
            {
                foreach (DMSType type in Enum.GetValues(typeof(DMSType)))
                {
                    currType = type;
                    properties = modelResourcesDesc.GetAllPropertyIds(type);

                    iteratorId = GdaQueryProxy.GetExtentValues(modelResourcesDesc.GetModelCodeFromType(type), properties);
                    int count = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                    while (count > 0)
                    {
                        List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                        for (int i = 0; i < rds.Count; i++)
                        {
                            ids.Add(rds[i].Id);
                        }

                        count = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                    }

                    bool ok = GdaQueryProxy.IteratorClose(iteratorId);

                    message = string.Format("Number of {0} in model {1}.", type, ids.Count);
                    Console.WriteLine(message);
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                }


                message = "Getting extent values for all DMS types successfully ended.";
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
            }

            catch (Exception e)
            {
                message = string.Format("Getting extent values for all DMS types failed for type {0}.\n\t{1}", currType, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

                throw;
            }

            return ids;
        }



        #endregion Test Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
