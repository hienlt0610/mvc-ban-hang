using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanHang.Models
{
    public class OnepayProperty
    {
        public const string HASH_CODE = "A3EFDFABA8653DF2342E8DAC29B51AF0";
            //"6D0870CDE5F24F34F3915FB0045120DB";

        public const string ACCESS_CODE = "D67342C2";
            //"6BEB2546";

        public const string MERCHANT_ID = "ONEPAY";
            //"TESTONEPAY";

        public const string URL_ONEPAY_TEST = "https://mtf.onepay.vn/onecomm-pay/vpc.op";

        public const string URL_ONEPAY_TRUTH = "http://onepay.vn/vpcpay/vpcpay.op";

        public const string COMMAND = "pay";

        public const string PAYGATE_LANGUAGE = "vn";

        public const string VERSION = "2";

        public const string AGAIN_LINK = "onepay.vn";
    }
}