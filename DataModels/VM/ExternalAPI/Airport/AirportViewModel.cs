using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DataModels.VM.ExternalAPI.Airport
{
    public class AirportViewModel
    {
        public List<AirportDetailsViewModel> Value { get; set; } = new List<AirportDetailsViewModel>();

        [JsonProperty("Zontinuation Token")]
        public string ZontinuationToken { get; set; }
    }

    public class AirportDetailsViewModel
    {
        [JsonProperty("Site Id")]
        public string SiteId { get; set; }

        [JsonProperty("Facility Type")]
        public string FacilityType { get; set; }

        [JsonProperty("Loc Id")]
        public string LocId { get; set; }

        [JsonProperty("Effective Date")]
        public string EffectiveDate { get; set; }
        public string Region { get; set; }
        public string ADO { get; set; }

        [JsonProperty("State Id")]
        public string StateId { get; set; }

        [JsonProperty("State Name")]
        public string StateName { get; set; }
        public string County { get; set; }

        [JsonProperty("County State")]
        public string CountyState { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public string Ownership { get; set; }
        public string Use { get; set; }
        public string Owner { get; set; }

        [JsonProperty("Owner Address")]
        public string OwnerAddress { get; set; }

        [JsonProperty("Owner City, State, Zip")]
        public string OwnerCityStateZip { get; set; }

        [JsonProperty("Owner Phone")]
        public string OwnerPhone { get; set; }
        public string Manager { get; set; }

        [JsonProperty("Manager Address")]
        public string ManagerAddress { get; set; }

        [JsonProperty("Manager City, State, Zip")]
        public string ManagerCityStateZip { get; set; }

        [JsonProperty("Manager Phone")]
        public string ManagerPhone { get; set; }

        [JsonProperty("ARP Latitude")]
        public string ARPLatitude { get; set; }

        [JsonProperty("ARP Latitude Sec")]
        public string ARPLatitudeSec { get; set; }

        [JsonProperty("ARP Longitude")]
        public string ARPLongitude { get; set; }

        [JsonProperty("ARP Longitude Sec")]
        public string ARPLongitudeSec { get; set; }

        [JsonProperty("ARP Method")]
        public string ARPMethod { get; set; }
        public string Elevation { get; set; }

        [JsonProperty("Elevation Method")]
        public string ElevationMethod { get; set; }

        [JsonProperty("Magnetic Variation")]
        public string MagneticVariation { get; set; }

        [JsonProperty("Magnetic Variation Year")]
        public string MagneticVariationYear { get; set; }

        [JsonProperty("Traffic Pattern Altitude")]
        public object TrafficPatternAltitude { get; set; }
        public string Sectional { get; set; }

        [JsonProperty("Distance From CBD")]
        public string DistanceFromCBD { get; set; }

        [JsonProperty("Direction From CBD")]
        public string DirectionFromCBD { get; set; }

        [JsonProperty("Land Area")]
        public string LandArea { get; set; }

        [JsonProperty("ARTCC Id")]
        public string ARTCCId { get; set; }

        [JsonProperty("ARTCC Computer ID")]
        public string ARTCCComputerID { get; set; }

        [JsonProperty("ARTCC Name")]
        public string ARTCCName { get; set; }

        [JsonProperty("Responsible ARTCC Id")]
        public string ResponsibleARTCCId { get; set; }

        [JsonProperty("Responsible ARTCC Computer Id")]
        public string ResponsibleARTCCComputerId { get; set; }

        [JsonProperty("Responsible ARTCC Name")]
        public string ResponsibleARTCCName { get; set; }

        [JsonProperty("Tie In FSS")]
        public string TieInFSS { get; set; }

        [JsonProperty("Tie In FSS Id")]
        public string TieInFSSId { get; set; }

        [JsonProperty("Tie In FSS Name")]
        public string TieInFSSName { get; set; }

        [JsonProperty("FSS Phone Number")]
        public string FSSPhoneNumber { get; set; }

        [JsonProperty("FSS Toll Free Number")]
        public string FSSTollFreeNumber { get; set; }

        [JsonProperty("Alternate FSS Id")]
        public string AlternateFSSId { get; set; }

        [JsonProperty("Alternate FSS Name")]
        public string AlternateFSSName { get; set; }

        [JsonProperty("Alternate FSS Toll Free Number")]
        public string AlternateFSSTollFreeNumber { get; set; }

        [JsonProperty("NOTAM Facility Id")]
        public string NOTAMFacilityId { get; set; }

        [JsonProperty("NOTAM Service")]
        public string NOTAMService { get; set; }

        [JsonProperty("Activation Date")]
        public string ActivationDate { get; set; }

        [JsonProperty("Airport Status Code")]
        public string AirportStatusCode { get; set; }

        [JsonProperty("Certification Type Date")]
        public object CertificationTypeDate { get; set; }

        [JsonProperty("Federal Agreements")]
        public object FederalAgreements { get; set; }

        [JsonProperty("Airspace Determination")]
        public string AirspaceDetermination { get; set; }

        [JsonProperty("Customs Airport Of Entry")]
        public string CustomsAirportOfEntry { get; set; }

        [JsonProperty("Customs Landing Rights")]
        public string CustomsLandingRights { get; set; }

        [JsonProperty("Military Joint Use")]
        public string MilitaryJointUse { get; set; }

        [JsonProperty("Military Landing Rights")]
        public string MilitaryLandingRights { get; set; }

        [JsonProperty("Inspection Method")]
        public string InspectionMethod { get; set; }

        [JsonProperty("Inspection Group")]
        public string InspectionGroup { get; set; }

        [JsonProperty("Last Inspection Date")]
        public string LastInspectionDate { get; set; }

        [JsonProperty("Last Owner Information Date")]
        public string LastOwnerInformationDate { get; set; }

        [JsonProperty("Fuel Types")]
        public string FuelTypes { get; set; }

        [JsonProperty("Airframe Repair")]
        public string AirframeRepair { get; set; }

        [JsonProperty("Power Plant Repair")]
        public string PowerPlantRepair { get; set; }

        [JsonProperty("Bottled Oxygen Type")]
        public string BottledOxygenType { get; set; }

        [JsonProperty("Bulk Oxygen Type")]
        public string BulkOxygenType { get; set; }

        [JsonProperty("Lighting Schedule")]
        public object LightingSchedule { get; set; }

        [JsonProperty("Beacon Schedule")]
        public object BeaconSchedule { get; set; }
        public string ATCT { get; set; }
        public string UNICOM { get; set; }
        public string CTAF { get; set; }

        [JsonProperty("Segmented Circle")]
        public string SegmentedCircle { get; set; }

        [JsonProperty("Beacon Color")]
        public object BeaconColor { get; set; }

        [JsonProperty("Non Commercial Landing Fee")]
        public string NonCommercialLandingFee { get; set; }

        [JsonProperty("Medical Use")]
        public object MedicalUse { get; set; }

        [JsonProperty("Single Engine Aircraft")]
        public string SingleEngineAircraft { get; set; }

        [JsonProperty("Multi Engine Aircraft")]
        public object MultiEngineAircraft { get; set; }

        [JsonProperty("Jet Engine Aircraft")]
        public object JetEngineAircraft { get; set; }
        public object Helicopters { get; set; }

        [JsonProperty("Gliders Operational")]
        public object GlidersOperational { get; set; }

        [JsonProperty("Military Operational")]
        public object MilitaryOperational { get; set; }
        public object Ultralights { get; set; }

        [JsonProperty("Commercial Operations")]
        public object CommercialOperations { get; set; }

        [JsonProperty("Commuter Operations")]
        public object CommuterOperations { get; set; }

        [JsonProperty("Air Taxi Operations")]
        public string AirTaxiOperations { get; set; }

        [JsonProperty("GA Local Operations")]
        public string GALocalOperations { get; set; }

        [JsonProperty("GA Itin Operations")]
        public string GAItinOperations { get; set; }

        [JsonProperty("Military Operations")]
        public object MilitaryOperations { get; set; }

        [JsonProperty("Operations Date")]
        public string OperationsDate { get; set; }

        [JsonProperty("Airport Position Source")]
        public string AirportPositionSource { get; set; }

        [JsonProperty("Airport Position Source Date")]
        public string AirportPositionSourceDate { get; set; }

        [JsonProperty("Airport Elevation Source")]
        public string AirportElevationSource { get; set; }

        [JsonProperty("Airport Elevation Source Date")]
        public string AirportElevationSourceDate { get; set; }

        [JsonProperty("Fuel Available")]
        public object FuelAvailable { get; set; }

        [JsonProperty("Transient Storage")]
        public string TransientStorage { get; set; }

        [JsonProperty("Other Services")]
        public string OtherServices { get; set; }

        [JsonProperty("Wind Indicator")]
        public string WindIndicator { get; set; }

        [JsonProperty("ICAO Id")]
        public string ICAOId { get; set; }

        [JsonProperty("NPIAS Hub")]
        public object NPIASHub { get; set; }

        [JsonProperty("NPIAS Role")]
        public object NPIASRole { get; set; }
        public Guid id { get; set; }
        public string _rid { get; set; }
        public string _self { get; set; }
        public string _etag { get; set; }
        public string _attachments { get; set; }
        public int _ts { get; set; }
    }

}
