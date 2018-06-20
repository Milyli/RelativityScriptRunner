namespace Milyli.ScriptRunner.ExternalIHelper
{
	using System;
	using Relativity.API;

	public class ExternalICPHelper : ExternalIHelper, ICPHelper
	{
		private int workspaceId;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExternalICPHelper"/> class.
		/// Creates a new test helper for the current machine's environment.
		/// </summary>
		/// <param name="workspaceId">The active workspace id</param>
		public ExternalICPHelper(int workspaceId)
			: base()
		{
			this.workspaceId = workspaceId;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExternalICPHelper"/> class.
		/// Creates a new test helper for the given database server name.
		/// </summary>
		/// <param name="workspaceId">The active workspace id</param>
		/// <param name="eddsServerName">The database server name</param>
		public ExternalICPHelper(int workspaceId, string eddsServerName)
			: base(eddsServerName)
		{
			this.workspaceId = workspaceId;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExternalICPHelper"/> class.
		/// Creates a new test helper using the given <see cref="Settings"/>
		/// </summary>
		/// <param name="workspaceId">The active workspace id</param>
		/// <param name="settings">The </param>
		public ExternalICPHelper(int workspaceId, Settings settings)
			: base(settings)
		{
			this.workspaceId = workspaceId;
		}

		public int GetActiveCaseID()
		{
			return this.workspaceId;
		}

		public IAuthenticationMgr GetAuthenticationManager()
		{
			throw new NotImplementedException();
		}

		public ICSRFManager GetCSRFManager()
		{
			throw new NotImplementedException();
		}
	}
}
