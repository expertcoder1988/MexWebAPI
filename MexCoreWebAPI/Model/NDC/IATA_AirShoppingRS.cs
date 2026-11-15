namespace MexCoreWebAPI.Model.NDC
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

        private IATA_AirShoppingRS iATA_AirShoppingRSField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
        public IATA_AirShoppingRS IATA_AirShoppingRS
        {
            get
            {
                return this.iATA_AirShoppingRSField;
            }
            set
            {
                this.iATA_AirShoppingRSField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS", IsNullable = false)]
    public partial class IATA_AirShoppingRS
    {

        private IATA_AirShoppingRSResponse responseField;

        private IATA_AirShoppingRSPayloadAttributes payloadAttributesField;

        /// <remarks/>
        public IATA_AirShoppingRSResponse Response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSPayloadAttributes PayloadAttributes
        {
            get
            {
                return this.payloadAttributesField;
            }
            set
            {
                this.payloadAttributesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponse
    {

        private IATA_AirShoppingRSResponseDataLists dataListsField;

        private IATA_AirShoppingRSResponseOffersGroup offersGroupField;

        private IATA_AirShoppingRSResponseShoppingResponse shoppingResponseField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseDataLists DataLists
        {
            get
            {
                return this.dataListsField;
            }
            set
            {
                this.dataListsField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroup OffersGroup
        {
            get
            {
                return this.offersGroupField;
            }
            set
            {
                this.offersGroupField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseShoppingResponse ShoppingResponse
        {
            get
            {
                return this.shoppingResponseField;
            }
            set
            {
                this.shoppingResponseField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseDataLists
    {

        private IATA_AirShoppingRSResponseDataListsBaggageAllowance[] baggageAllowanceListField;

        private IATA_AirShoppingRSResponseDataListsOriginDestList originDestListField;

        private IATA_AirShoppingRSResponseDataListsPaxJourney[] paxJourneyListField;

        private IATA_AirShoppingRSResponseDataListsPaxList paxListField;

        private IATA_AirShoppingRSResponseDataListsPaxSegment[] paxSegmentListField;

        private IATA_AirShoppingRSResponseDataListsServiceDefinition[] serviceDefinitionListField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("BaggageAllowance", IsNullable = false)]
        public IATA_AirShoppingRSResponseDataListsBaggageAllowance[] BaggageAllowanceList
        {
            get
            {
                return this.baggageAllowanceListField;
            }
            set
            {
                this.baggageAllowanceListField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseDataListsOriginDestList OriginDestList
        {
            get
            {
                return this.originDestListField;
            }
            set
            {
                this.originDestListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("PaxJourney", IsNullable = false)]
        public IATA_AirShoppingRSResponseDataListsPaxJourney[] PaxJourneyList
        {
            get
            {
                return this.paxJourneyListField;
            }
            set
            {
                this.paxJourneyListField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseDataListsPaxList PaxList
        {
            get
            {
                return this.paxListField;
            }
            set
            {
                this.paxListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("PaxSegment", IsNullable = false)]
        public IATA_AirShoppingRSResponseDataListsPaxSegment[] PaxSegmentList
        {
            get
            {
                return this.paxSegmentListField;
            }
            set
            {
                this.paxSegmentListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ServiceDefinition", IsNullable = false)]
        public IATA_AirShoppingRSResponseDataListsServiceDefinition[] ServiceDefinitionList
        {
            get
            {
                return this.serviceDefinitionListField;
            }
            set
            {
                this.serviceDefinitionListField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseDataListsBaggageAllowance
    {

        private string baggageAllowanceIDField;

        private IATA_AirShoppingRSResponseDataListsBaggageAllowancePieceAllowance pieceAllowanceField;

        private string typeCodeField;

        /// <remarks/>
        public string BaggageAllowanceID
        {
            get
            {
                return this.baggageAllowanceIDField;
            }
            set
            {
                this.baggageAllowanceIDField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseDataListsBaggageAllowancePieceAllowance PieceAllowance
        {
            get
            {
                return this.pieceAllowanceField;
            }
            set
            {
                this.pieceAllowanceField = value;
            }
        }

        /// <remarks/>
        public string TypeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseDataListsBaggageAllowancePieceAllowance
    {

        private string applicablePartyTextField;

        private IATA_AirShoppingRSResponseDataListsBaggageAllowancePieceAllowancePieceWeightAllowance pieceWeightAllowanceField;

        private decimal totalQtyField;

        /// <remarks/>
        public string ApplicablePartyText
        {
            get
            {
                return this.applicablePartyTextField;
            }
            set
            {
                this.applicablePartyTextField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseDataListsBaggageAllowancePieceAllowancePieceWeightAllowance PieceWeightAllowance
        {
            get
            {
                return this.pieceWeightAllowanceField;
            }
            set
            {
                this.pieceWeightAllowanceField = value;
            }
        }

        /// <remarks/>
        public decimal TotalQty
        {
            get
            {
                return this.totalQtyField;
            }
            set
            {
                this.totalQtyField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseDataListsBaggageAllowancePieceAllowancePieceWeightAllowance
    {

        private IATA_AirShoppingRSResponseDataListsBaggageAllowancePieceAllowancePieceWeightAllowanceMaximumWeightMeasure maximumWeightMeasureField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseDataListsBaggageAllowancePieceAllowancePieceWeightAllowanceMaximumWeightMeasure MaximumWeightMeasure
        {
            get
            {
                return this.maximumWeightMeasureField;
            }
            set
            {
                this.maximumWeightMeasureField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseDataListsBaggageAllowancePieceAllowancePieceWeightAllowanceMaximumWeightMeasure
    {

        private string unitCodeField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string UnitCode
        {
            get
            {
                return this.unitCodeField;
            }
            set
            {
                this.unitCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseDataListsOriginDestList
    {

        private IATA_AirShoppingRSResponseDataListsOriginDestListOriginDest originDestField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseDataListsOriginDestListOriginDest OriginDest
        {
            get
            {
                return this.originDestField;
            }
            set
            {
                this.originDestField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseDataListsOriginDestListOriginDest
    {

        private string destCodeField;

        private string originCodeField;

        private string originDestIDField;

        private string[] paxJourneyRefIDField;

        /// <remarks/>
        public string DestCode
        {
            get
            {
                return this.destCodeField;
            }
            set
            {
                this.destCodeField = value;
            }
        }

        /// <remarks/>
        public string OriginCode
        {
            get
            {
                return this.originCodeField;
            }
            set
            {
                this.originCodeField = value;
            }
        }

        /// <remarks/>
        public string OriginDestID
        {
            get
            {
                return this.originDestIDField;
            }
            set
            {
                this.originDestIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PaxJourneyRefID")]
        public string[] PaxJourneyRefID
        {
            get
            {
                return this.paxJourneyRefIDField;
            }
            set
            {
                this.paxJourneyRefIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseDataListsPaxJourney
    {

        private string paxJourneyIDField;

        private string[] paxSegmentRefIDField;

        /// <remarks/>
        public string PaxJourneyID
        {
            get
            {
                return this.paxJourneyIDField;
            }
            set
            {
                this.paxJourneyIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PaxSegmentRefID")]
        public string[] PaxSegmentRefID
        {
            get
            {
                return this.paxSegmentRefIDField;
            }
            set
            {
                this.paxSegmentRefIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseDataListsPaxList
    {

        private IATA_AirShoppingRSResponseDataListsPaxListPax paxField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseDataListsPaxListPax Pax
        {
            get
            {
                return this.paxField;
            }
            set
            {
                this.paxField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseDataListsPaxListPax
    {

        private string paxIDField;

        private string pTCField;

        /// <remarks/>
        public string PaxID
        {
            get
            {
                return this.paxIDField;
            }
            set
            {
                this.paxIDField = value;
            }
        }

        /// <remarks/>
        public string PTC
        {
            get
            {
                return this.pTCField;
            }
            set
            {
                this.pTCField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseDataListsPaxSegment
    {

        private IATA_AirShoppingRSResponseDataListsPaxSegmentArrival arrivalField;

        private IATA_AirShoppingRSResponseDataListsPaxSegmentDep depField;

        private string durationField;

        private IATA_AirShoppingRSResponseDataListsPaxSegmentMarketingCarrierInfo marketingCarrierInfoField;

        private IATA_AirShoppingRSResponseDataListsPaxSegmentOperatingCarrierInfo operatingCarrierInfoField;

        private string paxSegmentIDField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseDataListsPaxSegmentArrival Arrival
        {
            get
            {
                return this.arrivalField;
            }
            set
            {
                this.arrivalField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseDataListsPaxSegmentDep Dep
        {
            get
            {
                return this.depField;
            }
            set
            {
                this.depField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
        public string Duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseDataListsPaxSegmentMarketingCarrierInfo MarketingCarrierInfo
        {
            get
            {
                return this.marketingCarrierInfoField;
            }
            set
            {
                this.marketingCarrierInfoField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseDataListsPaxSegmentOperatingCarrierInfo OperatingCarrierInfo
        {
            get
            {
                return this.operatingCarrierInfoField;
            }
            set
            {
                this.operatingCarrierInfoField = value;
            }
        }

        /// <remarks/>
        public string PaxSegmentID
        {
            get
            {
                return this.paxSegmentIDField;
            }
            set
            {
                this.paxSegmentIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseDataListsPaxSegmentArrival
    {

        private System.DateTime aircraftScheduledDateTimeField;

        private string iATA_LocationCodeField;

        private string stationNameField;

        /// <remarks/>
        public System.DateTime AircraftScheduledDateTime
        {
            get
            {
                return this.aircraftScheduledDateTimeField;
            }
            set
            {
                this.aircraftScheduledDateTimeField = value;
            }
        }

        /// <remarks/>
        public string IATA_LocationCode
        {
            get
            {
                return this.iATA_LocationCodeField;
            }
            set
            {
                this.iATA_LocationCodeField = value;
            }
        }

        /// <remarks/>
        public string StationName
        {
            get
            {
                return this.stationNameField;
            }
            set
            {
                this.stationNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseDataListsPaxSegmentDep
    {

        private System.DateTime aircraftScheduledDateTimeField;

        private string iATA_LocationCodeField;

        private string stationNameField;

        /// <remarks/>
        public System.DateTime AircraftScheduledDateTime
        {
            get
            {
                return this.aircraftScheduledDateTimeField;
            }
            set
            {
                this.aircraftScheduledDateTimeField = value;
            }
        }

        /// <remarks/>
        public string IATA_LocationCode
        {
            get
            {
                return this.iATA_LocationCodeField;
            }
            set
            {
                this.iATA_LocationCodeField = value;
            }
        }

        /// <remarks/>
        public string StationName
        {
            get
            {
                return this.stationNameField;
            }
            set
            {
                this.stationNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseDataListsPaxSegmentMarketingCarrierInfo
    {

        private string carrierDesigCodeField;

        private string carrierNameField;

        private ushort marketingCarrierFlightNumberTextField;

        /// <remarks/>
        public string CarrierDesigCode
        {
            get
            {
                return this.carrierDesigCodeField;
            }
            set
            {
                this.carrierDesigCodeField = value;
            }
        }

        /// <remarks/>
        public string CarrierName
        {
            get
            {
                return this.carrierNameField;
            }
            set
            {
                this.carrierNameField = value;
            }
        }

        /// <remarks/>
        public ushort MarketingCarrierFlightNumberText
        {
            get
            {
                return this.marketingCarrierFlightNumberTextField;
            }
            set
            {
                this.marketingCarrierFlightNumberTextField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseDataListsPaxSegmentOperatingCarrierInfo
    {

        private string carrierDesigCodeField;

        private string carrierNameField;

        private ushort operatingCarrierFlightNumberTextField;

        /// <remarks/>
        public string CarrierDesigCode
        {
            get
            {
                return this.carrierDesigCodeField;
            }
            set
            {
                this.carrierDesigCodeField = value;
            }
        }

        /// <remarks/>
        public string CarrierName
        {
            get
            {
                return this.carrierNameField;
            }
            set
            {
                this.carrierNameField = value;
            }
        }

        /// <remarks/>
        public ushort OperatingCarrierFlightNumberText
        {
            get
            {
                return this.operatingCarrierFlightNumberTextField;
            }
            set
            {
                this.operatingCarrierFlightNumberTextField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseDataListsServiceDefinition
    {

        private string nameField;

        private string serviceCodeField;

        private IATA_AirShoppingRSResponseDataListsServiceDefinitionServiceDefinitionAssociation serviceDefinitionAssociationField;

        private string serviceDefinitionIDField;

        /// <remarks/>
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
        public string ServiceCode
        {
            get
            {
                return this.serviceCodeField;
            }
            set
            {
                this.serviceCodeField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseDataListsServiceDefinitionServiceDefinitionAssociation ServiceDefinitionAssociation
        {
            get
            {
                return this.serviceDefinitionAssociationField;
            }
            set
            {
                this.serviceDefinitionAssociationField = value;
            }
        }

        /// <remarks/>
        public string ServiceDefinitionID
        {
            get
            {
                return this.serviceDefinitionIDField;
            }
            set
            {
                this.serviceDefinitionIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseDataListsServiceDefinitionServiceDefinitionAssociation
    {

        private IATA_AirShoppingRSResponseDataListsServiceDefinitionServiceDefinitionAssociationBaggageAllowanceRef baggageAllowanceRefField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseDataListsServiceDefinitionServiceDefinitionAssociationBaggageAllowanceRef BaggageAllowanceRef
        {
            get
            {
                return this.baggageAllowanceRefField;
            }
            set
            {
                this.baggageAllowanceRefField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseDataListsServiceDefinitionServiceDefinitionAssociationBaggageAllowanceRef
    {

        private string baggageAllowanceRefIDField;

        /// <remarks/>
        public string BaggageAllowanceRefID
        {
            get
            {
                return this.baggageAllowanceRefIDField;
            }
            set
            {
                this.baggageAllowanceRefIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroup
    {

        private IATA_AirShoppingRSResponseOffersGroupAllOffersSummary allOffersSummaryField;

        private IATA_AirShoppingRSResponseOffersGroupOffer[] carrierOffersField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupAllOffersSummary AllOffersSummary
        {
            get
            {
                return this.allOffersSummaryField;
            }
            set
            {
                this.allOffersSummaryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Offer", IsNullable = false)]
        public IATA_AirShoppingRSResponseOffersGroupOffer[] CarrierOffers
        {
            get
            {
                return this.carrierOffersField;
            }
            set
            {
                this.carrierOffersField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupAllOffersSummary
    {

        private byte matchedOfferQtyField;

        /// <remarks/>
        public byte MatchedOfferQty
        {
            get
            {
                return this.matchedOfferQtyField;
            }
            set
            {
                this.matchedOfferQtyField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOffer
    {

        private IATA_AirShoppingRSResponseOffersGroupOfferJourneyOverview journeyOverviewField;

        private string offerIDField;

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItem offerItemField;

        private string ownerCodeField;

        private IATA_AirShoppingRSResponseOffersGroupOfferTotalPrice totalPriceField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferJourneyOverview JourneyOverview
        {
            get
            {
                return this.journeyOverviewField;
            }
            set
            {
                this.journeyOverviewField = value;
            }
        }

        /// <remarks/>
        public string OfferID
        {
            get
            {
                return this.offerIDField;
            }
            set
            {
                this.offerIDField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItem OfferItem
        {
            get
            {
                return this.offerItemField;
            }
            set
            {
                this.offerItemField = value;
            }
        }

        /// <remarks/>
        public string OwnerCode
        {
            get
            {
                return this.ownerCodeField;
            }
            set
            {
                this.ownerCodeField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferTotalPrice TotalPrice
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferJourneyOverview
    {

        private IATA_AirShoppingRSResponseOffersGroupOfferJourneyOverviewJourneyPriceClass journeyPriceClassField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferJourneyOverviewJourneyPriceClass JourneyPriceClass
        {
            get
            {
                return this.journeyPriceClassField;
            }
            set
            {
                this.journeyPriceClassField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferJourneyOverviewJourneyPriceClass
    {

        private string paxJourneyRefIDField;

        /// <remarks/>
        public string PaxJourneyRefID
        {
            get
            {
                return this.paxJourneyRefIDField;
            }
            set
            {
                this.paxJourneyRefIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItem
    {

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemFareDetail[] fareDetailField;

        private bool mandatoryIndField;

        private string offerItemIDField;

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemOfferItemPaymentTimeLimit offerItemPaymentTimeLimitField;

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPrice priceField;

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemService[] serviceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("FareDetail")]
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemFareDetail[] FareDetail
        {
            get
            {
                return this.fareDetailField;
            }
            set
            {
                this.fareDetailField = value;
            }
        }

        /// <remarks/>
        public bool MandatoryInd
        {
            get
            {
                return this.mandatoryIndField;
            }
            set
            {
                this.mandatoryIndField = value;
            }
        }

        /// <remarks/>
        public string OfferItemID
        {
            get
            {
                return this.offerItemIDField;
            }
            set
            {
                this.offerItemIDField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemOfferItemPaymentTimeLimit OfferItemPaymentTimeLimit
        {
            get
            {
                return this.offerItemPaymentTimeLimitField;
            }
            set
            {
                this.offerItemPaymentTimeLimitField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPrice Price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Service")]
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemService[] Service
        {
            get
            {
                return this.serviceField;
            }
            set
            {
                this.serviceField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemFareDetail
    {

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemFareDetailFareComponent fareComponentField;

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemFareDetailFarePriceType farePriceTypeField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemFareDetailFareComponent FareComponent
        {
            get
            {
                return this.fareComponentField;
            }
            set
            {
                this.fareComponentField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemFareDetailFarePriceType FarePriceType
        {
            get
            {
                return this.farePriceTypeField;
            }
            set
            {
                this.farePriceTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemFareDetailFareComponent
    {

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemFareDetailFareComponentCabinType cabinTypeField;

        private string fareBasisCodeField;

        private string fareTypeCodeField;

        private string paxSegmentRefIDField;

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemFareDetailFareComponentRBD rBDField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemFareDetailFareComponentCabinType CabinType
        {
            get
            {
                return this.cabinTypeField;
            }
            set
            {
                this.cabinTypeField = value;
            }
        }

        /// <remarks/>
        public string FareBasisCode
        {
            get
            {
                return this.fareBasisCodeField;
            }
            set
            {
                this.fareBasisCodeField = value;
            }
        }

        /// <remarks/>
        public string FareTypeCode
        {
            get
            {
                return this.fareTypeCodeField;
            }
            set
            {
                this.fareTypeCodeField = value;
            }
        }

        /// <remarks/>
        public string PaxSegmentRefID
        {
            get
            {
                return this.paxSegmentRefIDField;
            }
            set
            {
                this.paxSegmentRefIDField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemFareDetailFareComponentRBD RBD
        {
            get
            {
                return this.rBDField;
            }
            set
            {
                this.rBDField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemFareDetailFareComponentCabinType
    {

        private string cabinTypeCodeField;

        private string cabinTypeNameField;

        /// <remarks/>
        public string CabinTypeCode
        {
            get
            {
                return this.cabinTypeCodeField;
            }
            set
            {
                this.cabinTypeCodeField = value;
            }
        }

        /// <remarks/>
        public string CabinTypeName
        {
            get
            {
                return this.cabinTypeNameField;
            }
            set
            {
                this.cabinTypeNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemFareDetailFareComponentRBD
    {

        private string rBD_CodeField;

        /// <remarks/>
        public string RBD_Code
        {
            get
            {
                return this.rBD_CodeField;
            }
            set
            {
                this.rBD_CodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemFareDetailFarePriceType
    {

        private string farePriceTypeCodeField;

        private object priceField;

        /// <remarks/>
        public string FarePriceTypeCode
        {
            get
            {
                return this.farePriceTypeCodeField;
            }
            set
            {
                this.farePriceTypeCodeField = value;
            }
        }

        /// <remarks/>
        public object Price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemOfferItemPaymentTimeLimit
    {

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemOfferItemPaymentTimeLimitPaymentTimeLimitDate paymentTimeLimitDateField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemOfferItemPaymentTimeLimitPaymentTimeLimitDate PaymentTimeLimitDate
        {
            get
            {
                return this.paymentTimeLimitDateField;
            }
            set
            {
                this.paymentTimeLimitDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemOfferItemPaymentTimeLimitPaymentTimeLimitDate
    {

        private System.DateTime paymentTimeLimitDateTimeField;

        /// <remarks/>
        public System.DateTime PaymentTimeLimitDateTime
        {
            get
            {
                return this.paymentTimeLimitDateTimeField;
            }
            set
            {
                this.paymentTimeLimitDateTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPrice
    {

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceBaseAmount baseAmountField;

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceEquivAmount equivAmountField;

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceSurcharge surchargeField;

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceTaxSummary taxSummaryField;

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceTotalAmount totalAmountField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceBaseAmount BaseAmount
        {
            get
            {
                return this.baseAmountField;
            }
            set
            {
                this.baseAmountField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceEquivAmount EquivAmount
        {
            get
            {
                return this.equivAmountField;
            }
            set
            {
                this.equivAmountField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceSurcharge Surcharge
        {
            get
            {
                return this.surchargeField;
            }
            set
            {
                this.surchargeField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceTaxSummary TaxSummary
        {
            get
            {
                return this.taxSummaryField;
            }
            set
            {
                this.taxSummaryField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceTotalAmount TotalAmount
        {
            get
            {
                return this.totalAmountField;
            }
            set
            {
                this.totalAmountField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceBaseAmount
    {

        private string curCodeField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CurCode
        {
            get
            {
                return this.curCodeField;
            }
            set
            {
                this.curCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceEquivAmount
    {

        private string curCodeField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CurCode
        {
            get
            {
                return this.curCodeField;
            }
            set
            {
                this.curCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceSurcharge
    {

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceSurchargeTotalAmount totalAmountField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceSurchargeTotalAmount TotalAmount
        {
            get
            {
                return this.totalAmountField;
            }
            set
            {
                this.totalAmountField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceSurchargeTotalAmount
    {

        private string curCodeField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CurCode
        {
            get
            {
                return this.curCodeField;
            }
            set
            {
                this.curCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceTaxSummary
    {

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceTaxSummaryTax[] taxField;

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceTaxSummaryTotalTaxAmount totalTaxAmountField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Tax")]
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceTaxSummaryTax[] Tax
        {
            get
            {
                return this.taxField;
            }
            set
            {
                this.taxField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceTaxSummaryTotalTaxAmount TotalTaxAmount
        {
            get
            {
                return this.totalTaxAmountField;
            }
            set
            {
                this.totalTaxAmountField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceTaxSummaryTax
    {

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceTaxSummaryTaxAmount amountField;

        private string taxCodeField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceTaxSummaryTaxAmount Amount
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
        public string TaxCode
        {
            get
            {
                return this.taxCodeField;
            }
            set
            {
                this.taxCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceTaxSummaryTaxAmount
    {

        private string curCodeField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CurCode
        {
            get
            {
                return this.curCodeField;
            }
            set
            {
                this.curCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceTaxSummaryTotalTaxAmount
    {

        private string curCodeField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CurCode
        {
            get
            {
                return this.curCodeField;
            }
            set
            {
                this.curCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemPriceTotalAmount
    {

        private string curCodeField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CurCode
        {
            get
            {
                return this.curCodeField;
            }
            set
            {
                this.curCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemService
    {

        private string paxRefIDField;

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemServiceServiceAssociations serviceAssociationsField;

        private string serviceIDField;

        /// <remarks/>
        public string PaxRefID
        {
            get
            {
                return this.paxRefIDField;
            }
            set
            {
                this.paxRefIDField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemServiceServiceAssociations ServiceAssociations
        {
            get
            {
                return this.serviceAssociationsField;
            }
            set
            {
                this.serviceAssociationsField = value;
            }
        }

        /// <remarks/>
        public string ServiceID
        {
            get
            {
                return this.serviceIDField;
            }
            set
            {
                this.serviceIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemServiceServiceAssociations
    {

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemServiceServiceAssociationsPaxJourneyRef paxJourneyRefField;

        private IATA_AirShoppingRSResponseOffersGroupOfferOfferItemServiceServiceAssociationsServiceDefinitionRef serviceDefinitionRefField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemServiceServiceAssociationsPaxJourneyRef PaxJourneyRef
        {
            get
            {
                return this.paxJourneyRefField;
            }
            set
            {
                this.paxJourneyRefField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferOfferItemServiceServiceAssociationsServiceDefinitionRef ServiceDefinitionRef
        {
            get
            {
                return this.serviceDefinitionRefField;
            }
            set
            {
                this.serviceDefinitionRefField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemServiceServiceAssociationsPaxJourneyRef
    {

        private string paxJourneyRefIDField;

        /// <remarks/>
        public string PaxJourneyRefID
        {
            get
            {
                return this.paxJourneyRefIDField;
            }
            set
            {
                this.paxJourneyRefIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferOfferItemServiceServiceAssociationsServiceDefinitionRef
    {

        private string paxSegmentRefIDField;

        private string serviceDefinitionRefIDField;

        /// <remarks/>
        public string PaxSegmentRefID
        {
            get
            {
                return this.paxSegmentRefIDField;
            }
            set
            {
                this.paxSegmentRefIDField = value;
            }
        }

        /// <remarks/>
        public string ServiceDefinitionRefID
        {
            get
            {
                return this.serviceDefinitionRefIDField;
            }
            set
            {
                this.serviceDefinitionRefIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferTotalPrice
    {

        private IATA_AirShoppingRSResponseOffersGroupOfferTotalPriceBaseAmount baseAmountField;

        private IATA_AirShoppingRSResponseOffersGroupOfferTotalPriceSurcharge surchargeField;

        private IATA_AirShoppingRSResponseOffersGroupOfferTotalPriceTaxSummary taxSummaryField;

        private IATA_AirShoppingRSResponseOffersGroupOfferTotalPriceTotalAmount totalAmountField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferTotalPriceBaseAmount BaseAmount
        {
            get
            {
                return this.baseAmountField;
            }
            set
            {
                this.baseAmountField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferTotalPriceSurcharge Surcharge
        {
            get
            {
                return this.surchargeField;
            }
            set
            {
                this.surchargeField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferTotalPriceTaxSummary TaxSummary
        {
            get
            {
                return this.taxSummaryField;
            }
            set
            {
                this.taxSummaryField = value;
            }
        }

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferTotalPriceTotalAmount TotalAmount
        {
            get
            {
                return this.totalAmountField;
            }
            set
            {
                this.totalAmountField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferTotalPriceBaseAmount
    {

        private string curCodeField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CurCode
        {
            get
            {
                return this.curCodeField;
            }
            set
            {
                this.curCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferTotalPriceSurcharge
    {

        private IATA_AirShoppingRSResponseOffersGroupOfferTotalPriceSurchargeTotalAmount totalAmountField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferTotalPriceSurchargeTotalAmount TotalAmount
        {
            get
            {
                return this.totalAmountField;
            }
            set
            {
                this.totalAmountField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferTotalPriceSurchargeTotalAmount
    {

        private string curCodeField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CurCode
        {
            get
            {
                return this.curCodeField;
            }
            set
            {
                this.curCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferTotalPriceTaxSummary
    {

        private IATA_AirShoppingRSResponseOffersGroupOfferTotalPriceTaxSummaryTotalTaxAmount totalTaxAmountField;

        /// <remarks/>
        public IATA_AirShoppingRSResponseOffersGroupOfferTotalPriceTaxSummaryTotalTaxAmount TotalTaxAmount
        {
            get
            {
                return this.totalTaxAmountField;
            }
            set
            {
                this.totalTaxAmountField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferTotalPriceTaxSummaryTotalTaxAmount
    {

        private string curCodeField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CurCode
        {
            get
            {
                return this.curCodeField;
            }
            set
            {
                this.curCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseOffersGroupOfferTotalPriceTotalAmount
    {

        private string curCodeField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CurCode
        {
            get
            {
                return this.curCodeField;
            }
            set
            {
                this.curCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSResponseShoppingResponse
    {

        private string shoppingResponseRefIDField;

        /// <remarks/>
        public string ShoppingResponseRefID
        {
            get
            {
                return this.shoppingResponseRefIDField;
            }
            set
            {
                this.shoppingResponseRefIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRS")]
    public partial class IATA_AirShoppingRSPayloadAttributes
    {

        private string echoTokenTextField;

        /// <remarks/>
        public string EchoTokenText
        {
            get
            {
                return this.echoTokenTextField;
            }
            set
            {
                this.echoTokenTextField = value;
            }
        }
    }


}
