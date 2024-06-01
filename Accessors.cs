namespace RSODiplomApplication;

public partial class Members
{
    public int ID { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string FatherName { get; set; }
    public int Age { get; set; }
    public string PhoneNumber { get; set; }
    public string CityName { get; set; }
    public string StreetName { get; set; }
    public int HouseNumber { get; set; }
    public int ApartmentNumber { get; set; }
    public int PassportID { get; set; }
    public string SquadName { get; set; }
    public string PositionName { get; set; }
}

public partial class City
{
    public int ID { get; set; }
    public string CityName { get; set; }
}
public partial class Street
{
    public int ID { get; set; }
    public string StreetName { get; set; }
}
public partial class Position
{
    public int ID { get; set; }
    public string PositionName { get; set; }
}
public partial class Status
{
    public int ID { get; set; }
    public string StatusName { get; set; }
}

public partial class Catalog
{
    public int ID { get; set; }
    public string CatalogName { get; set; }
    public int Price { get; set; }
}
public partial class Staff
{
    public int ID { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string FatherName { get; set; }
    public string Position { get; set; }
}
public partial class Orders
{
    public int ID { get; set; }
    public int MemberID { get; set; }
    public string CatalogName { get; set; }
    public int Amount { get; set; }
    public string StatusName { get; set; }
}
public partial class Squad
{
    public int ID { get; set; }
    public string NameSquad { get; set; }
    public string Specialization { get; set; }
    public int StaffID { get; set; }
}

public partial class Events
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Type_Name { get; set; }
    public string Description { get; set; }
}

