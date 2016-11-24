using System;
using System.ServiceModel;

namespace Me.ThreeDWares.SugarCube {
	/// <summary>
	/// This interface describes the RPC methods that are available between the client side of the ShowUploaderWindow
	/// pipe and the server side, where the client is either the Manager or TestBed application and the server is
	/// the Upload Manager
	/// </summary>
	[ServiceContract]
	public interface IShowUploaderWindow {
		/// <summary>
		/// This is the only RPC method exposed by this interface, to cause the upload manager to show its
		/// main window
		/// </summary>
		[OperationContract]
		void ShowUploaderWindow();
	}
}
