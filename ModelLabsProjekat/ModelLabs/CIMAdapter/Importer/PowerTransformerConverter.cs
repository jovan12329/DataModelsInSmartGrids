namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	using FTN.Common;
    using System;

    /// <summary>
    /// PowerTransformerConverter has methods for populating
    /// ResourceDescription objects using PowerTransformerCIMProfile_Labs objects.
    /// </summary>
	public static class PowerTransformerConverter
	{


        #region MyPopulateResourceDescription

        public static void PopulateBasicIntervalScheduleProperties(FTN.BasicIntervalSchedule cimBIS, ResourceDescription rd)
        {
            if ((cimBIS != null) && (rd != null))
            {
                if (cimBIS.StartTimeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASICINTERVALSCHEDULE_STARTTIME, cimBIS.StartTime));
                }
                if (cimBIS.Value1MultiplierHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASICINTERVALSCHEDULE_VALUE1MULTIPL, (short)GetDMSUnitMultiplier(cimBIS.Value1Multiplier)));
                }
                if (cimBIS.Value1UnitHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASICINTERVALSCHEDULE_VALUE1UNIT, (short)GetDMSUnitSymbol(cimBIS.Value1Unit)));
                }
                if (cimBIS.Value2MultiplierHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASICINTERVALSCHEDULE_VALUE2MULTIPL, (short)GetDMSUnitMultiplier(cimBIS.Value2Multiplier)));
                }
                if (cimBIS.Value2UnitHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASICINTERVALSCHEDULE_VALUE2UNIT, (short)GetDMSUnitSymbol(cimBIS.Value2Unit)));
                }
                
            }

        }


        public static void PopulateSeasonProperties(FTN.Season cimSeason, ResourceDescription rd)
        {
            if ((cimSeason != null) && (rd != null))
            {
                

                if (cimSeason.EndDateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SEASON_ENDDATE, cimSeason.EndDate));
                }
                if (cimSeason.StartDateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SEASON_STARTDATE, cimSeason.StartDate));
                }
            }
        }


        public static void PopulateSeasonDayTypeScheduleProperties(FTN.SeasonDayTypeSchedule cimSDTS, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimSDTS != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateBasicIntervalScheduleProperties(cimSDTS, rd);

                
                if (cimSDTS.SeasonHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimSDTS.Season.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimSDTS.GetType().ToString()).Append(" rdfID = \"").Append(cimSDTS.ID);
                        report.Report.Append("\" - Failed to set reference to Location: rdfID \"").Append(cimSDTS.Season.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.SEASONDAYTYPESCHEDULE_SEASON, gid));
                }

                if (cimSDTS.DayTypeHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimSDTS.DayType.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimSDTS.GetType().ToString()).Append(" rdfID = \"").Append(cimSDTS.ID);
                        report.Report.Append("\" - Failed to set reference to Location: rdfID \"").Append(cimSDTS.DayType.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.SEASONDAYTYPESCHEDULE_DAYTYPE, gid));
                }




            }
        }


        public static void PopulateRegulationScheduleProperties(FTN.RegulationSchedule cimRS, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRS != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateSeasonDayTypeScheduleProperties(cimRS, rd,importHelper,report);

                
                if (cimRS.RegulatingControlHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimRS.RegulatingControl.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimRS.GetType().ToString()).Append(" rdfID = \"").Append(cimRS.ID);
                        report.Report.Append("\" - Failed to set reference to Location: rdfID \"").Append(cimRS.RegulatingControl.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.REGULATIONSCHEDULE_REGCONTROL, gid));
                }
            }
        }


        public static void PopulateEquipmentProperties(FTN.Equipment cimEquipment, ResourceDescription rd)
        {
            if ((cimEquipment != null) && (rd != null))
            {
                
                if (cimEquipment.AggregateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.EQUIPMENT_AGGREGATE, cimEquipment.Aggregate));
                }
                if (cimEquipment.NormallyInServiceHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.EQUIPMENT_NORMALINSERVICE, cimEquipment.NormallyInService));
                }
            }
        }



        public static void PopulateRegulatingCondEqProperties(FTN.RegulatingCondEq cimRCE, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRCE != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateEquipmentProperties(cimRCE, rd);


                if (cimRCE.RegulatingControlHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimRCE.RegulatingControl.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimRCE.GetType().ToString()).Append(" rdfID = \"").Append(cimRCE.ID);
                        report.Report.Append("\" - Failed to set reference to Location: rdfID \"").Append(cimRCE.RegulatingControl.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.REGULATINGCONDEQ_REGCONTROL, gid));
                }

                
            }
        }

        public static void PopulateRegulatingControlProperties(FTN.RegulatingControl cimRE, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRE != null) && (rd != null))
            {
                

                if (cimRE.DiscreteHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULATINGCONTROL_DISCRETE, cimRE.Discrete));
                }
                if (cimRE.ModeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULATINGCONTROL_MODE, (short)GetDMSRegulatingControlModeKind(cimRE.Mode)));
                }
                if (cimRE.MonitoredPhaseHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULATINGCONTROL_MONITOREDPHASE, (short)GetDMSPhaseCode(cimRE.MonitoredPhase)));
                }
                if (cimRE.TargetRangeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULATINGCONTROL_TARGETRANGE, cimRE.TargetRange));
                }
                if (cimRE.TargetValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULATINGCONTROL_TARGETVALUE, cimRE.TargetValue));
                }
                if (cimRE.TerminalHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimRE.Terminal.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimRE.GetType().ToString()).Append(" rdfID = \"").Append(cimRE.ID);
                        report.Report.Append("\" - Failed to set reference to Location: rdfID \"").Append(cimRE.Terminal.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.REGULATINGCONTROL_TERMINAL, gid));
                }
            }
        }


        public static void PopulateAsyncMachineProperties(FTN.AsynchronousMachine cimAsync, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimAsync != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateRegulatingCondEqProperties(cimAsync, rd,importHelper,report);

            }
        }


        #endregion


        

		#region Enums convert
		public static PhaseCode GetDMSPhaseCode(FTN.PhaseCode phases)
		{
			switch (phases)
			{
				case FTN.PhaseCode.A:
					return PhaseCode.A;
				case FTN.PhaseCode.AB:
					return PhaseCode.AB;
				case FTN.PhaseCode.ABC:
					return PhaseCode.ABC;
				case FTN.PhaseCode.ABCN:
					return PhaseCode.ABCN;
				case FTN.PhaseCode.ABN:
					return PhaseCode.ABN;
				case FTN.PhaseCode.AC:
					return PhaseCode.AC;
				case FTN.PhaseCode.ACN:
					return PhaseCode.ACN;
				case FTN.PhaseCode.AN:
					return PhaseCode.AN;
				case FTN.PhaseCode.B:
					return PhaseCode.B;
				case FTN.PhaseCode.BC:
					return PhaseCode.BC;
				case FTN.PhaseCode.BCN:
					return PhaseCode.BCN;
				case FTN.PhaseCode.BN:
					return PhaseCode.BN;
				case FTN.PhaseCode.C:
					return PhaseCode.C;
				case FTN.PhaseCode.CN:
					return PhaseCode.CN;
				case FTN.PhaseCode.N:
					return PhaseCode.N;
				case FTN.PhaseCode.s12N:
					return PhaseCode.ABN;
				case FTN.PhaseCode.s1N:
					return PhaseCode.AN;
				case FTN.PhaseCode.s2N:
					return PhaseCode.BN;
                default:
                    throw new ArgumentException("Unknown FTN.PhaseCode: " + phases);
            }
		}

        public static RegulatingControlModeKind GetDMSRegulatingControlModeKind(FTN.RegulatingControlModeKind regulatingControlModeKind)
        {
            switch (regulatingControlModeKind)
            {
                case FTN.RegulatingControlModeKind.reactivePower:
                    return RegulatingControlModeKind.reactivePower;
                case FTN.RegulatingControlModeKind.powerFactor:
                    return RegulatingControlModeKind.powerFactor;
                case FTN.RegulatingControlModeKind.activePower:
                    return RegulatingControlModeKind.activePower;
                case FTN.RegulatingControlModeKind.temperature:
                    return RegulatingControlModeKind.temperature;
                case FTN.RegulatingControlModeKind.voltage:
                    return RegulatingControlModeKind.voltage;
                case FTN.RegulatingControlModeKind.@fixed:
                    return RegulatingControlModeKind.@fixed;
                case FTN.RegulatingControlModeKind.timeScheduled:
                    return RegulatingControlModeKind.timeScheduled;
                case FTN.RegulatingControlModeKind.admittance:
                    return RegulatingControlModeKind.admittance;
                case FTN.RegulatingControlModeKind.currentFlow:
                    return RegulatingControlModeKind.currentFlow;
                default:
                    throw new ArgumentException("Unknown FTN.RegulatingControlModeKind: " + regulatingControlModeKind);
            }
        }

        public static UnitMultiplier GetDMSUnitMultiplier(FTN.UnitMultiplier unitMultiplier)
        {
            switch (unitMultiplier)
            {
                case FTN.UnitMultiplier.none:
                    return UnitMultiplier.none;
                case FTN.UnitMultiplier.m:
                    return UnitMultiplier.m;
                case FTN.UnitMultiplier.G:
                    return UnitMultiplier.G;
                case FTN.UnitMultiplier.n:
                    return UnitMultiplier.n;
                case FTN.UnitMultiplier.d:
                    return UnitMultiplier.d;
                case FTN.UnitMultiplier.k:
                    return UnitMultiplier.k;
                case FTN.UnitMultiplier.c:
                    return UnitMultiplier.c;
                case FTN.UnitMultiplier.T:
                    return UnitMultiplier.T;
                case FTN.UnitMultiplier.M:
                    return UnitMultiplier.M;
                case FTN.UnitMultiplier.micro:
                    return UnitMultiplier.micro;
                case FTN.UnitMultiplier.p:
                    return UnitMultiplier.p;
                default:
                    throw new ArgumentException("Unknown FTN.UnitMultiplier: " + unitMultiplier);
            }
        }

        public static UnitSymbol GetDMSUnitSymbol(FTN.UnitSymbol unitSymbol)
        {
            switch (unitSymbol)
            {
                case FTN.UnitSymbol.A:
                    return UnitSymbol.A;
                case FTN.UnitSymbol.J:
                    return UnitSymbol.J;
                case FTN.UnitSymbol.Hz:
                    return UnitSymbol.Hz;
                case FTN.UnitSymbol.ohm:
                    return UnitSymbol.ohm;
                case FTN.UnitSymbol.deg:
                    return UnitSymbol.deg;
                case FTN.UnitSymbol.Wh:
                    return UnitSymbol.Wh;
                case FTN.UnitSymbol.S:
                    return UnitSymbol.S;
                case FTN.UnitSymbol.VAr:
                    return UnitSymbol.VAr;
                case FTN.UnitSymbol.m:
                    return UnitSymbol.m;
                case FTN.UnitSymbol.VA:
                    return UnitSymbol.VA;
                case FTN.UnitSymbol.VAh:
                    return UnitSymbol.VAh;
                case FTN.UnitSymbol.H:
                    return UnitSymbol.H;
                case FTN.UnitSymbol.N:
                    return UnitSymbol.N;
                case FTN.UnitSymbol.Pa:
                    return UnitSymbol.Pa;
                case FTN.UnitSymbol.h:
                    return UnitSymbol.h;
                case FTN.UnitSymbol.V:
                    return UnitSymbol.V;
                case FTN.UnitSymbol.g:
                    return UnitSymbol.g;
                case FTN.UnitSymbol.none:
                    return UnitSymbol.none;
                case FTN.UnitSymbol.W:
                    return UnitSymbol.W;
                case FTN.UnitSymbol.rad:
                    return UnitSymbol.rad;
                case FTN.UnitSymbol.VArh:
                    return UnitSymbol.VArh;
                case FTN.UnitSymbol.m3:
                    return UnitSymbol.m3;
                case FTN.UnitSymbol.degC:
                    return UnitSymbol.degC;
                case FTN.UnitSymbol.F:
                    return UnitSymbol.F;
                case FTN.UnitSymbol.s:
                    return UnitSymbol.s;
                case FTN.UnitSymbol.min:
                    return UnitSymbol.min;
                case FTN.UnitSymbol.m2:
                    return UnitSymbol.m2;
                default:
                    throw new ArgumentException("Unknown FTN.UnitSymbol: " + unitSymbol);
            }
        }
        #endregion Enums convert
    }
}
