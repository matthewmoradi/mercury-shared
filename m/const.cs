namespace mercury.model
{
    public class _const
    {
        public static string prefix { get; } = "const";
        public string id { get; set; }
        public string gw_payir_api { get; set; }  = "test";
        public string gw_zp_api { get; set; }  = "test";
        public string gw_mp_api { get; set; }  = "test";
        public string gw_addr_callback { get; set; }  = "http://127.0.0.1:5000/payresult/";
        public int credit_toadd_min { get; set; } = 10000;
        public int credit_toadd_max { get; set; }  = 10000000;
        public int enterance_amount_guest { get; set; }  = 200000;
        public int enterance_amount_membership { get; set; }  = 500000;
        public int membership_days { get; set; }  = 60;
        public int membership_enterance_max { get; set; }  = 4;
        public int member_codes_max_per_day { get; set; }  = 9;
        public bool gw_active_payir { get; set; }  = false;
        public bool gw_active_zp { get; set; }  = false;
        public bool gw_active_mp { get; set; }  = true;
        
    }
}