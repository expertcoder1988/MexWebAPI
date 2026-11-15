using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MexCoreWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countries_CountryID",
                table: "Cities");

            migrationBuilder.RenameColumn(
                name: "state",
                table: "Cities",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "elevation",
                table: "Cities",
                newName: "Elevation");

            migrationBuilder.RenameColumn(
                name: "countrycode",
                table: "Cities",
                newName: "countryCode");

            migrationBuilder.RenameColumn(
                name: "TimeZone",
                table: "Cities",
                newName: "timeZone");

            migrationBuilder.RenameColumn(
                name: "NumAirports",
                table: "Cities",
                newName: "numAirports");

            migrationBuilder.RenameColumn(
                name: "DestinationCode",
                table: "Cities",
                newName: "destinationCode");

            migrationBuilder.RenameColumn(
                name: "CountryID",
                table: "Cities",
                newName: "countryID");

            migrationBuilder.RenameColumn(
                name: "CityProfilePath",
                table: "Cities",
                newName: "cityProfilePath");

            migrationBuilder.RenameColumn(
                name: "CityName",
                table: "Cities",
                newName: "cityName");

            migrationBuilder.RenameColumn(
                name: "CityDailingCode",
                table: "Cities",
                newName: "cityDailingCode");

            migrationBuilder.RenameColumn(
                name: "CityCode",
                table: "Cities",
                newName: "cityCode");

            migrationBuilder.RenameColumn(
                name: "CityID",
                table: "Cities",
                newName: "cityID");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_CountryID",
                table: "Cities",
                newName: "IX_Cities_countryID");

            migrationBuilder.CreateTable(
                name: "AirlineDefaultServiceFees",
                columns: table => new
                {
                    AirlineServiceFeeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PassengerType = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ServiceFeeType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirlineDefaultServiceFees", x => x.AirlineServiceFeeID);
                });

            migrationBuilder.CreateTable(
                name: "AirlineSpecificServiceFees",
                columns: table => new
                {
                    AirlineSpecificID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AirlineCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    FeeType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    ServiceType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PassengerType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirlineSpecificServiceFees", x => x.AirlineSpecificID);
                });

            migrationBuilder.CreateTable(
                name: "AirPriceRsps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirPriceRsps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookingInfoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Doc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bookingTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supplier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adults = table.Column<int>(type: "int", nullable: true),
                    Children = table.Column<int>(type: "int", nullable: true),
                    Infants = table.Column<int>(type: "int", nullable: true),
                    BookingID = table.Column<int>(type: "int", nullable: true),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    count = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    from = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    to = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    start = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    end = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlightNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    serviceclass0 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stops0 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total0 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    amount0 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    taxes0 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    currency0 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    totaltime0 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    totaltime1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stops1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdultTotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdultBasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdultTaxes = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ChildTotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ChildBasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ChildTaxes = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InfantTotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InfantTaxes = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AirlineCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    document = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    documenttype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    expiry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dob = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingInfoes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    RegestrationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LicenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Addressline2 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CityID = table.Column<int>(type: "int", nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ParentID = table.Column<int>(type: "int", nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CustomerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LogoPath = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerMask = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adults = table.Column<int>(type: "int", nullable: false),
                    Children = table.Column<int>(type: "int", nullable: false),
                    Infants = table.Column<int>(type: "int", nullable: false),
                    PNR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdultBasicFare = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ChildBasicFare = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InfantBasicFare = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdultTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ChildTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InfantTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalFare = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalTaxes = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GrandTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CustomerComission = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CustomerOComission = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AirlineComission = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Incentive = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IncentiveScale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BookingStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingTypeID = table.Column<int>(type: "int", nullable: true),
                    LeadFirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeadLastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryPhone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceClasses",
                columns: table => new
                {
                    ServiceClassID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    DefaultSelected = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceClasses", x => x.ServiceClassID);
                });

            migrationBuilder.CreateTable(
                name: "Incentives",
                columns: table => new
                {
                    IncentiveID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncentiveType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Incentives = table.Column<decimal>(type: "money", nullable: true),
                    Scale = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RefIncentives = table.Column<decimal>(type: "money", nullable: true),
                    RefScale = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ServiceID = table.Column<int>(type: "int", nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incentives", x => x.IncentiveID);
                    table.ForeignKey(
                        name: "FK_Incentives_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID");
                });

            migrationBuilder.CreateTable(
                name: "PassengerDetails",
                columns: table => new
                {
                    PassengerDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightBookingID = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: true),
                    DOB = table.Column<DateTime>(type: "date", nullable: true),
                    DocumentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Document = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DOE = table.Column<DateTime>(type: "date", nullable: true),
                    TicketNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TicketedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PassengerStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    SSR = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PassegerType = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerDetails", x => x.PassengerDetailID);
                    table.ForeignKey(
                        name: "FK_PassengerDetails_Reservations_FlightBookingID",
                        column: x => x.FlightBookingID,
                        principalTable: "Reservations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FlightSegments",
                columns: table => new
                {
                    FlightSegmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginID = table.Column<int>(type: "int", nullable: true),
                    DestinationID = table.Column<int>(type: "int", nullable: true),
                    DepartureDate = table.Column<DateTime>(type: "date", nullable: true),
                    DepartureTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartureGMTOffset = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArrivalDate = table.Column<DateTime>(type: "date", nullable: true),
                    ArrivalTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArrivalGMTOffset = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AirEquipType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ETicket = table.Column<bool>(type: "bit", nullable: true),
                    OperatingAirlineID = table.Column<int>(type: "int", nullable: true),
                    MarketingAirlineID = table.Column<int>(type: "int", nullable: true),
                    OpertingAirlineAirlineID = table.Column<int>(type: "int", nullable: false),
                    OperatingAirlineCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MarketingAirlineCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OperatingAirlineName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FlightNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FlightBookingID = table.Column<int>(type: "int", nullable: true),
                    ServiceClassID = table.Column<int>(type: "int", nullable: true),
                    ResBookDesigCode = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Way = table.Column<byte>(type: "tinyint", nullable: true),
                    cityID = table.Column<int>(type: "int", nullable: false),
                    City1cityID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightSegments", x => x.FlightSegmentID);
                    table.ForeignKey(
                        name: "FK_FlightSegments_Airlines_MarketingAirlineID",
                        column: x => x.MarketingAirlineID,
                        principalTable: "Airlines",
                        principalColumn: "AirlineID");
                    table.ForeignKey(
                        name: "FK_FlightSegments_Airlines_OpertingAirlineAirlineID",
                        column: x => x.OpertingAirlineAirlineID,
                        principalTable: "Airlines",
                        principalColumn: "AirlineID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlightSegments_Cities_City1cityID",
                        column: x => x.City1cityID,
                        principalTable: "Cities",
                        principalColumn: "cityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlightSegments_Cities_cityID",
                        column: x => x.cityID,
                        principalTable: "Cities",
                        principalColumn: "cityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlightSegments_Reservations_FlightBookingID",
                        column: x => x.FlightBookingID,
                        principalTable: "Reservations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FlightSegments_ServiceClasses_ServiceClassID",
                        column: x => x.ServiceClassID,
                        principalTable: "ServiceClasses",
                        principalColumn: "ServiceClassID");
                });

            migrationBuilder.CreateTable(
                name: "BookingRefund",
                columns: table => new
                {
                    BookingRefundID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PassengerDetailID = table.Column<int>(type: "int", nullable: true),
                    FlightSegmentID = table.Column<int>(type: "int", nullable: true),
                    RefundDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefundCharges = table.Column<decimal>(type: "money", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingRefund", x => x.BookingRefundID);
                    table.ForeignKey(
                        name: "FK_BookingRefund_FlightSegments_FlightSegmentID",
                        column: x => x.FlightSegmentID,
                        principalTable: "FlightSegments",
                        principalColumn: "FlightSegmentID");
                    table.ForeignKey(
                        name: "FK_BookingRefund_PassengerDetails_PassengerDetailID",
                        column: x => x.PassengerDetailID,
                        principalTable: "PassengerDetails",
                        principalColumn: "PassengerDetailID");
                });

            migrationBuilder.CreateTable(
                name: "FareBasisCodes",
                columns: table => new
                {
                    FareBasisCodeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaxType = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    FlightSegmentID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FareBasisCodes", x => x.FareBasisCodeID);
                    table.ForeignKey(
                        name: "FK_FareBasisCodes_FlightSegments_FlightSegmentID",
                        column: x => x.FlightSegmentID,
                        principalTable: "FlightSegments",
                        principalColumn: "FlightSegmentID");
                });

            migrationBuilder.CreateTable(
                name: "FlightSeat",
                columns: table => new
                {
                    SeatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PassengerDetailID = table.Column<int>(type: "int", nullable: true),
                    FlightSegmentID = table.Column<int>(type: "int", nullable: true),
                    TicketedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TicketNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SeatNumber = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    PaxSegmentStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightSeat", x => x.SeatID);
                    table.ForeignKey(
                        name: "FK_FlightSeat_FlightSegments_FlightSegmentID",
                        column: x => x.FlightSegmentID,
                        principalTable: "FlightSegments",
                        principalColumn: "FlightSegmentID");
                    table.ForeignKey(
                        name: "FK_FlightSeat_PassengerDetails_PassengerDetailID",
                        column: x => x.PassengerDetailID,
                        principalTable: "PassengerDetails",
                        principalColumn: "PassengerDetailID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingRefund_FlightSegmentID",
                table: "BookingRefund",
                column: "FlightSegmentID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingRefund_PassengerDetailID",
                table: "BookingRefund",
                column: "PassengerDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_FareBasisCodes_FlightSegmentID",
                table: "FareBasisCodes",
                column: "FlightSegmentID");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSeat_FlightSegmentID",
                table: "FlightSeat",
                column: "FlightSegmentID");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSeat_PassengerDetailID",
                table: "FlightSeat",
                column: "PassengerDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSegments_City1cityID",
                table: "FlightSegments",
                column: "City1cityID");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSegments_cityID",
                table: "FlightSegments",
                column: "cityID");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSegments_FlightBookingID",
                table: "FlightSegments",
                column: "FlightBookingID");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSegments_MarketingAirlineID",
                table: "FlightSegments",
                column: "MarketingAirlineID");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSegments_OpertingAirlineAirlineID",
                table: "FlightSegments",
                column: "OpertingAirlineAirlineID");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSegments_ServiceClassID",
                table: "FlightSegments",
                column: "ServiceClassID");

            migrationBuilder.CreateIndex(
                name: "IX_Incentives_CustomerID",
                table: "Incentives",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerDetails_FlightBookingID",
                table: "PassengerDetails",
                column: "FlightBookingID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_countryID",
                table: "Cities",
                column: "countryID",
                principalTable: "Countries",
                principalColumn: "CountryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countries_countryID",
                table: "Cities");

            migrationBuilder.DropTable(
                name: "AirlineDefaultServiceFees");

            migrationBuilder.DropTable(
                name: "AirlineSpecificServiceFees");

            migrationBuilder.DropTable(
                name: "AirPriceRsps");

            migrationBuilder.DropTable(
                name: "BookingInfoes");

            migrationBuilder.DropTable(
                name: "BookingRefund");

            migrationBuilder.DropTable(
                name: "FareBasisCodes");

            migrationBuilder.DropTable(
                name: "FlightSeat");

            migrationBuilder.DropTable(
                name: "Incentives");

            migrationBuilder.DropTable(
                name: "FlightSegments");

            migrationBuilder.DropTable(
                name: "PassengerDetails");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "ServiceClasses");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.RenameColumn(
                name: "timeZone",
                table: "Cities",
                newName: "TimeZone");

            migrationBuilder.RenameColumn(
                name: "numAirports",
                table: "Cities",
                newName: "NumAirports");

            migrationBuilder.RenameColumn(
                name: "destinationCode",
                table: "Cities",
                newName: "DestinationCode");

            migrationBuilder.RenameColumn(
                name: "countryID",
                table: "Cities",
                newName: "CountryID");

            migrationBuilder.RenameColumn(
                name: "countryCode",
                table: "Cities",
                newName: "countrycode");

            migrationBuilder.RenameColumn(
                name: "cityProfilePath",
                table: "Cities",
                newName: "CityProfilePath");

            migrationBuilder.RenameColumn(
                name: "cityName",
                table: "Cities",
                newName: "CityName");

            migrationBuilder.RenameColumn(
                name: "cityDailingCode",
                table: "Cities",
                newName: "CityDailingCode");

            migrationBuilder.RenameColumn(
                name: "cityCode",
                table: "Cities",
                newName: "CityCode");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Cities",
                newName: "state");

            migrationBuilder.RenameColumn(
                name: "Elevation",
                table: "Cities",
                newName: "elevation");

            migrationBuilder.RenameColumn(
                name: "cityID",
                table: "Cities",
                newName: "CityID");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_countryID",
                table: "Cities",
                newName: "IX_Cities_CountryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_CountryID",
                table: "Cities",
                column: "CountryID",
                principalTable: "Countries",
                principalColumn: "CountryID");
        }
    }
}
