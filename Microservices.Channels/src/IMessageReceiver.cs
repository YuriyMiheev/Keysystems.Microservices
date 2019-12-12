namespace Microservices.Channels
{
	public interface IMessageReceiver
	{
		Message ReceiveMessage(Message msg);
	}
}
