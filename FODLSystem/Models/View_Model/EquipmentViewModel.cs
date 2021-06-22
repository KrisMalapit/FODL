using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FODLSystem.Models.View_Model
{

        public class FireExtinguisherViewModel
    {
        public int LocationFireExtinguisherId { get; set; }
        public int LocationInergenTankId { get; set; }
        public string Cylinder { get; set; }
        public string Lever { get; set; }
        public string Gauge { get; set; }
        public string SafetySeal { get; set; }
        public string Hose { get; set; }
        public string Remarks { get; set; }
        public string Pressure { get; set; }
        public int AreaId { get; set; }
        public int ItemId { get; set; }
        public int ID { get; set; }
        public string Safe { get; set; }
        public string UnSafe { get; set; }
        public int BicycleEntryRowId { get; set; }
        public int BicycleId { get; set; }

        public string InspectedBy { get; set; }
        public string ReviewedBy { get; set; }
        public string NotedBy { get; set; }
     



    }
    

    public class EmergencyLightViewModel
    {
        public int LocationEmergencyLightId { get; set; }
        public string Battery { get; set; }
        public string Bulb { get; set; }
        public string Usable { get; set; }
        public int ItemId { get; set; }
        public string Remarks { get; set; }
        public int AreaId { get; set; }
        public int ID { get; set; }

        public string InspectedBy { get; set; }
        public string ReviewedBy { get; set; }
        public string NotedBy { get; set; }

    }
    public class FireHydrantViewModel
    {
        public int LocationFireHydrantId { get; set; }
        public int ItemId { get; set; }
        public string GlassCabinet { get; set; }
        public string Hanger { get; set; }
        public string Hose15 { get; set; }
        public string Nozzle15 { get; set; }
        public string Hose25 { get; set; }
        public string Nozzle25 { get; set; }
        public string SpecialTools { get; set; }

        public string Remarks { get; set; }
        public int AreaId { get; set; }
        public int ID { get; set; }

        public string InspectedBy { get; set; }
        public string ReviewedBy { get; set; }
        public string NotedBy { get; set; }

    }
    public class EquipmentViewModel
    {
        public int ID { get; set; }
        public string Text { get; set; }
    }
}
