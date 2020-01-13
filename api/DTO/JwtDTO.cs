namespace DTO {
    public class JwtDTO {

        private string access_token;
        private int expires_in;

        public JwtDTO(string access_token, int expires_in) {
            this.access_token = access_token;
            this.expires_in = expires_in;
        }

        public string Acces_token {get; set;}
        public int Expires_in {get; set;}
    }
}