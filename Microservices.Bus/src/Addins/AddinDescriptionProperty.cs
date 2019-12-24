using System.Diagnostics;

namespace Microservices.Bus.Addins
{
	/// <summary>
	/// Св-во описания дополнения.
	/// </summary>
	[DebuggerDisplay("{this.Name}={this.Value}")]
	public class AddinDescriptionProperty
	{
		public string Name { get; set; }

		public string Value { get; set; }

		public string Type { get; set; }

		public string Format { get; set; }

		public string DefaultValue { get; set; }

		public string Comment { get; set; }

		public bool ReadOnly { get; set; }

		public bool Secret { get; set; }
	}
}
