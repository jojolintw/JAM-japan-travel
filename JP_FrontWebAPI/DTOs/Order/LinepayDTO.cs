namespace JP_FrontWebAPI.DTOs.Order
{
    public class LinepayDTO
    {
        public class PaymentRequestDto
        {
            public int amount { get; set; }
            //public string orderId { get; set; }
        }

        public class PaymentResponseDto
        {
            public PaymentResponseInfo info { get; set; }
        }

        public class PaymentResponseInfo
        {
            public PaymentUrl paymentUrl { get; set; }
        }

        public class PaymentUrl
        {
            public string web { get; set; }
        }
    }
}
