namespace MexCoreWebAPI.Model
{
   

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
        public partial class Envelope
        {

            private EnvelopeBody bodyField;

            /// <remarks/>
            public EnvelopeBody Body
            {
                get
                {
                    return this.bodyField;
                }
                set
                {
                    this.bodyField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBody
        {

            private LowFareSearchRsp lowFareSearchRspField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.travelport.com/schema/air_v42_0")]
            public LowFareSearchRsp LowFareSearchRsp
            {
                get
                {
                    return this.lowFareSearchRspField;
                }
                set
                {
                    this.lowFareSearchRspField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.travelport.com/schema/air_v42_0", IsNullable = false)]
        public partial class LowFareSearchRsp
        {

            private LowFareSearchRspFlightDetails[] flightDetailsListField;

            private LowFareSearchRspAirSegment[] airSegmentListField;

            private LowFareSearchRspFareInfo[] fareInfoListField;

            private LowFareSearchRspRouteList routeListField;

            private LowFareSearchRspAirPricePoint[] airPricePointListField;

            private LowFareSearchRspBrand[] brandListField;

            private string traceIdField;

            private string transactionIdField;

            private ushort responseTimeField;

            private string distanceUnitsField;

            private string currencyTypeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("FlightDetails", IsNullable = false)]
            public LowFareSearchRspFlightDetails[] FlightDetailsList
            {
                get
                {
                    return this.flightDetailsListField;
                }
                set
                {
                    this.flightDetailsListField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("AirSegment", IsNullable = false)]
            public LowFareSearchRspAirSegment[] AirSegmentList
            {
                get
                {
                    return this.airSegmentListField;
                }
                set
                {
                    this.airSegmentListField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("FareInfo", IsNullable = false)]
            public LowFareSearchRspFareInfo[] FareInfoList
            {
                get
                {
                    return this.fareInfoListField;
                }
                set
                {
                    this.fareInfoListField = value;
                }
            }

            /// <remarks/>
            public LowFareSearchRspRouteList RouteList
            {
                get
                {
                    return this.routeListField;
                }
                set
                {
                    this.routeListField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("AirPricePoint", IsNullable = false)]
            public LowFareSearchRspAirPricePoint[] AirPricePointList
            {
                get
                {
                    return this.airPricePointListField;
                }
                set
                {
                    this.airPricePointListField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Brand", IsNullable = false)]
            public LowFareSearchRspBrand[] BrandList
            {
                get
                {
                    return this.brandListField;
                }
                set
                {
                    this.brandListField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string TraceId
            {
                get
                {
                    return this.traceIdField;
                }
                set
                {
                    this.traceIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string TransactionId
            {
                get
                {
                    return this.transactionIdField;
                }
                set
                {
                    this.transactionIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort ResponseTime
            {
                get
                {
                    return this.responseTimeField;
                }
                set
                {
                    this.responseTimeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DistanceUnits
            {
                get
                {
                    return this.distanceUnitsField;
                }
                set
                {
                    this.distanceUnitsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string CurrencyType
            {
                get
                {
                    return this.currencyTypeField;
                }
                set
                {
                    this.currencyTypeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspFlightDetails
        {

            private string keyField;

            private string originField;

            private string destinationField;

            private System.DateTime departureTimeField;

            private System.DateTime arrivalTimeField;

            private byte flightTimeField;

            private ushort travelTimeField;

            private string equipmentField;

            private string destinationTerminalField;

            private byte originTerminalField;

            private bool originTerminalFieldSpecified;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Key
            {
                get
                {
                    return this.keyField;
                }
                set
                {
                    this.keyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Origin
            {
                get
                {
                    return this.originField;
                }
                set
                {
                    this.originField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Destination
            {
                get
                {
                    return this.destinationField;
                }
                set
                {
                    this.destinationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public System.DateTime DepartureTime
            {
                get
                {
                    return this.departureTimeField;
                }
                set
                {
                    this.departureTimeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public System.DateTime ArrivalTime
            {
                get
                {
                    return this.arrivalTimeField;
                }
                set
                {
                    this.arrivalTimeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte FlightTime
            {
                get
                {
                    return this.flightTimeField;
                }
                set
                {
                    this.flightTimeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort TravelTime
            {
                get
                {
                    return this.travelTimeField;
                }
                set
                {
                    this.travelTimeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Equipment
            {
                get
                {
                    return this.equipmentField;
                }
                set
                {
                    this.equipmentField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DestinationTerminal
            {
                get
                {
                    return this.destinationTerminalField;
                }
                set
                {
                    this.destinationTerminalField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte OriginTerminal
            {
                get
                {
                    return this.originTerminalField;
                }
                set
                {
                    this.originTerminalField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool OriginTerminalSpecified
            {
                get
                {
                    return this.originTerminalFieldSpecified;
                }
                set
                {
                    this.originTerminalFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspAirSegment
        {

            private LowFareSearchRspAirSegmentCodeshareInfo codeshareInfoField;

            private LowFareSearchRspAirSegmentAirAvailInfo airAvailInfoField;

            private LowFareSearchRspAirSegmentFlightDetailsRef flightDetailsRefField;

            private string keyField;

            private byte groupField;

            private string carrierField;

            private ushort flightNumberField;

            private string originField;

            private string destinationField;

            private System.DateTime departureTimeField;

            private System.DateTime arrivalTimeField;

            private byte flightTimeField;

            private ushort distanceField;

            private string eTicketabilityField;

            private string equipmentField;

            private bool changeOfPlaneField;

            private string participantLevelField;

            private bool linkAvailabilityField;

            private string polledAvailabilityOptionField;

            private bool optionalServicesIndicatorField;

            private string availabilitySourceField;

            private string availabilityDisplayTypeField;

            /// <remarks/>
            public LowFareSearchRspAirSegmentCodeshareInfo CodeshareInfo
            {
                get
                {
                    return this.codeshareInfoField;
                }
                set
                {
                    this.codeshareInfoField = value;
                }
            }

            /// <remarks/>
            public LowFareSearchRspAirSegmentAirAvailInfo AirAvailInfo
            {
                get
                {
                    return this.airAvailInfoField;
                }
                set
                {
                    this.airAvailInfoField = value;
                }
            }

            /// <remarks/>
            public LowFareSearchRspAirSegmentFlightDetailsRef FlightDetailsRef
            {
                get
                {
                    return this.flightDetailsRefField;
                }
                set
                {
                    this.flightDetailsRefField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Key
            {
                get
                {
                    return this.keyField;
                }
                set
                {
                    this.keyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Group
            {
                get
                {
                    return this.groupField;
                }
                set
                {
                    this.groupField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Carrier
            {
                get
                {
                    return this.carrierField;
                }
                set
                {
                    this.carrierField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort FlightNumber
            {
                get
                {
                    return this.flightNumberField;
                }
                set
                {
                    this.flightNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Origin
            {
                get
                {
                    return this.originField;
                }
                set
                {
                    this.originField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Destination
            {
                get
                {
                    return this.destinationField;
                }
                set
                {
                    this.destinationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public System.DateTime DepartureTime
            {
                get
                {
                    return this.departureTimeField;
                }
                set
                {
                    this.departureTimeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public System.DateTime ArrivalTime
            {
                get
                {
                    return this.arrivalTimeField;
                }
                set
                {
                    this.arrivalTimeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte FlightTime
            {
                get
                {
                    return this.flightTimeField;
                }
                set
                {
                    this.flightTimeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort Distance
            {
                get
                {
                    return this.distanceField;
                }
                set
                {
                    this.distanceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ETicketability
            {
                get
                {
                    return this.eTicketabilityField;
                }
                set
                {
                    this.eTicketabilityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Equipment
            {
                get
                {
                    return this.equipmentField;
                }
                set
                {
                    this.equipmentField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool ChangeOfPlane
            {
                get
                {
                    return this.changeOfPlaneField;
                }
                set
                {
                    this.changeOfPlaneField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ParticipantLevel
            {
                get
                {
                    return this.participantLevelField;
                }
                set
                {
                    this.participantLevelField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool LinkAvailability
            {
                get
                {
                    return this.linkAvailabilityField;
                }
                set
                {
                    this.linkAvailabilityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string PolledAvailabilityOption
            {
                get
                {
                    return this.polledAvailabilityOptionField;
                }
                set
                {
                    this.polledAvailabilityOptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool OptionalServicesIndicator
            {
                get
                {
                    return this.optionalServicesIndicatorField;
                }
                set
                {
                    this.optionalServicesIndicatorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string AvailabilitySource
            {
                get
                {
                    return this.availabilitySourceField;
                }
                set
                {
                    this.availabilitySourceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string AvailabilityDisplayType
            {
                get
                {
                    return this.availabilityDisplayTypeField;
                }
                set
                {
                    this.availabilityDisplayTypeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspAirSegmentCodeshareInfo
        {

            private string operatingCarrierField;

            private ushort operatingFlightNumberField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string OperatingCarrier
            {
                get
                {
                    return this.operatingCarrierField;
                }
                set
                {
                    this.operatingCarrierField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort OperatingFlightNumber
            {
                get
                {
                    return this.operatingFlightNumberField;
                }
                set
                {
                    this.operatingFlightNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspAirSegmentAirAvailInfo
        {

            private string providerCodeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ProviderCode
            {
                get
                {
                    return this.providerCodeField;
                }
                set
                {
                    this.providerCodeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspAirSegmentFlightDetailsRef
        {

            private string keyField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Key
            {
                get
                {
                    return this.keyField;
                }
                set
                {
                    this.keyField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspFareInfo
        {

            private LowFareSearchRspFareInfoFareSurcharge fareSurchargeField;

            private LowFareSearchRspFareInfoBaggageAllowance baggageAllowanceField;

            private LowFareSearchRspFareInfoFareRuleKey fareRuleKeyField;

            private LowFareSearchRspFareInfoBrand brandField;

            private string keyField;

            private string fareBasisField;

            private string passengerTypeCodeField;

            private string originField;

            private string destinationField;

            private System.DateTime effectiveDateField;

            private System.DateTime departureDateField;

            private string amountField;

            private bool negotiatedFareField;

            private System.DateTime notValidBeforeField;

            private System.DateTime notValidAfterField;

            /// <remarks/>
            public LowFareSearchRspFareInfoFareSurcharge FareSurcharge
            {
                get
                {
                    return this.fareSurchargeField;
                }
                set
                {
                    this.fareSurchargeField = value;
                }
            }

            /// <remarks/>
            public LowFareSearchRspFareInfoBaggageAllowance BaggageAllowance
            {
                get
                {
                    return this.baggageAllowanceField;
                }
                set
                {
                    this.baggageAllowanceField = value;
                }
            }

            /// <remarks/>
            public LowFareSearchRspFareInfoFareRuleKey FareRuleKey
            {
                get
                {
                    return this.fareRuleKeyField;
                }
                set
                {
                    this.fareRuleKeyField = value;
                }
            }

            /// <remarks/>
            public LowFareSearchRspFareInfoBrand Brand
            {
                get
                {
                    return this.brandField;
                }
                set
                {
                    this.brandField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Key
            {
                get
                {
                    return this.keyField;
                }
                set
                {
                    this.keyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string FareBasis
            {
                get
                {
                    return this.fareBasisField;
                }
                set
                {
                    this.fareBasisField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string PassengerTypeCode
            {
                get
                {
                    return this.passengerTypeCodeField;
                }
                set
                {
                    this.passengerTypeCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Origin
            {
                get
                {
                    return this.originField;
                }
                set
                {
                    this.originField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Destination
            {
                get
                {
                    return this.destinationField;
                }
                set
                {
                    this.destinationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public System.DateTime EffectiveDate
            {
                get
                {
                    return this.effectiveDateField;
                }
                set
                {
                    this.effectiveDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
            public System.DateTime DepartureDate
            {
                get
                {
                    return this.departureDateField;
                }
                set
                {
                    this.departureDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Amount
            {
                get
                {
                    return this.amountField;
                }
                set
                {
                    this.amountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool NegotiatedFare
            {
                get
                {
                    return this.negotiatedFareField;
                }
                set
                {
                    this.negotiatedFareField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
            public System.DateTime NotValidBefore
            {
                get
                {
                    return this.notValidBeforeField;
                }
                set
                {
                    this.notValidBeforeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
            public System.DateTime NotValidAfter
            {
                get
                {
                    return this.notValidAfterField;
                }
                set
                {
                    this.notValidAfterField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspFareInfoFareSurcharge
        {

            private string keyField;

            private string typeField;

            private string amountField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Key
            {
                get
                {
                    return this.keyField;
                }
                set
                {
                    this.keyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Amount
            {
                get
                {
                    return this.amountField;
                }
                set
                {
                    this.amountField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspFareInfoBaggageAllowance
        {

            private byte numberOfPiecesField;

            private bool numberOfPiecesFieldSpecified;

            private LowFareSearchRspFareInfoBaggageAllowanceMaxWeight maxWeightField;

            /// <remarks/>
            public byte NumberOfPieces
            {
                get
                {
                    return this.numberOfPiecesField;
                }
                set
                {
                    this.numberOfPiecesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool NumberOfPiecesSpecified
            {
                get
                {
                    return this.numberOfPiecesFieldSpecified;
                }
                set
                {
                    this.numberOfPiecesFieldSpecified = value;
                }
            }

            /// <remarks/>
            public LowFareSearchRspFareInfoBaggageAllowanceMaxWeight MaxWeight
            {
                get
                {
                    return this.maxWeightField;
                }
                set
                {
                    this.maxWeightField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspFareInfoBaggageAllowanceMaxWeight
        {

            private byte valueField;

            private bool valueFieldSpecified;

            private string unitField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ValueSpecified
            {
                get
                {
                    return this.valueFieldSpecified;
                }
                set
                {
                    this.valueFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Unit
            {
                get
                {
                    return this.unitField;
                }
                set
                {
                    this.unitField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspFareInfoFareRuleKey
        {

            private string fareInfoRefField;

            private string providerCodeField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string FareInfoRef
            {
                get
                {
                    return this.fareInfoRefField;
                }
                set
                {
                    this.fareInfoRefField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ProviderCode
            {
                get
                {
                    return this.providerCodeField;
                }
                set
                {
                    this.providerCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspFareInfoBrand
        {

            private LowFareSearchRspFareInfoBrandUpsellBrand upsellBrandField;

            private string keyField;

            private bool brandFoundField;

            private bool brandFoundFieldSpecified;

            private bool upSellBrandFoundField;

            private bool upSellBrandFoundFieldSpecified;

            private uint brandIDField;

            private bool brandIDFieldSpecified;

            private uint upSellBrandIDField;

            private bool upSellBrandIDFieldSpecified;

            /// <remarks/>
            public LowFareSearchRspFareInfoBrandUpsellBrand UpsellBrand
            {
                get
                {
                    return this.upsellBrandField;
                }
                set
                {
                    this.upsellBrandField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Key
            {
                get
                {
                    return this.keyField;
                }
                set
                {
                    this.keyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool BrandFound
            {
                get
                {
                    return this.brandFoundField;
                }
                set
                {
                    this.brandFoundField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool BrandFoundSpecified
            {
                get
                {
                    return this.brandFoundFieldSpecified;
                }
                set
                {
                    this.brandFoundFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool UpSellBrandFound
            {
                get
                {
                    return this.upSellBrandFoundField;
                }
                set
                {
                    this.upSellBrandFoundField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool UpSellBrandFoundSpecified
            {
                get
                {
                    return this.upSellBrandFoundFieldSpecified;
                }
                set
                {
                    this.upSellBrandFoundFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint BrandID
            {
                get
                {
                    return this.brandIDField;
                }
                set
                {
                    this.brandIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool BrandIDSpecified
            {
                get
                {
                    return this.brandIDFieldSpecified;
                }
                set
                {
                    this.brandIDFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint UpSellBrandID
            {
                get
                {
                    return this.upSellBrandIDField;
                }
                set
                {
                    this.upSellBrandIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool UpSellBrandIDSpecified
            {
                get
                {
                    return this.upSellBrandIDFieldSpecified;
                }
                set
                {
                    this.upSellBrandIDFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspFareInfoBrandUpsellBrand
        {

            private string fareBasisField;

            private string fareInfoRefField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string FareBasis
            {
                get
                {
                    return this.fareBasisField;
                }
                set
                {
                    this.fareBasisField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string FareInfoRef
            {
                get
                {
                    return this.fareInfoRefField;
                }
                set
                {
                    this.fareInfoRefField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspRouteList
        {

            private LowFareSearchRspRouteListRoute routeField;

            /// <remarks/>
            public LowFareSearchRspRouteListRoute Route
            {
                get
                {
                    return this.routeField;
                }
                set
                {
                    this.routeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspRouteListRoute
        {

            private LowFareSearchRspRouteListRouteLeg[] legField;

            private string keyField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Leg")]
            public LowFareSearchRspRouteListRouteLeg[] Leg
            {
                get
                {
                    return this.legField;
                }
                set
                {
                    this.legField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Key
            {
                get
                {
                    return this.keyField;
                }
                set
                {
                    this.keyField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspRouteListRouteLeg
        {

            private string keyField;

            private byte groupField;

            private string originField;

            private string destinationField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Key
            {
                get
                {
                    return this.keyField;
                }
                set
                {
                    this.keyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Group
            {
                get
                {
                    return this.groupField;
                }
                set
                {
                    this.groupField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Origin
            {
                get
                {
                    return this.originField;
                }
                set
                {
                    this.originField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Destination
            {
                get
                {
                    return this.destinationField;
                }
                set
                {
                    this.destinationField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspAirPricePoint
        {

            private LowFareSearchRspAirPricePointAirPricingInfo airPricingInfoField;

            private string keyField;

            private string totalPriceField;

            private string basePriceField;

            private string approximateTotalPriceField;

            private string approximateBasePriceField;

            private string equivalentBasePriceField;

            private string taxesField;

            private string approximateTaxesField;

            private bool completeItineraryField;

            /// <remarks/>
            public LowFareSearchRspAirPricePointAirPricingInfo AirPricingInfo
            {
                get
                {
                    return this.airPricingInfoField;
                }
                set
                {
                    this.airPricingInfoField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Key
            {
                get
                {
                    return this.keyField;
                }
                set
                {
                    this.keyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string TotalPrice
            {
                get
                {
                    return this.totalPriceField;
                }
                set
                {
                    this.totalPriceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string BasePrice
            {
                get
                {
                    return this.basePriceField;
                }
                set
                {
                    this.basePriceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ApproximateTotalPrice
            {
                get
                {
                    return this.approximateTotalPriceField;
                }
                set
                {
                    this.approximateTotalPriceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ApproximateBasePrice
            {
                get
                {
                    return this.approximateBasePriceField;
                }
                set
                {
                    this.approximateBasePriceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string EquivalentBasePrice
            {
                get
                {
                    return this.equivalentBasePriceField;
                }
                set
                {
                    this.equivalentBasePriceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Taxes
            {
                get
                {
                    return this.taxesField;
                }
                set
                {
                    this.taxesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ApproximateTaxes
            {
                get
                {
                    return this.approximateTaxesField;
                }
                set
                {
                    this.approximateTaxesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool CompleteItinerary
            {
                get
                {
                    return this.completeItineraryField;
                }
                set
                {
                    this.completeItineraryField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspAirPricePointAirPricingInfo
        {

            private LowFareSearchRspAirPricePointAirPricingInfoFareInfoRef[] fareInfoRefField;

            private LowFareSearchRspAirPricePointAirPricingInfoTaxInfo[] taxInfoField;

            private string fareCalcField;

            private LowFareSearchRspAirPricePointAirPricingInfoPassengerType passengerTypeField;

            private LowFareSearchRspAirPricePointAirPricingInfoChangePenalty changePenaltyField;

            private LowFareSearchRspAirPricePointAirPricingInfoCancelPenalty cancelPenaltyField;

            private LowFareSearchRspAirPricePointAirPricingInfoFlightOption[] flightOptionsListField;

            private string keyField;

            private string totalPriceField;

            private string basePriceField;

            private string approximateTotalPriceField;

            private string approximateBasePriceField;

            private string equivalentBasePriceField;

            private string taxesField;

            private System.DateTime latestTicketingTimeField;

            private string pricingMethodField;

            private bool refundableField;

            private bool refundableFieldSpecified;

            private string eTicketabilityField;

            private string platingCarrierField;

            private string providerCodeField;

            private bool cat35IndicatorField;

            private bool cat35IndicatorFieldSpecified;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("FareInfoRef")]
            public LowFareSearchRspAirPricePointAirPricingInfoFareInfoRef[] FareInfoRef
            {
                get
                {
                    return this.fareInfoRefField;
                }
                set
                {
                    this.fareInfoRefField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("TaxInfo")]
            public LowFareSearchRspAirPricePointAirPricingInfoTaxInfo[] TaxInfo
            {
                get
                {
                    return this.taxInfoField;
                }
                set
                {
                    this.taxInfoField = value;
                }
            }

            /// <remarks/>
            public string FareCalc
            {
                get
                {
                    return this.fareCalcField;
                }
                set
                {
                    this.fareCalcField = value;
                }
            }

            /// <remarks/>
            public LowFareSearchRspAirPricePointAirPricingInfoPassengerType PassengerType
            {
                get
                {
                    return this.passengerTypeField;
                }
                set
                {
                    this.passengerTypeField = value;
                }
            }

            /// <remarks/>
            public LowFareSearchRspAirPricePointAirPricingInfoChangePenalty ChangePenalty
            {
                get
                {
                    return this.changePenaltyField;
                }
                set
                {
                    this.changePenaltyField = value;
                }
            }

            /// <remarks/>
            public LowFareSearchRspAirPricePointAirPricingInfoCancelPenalty CancelPenalty
            {
                get
                {
                    return this.cancelPenaltyField;
                }
                set
                {
                    this.cancelPenaltyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("FlightOption", IsNullable = false)]
            public LowFareSearchRspAirPricePointAirPricingInfoFlightOption[] FlightOptionsList
            {
                get
                {
                    return this.flightOptionsListField;
                }
                set
                {
                    this.flightOptionsListField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Key
            {
                get
                {
                    return this.keyField;
                }
                set
                {
                    this.keyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string TotalPrice
            {
                get
                {
                    return this.totalPriceField;
                }
                set
                {
                    this.totalPriceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string BasePrice
            {
                get
                {
                    return this.basePriceField;
                }
                set
                {
                    this.basePriceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ApproximateTotalPrice
            {
                get
                {
                    return this.approximateTotalPriceField;
                }
                set
                {
                    this.approximateTotalPriceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ApproximateBasePrice
            {
                get
                {
                    return this.approximateBasePriceField;
                }
                set
                {
                    this.approximateBasePriceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string EquivalentBasePrice
            {
                get
                {
                    return this.equivalentBasePriceField;
                }
                set
                {
                    this.equivalentBasePriceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Taxes
            {
                get
                {
                    return this.taxesField;
                }
                set
                {
                    this.taxesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public System.DateTime LatestTicketingTime
            {
                get
                {
                    return this.latestTicketingTimeField;
                }
                set
                {
                    this.latestTicketingTimeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string PricingMethod
            {
                get
                {
                    return this.pricingMethodField;
                }
                set
                {
                    this.pricingMethodField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Refundable
            {
                get
                {
                    return this.refundableField;
                }
                set
                {
                    this.refundableField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool RefundableSpecified
            {
                get
                {
                    return this.refundableFieldSpecified;
                }
                set
                {
                    this.refundableFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ETicketability
            {
                get
                {
                    return this.eTicketabilityField;
                }
                set
                {
                    this.eTicketabilityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string PlatingCarrier
            {
                get
                {
                    return this.platingCarrierField;
                }
                set
                {
                    this.platingCarrierField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ProviderCode
            {
                get
                {
                    return this.providerCodeField;
                }
                set
                {
                    this.providerCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Cat35Indicator
            {
                get
                {
                    return this.cat35IndicatorField;
                }
                set
                {
                    this.cat35IndicatorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool Cat35IndicatorSpecified
            {
                get
                {
                    return this.cat35IndicatorFieldSpecified;
                }
                set
                {
                    this.cat35IndicatorFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspAirPricePointAirPricingInfoFareInfoRef
        {

            private string keyField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Key
            {
                get
                {
                    return this.keyField;
                }
                set
                {
                    this.keyField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspAirPricePointAirPricingInfoTaxInfo
        {

            private string categoryField;

            private string amountField;

            private string keyField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Category
            {
                get
                {
                    return this.categoryField;
                }
                set
                {
                    this.categoryField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Amount
            {
                get
                {
                    return this.amountField;
                }
                set
                {
                    this.amountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Key
            {
                get
                {
                    return this.keyField;
                }
                set
                {
                    this.keyField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspAirPricePointAirPricingInfoPassengerType
        {

            private string codeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Code
            {
                get
                {
                    return this.codeField;
                }
                set
                {
                    this.codeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspAirPricePointAirPricingInfoChangePenalty
        {

            private decimal percentageField;

            private bool percentageFieldSpecified;

            private string amountField;

            /// <remarks/>
            public decimal Percentage
            {
                get
                {
                    return this.percentageField;
                }
                set
                {
                    this.percentageField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool PercentageSpecified
            {
                get
                {
                    return this.percentageFieldSpecified;
                }
                set
                {
                    this.percentageFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string Amount
            {
                get
                {
                    return this.amountField;
                }
                set
                {
                    this.amountField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspAirPricePointAirPricingInfoCancelPenalty
        {

            private decimal percentageField;

            private bool percentageFieldSpecified;

            private string amountField;

            /// <remarks/>
            public decimal Percentage
            {
                get
                {
                    return this.percentageField;
                }
                set
                {
                    this.percentageField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool PercentageSpecified
            {
                get
                {
                    return this.percentageFieldSpecified;
                }
                set
                {
                    this.percentageFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string Amount
            {
                get
                {
                    return this.amountField;
                }
                set
                {
                    this.amountField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspAirPricePointAirPricingInfoFlightOption
        {

            private LowFareSearchRspAirPricePointAirPricingInfoFlightOptionOption[] optionField;

            private string legRefField;

            private string destinationField;

            private string originField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Option")]
            public LowFareSearchRspAirPricePointAirPricingInfoFlightOptionOption[] Option
            {
                get
                {
                    return this.optionField;
                }
                set
                {
                    this.optionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string LegRef
            {
                get
                {
                    return this.legRefField;
                }
                set
                {
                    this.legRefField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Destination
            {
                get
                {
                    return this.destinationField;
                }
                set
                {
                    this.destinationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Origin
            {
                get
                {
                    return this.originField;
                }
                set
                {
                    this.originField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspAirPricePointAirPricingInfoFlightOptionOption
        {

            private LowFareSearchRspAirPricePointAirPricingInfoFlightOptionOptionBookingInfo[] bookingInfoField;

            private LowFareSearchRspAirPricePointAirPricingInfoFlightOptionOptionConnection[] connectionField;

            private string keyField;

            private string travelTimeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("BookingInfo")]
            public LowFareSearchRspAirPricePointAirPricingInfoFlightOptionOptionBookingInfo[] BookingInfo
            {
                get
                {
                    return this.bookingInfoField;
                }
                set
                {
                    this.bookingInfoField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Connection")]
            public LowFareSearchRspAirPricePointAirPricingInfoFlightOptionOptionConnection[] Connection
            {
                get
                {
                    return this.connectionField;
                }
                set
                {
                    this.connectionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Key
            {
                get
                {
                    return this.keyField;
                }
                set
                {
                    this.keyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "duration")]
            public string TravelTime
            {
                get
                {
                    return this.travelTimeField;
                }
                set
                {
                    this.travelTimeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspAirPricePointAirPricingInfoFlightOptionOptionBookingInfo
        {

            private string bookingCodeField;

            private byte bookingCountField;

            private string cabinClassField;

            private string fareInfoRefField;

            private string segmentRefField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string BookingCode
            {
                get
                {
                    return this.bookingCodeField;
                }
                set
                {
                    this.bookingCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte BookingCount
            {
                get
                {
                    return this.bookingCountField;
                }
                set
                {
                    this.bookingCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string CabinClass
            {
                get
                {
                    return this.cabinClassField;
                }
                set
                {
                    this.cabinClassField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string FareInfoRef
            {
                get
                {
                    return this.fareInfoRefField;
                }
                set
                {
                    this.fareInfoRefField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string SegmentRef
            {
                get
                {
                    return this.segmentRefField;
                }
                set
                {
                    this.segmentRefField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspAirPricePointAirPricingInfoFlightOptionOptionConnection
        {

            private byte segmentIndexField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte SegmentIndex
            {
                get
                {
                    return this.segmentIndexField;
                }
                set
                {
                    this.segmentIndexField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspBrand
        {

            private LowFareSearchRspBrandTitle[] titleField;

            private LowFareSearchRspBrandText[] textField;

            private LowFareSearchRspBrandImageLocation[] imageLocationField;

            private string keyField;

            private uint brandIDField;

            private string nameField;

            private bool brandedDetailsAvailableField;

            private string carrierField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Title")]
            public LowFareSearchRspBrandTitle[] Title
            {
                get
                {
                    return this.titleField;
                }
                set
                {
                    this.titleField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Text")]
            public LowFareSearchRspBrandText[] Text
            {
                get
                {
                    return this.textField;
                }
                set
                {
                    this.textField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ImageLocation")]
            public LowFareSearchRspBrandImageLocation[] ImageLocation
            {
                get
                {
                    return this.imageLocationField;
                }
                set
                {
                    this.imageLocationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Key
            {
                get
                {
                    return this.keyField;
                }
                set
                {
                    this.keyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint BrandID
            {
                get
                {
                    return this.brandIDField;
                }
                set
                {
                    this.brandIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool BrandedDetailsAvailable
            {
                get
                {
                    return this.brandedDetailsAvailableField;
                }
                set
                {
                    this.brandedDetailsAvailableField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Carrier
            {
                get
                {
                    return this.carrierField;
                }
                set
                {
                    this.carrierField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspBrandTitle
        {

            private string typeField;

            private string languageCodeField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string LanguageCode
            {
                get
                {
                    return this.languageCodeField;
                }
                set
                {
                    this.languageCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspBrandText
        {

            private string typeField;

            private string languageCodeField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string LanguageCode
            {
                get
                {
                    return this.languageCodeField;
                }
                set
                {
                    this.languageCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public partial class LowFareSearchRspBrandImageLocation
        {

            private string typeField;

            private ushort imageWidthField;

            private ushort imageHeightField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort ImageWidth
            {
                get
                {
                    return this.imageWidthField;
                }
                set
                {
                    this.imageWidthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort ImageHeight
            {
                get
                {
                    return this.imageHeightField;
                }
                set
                {
                    this.imageHeightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

}
