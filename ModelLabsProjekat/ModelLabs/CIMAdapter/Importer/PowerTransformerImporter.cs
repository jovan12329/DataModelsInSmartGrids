using System;
using System.Collections.Generic;
using CIM.Model;
using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	/// <summary>
	/// PowerTransformerImporter
	/// </summary>
	public class PowerTransformerImporter
	{
		/// <summary> Singleton </summary>
		private static PowerTransformerImporter ptImporter = null;
		private static object singletoneLock = new object();

		private ConcreteModel concreteModel;
		private Delta delta;
		private ImportHelper importHelper;
		private TransformAndLoadReport report;


		#region Properties
		public static PowerTransformerImporter Instance
		{
			get
			{
				if (ptImporter == null)
				{
					lock (singletoneLock)
					{
						if (ptImporter == null)
						{
							ptImporter = new PowerTransformerImporter();
							ptImporter.Reset();
						}
					}
				}
				return ptImporter;
			}
		}

		public Delta NMSDelta
		{
			get 
			{
				return delta;
			}
		}
		#endregion Properties


		public void Reset()
		{
			concreteModel = null;
			delta = new Delta();
			importHelper = new ImportHelper();
			report = null;
		}

		public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
		{
			LogManager.Log("Importing PowerTransformer Elements...", LogLevel.Info);
			report = new TransformAndLoadReport();
			concreteModel = cimConcreteModel;
			delta.ClearDeltaOperations();

			if ((concreteModel != null) && (concreteModel.ModelMap != null))
			{
				try
				{
					// convert into DMS elements
					ConvertModelAndPopulateDelta();
				}
				catch (Exception ex)
				{
					string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
					LogManager.Log(message);
					report.Report.AppendLine(ex.Message);
					report.Success = false;
				}
			}
			LogManager.Log("Importing PowerTransformer Elements - END.", LogLevel.Info);
			return report;
		}

		/// <summary>
		/// Method performs conversion of network elements from CIM based concrete model into DMS model.
		/// </summary>
		private void ConvertModelAndPopulateDelta()
		{
			LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

			
			//My Imports
			 
			 ImportTerminals();
			 ImportSeasons();
			 ImportDayTypes();
			 ImportRegulatingControls();
			 ImportAsyncMachines();
			 ImportRegulationSchedules();
			 
			 


			LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
		}

        #region MyImports

		//BEGIN_TERMINAL
        private void ImportTerminals()
        {
            SortedDictionary<string, object> cimTerminals = concreteModel.GetAllObjectsOfType("FTN.Terminal");
            if (cimTerminals != null)
            {
                foreach (KeyValuePair<string, object> cimTerminalPair in cimTerminals)
                {
                    FTN.Terminal cimTerminal = cimTerminalPair.Value as FTN.Terminal;

                    ResourceDescription rd = CreateTerminalResourceDescription(cimTerminal);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Terminal ID = ").Append(cimTerminal.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Terminal ID = ").Append(cimTerminal.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }

        }

        private ResourceDescription CreateTerminalResourceDescription(FTN.Terminal cimTerminal)
        {
            ResourceDescription rd = null;
            if (cimTerminal != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.TERMINAL, importHelper.CheckOutIndexForDMSType(DMSType.TERMINAL));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimTerminal.ID, gid);

            }
            return rd;

        }
        //END_TERMINAL



        //BEGIN_SEASON
        private void ImportSeasons()
        {
            SortedDictionary<string, object> cimSeasons = concreteModel.GetAllObjectsOfType("FTN.Season");
            if (cimSeasons != null)
            {
                foreach (KeyValuePair<string, object> cimSeasonPair in cimSeasons)
                {
                    FTN.Season cimSeason = cimSeasonPair.Value as FTN.Season;

                    ResourceDescription rd = CreateSeasonResourceDescription(cimSeason);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Season ID = ").Append(cimSeason.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Season ID = ").Append(cimSeason.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }

        }

        private ResourceDescription CreateSeasonResourceDescription(FTN.Season cimSeason)
        {
            ResourceDescription rd = null;
            if (cimSeason != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.SEASON, importHelper.CheckOutIndexForDMSType(DMSType.SEASON));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimSeason.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateSeasonProperties(cimSeason, rd);
            }
            return rd;

        }
        //END_SEASON


        //BEGIN_DAYTYPES
        private void ImportDayTypes()
        {
            SortedDictionary<string, object> cimTypes = concreteModel.GetAllObjectsOfType("FTN.DayType");
            if (cimTypes != null)
            {
                foreach (KeyValuePair<string, object> cimTypePair in cimTypes)
                {
                    FTN.DayType cimType = cimTypePair.Value as FTN.DayType;

                    ResourceDescription rd = CreateDayTypeResourceDescription(cimType);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("DayType ID = ").Append(cimType.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("DayType ID = ").Append(cimType.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }

        }

        private ResourceDescription CreateDayTypeResourceDescription(FTN.DayType cimType)
        {
            ResourceDescription rd = null;
            if (cimType != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.DAYTYPE, importHelper.CheckOutIndexForDMSType(DMSType.DAYTYPE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimType.ID, gid);

            }
            return rd;

        }
        //END_DAYTYPES


        //BEGIN_REGULATINGCONTROLS
        private void ImportRegulatingControls()
        {
            SortedDictionary<string, object> cimControls = concreteModel.GetAllObjectsOfType("FTN.RegulatingControl");
            if (cimControls != null)
            {
                foreach (KeyValuePair<string, object> cimControlPair in cimControls)
                {
                    FTN.RegulatingControl cimControl = cimControlPair.Value as FTN.RegulatingControl;

                    ResourceDescription rd = CreateRegulatingControlResourceDescription(cimControl);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("RegulatingControl ID = ").Append(cimControl.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("RegulatingControl ID = ").Append(cimControl.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }

        }

        private ResourceDescription CreateRegulatingControlResourceDescription(FTN.RegulatingControl cimControl)
        {
            ResourceDescription rd = null;
            if (cimControl != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.REGULATINGCONTROL, importHelper.CheckOutIndexForDMSType(DMSType.REGULATINGCONTROL));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimControl.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateRegulatingControlProperties(cimControl, rd, importHelper, report);
            }
            return rd;

        }
        //END_REGULATINGCONTROLS


        //BEGIN_ASYNCMACHINES
        private void ImportAsyncMachines()
        {
            SortedDictionary<string, object> cimAsyncs = concreteModel.GetAllObjectsOfType("FTN.AsynchronousMachine");
            if (cimAsyncs != null)
            {
                foreach (KeyValuePair<string, object> cimAsyncPair in cimAsyncs)
                {
                    FTN.AsynchronousMachine cimAsync = cimAsyncPair.Value as FTN.AsynchronousMachine;

                    ResourceDescription rd = CreateAsyncMachineResourceDescription(cimAsync);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("AsynchronousMachine ID = ").Append(cimAsync.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("AsynchronousMachine ID = ").Append(cimAsync.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }

        }

        private ResourceDescription CreateAsyncMachineResourceDescription(FTN.AsynchronousMachine cimAsync)
        {
            ResourceDescription rd = null;
            if (cimAsync != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.ASYNCMACHINE, importHelper.CheckOutIndexForDMSType(DMSType.ASYNCMACHINE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimAsync.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateAsyncMachineProperties(cimAsync, rd, importHelper, report);
            }
            return rd;

        }
        //END_ASYNCMACHINES


        //BEGIN_REGULATIONSCHEDULES
        private void ImportRegulationSchedules()
        {
            SortedDictionary<string, object> cimScheds = concreteModel.GetAllObjectsOfType("FTN.RegulationSchedule");
            if (cimScheds != null)
            {
                foreach (KeyValuePair<string, object> cimSchedPair in cimScheds)
                {
                    FTN.RegulationSchedule cimSched = cimSchedPair.Value as FTN.RegulationSchedule;

                    ResourceDescription rd = CreateRegulationScheduleResourceDescription(cimSched);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("RegulationSchedule ID = ").Append(cimSched.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("RegulationSchedule ID = ").Append(cimSched.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }

        }

        private ResourceDescription CreateRegulationScheduleResourceDescription(FTN.RegulationSchedule cimSched)
        {
            ResourceDescription rd = null;
            if (cimSched != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.REGULATIONSCHEDULE, importHelper.CheckOutIndexForDMSType(DMSType.REGULATIONSCHEDULE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimSched.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateRegulationScheduleProperties(cimSched, rd, importHelper, report);
            }
            return rd;

        }
        //END_REGULATIONSCHEDULES







        #endregion

        
	}
}

