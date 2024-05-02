using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Wires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class BasicIntervalSchedule : IdentifiedObject
    {
        private DateTime startTime;

        private UnitMultiplier value1Multiplier;

        private UnitSymbol value1Unit;

        private UnitMultiplier value2Multiplier;

        private UnitSymbol value2Unit;




        public BasicIntervalSchedule(long globalId) : base(globalId)
        {
        }

        public DateTime StartTime { get => startTime; set => startTime = value; }
        public UnitMultiplier Value1Multiplier { get => value1Multiplier; set => value1Multiplier = value; }
        public UnitSymbol Value1Unit { get => value1Unit; set => value1Unit = value; }
        public UnitMultiplier Value2Multiplier { get => value2Multiplier; set => value2Multiplier = value; }
        public UnitSymbol Value2Unit { get => value2Unit; set => value2Unit = value; }


        public override bool Equals(object obj)
        {

            if (base.Equals(obj))
            {
                BasicIntervalSchedule x = (BasicIntervalSchedule)obj;
                return (x.StartTime == this.StartTime &&
                        x.Value1Multiplier == this.Value1Multiplier &&
                        x.Value1Unit == this.Value1Unit &&
                        x.Value2Multiplier == this.Value2Multiplier&&
                        x.Value2Unit == this.Value2Unit);
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
                case ModelCode.BASICINTERVALSCHEDULE_STARTTIME:
                case ModelCode.BASICINTERVALSCHEDULE_VALUE1MULTIPL:
                case ModelCode.BASICINTERVALSCHEDULE_VALUE1UNIT:
                case ModelCode.BASICINTERVALSCHEDULE_VALUE2MULTIPL:
                case ModelCode.BASICINTERVALSCHEDULE_VALUE2UNIT:

                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.BASICINTERVALSCHEDULE_STARTTIME:
                    property.SetValue(startTime);
                    break;

                case ModelCode.BASICINTERVALSCHEDULE_VALUE1MULTIPL:
                    property.SetValue((short)value1Multiplier);
                    break;

                case ModelCode.BASICINTERVALSCHEDULE_VALUE1UNIT:
                    property.SetValue((short)value1Unit);
                    break;

                case ModelCode.BASICINTERVALSCHEDULE_VALUE2MULTIPL:
                    property.SetValue((short)value2Multiplier);
                    break;

                case ModelCode.BASICINTERVALSCHEDULE_VALUE2UNIT:
                    property.SetValue((short)value2Unit);
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
                case ModelCode.BASICINTERVALSCHEDULE_STARTTIME:
                    startTime = property.AsDateTime();
                    break;

                case ModelCode.BASICINTERVALSCHEDULE_VALUE1MULTIPL:
                    value1Multiplier = (UnitMultiplier)property.AsEnum();
                    break;

                case ModelCode.BASICINTERVALSCHEDULE_VALUE1UNIT:
                    value1Unit = (UnitSymbol)property.AsEnum();
                    break;

                case ModelCode.BASICINTERVALSCHEDULE_VALUE2MULTIPL:
                    value2Multiplier = (UnitMultiplier)property.AsEnum();
                    break;

                case ModelCode.BASICINTERVALSCHEDULE_VALUE2UNIT:
                    value2Unit = (UnitSymbol)property.AsEnum();
                    break;


                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation



    }
}
