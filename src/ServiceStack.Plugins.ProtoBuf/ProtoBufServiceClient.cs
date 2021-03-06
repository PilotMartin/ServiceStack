using ProtoBuf;
using System.IO;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;

namespace ServiceStack.Plugins.ProtoBuf
{
	public class ProtoBufServiceClient : ServiceClientBase
	{
        public override string Format
        {
            get { return "x-protobuf"; }
        }

		public ProtoBufServiceClient(string baseUri)
		{
			SetBaseUri(baseUri);
		}

		public ProtoBufServiceClient(string syncReplyBaseUri, string asyncOneWayBaseUri)
			: base(syncReplyBaseUri, asyncOneWayBaseUri) {}

		public override void SerializeToStream(IRequestContext requestContext, object request, Stream stream)
		{
			try
			{
				Serializer.NonGeneric.Serialize(stream, request);
			}
			catch (Exception ex)
			{
				throw new SerializationException("ProtoBufServiceClient: Error serializing: " + ex.Message, ex);
			}
		}

		public override T DeserializeFromStream<T>(Stream stream)
		{
			try
			{
				return Serializer.Deserialize<T>(stream);
			}
			catch (Exception ex)
			{
				throw new SerializationException("ProtoBufServiceClient: Error deserializing: " + ex.Message, ex);
			}
		}

		public override string ContentType
		{
			get { return Common.Web.ContentType.ProtoBuf; }
		}

		public override StreamDeserializerDelegate StreamDeserializer
		{
			get { return Deserialize; }
		}
		
		private static object Deserialize(Type type, Stream source)
		{
			try
			{
				return Serializer.NonGeneric.Deserialize(type, source);
			}
			catch (Exception ex)
			{
				throw new SerializationException("ProtoBufServiceClient: Error deserializing: " + ex.Message, ex);
			}
		}
	}
}
