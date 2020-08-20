namespace DTO {
    public class JwtDTO {
        
        public JwtDTO(string access_token, int expires_in, string[] roles) {
           Acces_token = access_token;
           Expires_in = expires_in;
           Roles = roles;
        }

        public string Acces_token { get; set; }
        public int Expires_in { get; set; }
        public string[] Roles { get; set; }
    }
}
