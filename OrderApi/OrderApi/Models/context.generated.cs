//---------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by T4Model template for T4 (https://github.com/linq2db/linq2db).
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------

#pragma warning disable 1591

using System;
using System.Linq;

using LinqToDB;
using LinqToDB.Mapping;

namespace OrderApi
{
	/// <summary>
	/// Database       : order
	/// Data Source    : 45.40.200.146
	/// Server Version : 11.00.3000
	/// </summary>
	public partial class OrderDB : LinqToDB.Data.DataConnection
	{
		public ITable<FOOD>            Foods            { get { return this.GetTable<FOOD>(); } }
		public ITable<FoodDetail>      FoodDetails      { get { return this.GetTable<FoodDetail>(); } }
		public ITable<FoodType>        FoodTypes        { get { return this.GetTable<FoodType>(); } }
		public ITable<IMAGE>           Images           { get { return this.GetTable<IMAGE>(); } }
		public ITable<OrderDetail>     OrderDetails     { get { return this.GetTable<OrderDetail>(); } }
		public ITable<OrderDetailFood> OrderDetailFoods { get { return this.GetTable<OrderDetailFood>(); } }
		public ITable<OrderHead>       OrderHeads       { get { return this.GetTable<OrderHead>(); } }
		public ITable<SHOP>            Shops            { get { return this.GetTable<SHOP>(); } }
		public ITable<ShopDesk>        ShopDesks        { get { return this.GetTable<ShopDesk>(); } }
		public ITable<TEST>            Tests            { get { return this.GetTable<TEST>(); } }

		public OrderDB()
		{
			InitDataContext();
			InitMappingSchema();
		}

		public OrderDB(string configuration)
			: base(configuration)
		{
			InitDataContext();
			InitMappingSchema();
		}

		partial void InitDataContext  ();
		partial void InitMappingSchema();
	}

	[Table(Schema="dbo", Name="FOOD")]
	public partial class FOOD
	{
		[Column(),                    PrimaryKey,  NotNull] public string    ID               { get; set; } // varchar(80)
		[Column("DATETIME_CREATED"),               NotNull] public DateTime  DatetimeCreated  { get; set; } // datetime
		[Column("USER_CREATED"),                   NotNull] public string    UserCreated      { get; set; } // varchar(80)
		[Column("DATETIME_MODIFIED"),    Nullable         ] public DateTime? DatetimeModified { get; set; } // datetime
		[Column("USER_MODIFIED"),        Nullable         ] public string    UserModified     { get; set; } // varchar(80)
		[Column(),                                 NotNull] public char      STATE            { get; set; } // char(1)
		[Column("TYPE_ID"),              Nullable         ] public string    TypeId           { get; set; } // varchar(80)
		[Column(),                       Nullable         ] public string    NAME             { get; set; } // varchar(80)
		[Column(),                       Nullable         ] public string    TAG              { get; set; } // varchar(80)
	}

	[Table(Schema="dbo", Name="FOOD_DETAIL")]
	public partial class FoodDetail
	{
		[Column(),                    PrimaryKey,  NotNull] public string    ID               { get; set; } // varchar(80)
		[Column("DATETIME_CREATED"),               NotNull] public DateTime  DatetimeCreated  { get; set; } // datetime
		[Column("USER_CREATED"),                   NotNull] public string    UserCreated      { get; set; } // varchar(80)
		[Column("DATETIME_MODIFIED"),    Nullable         ] public DateTime? DatetimeModified { get; set; } // datetime
		[Column("USER_MODIFIED"),        Nullable         ] public string    UserModified     { get; set; } // varchar(80)
		[Column(),                                 NotNull] public char      STATE            { get; set; } // char(1)
		[Column("FOOD_ID"),              Nullable         ] public string    FoodId           { get; set; } // varchar(80)
		[Column(),                       Nullable         ] public string    NAME             { get; set; } // varchar(80)
		[Column(),                       Nullable         ] public decimal?  PRICE            { get; set; } // decimal(18, 2)
		[Column("DETAIL_DESC"),          Nullable         ] public string    DetailDesc       { get; set; } // varchar(240)
	}

	[Table(Schema="dbo", Name="FOOD_TYPE")]
	public partial class FoodType
	{
		[Column(),                    PrimaryKey,  NotNull] public string    ID               { get; set; } // varchar(80)
		[Column("DATETIME_CREATED"),               NotNull] public DateTime  DatetimeCreated  { get; set; } // datetime
		[Column("USER_CREATED"),                   NotNull] public string    UserCreated      { get; set; } // varchar(80)
		[Column("DATETIME_MODIFIED"),    Nullable         ] public DateTime? DatetimeModified { get; set; } // datetime
		[Column("USER_MODIFIED"),        Nullable         ] public string    UserModified     { get; set; } // varchar(80)
		[Column(),                                 NotNull] public char      STATE            { get; set; } // char(1)
		[Column("SHOP_ID"),              Nullable         ] public string    ShopId           { get; set; } // varchar(80)
		[Column("TYPE_NAME"),            Nullable         ] public string    TypeName         { get; set; } // varchar(80)
		[Column(),                       Nullable         ] public string    ICON             { get; set; } // varchar(80)
	}

	[Table(Schema="dbo", Name="IMAGE")]
	public partial class IMAGE
	{
		[Column(),                    PrimaryKey,  NotNull] public string    ID               { get; set; } // varchar(80)
		[Column("DATETIME_CREATED"),               NotNull] public DateTime  DatetimeCreated  { get; set; } // datetime
		[Column("USER_CREATED"),                   NotNull] public string    UserCreated      { get; set; } // varchar(80)
		[Column("DATETIME_MODIFIED"),    Nullable         ] public DateTime? DatetimeModified { get; set; } // datetime
		[Column("USER_MODIFIED"),        Nullable         ] public string    UserModified     { get; set; } // varchar(80)
		[Column(),                                 NotNull] public char      STATE            { get; set; } // char(1)
		[Column("CONNECT_ID"),                     NotNull] public string    ConnectId        { get; set; } // varchar(80)
		[Column(),                       Nullable         ] public string    URL              { get; set; } // varchar(240)
	}

	[Table(Schema="dbo", Name="ORDER_DETAIL")]
	public partial class OrderDetail
	{
		[Column(),                    PrimaryKey,  NotNull] public string    ID               { get; set; } // varchar(80)
		[Column("DATETIME_CREATED"),               NotNull] public DateTime  DatetimeCreated  { get; set; } // datetime
		[Column("USER_CREATED"),                   NotNull] public string    UserCreated      { get; set; } // varchar(80)
		[Column("DATETIME_MODIFIED"),    Nullable         ] public DateTime? DatetimeModified { get; set; } // datetime
		[Column("USER_MODIFIED"),        Nullable         ] public string    UserModified     { get; set; } // varchar(80)
		[Column(),                                 NotNull] public char      STATE            { get; set; } // char(1)
		[Column("USER_ORDER"),           Nullable         ] public string    UserOrder        { get; set; } // varchar(80)
		[Column("PRRENT_ORDER_ID"),      Nullable         ] public string    PrrentOrderId    { get; set; } // varchar(80)
	}

	[Table(Schema="dbo", Name="ORDER_DETAIL_FOOD")]
	public partial class OrderDetailFood
	{
		[Column(),                    PrimaryKey,  NotNull] public string    ID               { get; set; } // varchar(80)
		[Column("DATETIME_CREATED"),               NotNull] public DateTime  DatetimeCreated  { get; set; } // datetime
		[Column("USER_CREATED"),                   NotNull] public string    UserCreated      { get; set; } // varchar(80)
		[Column("DATETIME_MODIFIED"),    Nullable         ] public DateTime? DatetimeModified { get; set; } // datetime
		[Column("USER_MODIFIED"),        Nullable         ] public string    UserModified     { get; set; } // varchar(80)
		[Column(),                                 NotNull] public char      STATE            { get; set; } // char(1)
		[Column("USER_ORDER"),           Nullable         ] public string    UserOrder        { get; set; } // varchar(80)
		[Column("ORDER_DETAIL_ID"),      Nullable         ] public string    OrderDetailId    { get; set; } // varchar(80)
		[Column("FOOD_DETAIL_ID"),       Nullable         ] public string    FoodDetailId     { get; set; } // varchar(80)
		[Column(),                       Nullable         ] public decimal?  QTY              { get; set; } // decimal(18, 0)
	}

	[Table(Schema="dbo", Name="ORDER_HEAD")]
	public partial class OrderHead
	{
		[Column(),                    PrimaryKey,  NotNull] public string    ID               { get; set; } // varchar(80)
		[Column("DATETIME_CREATED"),               NotNull] public DateTime  DatetimeCreated  { get; set; } // datetime
		[Column("USER_CREATED"),                   NotNull] public string    UserCreated      { get; set; } // varchar(80)
		[Column("DATETIME_MODIFIED"),    Nullable         ] public DateTime? DatetimeModified { get; set; } // datetime
		[Column("USER_MODIFIED"),        Nullable         ] public string    UserModified     { get; set; } // varchar(80)
		[Column(),                                 NotNull] public char      STATE            { get; set; } // char(1)
		[Column("SHOP_ID"),              Nullable         ] public string    ShopId           { get; set; } // varchar(80)
		[Column("DESC_NUM"),             Nullable         ] public string    DescNum          { get; set; } // varchar(80)
		[Column("IS_CLOSE"),             Nullable         ] public char?     IsClose          { get; set; } // char(1)
		[Column("IS_PRINT"),             Nullable         ] public char?     IsPrint          { get; set; } // char(1)
		[Column("PERSON_NUM"),           Nullable         ] public decimal?  PersonNum        { get; set; } // decimal(18, 0)
	}

	[Table(Schema="dbo", Name="SHOP")]
	public partial class SHOP
	{
		[Column(),                    PrimaryKey,  NotNull] public string    ID               { get; set; } // varchar(80)
		[Column("DATETIME_CREATED"),               NotNull] public DateTime  DatetimeCreated  { get; set; } // datetime
		[Column("USER_CREATED"),                   NotNull] public string    UserCreated      { get; set; } // varchar(80)
		[Column("DATETIME_MODIFIED"),    Nullable         ] public DateTime? DatetimeModified { get; set; } // datetime
		[Column("USER_MODIFIED"),        Nullable         ] public string    UserModified     { get; set; } // varchar(80)
		[Column(),                                 NotNull] public char      STATE            { get; set; } // char(1)
		[Column(),                       Nullable         ] public string    NAME             { get; set; } // varchar(80)
		[Column(),                       Nullable         ] public string    ADDRESS          { get; set; } // varchar(80)
		[Column(),                       Nullable         ] public string    ACCOUNT          { get; set; } // varchar(80)
		[Column(),                       Nullable         ] public string    PASSWORD         { get; set; } // varchar(80)
		[Column(),                       Nullable         ] public string    TEL              { get; set; } // varchar(80)
		[Column("IS_ADMIN"),             Nullable         ] public string    IsAdmin          { get; set; } // varchar(10)
	}

	[Table(Schema="dbo", Name="SHOP_DESK")]
	public partial class ShopDesk
	{
		[Column(),                    PrimaryKey,  NotNull] public string    ID               { get; set; } // varchar(80)
		[Column("DATETIME_CREATED"),               NotNull] public DateTime  DatetimeCreated  { get; set; } // datetime
		[Column("USER_CREATED"),                   NotNull] public string    UserCreated      { get; set; } // varchar(80)
		[Column("DATETIME_MODIFIED"),    Nullable         ] public DateTime? DatetimeModified { get; set; } // datetime
		[Column("USER_MODIFIED"),        Nullable         ] public string    UserModified     { get; set; } // varchar(80)
		[Column(),                                 NotNull] public char      STATE            { get; set; } // char(1)
		[Column("SHOP_ID"),              Nullable         ] public string    ShopId           { get; set; } // varchar(80)
		[Column("DESK_COUNT"),           Nullable         ] public string    DeskCount        { get; set; } // varchar(80)
		[Column("DESC_DESC"),            Nullable         ] public string    DescDesc         { get; set; } // varchar(80)
	}

	[Table(Schema="dbo", Name="TEST")]
	public partial class TEST
	{
		[Column, Nullable] public string ID  { get; set; } // nchar(10)
		[Column, Nullable] public string WWW { get; set; } // nchar(10)
		[Column, Nullable] public string EEE { get; set; } // nchar(10)
	}

	public static partial class TableExtensions
	{
		public static FOOD Find(this ITable<FOOD> table, string ID)
		{
			return table.FirstOrDefault(t =>
				t.ID == ID);
		}

		public static FoodDetail Find(this ITable<FoodDetail> table, string ID)
		{
			return table.FirstOrDefault(t =>
				t.ID == ID);
		}

		public static FoodType Find(this ITable<FoodType> table, string ID)
		{
			return table.FirstOrDefault(t =>
				t.ID == ID);
		}

		public static IMAGE Find(this ITable<IMAGE> table, string ID)
		{
			return table.FirstOrDefault(t =>
				t.ID == ID);
		}

		public static OrderDetail Find(this ITable<OrderDetail> table, string ID)
		{
			return table.FirstOrDefault(t =>
				t.ID == ID);
		}

		public static OrderDetailFood Find(this ITable<OrderDetailFood> table, string ID)
		{
			return table.FirstOrDefault(t =>
				t.ID == ID);
		}

		public static OrderHead Find(this ITable<OrderHead> table, string ID)
		{
			return table.FirstOrDefault(t =>
				t.ID == ID);
		}

		public static SHOP Find(this ITable<SHOP> table, string ID)
		{
			return table.FirstOrDefault(t =>
				t.ID == ID);
		}

		public static ShopDesk Find(this ITable<ShopDesk> table, string ID)
		{
			return table.FirstOrDefault(t =>
				t.ID == ID);
		}
	}
}

#pragma warning restore 1591
