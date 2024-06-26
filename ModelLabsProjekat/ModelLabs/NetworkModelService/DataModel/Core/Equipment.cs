﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
	public class Equipment : PowerSystemResource
	{		
		private bool aggregate=false;
		private bool normallyInService=false;
						
		public Equipment(long globalId) : base(globalId) 
		{
		}
	
		public bool Aggregate
        {
			get
			{
				return aggregate;
			}

			set
			{
                aggregate = value;
			}
		}

		public bool NormallyInService
        {
			get 
			{
				return normallyInService; 
			}
			
			set
			{
                normallyInService = value; 
			}
		}

		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				Equipment x = (Equipment)obj;
				return ((x.Aggregate == this.Aggregate) &&
						(x.NormallyInService == this.NormallyInService));
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

		public override bool HasProperty(ModelCode property)
		{
			switch (property)
			{
                case ModelCode.EQUIPMENT_AGGREGATE:
                case ModelCode.EQUIPMENT_NORMALINSERVICE:

                    return true;
				default:
					return base.HasProperty(property);
			}
		}

		public override void GetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.EQUIPMENT_AGGREGATE:
					property.SetValue(aggregate);
					break;

				case ModelCode.EQUIPMENT_NORMALINSERVICE:
					property.SetValue(normallyInService);
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
				case ModelCode.EQUIPMENT_AGGREGATE:					
					aggregate = property.AsBool();
					break;

				case ModelCode.EQUIPMENT_NORMALINSERVICE:
					normallyInService = property.AsBool();
					break;
			
				default:
					base.SetProperty(property);
					break;
			}
		}		

		#endregion IAccess implementation
	}
}
