namespace Api.Models
{
    public class Vehicle
    {

        public Guid Id { get; set; }
        public VehicleType VehicleType { get; set; }
        public string Make { get; set; }
        public string Year { get; set; }

    }

    public enum VehicleType
    {
        VA,
        VB
    }
}
