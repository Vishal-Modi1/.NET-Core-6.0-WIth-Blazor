namespace DataModels.Enums
{
    public enum DiscrepancyStatus : byte
    {
        New_Pending = 1,
        Verified_PedingtoRepair = 2,
        Unable_To_Verify = 3,
        Verified_Acceptable_As_Noted = 4,
        Verified_And_Repaired = 5,
    }
}
