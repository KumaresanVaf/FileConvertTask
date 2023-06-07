namespace FileModel
{
    public class JsonModel
    {
        public string? EnrollmentBatchDetailId { get; set; }
        public int EnrollmentSalesTypeId { get; set; }
        public int EnrollmentStatusId { get; set; }
        public string? CorrelationId { get; set; }
        public string? TermsofServiceAgreement { get; set; }
        public string? TermsofUseAgreement { get; set; }
        public bool IsActive { get; set; }
        public string? EnrollmentSourceCode { get; set; }
        public string? ConfirmationNumber { get; set; }
        public string? EnrollmentHoldReasonCode { get; set; }
        public string? TPVCode { get; set; }
        public string? EnrollmentStatusCode { get; set; }
        public string? EnrollmentStatusReasonCode { get; set; }
        public string? EnrollmentStatusJSON { get; set; }
        public bool IsPicked { get; set; }
        public int LastCompletedStep { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? DepositDate { get; set; }
        public string? ExtendedProperties { get; set; }
        public string? RequestDate { get; set; }
        public int DivisionId { get; set; }
        public string? DivisionName { get; set; }
        public string? DivisionCode { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime CreateDate { get; set; }
        public string? LastModifiedByName { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string? id { get; set; }
    }
}