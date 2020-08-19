namespace DTO {
    public class JwtDTO {

        private string access_token;
        private int expires_in;

        public JwtDTO(string access_token, int expires_in) {
            Acces_token = access_token;
            Expires_in = expires_in;
        }

        public string Acces_token {get; set;}
        public int Expires_in {get; set;}
    }
}