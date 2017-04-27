namespace Milyli.ScriptRunner.Data.Test.UnitTests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using Milyli.ScriptRunner.Data.Xml.Script;
    using NUnit.Framework;

    [TestFixture]
    public class TestRelativityScript
    {
        [Test]
        public void TestXmlDeserialization()
        {
            var script = @"
<script>
    <name>Globally Administer Keyboard Shortcuts</name>
    <description>This Relativity script forcibly assigns a keyboard shortcut to a specific system function. This script runs against all workspaces.</description>
    <category>System Functionality</category>
    <key>E0C92C54-6B22-44CF-A75D-3459DB5B0765</key>
    <version>8.1.3</version>
    <input>
        <constant id=""shiftInp"" name=""Shift:"" type=""text"">
            <option value = ""..."" />
            <option>On</option>
            <option>Off</option>
        </constant>
        <constant id=""altInp"" name=""Alt:"" type=""text"">
            <option value = ""..."" />
            <option>On</option>
            <option>Off</option>
        </constant>
    </input>
    <action returns=""table"" allowhtmltagsinoutput=""true"" timeout=""1800"">
                                                                                                DECLARE @shift NVARCHAR(1) SET @shift = (SELECT CASE WHEN #shiftInp# = 'On' THEN '1' ELSE '0' END)
DECLARE @alt NVARCHAR(1) SET @alt = (SELECT CASE WHEN #altInp# = 'On' THEN '1' ELSE '0' END)
DECLARE @ctrl NVARCHAR(1) SET @ctrl = (SELECT CASE WHEN #ctrlInp# = 'On' THEN '1' ELSE '0' END)
IF(CAST(@ctrl AS INT) + CAST(@alt AS INT)) = 0 BEGIN
                                                                                                
                                                                                                    SELECT '&lt;span style=""color:red""&gt;Either ""Ctrl (Command for Mac)"" or ""Alt"" must be selected&lt;/span&gt;' AS[ERROR]
                                                                                                
                                                                                                    GOTO THEEND
                                                                                                END
                                                                                                DECLARE @locations TABLE(LOC NVARCHAR(MAX))
                                                                                                INSERT @locations SELECT '[' + DBLocation + '].[EDDS' + CAST(ArtifactID AS NVARCHAR(20)) + '].[EDDSDBO].[ReassignKeyboardShortcut] 
                                                                                                
                                                                                                    @shift = ' + @shift + ',
                                                                                                    @alt = ' + @alt + ',
                                                                                                    @ctrl = ' + @ctrl + ',
                                                                                                    @key = ' + #keyInp# + ',
                                                                                                    @actionName = ''' + #actionInp# + ''''
                                                                                                
                                                                                                 FROM ExtendedCase
                                                                                                DECLARE @location NVARCHAR(MAX)
                                                                                                DECLARE @output TABLE(LOC NVARCHAR(MAX))
                                                                                                WHILE EXISTS(SELECT TOP 1 LOC FROM @locations) BEGIN
                                                                                                
                                                                                                    SET @location = (SELECT TOP 1 LOC FROM @locations)

	INSERT INTO @output VALUES (@location)
    EXEC SP_EXECUTESQL @location

    DELETE FROM @locations WHERE LOC = @location
END

SELECT * FROM @output

THEEND:


    </action>
</script>
";
            var deser = new XmlSerializer(typeof(Script), string.Empty);

            using (var reader = new StringReader(script))
            {
                var result = (Script)deser.Deserialize(reader);
                Assert.That((result?.Input?.Constants?.Count ?? 0) > 0, "expected an input count > 0");
                var firstConstant = result.Input.Constants.First();
                Assert.That(firstConstant.Id == "shiftInp", "expected \"shiftInp\", got {0}", firstConstant.Id);
                Assert.That(firstConstant.Name == "Shift:", "expected \"Shift:\", got {0}", firstConstant.Name);
                Assert.That(firstConstant.Options.Count == 3);
            }
        }
    }
}
