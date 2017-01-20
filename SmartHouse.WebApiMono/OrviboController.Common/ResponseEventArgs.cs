namespace OrviboController.Common
{
    public class ResponseEventArgs
    {
        private readonly Response _response;

        public ResponseEventArgs(Response response)
        {
            _response = response;
        }

        public Response Response
        {
            get { return _response; }
        }
    }      
}
