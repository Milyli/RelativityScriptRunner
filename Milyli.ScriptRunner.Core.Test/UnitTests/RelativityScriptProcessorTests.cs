namespace Milyli.ScriptRunner.Core.Test.UnitTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using global::Relativity.API;
	using kCura.Relativity.Client;
	using Milyli.ScriptRunner.Core.Models;
	using Milyli.ScriptRunner.Core.Tools;
	using Moq;
	using NUnit.Framework;

	[TestFixture(Category = "Unit")]
	public class RelativityScriptProcessorTests
	{
		private RelativityScriptProcessor relativityScriptProcessor;
		private Mock<IHelper> helperMock;

		[SetUp]
		public void Setup()
		{
			this.helperMock = new Mock<IHelper>(MockBehavior.Strict);
			this.relativityScriptProcessor = new RelativityScriptProcessor(this.helperMock.Object);
		}

		[Test]
		public void GetSavedSearchIds_ReturnsExpected()
		{
			// Arrange
			var firstSearchId = 1234;
			var secondSearchId = 12345;
			var expectedSearchIds = new List<int> { firstSearchId, secondSearchId };
			var populatedInputs = new List<JobScriptInput>
			{
				new JobScriptInput
				{
					InputId = nameof(firstSearchId),
					InputValue = firstSearchId.ToString(),
				},
				new JobScriptInput
				{
					InputId = nameof(secondSearchId),
					InputValue = secondSearchId.ToString(),
				},
				new JobScriptInput
				{
					InputId = "otherInput",
					InputValue = "some other input",
				},
			};
			var relativityInputs = new List<RelativityScriptInputDetails>
			{
				new RelativityScriptInputDetails
				{
					Id = nameof(firstSearchId),
					InputType = RelativityScriptInputDetailsScriptInputType.SavedSearch,
				},
				new RelativityScriptInputDetails
				{
					Id = nameof(secondSearchId),
					InputType = RelativityScriptInputDetailsScriptInputType.SavedSearch,
				},
				new RelativityScriptInputDetails
				{
					Id = "otherInput",
					InputType = RelativityScriptInputDetailsScriptInputType.Constant,
				},
			};

			// Act
			var searchIds = this.relativityScriptProcessor.GetSavedSearchIds(populatedInputs, relativityInputs);

			// Assert
			CollectionAssert.AreEquivalent(expectedSearchIds, searchIds);
		}

		[Test]
		public void ReplaceCaseArtifactId()
		{
			// Arrange
			var workspaceId = 123456;
			var templateSql = @"UPDATE[EDDSDBO].[Foo]
SET[Bar] = {0}";
			var scriptSql = string.Format(templateSql, "#CaseArtifactID#");
			var expectedSql = string.Format(templateSql, workspaceId);
			var dbContextStub = new DBContextStub();
			dbContextStub.IsMasterDatabase = true;
			this.helperMock.Setup(h => h.GetDBContext(workspaceId)).Returns(dbContextStub);

			// Act
			var generatedSql = this.relativityScriptProcessor.SubstituteGlobalVariables(workspaceId, scriptSql);

			// Assert
			Assert.AreEqual(expectedSql, generatedSql);
		}

		[Test]
		[Description("Verifies replacement of the MasterDatabasePrepend on scripts executed in non-admin workspaces on master server")]
		public void ReplaceEddsPrepend_WorkspaceOnMasterServer()
		{
			// Arrange
			var workspaceId = 123456;
			var templateSql = @"UPDATE {0}[Foo]
SET[Bar] = 1";
			var scriptSql = string.Format(templateSql, "#MasterDatabasePrepend#");
			var expectedSql = string.Format(templateSql, "[EDDS].[EDDSDBO].");
			var workspaceContextStub = new DBContextStub();
			workspaceContextStub.IsMasterDatabase = false;
			workspaceContextStub.ServerName = "coolServer";
			var masterContextStub = new DBContextStub();
			masterContextStub.IsMasterDatabase = true;
			masterContextStub.ServerName = "coolServer";

			this.helperMock.Setup(h => h.GetDBContext(workspaceId)).Returns(workspaceContextStub);
			this.helperMock.Setup(h => h.GetDBContext(-1)).Returns(masterContextStub);

			// Act
			var generatedSql = this.relativityScriptProcessor.SubstituteGlobalVariables(workspaceId, scriptSql);

			// Assert
			Assert.AreEqual(expectedSql, generatedSql);
		}

		[Test]
		[Description("Verifies replacement of the MasterDatabasePrepend on scripts executed in non-admin workspaces on distributed servers")]
		public void ReplaceEddsPrepend_WorkspaceOnDistributedServer()
		{
			// Arrange
			var workspaceId = 123456;
			var templateSql = @"UPDATE {0}[Foo]
SET[Bar] = 1";
			var scriptSql = string.Format(templateSql, "#MasterDatabasePrepend#");
			var expectedSql = string.Format(templateSql, "[masterServer].[EDDS].[EDDSDBO].");
			var workspaceContextStub = new DBContextStub();
			workspaceContextStub.IsMasterDatabase = false;
			workspaceContextStub.ServerName = "otherServer";
			var masterContextStub = new DBContextStub();
			masterContextStub.IsMasterDatabase = true;
			masterContextStub.ServerName = "masterServer";

			this.helperMock.Setup(h => h.GetDBContext(workspaceId)).Returns(workspaceContextStub);
			this.helperMock.Setup(h => h.GetDBContext(-1)).Returns(masterContextStub);

			// Act
			var generatedSql = this.relativityScriptProcessor.SubstituteGlobalVariables(workspaceId, scriptSql);

			// Assert
			Assert.AreEqual(expectedSql, generatedSql);
		}

		[Test]
		[Description("Verifies replacement of the MasterDatabasePrepend on scripts executed in admin workspace")]
		public void ReplaceEddsPrepend_Admin()
		{
			// Arrange
			var workspaceId = -1;
			var templateSql = @"UPDATE {0}[Foo]
SET[Bar] = 1";
			var scriptSql = string.Format(templateSql, "#MasterDatabasePrepend#");
			var expectedSql = string.Format(templateSql, "[EDDS].[EDDSDBO].");
			var masterContextStub = new DBContextStub();
			masterContextStub.IsMasterDatabase = true;

			this.helperMock.Setup(h => h.GetDBContext(workspaceId)).Returns(masterContextStub);

			// Act
			var generatedSql = this.relativityScriptProcessor.SubstituteGlobalVariables(workspaceId, scriptSql);

			// Assert
			Assert.AreEqual(expectedSql, generatedSql);
		}

		[TestCase(RelativityScriptInputDetailsScriptInputType.Field)]
		[TestCase(RelativityScriptInputDetailsScriptInputType.Sql)]
		[TestCase(RelativityScriptInputDetailsScriptInputType.SearchProvider)]
		[Description("Verifies expected replacement for script inputs which are directly substituted")]
		public void ReplaceScriptInput_DirectReplace(RelativityScriptInputDetailsScriptInputType inputType)
		{
			// Arrange
			var templateSql = @"UPDATE [Foo]
SET[Bar] = {0}";

			var inputId = "my_input";
			var inputValue = "coolValue";
			var expectedSql = string.Format(templateSql, inputValue);
			var scriptSql = string.Format(templateSql, $"#{inputId}#");

			var populatedInputs = new List<JobScriptInput>
			{
				new JobScriptInput
				{
					InputId = inputId,
					InputValue = inputValue,
				}
			};
			var relativityInputs = new List<RelativityScriptInputDetails>
			{
				new RelativityScriptInputDetails
				{
					Id = inputId,
					InputType = inputType,
				}
			};

			// Act
			var generatedSql = this.relativityScriptProcessor.SubstituteScriptInputs(populatedInputs, relativityInputs, scriptSql, null, 1);

			// Assert
			Assert.AreEqual(expectedSql, generatedSql);
		}

		[TestCase("date")]
		[TestCase("datetime")]
		[TestCase("text")]
		[Description("Verifies expected replacement for constant script inputs which should be substituted as strings")]
		public void ReplaceScriptInput_ConstantInputString(string dataType)
		{
			// Arrange
			var templateSql = @"UPDATE [Foo]
SET[Bar] = {0}";

			var inputId = "my_input";
			var inputValue = "coolValue";
			var expectedSql = string.Format(templateSql, $"'{inputValue}'");
			var scriptSql = string.Format(templateSql, $"#{inputId}#");

			var populatedInputs = new List<JobScriptInput>
			{
				new JobScriptInput
				{
					InputId = inputId,
					InputValue = inputValue,
				}
			};
			var relativityInputs = new List<RelativityScriptInputDetails>
			{
				new RelativityScriptInputDetails
				{
					Id = inputId,
					InputType = RelativityScriptInputDetailsScriptInputType.Constant,
					Attributes = new Dictionary<string, string> { { "DataType", dataType } }
				}
			};

			// Act
			var generatedSql = this.relativityScriptProcessor.SubstituteScriptInputs(populatedInputs, relativityInputs, scriptSql, null, 1);

			// Assert
			Assert.AreEqual(expectedSql, generatedSql);
		}

		[TestCase("user")]
		[TestCase("number")]
		[Description("Verifies expected replacement for constant script inputs which should be substituted directly")]
		public void ReplaceScriptInput_ConstantInputDirect(string dataType)
		{
			// Arrange
			var templateSql = @"UPDATE [Foo]
SET[Bar] = {0}";

			var inputId = "my_input";
			var inputValue = "coolValue";
			var expectedSql = string.Format(templateSql, $"{inputValue}");
			var scriptSql = string.Format(templateSql, $"#{inputId}#");

			var populatedInputs = new List<JobScriptInput>
			{
				new JobScriptInput
				{
					InputId = inputId,
					InputValue = inputValue,
				}
			};
			var relativityInputs = new List<RelativityScriptInputDetails>
			{
				new RelativityScriptInputDetails
				{
					Id = inputId,
					InputType = RelativityScriptInputDetailsScriptInputType.Constant,
					Attributes = new Dictionary<string, string> { { "DataType", dataType } }
				}
			};

			// Act
			var generatedSql = this.relativityScriptProcessor.SubstituteScriptInputs(populatedInputs, relativityInputs, scriptSql, null, 1);

			// Assert
			Assert.AreEqual(expectedSql, generatedSql);
		}

		[TestCase("Central Standard Time", -6)]
		[TestCase("Nepal Standard Time", 5.75)]
		[Description("Verifies expected replacement for timezone inputs")]
		public void ReplaceScriptInput_ConstantInputTimezone(string timeZoneName, decimal offset)
		{
			// Arrange
			var timeZone = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(tzi => tzi.Id == timeZoneName);
			if (timeZone.IsDaylightSavingTime(DateTime.UtcNow))
			{
				offset++;
			}

			var templateSql = @"UPDATE [Foo]
SET[Bar] = {0}";

			var inputId = "my_input";
			var inputValue = timeZoneName;
			var expectedSql = string.Format(templateSql, offset);
			var scriptSql = string.Format(templateSql, $"#{inputId}#");

			var populatedInputs = new List<JobScriptInput>
			{
				new JobScriptInput
				{
					InputId = inputId,
					InputValue = timeZoneName,
				}
			};
			var relativityInputs = new List<RelativityScriptInputDetails>
			{
				new RelativityScriptInputDetails
				{
					Id = inputId,
					InputType = RelativityScriptInputDetailsScriptInputType.Constant,
					Attributes = new Dictionary<string, string> { { "DataType", "timezone" } }
				}
			};

			// Act
			var generatedSql = this.relativityScriptProcessor.SubstituteScriptInputs(populatedInputs, relativityInputs, scriptSql, null, 1);

			// Assert
			Assert.AreEqual(expectedSql, generatedSql);
		}

		[Test]
		[Description("Verifies expected replacement for saved search inputs")]
		public void ReplaceScriptInput_SavedSearch()
		{
			// Arrange
			var templateSql = @"INSERT INTO 
                                EDDSDBO.RelativityTempTable
                SELECT
                                [Document].ArtifactID,
                                NULL
                {0} AND [Document].ArtifactID";

			var inputId = "my_input";
			var inputValue = "123456";
			var searchTablePrepend = Guid.NewGuid().ToString().Replace("-", string.Empty);
			var generatedSearchTableName = searchTablePrepend + "_" + inputValue;
			var substitutedSql = string.Format(
				@"FROM [Document], {0} (NOLOCK)
WHERE {0}.DocId = [Document].ArtifactID",
				generatedSearchTableName);
			var expectedSql = string.Format(templateSql, substitutedSql);
			var scriptSql = string.Format(templateSql, $"#{inputId}#");

			var populatedInputs = new List<JobScriptInput>
			{
				new JobScriptInput
				{
					InputId = inputId,
					InputValue = inputValue,
				}
			};
			var relativityInputs = new List<RelativityScriptInputDetails>
			{
				new RelativityScriptInputDetails
				{
					Id = inputId,
					InputType = RelativityScriptInputDetailsScriptInputType.SavedSearch,
				}
			};

			// Act
			var generatedSql = this.relativityScriptProcessor.SubstituteScriptInputs(populatedInputs, relativityInputs, scriptSql, searchTablePrepend, 1);

			// Assert
			Assert.AreEqual(expectedSql, generatedSql);
		}
	}
}
