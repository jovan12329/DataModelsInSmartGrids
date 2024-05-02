using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using FTN.Services.NetworkModelService.DataModel.LoadModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class RegulationSchedule : SeasonDayTypeSchedule
    {
        private long regulatingControl = 0;
        public RegulationSchedule(long globalId) : base(globalId)
        {
        }

        public long RegulatingControl { get => regulatingControl; set => regulatingControl = value; }


        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                RegulationSchedule x = (RegulationSchedule)obj;
                return (x.regulatingControl == this.regulatingControl);
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
                case ModelCode.REGULATIONSCHEDULE_REGCONTROL:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.REGULATIONSCHEDULE_REGCONTROL:
                    property.SetValue(regulatingControl);
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
                case ModelCode.REGULATIONSCHEDULE_REGCONTROL:
                    regulatingControl = property.AsReference();
                    break;


                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation

        #region IReference implementation	

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (regulatingControl != 0 && (refType != TypeOfReference.Reference || refType != TypeOfReference.Both))
            {
                references[ModelCode.REGULATIONSCHEDULE_REGCONTROL] = new List<long>();
                references[ModelCode.REGULATIONSCHEDULE_REGCONTROL].Add(regulatingControl);
            }

           

            base.GetReferences(references, refType);
        }

        #endregion IReference implementation







    }
}
