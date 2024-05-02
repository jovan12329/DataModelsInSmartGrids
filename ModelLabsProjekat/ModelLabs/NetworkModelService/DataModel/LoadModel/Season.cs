using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using FTN.Services.NetworkModelService.DataModel.Wires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.LoadModel
{
    public class Season : IdentifiedObject
    {

        private DateTime endDate;
        private DateTime startDate;
        private List<long> seasonDayTypeSchedules = new List<long>();



        public Season(long globalId) : base(globalId)
        {
        }

        public DateTime EndDate { get => endDate; set => endDate = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public List<long> SeasonDayTypeSchedules { get => seasonDayTypeSchedules; set => seasonDayTypeSchedules = value; }



        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Season x = (Season)obj;
                return (x.endDate == this.endDate && x.startDate == this.startDate &&
                        CompareHelper.CompareLists(x.seasonDayTypeSchedules, this.seasonDayTypeSchedules));
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        #region IAccess implementation

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.SEASON_ENDDATE:
                case ModelCode.SEASON_STARTDATE:
                case ModelCode.SEASON_SEASONDAYSCHEDS:
                    return true;

                default:
                    return base.HasProperty(t);

            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.SEASON_ENDDATE:
                    property.SetValue(endDate);
                    break;

                case ModelCode.SEASON_STARTDATE:
                    property.SetValue(startDate);
                    break;

                case ModelCode.SEASON_SEASONDAYSCHEDS:
                    property.SetValue(seasonDayTypeSchedules);
                    break;

                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.SEASON_ENDDATE:
                    endDate = property.AsDateTime();
                    break;

                case ModelCode.SEASON_STARTDATE:
                    startDate = property.AsDateTime();
                    break;

                
                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation

        #region IReference implementation

        public override bool IsReferenced
        {
            get
            {
                return seasonDayTypeSchedules.Count != 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {

            if (seasonDayTypeSchedules != null && seasonDayTypeSchedules.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.SEASON_SEASONDAYSCHEDS] = seasonDayTypeSchedules.GetRange(0, seasonDayTypeSchedules.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.SEASONDAYTYPESCHEDULE_SEASON:
                    seasonDayTypeSchedules.Add(globalId);
                    break;

                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.SEASONDAYTYPESCHEDULE_SEASON:

                    if (seasonDayTypeSchedules.Contains(globalId))
                    {
                        seasonDayTypeSchedules.Remove(globalId);
                    }
                    else
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
                    }

                    break;

                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

        #endregion IReference implementation





    }
}
