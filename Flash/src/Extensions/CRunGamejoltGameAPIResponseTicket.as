package Extensions
{
	public class CRunGamejoltGameAPIResponseTicket
	{
        public var URL:String;
        public var Type:String;
        public var Data:Object = null;
        public var Trigger:int = 0;
        public var HasTrigger:Boolean = false;

        public var HasError:Boolean = false;
        public var Error:String = "";

        public function CRunGamejoltGameAPIResponseTicket(url:String, type:String)
        {
            URL = url;
            Type = type;
        }
	}
}