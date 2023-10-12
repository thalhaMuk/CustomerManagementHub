using System;
using System.Collections.Generic;

namespace CustomerManagementHub.DataAccess.Models;

public partial class CustomerModel
{
    public string Id { get; set; } = null!;

    public int Number { get; set; }

    public int Age { get; set; }

    public string EyeColor { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string Company { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int Addressnumber { get; set; }

    public string Addressstreet { get; set; } = null!;

    public string Addresscity { get; set; } = null!;

    public string Addressstate { get; set; } = null!;

    public int Addresszipcode { get; set; }

    public string About { get; set; } = null!;

    public string Registered { get; set; } = null!;

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public string Tags0 { get; set; } = null!;

    public string Tags1 { get; set; } = null!;

    public string Tags2 { get; set; } = null!;

    public string Tags3 { get; set; } = null!;

    public string Tags4 { get; set; } = null!;

    public string Tags5 { get; set; } = null!;

    public string Tags6 { get; set; } = null!;
}
