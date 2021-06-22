using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FODLSystem.Models
{
    public class BicycleEntryDetail
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int BicycleEntryHeaderId { get; set; }
        public virtual BicycleEntryHeader BicycleHeaders { get; set; }

        public int FrameSafe { get; set; }
        public int FrameUnSafe { get; set; }
        public string FrameRemarks { get; set; }


        public int FrontForkSafe { get; set; }
        public int FrontForkUnSafe { get; set; }
        public string FrontForkRemarks { get; set; }

        public int HandlebarSafe { get; set; }
        public int HandlebarUnSafe { get; set; }
        public string HandlebarRemarks { get; set; }


        public int SeatSafe { get; set; }
        public int SeatUnSafe { get; set; }
        public string SeatRemarks { get; set; }

        public int FrontRearSafe { get; set; }
        public int FrontRearUnSafe { get; set; }
        public string FrontRearRemarks { get; set; }

        public int BrakeSafe { get; set; }
        public int BrakeUnSafe { get; set; }
        public string BrakeRemarks { get; set; }


        public int CrankChainSafe { get; set; }
        public int CrankChainUnSafe { get; set; }
        public string CrankChainRemarks { get; set; }


        public int ChainSafe { get; set; }
        public int ChainUnSafe { get; set; }
        public string ChainRemarks { get; set; }

        public string InspectedBy { get; set; }
        public string NotedBy { get; set; }
        public string ImageUrl { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
       

    }
}
